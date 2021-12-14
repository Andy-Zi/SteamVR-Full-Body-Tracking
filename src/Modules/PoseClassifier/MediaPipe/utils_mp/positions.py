#from typing import dataclass
import json
from operator import pos
import mediapipe as mp


class Positions:

    def __init__(self, outputfile: str = "../landmarks.txt", inputfile: str = "../keypoints.txt"):

        self.input_file = inputfile

        self.export_file = outputfile
        self.keys = self._read_point_names()
        self.points = {key: None for key in self.keys}
        self.previous_positions = self.points
        self.mp_pose = mp.solutions.pose

    def manage_points(self, results):

        positions = self._calc_positions(results=results)

        print(f"{positions=}")
        if positions == []:
            return

        landmarks = self._calc_landmarks(positions=positions)

        print(f"{landmarks=}")
        self._prepare_data(landmarks=landmarks)
        self.write_file()

    def _calc_positions(self, results):

        positions = []

        for ind, name in enumerate(self.keys):

            try:
                position = results.pose_landmarks.landmark[self.mp_pose.PoseLandmark[name.upper(
                )]]
            except AttributeError:
                # single missing value
                if len(self.previous_positions) > ind and self.previous_positions:
                    position = self.previous_positions[name]

            finally:
                positions.append(position)

        assert len(positions) == len(self.keys)
        self.previous_positions = positions
        return positions

    def _calc_landmarks(self, positions):
        """listify positions

        Returns:
            [list]: [list of lists of points [x,y,z]]
        """
        # TODO:
        # Man könnte die werte von der Sichtbarkeit abhängig machen:
        # position.visibility und einen default wert benutzen
        if not positions:
            return

        points = []
        for position in positions:
            if position:
                points.append([position.x, position.y, position.z])
            else:
                continue
        return points

    def _prepare_data(self, landmarks: list):
        """fills in points for keys

        Args:
            landmarks ([list]): [list of points: x,y,z]
        """
        for ind, key in enumerate(self.keys):
            if landmarks != []:
                self.points[key] = landmarks[ind]

    def write_file(self):
        with open(self.export_file, "w+") as f:
            json.dump(self.points, f, indent=4)

    def _read_point_names(self):
        """
        read landmark names to keep track of
        """

        with open(self.input_file, "r") as f:
            lines = f.read().split("\n")
            return lines
