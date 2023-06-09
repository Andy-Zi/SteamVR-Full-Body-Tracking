#include "TrackerDevice.hpp"
#include <Windows.h>

ptscDriver::TrackerDevice::TrackerDevice(std::string serial, int deviceId, TrackerRole trackerRole_) :
    serial_(serial), trackerRole(trackerRole_), deviceId_(deviceId)
{
    this->last_pose_ = MakeDefaultPose();
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
    //auto pose = IVRDevice::MakeDefaultPose();

    vr::TrackedDevicePose_t hmd_pose;
    vr::VRServerDriverHost()->GetRawTrackedDevicePoses(0, &hmd_pose, 1);
    vr::HmdQuaternion_t q = get_HMD_rotation(hmd_pose);
    vr::HmdVector3_t vector = get_HMD_absolute_position(hmd_pose);

    // Setup pose for this frame
    vr::DriverPose_t pose = this->wanted_pose_;
    pose.vecPosition[0] = pose.vecPosition[0] + vector.v[0];
    pose.vecPosition[1] = pose.vecPosition[1] + vector.v[1];
    pose.vecPosition[2] = pose.vecPosition[2] + vector.v[2];

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

    // Set some universe ID (Must be 2 or higher)
    GetDriver()->GetProperties()->SetUint64Property(props, vr::Prop_CurrentUniverseId_Uint64, 4);

    // Set up a model "number" (not needed but good to have)
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_ModelNumber_String, "ptsc Virtual Tracker");

    // Opt out of hand selection
    GetDriver()->GetProperties()->SetInt32Property(props, vr::Prop_ControllerRoleHint_Int32, vr::ETrackedControllerRole::TrackedControllerRole_OptOut);
    vr::VRProperties()->SetInt32Property(props, vr::Prop_DeviceClass_Int32, vr::TrackedDeviceClass_GenericTracker);
    vr::VRProperties()->SetInt32Property(props, vr::Prop_ControllerHandSelectionPriority_Int32, -1);

    // Set up a render model path
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_RenderModelName_String, "{htc}/rendermodels/vr_tracker_vive_1_0");

    // Set the icon
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceReady_String, "{ptsc}/icons/tracker_ready.png");

    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceOff_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceSearching_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceSearchingAlert_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceReadyAlert_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceNotReady_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceStandby_String, "{ptsc}/icons/tracker_not_ready.png");
    GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_NamedIconPathDeviceAlertLow_String, "{ptsc}/icons/tracker_not_ready.png");

    // Automatically select vive tracker roles and set hints for games that need it (Beat Saber avatar mod, for example)
    auto roleHint = getViveRoleHint(trackerRole);
    if (roleHint != "")
        GetDriver()->GetProperties()->SetStringProperty(props, vr::Prop_ControllerType_String, roleHint.c_str());

    auto role = getViveRole(trackerRole);
    if (role != "")
        vr::VRSettings()->SetString(vr::k_pch_Trackers_Section, ("/devices/ptsc/" + this->serial_).c_str(), role.c_str());

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

vr::HmdQuaternion_t ptscDriver::TrackerDevice::get_HMD_rotation(vr::TrackedDevicePose_t hmd_pose)
{
    vr::HmdQuaternion_t q;

    q.w = sqrt(fmax(0, 1 + hmd_pose.mDeviceToAbsoluteTracking.m[0][0] + hmd_pose.mDeviceToAbsoluteTracking.m[1][1] + hmd_pose.mDeviceToAbsoluteTracking.m[2][2])) / 2;
    q.x = sqrt(fmax(0, 1 + hmd_pose.mDeviceToAbsoluteTracking.m[0][0] - hmd_pose.mDeviceToAbsoluteTracking.m[1][1] - hmd_pose.mDeviceToAbsoluteTracking.m[2][2])) / 2;
    q.y = sqrt(fmax(0, 1 - hmd_pose.mDeviceToAbsoluteTracking.m[0][0] + hmd_pose.mDeviceToAbsoluteTracking.m[1][1] - hmd_pose.mDeviceToAbsoluteTracking.m[2][2])) / 2;
    q.z = sqrt(fmax(0, 1 - hmd_pose.mDeviceToAbsoluteTracking.m[0][0] - hmd_pose.mDeviceToAbsoluteTracking.m[1][1] + hmd_pose.mDeviceToAbsoluteTracking.m[2][2])) / 2;
    q.x = copysign(q.x, hmd_pose.mDeviceToAbsoluteTracking.m[2][1] - hmd_pose.mDeviceToAbsoluteTracking.m[1][2]);
    q.y = copysign(q.y, hmd_pose.mDeviceToAbsoluteTracking.m[0][2] - hmd_pose.mDeviceToAbsoluteTracking.m[2][0]);
    q.z = copysign(q.z, hmd_pose.mDeviceToAbsoluteTracking.m[1][0] - hmd_pose.mDeviceToAbsoluteTracking.m[0][1]);

    return q;
}

vr::HmdVector3_t ptscDriver::TrackerDevice::get_HMD_absolute_position(vr::TrackedDevicePose_t hmd_pose)
{
    vr::HmdVector3_t vector;

    vector.v[0] = hmd_pose.mDeviceToAbsoluteTracking.m[0][3];
    vector.v[1] = hmd_pose.mDeviceToAbsoluteTracking.m[1][3];
    vector.v[2] = hmd_pose.mDeviceToAbsoluteTracking.m[2][3];

    return vector;
}

// PoseVR methods
void ptscDriver::TrackerDevice::UpdatePos(double x, double y, double z)
{
    this->wanted_pose_.vecPosition[0] = x;//position_smoothing * x + (1 - position_smoothing) * this->wanted_pose_.vecPosition[0];  //can do some motion smoothing here?
    this->wanted_pose_.vecPosition[1] = y;//position_smoothing * y + (1 - position_smoothing) * this->wanted_pose_.vecPosition[1];
    this->wanted_pose_.vecPosition[2] = z;//position_smoothing * z + (1 - position_smoothing) * this->wanted_pose_.vecPosition[2];
}

void ptscDriver::TrackerDevice::UpdateRot(double w, double x, double y, double z)
{
    struct vr::HmdQuaternion_t q = { w, x, y, z };

    q.w = w;
    q.x = x;
    q.y = y;
    q.z = z;

    this->wanted_pose_.qRotation = q;   // probablty can do some smoothing/slerp stuff here
}