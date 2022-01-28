#include "VRDriver.hpp"
#include <Driver/TrackerDevice.hpp>

std::mutex mymutex;

vr::EVRInitError ptscDriver::VRDriver::Init(vr::IVRDriverContext* pDriverContext)
{
    // Perform driver context initialisation
    if (vr::EVRInitError init_error = vr::InitServerDriverContext(pDriverContext); init_error != vr::EVRInitError::VRInitError_None) {
        return init_error;
    }

    Log("Activating ptsc Driver...");

    Log("Connecting to pipe...");
    LPTSTR lpszPipename = (LPTSTR)(L"\\\\.\\pipe\\PTSCDriverPipe");

    // Open the named pipe
    // Most of these parameters aren't very relevant for pipes.
    pipe = CreateFile(
        lpszPipename,
        GENERIC_READ, // only need read access
        FILE_SHARE_READ | FILE_SHARE_WRITE,
        NULL,
        OPEN_EXISTING,
        FILE_ATTRIBUTE_NORMAL,
        NULL);

    // trackers not created if pipe client isn't connected to server
    if (pipe == INVALID_HANDLE_VALUE)
    {
        Log("Failed to connect to pipe. PoseVR not creating trackers.");
        //system("pause");
        pipe_connected = false;
        CloseHandle(pipe);
        return vr::VRInitError_None;
    }

    // Initialize pipe thread
    Log("Starting pipe thread");
    std::thread pipeThread(&ptscDriver::VRDriver::PipeThread, this);
    pipeThread.detach();

    auto waist_tracker = std::make_shared<TrackerDevice>("0Waist_TrackerDevice",0, static_cast<TrackerRole>(WAIST));
    this->AddTracker(waist_tracker);
    this->trackers_.push_back(waist_tracker);

    auto left_foot_tracker = std::make_shared<TrackerDevice>("1LeftFoot_TrackerDevice",1, static_cast<TrackerRole>(LEFT_FOOT));
    this->AddTracker(left_foot_tracker);
    this->trackers_.push_back(left_foot_tracker);

    auto right_foot_tracker = std::make_shared<TrackerDevice>("2RightFoot_TrackerDevice",2, static_cast<TrackerRole>(RIGHT_FOOT));
    this->AddTracker(right_foot_tracker);
    this->trackers_.push_back(right_foot_tracker);

    Log("ptscDriver Loaded Successfully");

	return vr::VRInitError_None;
}

void ptscDriver::VRDriver::Cleanup()
{
}

void ptscDriver::VRDriver::RunFrame()
{
    if (pipe_connected)
    {
        // Collect events
        vr::VREvent_t event;
        std::vector<vr::VREvent_t> events;
        while (vr::VRServerDriverHost()->PollNextEvent(&event, sizeof(event)))
        {
            events.push_back(event);
        }
        this->openvr_events_ = events;

        //// Update frame timing
        //std::chrono::system_clock::time_point now = std::chrono::system_clock::now();
        //this->frame_timing_ = std::chrono::duration_cast<std::chrono::milliseconds>(now - this->last_frame_time_);
        //this->last_frame_time_ = now;

        // lock thread to for thread safety
        std::lock_guard<std::mutex> g(mymutex);

        std::vector<double> waist_pos = poseData.get_desired_tracker_position("waist");
        std::vector<double> left_foot_pos = poseData.get_desired_tracker_position("left_foot");
        std::vector<double> right_foot_pos = poseData.get_desired_tracker_position("right_foot");


        this->trackers_[0]->UpdatePos(waist_pos[0], waist_pos[1], waist_pos[2]);
        this->trackers_[0]->UpdateRot(waist_pos[3], waist_pos[4], waist_pos[5], waist_pos[6]);
        this->trackers_[1]->UpdatePos(left_foot_pos[0], left_foot_pos[1], left_foot_pos[2]);
        this->trackers_[1]->UpdateRot(left_foot_pos[3], left_foot_pos[4], left_foot_pos[5], left_foot_pos[6]);
        this->trackers_[2]->UpdatePos(right_foot_pos[0], right_foot_pos[1], right_foot_pos[2]);
        this->trackers_[2]->UpdateRot(right_foot_pos[3], right_foot_pos[4], right_foot_pos[5], right_foot_pos[6]);

        for (auto& device : this->devices_)
            device->Update();
    }
}

bool ptscDriver::VRDriver::ShouldBlockStandbyMode()
{
    return false;
}

void ptscDriver::VRDriver::EnterStandby()
{
}

void ptscDriver::VRDriver::LeaveStandby()
{
}

std::vector<std::shared_ptr<ptscDriver::IVRDevice>> ptscDriver::VRDriver::GetDevices()
{
    return this->devices_;
}

std::vector<vr::VREvent_t> ptscDriver::VRDriver::GetOpenVREvents()
{
    return this->openvr_events_;
}

std::chrono::milliseconds ptscDriver::VRDriver::GetLastFrameTime()
{
    return this->frame_timing_;
}

bool ptscDriver::VRDriver::AddTracker(std::shared_ptr<IVRDevice> device)
{
    bool result = vr::VRServerDriverHost()->TrackedDeviceAdded(device->GetSerial().c_str(), vr::ETrackedDeviceClass::TrackedDeviceClass_GenericTracker, device.get());
    if(result)
        this->devices_.push_back(device);
    return result;
}

ptscDriver::SettingsValue ptscDriver::VRDriver::GetSettingsValue(std::string key)
{
    vr::EVRSettingsError err = vr::EVRSettingsError::VRSettingsError_None;
    int int_value = vr::VRSettings()->GetInt32(settings_key_.c_str(), key.c_str(), &err);
    if (err == vr::EVRSettingsError::VRSettingsError_None) {
        return int_value;
    }
    err = vr::EVRSettingsError::VRSettingsError_None;
    float float_value = vr::VRSettings()->GetFloat(settings_key_.c_str(), key.c_str(), &err);
    if (err == vr::EVRSettingsError::VRSettingsError_None) {
        return float_value;
    }
    err = vr::EVRSettingsError::VRSettingsError_None;
    bool bool_value = vr::VRSettings()->GetBool(settings_key_.c_str(), key.c_str(), &err);
    if (err == vr::EVRSettingsError::VRSettingsError_None) {
        return bool_value;
    }
    std::string str_value;
    str_value.reserve(1024);
    vr::VRSettings()->GetString(settings_key_.c_str(), key.c_str(), str_value.data(), 1024, &err);
    if (err == vr::EVRSettingsError::VRSettingsError_None) {
        return str_value;
    }
    err = vr::EVRSettingsError::VRSettingsError_None;

    return SettingsValue();
}

void ptscDriver::VRDriver::Log(std::string message)
{
    std::string message_endl = message + "\n";
    vr::VRDriverLog()->Log(message_endl.c_str());
}

vr::IVRDriverInput* ptscDriver::VRDriver::GetInput()
{
    return vr::VRDriverInput();
}

vr::CVRPropertyHelpers* ptscDriver::VRDriver::GetProperties()
{
    return vr::VRProperties();
}

vr::IVRServerDriverHost* ptscDriver::VRDriver::GetDriverHost()
{
    return vr::VRServerDriverHost();
}

void ptscDriver::VRDriver::PipeThread()
{
    Log("Reading data from pipe...");

    // The read operation will block until there is data to read
    char buffer[1024];
    DWORD numBytesRead = 0;
    while (true)
    {
        BOOL result = ReadFile(
            pipe,
            buffer,             // the data from the pipe will be put here
            511 * sizeof(char), // number of bytes allocated
            &numBytesRead,      // this will store number of bytes actually read
            NULL                // not using overlapped IO
        );

        if (result)
        {
            buffer[numBytesRead / sizeof(char)] = '\0'; // null terminate the string
            
            poseData.add_data(buffer);

        }
        else
        {
            break;
        }
    }
    // Close our pipe handle
    CloseHandle(pipe);

    Log("Closing pipe PoseVR.");
}
