#pragma once

#include <cstdlib>
#include <memory>

#include <openvr_driver.h>

#include <IVRDriver.h>

extern "C" __declspec(dllexport) void* HmdDriverFactory(const char* interface_name, int* return_code);

namespace PoseVRDriver {
    std::shared_ptr<PoseVRDriver::IVRDriver> GetDriver();
}