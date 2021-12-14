#include "VRDriver.hpp"
#include <Driver/HMDDevice.hpp>
#include <Driver/TrackerDevice.hpp>
#include <Driver/ControllerDevice.hpp>
#include <Driver/TrackingReferenceDevice.hpp>


vr::EVRInitError ptscDriver::VRDriver::Init(vr::IVRDriverContext* pDriverContext)
{
    // Perform driver context initialisation
    if (vr::EVRInitError init_error = vr::InitServerDriverContext(pDriverContext); init_error != vr::EVRInitError::VRInitError_None) {
        return init_error;
    }

    Log("Activating ptscDriver...");

    Log("Connecting to pipe...");
    LPTSTR lpszPipename = (LPTSTR)(L"\\\\.\\pipe\\posevr_pipe");

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

    // Add a HMD
    //this->AddDevice(std::make_shared<HMDDevice>("ptsc_HMDDevice"));

    // Add a couple controllers
    //this->AddDevice(std::make_shared<ControllerDevice>("ptsc_ControllerDevice_Left", ControllerDevice::Handedness::LEFT));
    //this->AddDevice(std::make_shared<ControllerDevice>("ptsc_ControllerDevice_Right", ControllerDevice::Handedness::RIGHT));

    // Add a tracker
    this->AddDevice(std::make_shared<TrackerDevice>("waist_TrackerDevice"));
    this->AddDevice(std::make_shared<TrackerDevice>("leftfoot_TrackerDevice"));
    this->AddDevice(std::make_shared<TrackerDevice>("rightfoot_TrackerDevice"));

    // Add a couple tracking references
    //this->AddDevice(std::make_shared<TrackingReferenceDevice>("ptsc_TrackingReference_A"));
    //this->AddDevice(std::make_shared<TrackingReferenceDevice>("ptsc_TrackingReference_B"));

    Log("ptscDriver Loaded Successfully");

	return vr::VRInitError_None;
}

void ptscDriver::VRDriver::Cleanup()
{
}

void ptscDriver::VRDriver::RunFrame()
{
    // Collect events
    vr::VREvent_t event;
    std::vector<vr::VREvent_t> events;
    while (vr::VRServerDriverHost()->PollNextEvent(&event, sizeof(event)))
    {
        events.push_back(event);
    }
    this->openvr_events_ = events;

    // Update frame timing
    std::chrono::system_clock::time_point now = std::chrono::system_clock::now();
    this->frame_timing_ = std::chrono::duration_cast<std::chrono::milliseconds>(now - this->last_frame_time_);
    this->last_frame_time_ = now;

    // Update devices
    for (auto& device : this->devices_)
        device->Update();
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

bool ptscDriver::VRDriver::AddDevice(std::shared_ptr<IVRDevice> device)
{
    vr::ETrackedDeviceClass openvr_device_class;
    // Remember to update this switch when new device types are added
    switch (device->GetDeviceType()) {
        case DeviceType::CONTROLLER:
            openvr_device_class = vr::ETrackedDeviceClass::TrackedDeviceClass_Controller;
            break;
        case DeviceType::HMD:
            openvr_device_class = vr::ETrackedDeviceClass::TrackedDeviceClass_HMD;
            break;
        case DeviceType::TRACKER:
            openvr_device_class = vr::ETrackedDeviceClass::TrackedDeviceClass_GenericTracker;
            break;
        case DeviceType::TRACKING_REFERENCE:
            openvr_device_class = vr::ETrackedDeviceClass::TrackedDeviceClass_TrackingReference;
            break;
        default:
            return false;
    }
    bool result = vr::VRServerDriverHost()->TrackedDeviceAdded(device->GetSerial().c_str(), openvr_device_class, device.get());
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
    char buffer[512];
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

           //TODO PIPE DATA nutzen
    }
    // Close our pipe handle
    CloseHandle(pipe);

    Log("Closing pipe PoseVR.");

    //system("pause");
}