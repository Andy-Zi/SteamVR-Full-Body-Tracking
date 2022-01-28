#pragma once

#include <string>
#include <unordered_map>
#include <vector>

namespace ptscDriver {
	class PoseData {
	public:
		// parses buffer string into time and positions
		PoseData();

		void add_data(const std::string& buffer);
		bool contains(const std::string& position);

		std::vector<double> get_position_vector(const std::string& position);
		std::vector<double> get_desired_tracker_position(const std::string& tracker_name); // implement later

	public:


		// current positions
		std::unordered_map<std::string, std::vector<double>> cur_positions; // hashmap key(position) : value(vector xyz)
	};
}