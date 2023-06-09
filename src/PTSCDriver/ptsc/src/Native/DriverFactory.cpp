#include "DriverFactory.hpp"
#include <thread>
#include <Driver/VRDriver.hpp>
#include <Windows.h>
#include <sstream>

static std::shared_ptr<ptscDriver::IVRDriver> driver;

void* HmdDriverFactory(const char* interface_name, int* return_code) {
	if (std::strcmp(interface_name, vr::IServerTrackedDeviceProvider_Version) == 0) {
		if (!driver) {
			driver = std::make_shared<ptscDriver::VRDriver>();
		}
		return driver.get();
	}

	if (return_code)
		*return_code = vr::VRInitError_Init_InterfaceNotFound;

	return nullptr;
}

std::shared_ptr<ptscDriver::IVRDriver> ptscDriver::GetDriver() {
	return driver;
}
