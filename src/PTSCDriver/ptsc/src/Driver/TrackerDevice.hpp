#pragma once

#include <chrono>
#include <cmath>

#include <linalg.h>

#include <Driver/IVRDevice.hpp>
#include <Native/DriverFactory.hpp>
#include "TrackerRole.hpp"

#include <windows.h>
#include <thread>
#include <string>
#include <mutex>
#include <algorithm>

namespace ptscDriver {
    class TrackerDevice : public IVRDevice {
        public:

            TrackerDevice(std::string serial, int deviceId, TrackerRole trackerRole);
            ~TrackerDevice() = default;

            // Inherited via IVRDevice
            virtual std::string GetSerial() override;
            virtual void Update() override;
            virtual vr::TrackedDeviceIndex_t GetDeviceIndex() override;
            virtual DeviceType GetDeviceType() override;

            virtual vr::EVRInitError Activate(uint32_t unObjectId) override;
            virtual void Deactivate() override;
            virtual void EnterStandby() override;
            virtual void* GetComponent(const char* pchComponentNameAndVersion) override;
            virtual void DebugRequest(const char* pchRequest, char* pchResponseBuffer, uint32_t unResponseBufferSize) override;
            virtual vr::DriverPose_t GetPose() override;

            // PoseVR methods
            void UpdatePos(double x, double y, double z);
            void UpdateRot(double w, double x, double y, double z);

            vr::DriverPose_t wanted_pose_ = IVRDevice::MakeDefaultPose();

    private:
        vr::TrackedDeviceIndex_t device_index_ = vr::k_unTrackedDeviceIndexInvalid;
        std::string serial_;
        int deviceId_;
        TrackerRole trackerRole;

        vr::DriverPose_t last_pose_ = IVRDevice::MakeDefaultPose();

        bool did_vibrate_ = false;
        float vibrate_anim_state_ = 0.f;

        vr::VRInputComponentHandle_t haptic_component_ = 0;

        vr::VRInputComponentHandle_t system_click_component_ = 0;
        vr::VRInputComponentHandle_t system_touch_component_ = 0;

        virtual vr::HmdQuaternion_t get_HMD_rotation(vr::TrackedDevicePose_t hmd_pose);
        virtual vr::HmdVector3_t get_HMD_absolute_position(vr::TrackedDevicePose_t hmd_pose);
    };
};