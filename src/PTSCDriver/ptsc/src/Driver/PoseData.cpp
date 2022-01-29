#include "PoseData.hpp"
#include <cmath>

ptscDriver::PoseData::PoseData()
{
}

void ptscDriver::PoseData::add_data(const std::string& s)
{
	// parses buffer and adds to cur_positions
	bool cur_positions_empty = cur_positions.empty();
	auto prev_positions = cur_positions;	//testing smoothing
	cur_positions.clear();
	std::string delim = ";";

	bool time_parsed = false;
	std::string cur_position = "";
	int cur_dimension = 7;
	auto start = 0U;
	auto end = s.find(delim);
	while (end != std::string::npos)
	{
		std::string cur = s.substr(start, end - start);
		if (cur_dimension == 7)
		{
			cur_position = cur;
			cur_positions[cur_position] = std::vector<double>();
			cur_dimension = 0;
		}
		else
		{
			if (cur_positions_empty)
				cur_positions[cur_position].push_back(std::stod(cur));
			else
			{
				cur_positions[cur_position].push_back(std::stod(cur));
			}

			cur_dimension += 1;
		}
		start = end + delim.length();
		end = s.find(delim, start);
	}
}

bool ptscDriver::PoseData::contains(const std::string& position)
{
	return !(cur_positions.find(position) == cur_positions.end());
}

std::vector<double> ptscDriver::PoseData::get_position_vector(const std::string& position)
{
	std::vector<double> v;
	try
	{
		if (!contains(position))
		{
			v = { 0, 0, 0, 0, 0, 0, 0 };
			return v;
		}
		if (cur_positions[position].size() == 7)
			return cur_positions[position];
	}
	catch (...)
	{
		
	}
	v = { 0, 0, 0, 0, 0, 0, 0 };
	return v;
}

std::vector<double> ptscDriver::PoseData::get_desired_tracker_position(const std::string& tracker_name)
{
	// tracker_name: waist, left_foot, right_foot
	std::vector<double> v;
	if (cur_positions.empty())
	{
		v = { 0, 0, 0, 0, 0, 0, 0 };
		return v;
	}
	if (tracker_name == "waist")
	{
		v = get_position_vector("waist");
	}
	else if (tracker_name == "left_foot")
	{
		v = get_position_vector("left_foot");
	}
	else if (tracker_name == "right_foot")
	{
		v = get_position_vector("right_foot");
	}
	return v;
}
