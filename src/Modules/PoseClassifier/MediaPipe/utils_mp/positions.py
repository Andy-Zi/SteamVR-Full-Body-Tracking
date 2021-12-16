from typing import Optional
import json
import mediapipe as mp
import numpy as np


class Positions:
    defaultPosition: list[float] = [.0, .0, .0]
    position_visible_threshold: float = 0.5

    def __init__(self, ignore_hidden_points:Optional[bool]=False, outputfile: str = "/Users/macbook/Documents/KI_Master/AR-VR/arvr-projekt-modularbeit/src/Modules/PoseClassifier/landmarks.txt", inputfile: str = "/Users/macbook/Documents/KI_Master/AR-VR/arvr-projekt-modularbeit/src/Modules/PoseClassifier/keypoints.txt"):
        # settings
        self.use_visibility_threshold = ignore_hidden_points   
        self.input_file = inputfile
        self.export_file = outputfile
        self.keys = self._read_point_names()

        # helping vars
        self.not_visible_names: list[str] = []
        self.points: dict[str, list[float]] = {key: [] for key in self.keys}
        self.num_landmarks = len(self.keys)

        self.previous_positions: list[list[float]] = []
        self.mp_pose = mp.solutions.pose
        self.current_positions: list[str] = []

    def manage_points(self, results)-> dict[str, list[float]]:
        """loads keypoint names from keypoints.txt and fills them with values"""

        positions = self._load_positions(results=results)

        if positions == []:
            return
        landmarks = self._calc_landmarks(positions=positions)
        
        self._prepare_data(landmarks=landmarks)
        
        return self.points
        # self.write_file()

    def _load_positions(self, results):
        """ extracts the landmarks from the result"""
        positions: list[list[float]] = []
        position: list[float] = []
        #PoseLandmarks = [str(x).split(".")[1].lower() for x in self.mp_pose.PoseLandmark]
        #print(f"{PoseLandmarks}")
        for ind, name in enumerate(self.keys):
            try:
                position = results.pose_world_landmarks.landmark[self.mp_pose.PoseLandmark[name.upper(
                )]]
            except AttributeError:
                # fill in single missing value with last position
                if len(self.previous_positions) > ind and self.previous_positions:
                    position = self.previous_positions[ind]
                    self._set_name_warning(name)

            finally:
                positions.append(position)

        assert len(positions) == len(self.keys)
        
        return positions

    def _set_name_warning(self, name: str):
        """keep track of landmarks that cause trouble"""
        self.not_visible_names.append(name)
        print(f"{name=} caused an AttributeError \n")

    def _calc_landmarks(self, positions: list[float]):
        """listify positions

        Returns:
            [list]: [list of lists of self.current_position [x,y,z]]
        """

        # werte von der Sichtbarkeit abhängig:
        # position.visibility<self.position_visibility_threshold setzt einen default wert
        if not positions:
            return
        landmarks: list[list[float]] = []
        for i, position in enumerate(positions):
            if position:
                if self.use_visibility_threshold and position.visibility < self.position_visible_threshold:
                    landmarks.append(self.defaultPosition)
                else:
                    # , self.dist(position,index=i)])
                    landmarks.append([position.x, position.y, position.z])
            else:
                continue
        self.previous_position = landmarks
        return landmarks



    def _prepare_data(self, landmarks: list[list[float]]) -> dict[str,Optional[list[float]]]:
        """fills in points for keys

        Args:
            landmarks (list[float]): [list of points: x,y,z]
        """
        for ind, key in enumerate(self.keys):
            if landmarks:
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
