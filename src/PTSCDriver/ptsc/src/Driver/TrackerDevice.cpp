#include "TrackerDevice.hpp"
#include <Windows.h>

ptscDriver::TrackerDevice::TrackerDevice(std::string serial):
    serial_(serial)
{
}

std::string ptscDriver::TrackerDevice::GetSerial()
{
    return this->serial_;
}

void ptscDriver::TrackerDevice::Update()
{
    if (this->device_index_ == vr::k_unTrackedDeviceIndexInvalid)
        return;

    // Check if this device was asked to be identified
    auto events = GetDriver()->GetOpenVREvents();
    for (auto event : events) {
        // Note here, event.trackedDeviceIndex does not necissarily equal this->device_index_, not sure why, but the component handle will match so we can just use that instead
        //if (event.trackedDeviceIndex == this->device_index_) {
        if (event.eventType == vr::EVREventType::VREvent_Input_HapticVibration) {
            if (event.data.hapticVibration.componentHandle == this->haptic_component_) {
                this->did_vibrate_ = true;
            }
        }
        //}
    }

    // Check if we need to keep vibrating
    if (this->did_vibrate_) {
        this->vibrate_anim_state_ += (GetDriver()->GetLastFrameTime().count()/1000.f);
        if (this->vibrate_anim_state_ > 1.0f) {
            this->did_vibrate_ = false;
            this->vibrate_anim_state_ = 0.0f;
        }
    }

    // Setup pose for this frame
    auto pose = IVRDevice::MakeDefaultPose();

    // Find a HMD
    //auto devices = GetDriver()->GetDevices();
    //auto hmd = std::find_if(devices.begin(), devices.end(), [](const std::shared_ptr<IVRDevice>& device_ptr) {return device_ptr->GetDeviceType() == DeviceType::HMD; });
    //if (hmd != devices.end()) {
        // Found a HMD
        //vr::DriverPose_t hmd_pose = (*hmd)->GetPose();
        vr::TrackedDevicePose_t hmd_pose;
        vr::VRServerDriverHost()->GetRawTrackedDevicePoses(0, &hmd_pose, 1);

        vr::HmdQuaternion_t q;

         q.w = sqrt(fmax(0, 1 + hmd_pose.mDeviceToAbsoluteTracking.m[0][0] + hmd_pose.mDeviceToAbsoluteTracking.m[1][1] + hmd_pose.mDeviceToAbsoluteTracking.m[2][2])) / 2;
        q.x = sqrt(fmax(0, 1 + hmd_pose.mDeviceToAbsoluteTracking.m[0][0] - hmd_pose.mDeviceToAbsoluteTracking.m[1][1] - hmd_pose.mDeviceToAbsoluteTracking.m[2][2])) / 2;
        q.y = sqrt(fmax(0, 1 - hmd_pose.mDeviceToAbsoluteTracking.m[0][0] + hmd_pose.mDeviceToAbsoluteTracking.m[1][1] - hmd_pose.mDeviceToAbsoluteTracking.m[2][2])) / 2;
        q.z = sqrt(fmax(0, 1 - hmd_pose.mDeviceToAbsoluteTracking.m[0][0] - hmd_pose.mDeviceToAbsoluteTracking.m[1][1] + hmd_pose.mDeviceToAbsoluteTracking.m[2][2])) / 2;
        q.x = copysign(q.x, hmd_pose.mDeviceToAbsoluteTracking.m[2][1] - hmd_pose.mDeviceToAbsoluteTracking.m[1][2]);
        q.y = copysign(q.y, hmd_pose.mDeviceToAbsoluteTracking.m[0][2] - hmd_pose.mDeviceToAbsoluteTracking.m[2][0]);
        q.z = copysign(q.z, hmd_pose.mDeviceToAbsoluteTracking.m[1][0] - hmd_pose.mDeviceToAbsoluteTracking.m[0][1]);



        vr::HmdVector3_t vector;

        vector.v[0] = hmd_pose.mDeviceToAbsoluteTracking.m[0][3];
        vector.v[1] = hmd_pose.mDeviceToAbsoluteTracking.m[1][3];
        vector.v[2] = hmd_pose.mDeviceToAbsoluteTracking.m[2][3];

        // Here we setup some transforms so our controllers are offset from the headset by a small amount so we can see them
        linalg::vec<float, 3> hmd_position{ (float)vector.v[0], (float)vector.v[1], (float)vector.v[2]};
        linalg::vec<float, 4> hmd_rotation{ (float)q.x, (float)q.y, (float)q.z, (float)q.w };

        // Do shaking animation if haptic vibration was requested
        float controller_y = -0.35f + 0.01f * std::sinf(8 * 3.1415f * vibrate_anim_state_);

        linalg::vec<float, 3> hmd_pose_offset = { 0.f, controller_y, -0.5f };

        hmd_pose_offset = linalg::qrot(hmd_rotation, hmd_pose_offset);

        linalg::vec<float, 3> final_pose = hmd_pose_offset + hmd_position;

        pose.vecPosition[0] = final_pose.x;
        pose.vecPosition[1] = final_pose.y;
        pose.vecPosition[2] = final_pose.z;

        pose.qRotation.w = hmd_rotation.w;
        pose.qRotation.x = hmd_rotation.x;
        pose.qRotation.y = hmd_rotation.y;
        pose.qRotation.z = hmd_rotation.z;
    //}
    // Post pose
    GetDriver()->GetDriverHost()->TrackedDevicePoseUpdated(this->device_index_, pose, sizeof(vr::DriverPose_t));
    this->last_pose_ = pose;
}

DeviceType ptscDriver::TrackerDevice::GetDeviceType()
{
    return DeviceType::TRACKER;
}

vr::TrackedDeviceIndex_t ptscDriver::TrackerDevice::GetDeviceIndex()
{
    return this->device_index_;
}

vr::EVRInitError ptscDriver::TrackerDevice::Activate(uint32_t unObjectId)
{
    this->device_index_ = unObjectId;

    GetDriver()->Log("Activating tracker " + this->serial_);

    // Get the properties handle
    auto props = GetDriver()->GetProperties()->TrackedDeviceToPropertyContainer(this->device_index_);

    // Setup inputs and outputs
    GetDriver()->GetInput()->CreateHapticComponent(props, "/output/haptic", &this->haptic_component_);

    GetDriver()->GetInput()->CreateBooleanComponent(props, "/input/system/click", &this->system_click_component_);
    GetDriver()->GetInput()->CreateBooleanComponent(props, "/input/system/touch", &this->system_touch_component_);

    // Set some universe ID (Must be 2 or higher)
    GetDriver()->GetProperties()->SetUint64Property(props, vr::Prop_CurrentUniverseId_Uint64, 2);
    
    // Set up a model "number" (not needed but good to have)
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_ModelNumber_String, "ptsc_tracker");

    // Opt out of hand selection
    GetDriver()->GetProperties()->SetInt32Property(props, vr::Prop_ControllerRoleHint_Int32, vr::ETrackedControllerRole::TrackedControllerRole_OptOut);

    // Set up a render model path
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_RenderModelName_String, "vr_controller_05_wireless_b");

    // Set controller profile
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_InputProfilePath_String, "{ptsc}/input/ptsc_tracker_bindings.json");

    // Set the icon
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceReady_String, "{ptsc}/icons/tracker_ready.png");

    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceOff_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceSearching_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceSearchingAlert_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceReadyAlert_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceNotReady_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceStandby_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceAlertLow_String, "{ptsc}/icons/tracker_not_ready.png");

    return vr::EVRInitError::VRInitError_None;
}

void ptscDriver::TrackerDevice::Deactivate()
{
    this->device_index_ = vr::k_unTrackedDeviceIndexInvalid;
}

void ptscDriver::TrackerDevice::EnterStandby()
{
}

void* ptscDriver::TrackerDevice::GetComponent(const char* pchComponentNameAndVersion)
{
    return nullptr;
}

void ptscDriver::TrackerDevice::DebugRequest(const char* pchRequest, char* pchResponseBuffer, uint32_t unResponseBufferSize)
{
    if (unResponseBufferSize >= 1)
        pchResponseBuffer[0] = 0;
}

vr::DriverPose_t ptscDriver::TrackerDevice::GetPose()
{
    return last_pose_;
}
