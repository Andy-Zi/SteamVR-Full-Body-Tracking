#from typing import dataclass
import json
import mediapipe as mp
import numpy as np


class Positions:
    defaultPosition = [0, 0, 0]
    position_visible_threshold: float = 0.5

    def __init__(self, outputfile: str = "src/Modules/PoseClassifier/landmarks.txt", inputfile: str = "/Users/macbook/Documents/KI_Master/AR-VR/arvr-projekt-modularbeit/src/Modules/PoseClassifier/keypoints.txt"):
        self.use_visibility_threshold = False # don't set values to 0 if not visible
        self.input_file = inputfile
        self.export_file = outputfile
        self.keys = self._read_point_names()
        self.points = {key: [] for key in self.keys}
        self.num_landmarks = len(self.keys)
        self.previous_position = None#[[] for _ in range(self.num_landmarks)]
        self.mp_pose = mp.solutions.pose
        self.current_position = list()#None#self.previous_position

    def manage_points(self, results):

        positions = self._load_positions(results=results)
        # print(f"{positions=}")
        if positions == []:
            return
        landmarks = self._calc_landmarks(positions=positions)
        # print(f"{landmarks=}")
        self._prepare_data(landmarks=landmarks)
        self.write_file()

    def _load_positions(self, results):
        """ extracts the landmarks from the result"""
        positions = []
        position = None
        
        for ind, name in enumerate(self.keys):

            try:
                position = results.pose_world_landmarks.landmark[self.mp_pose.PoseLandmark[name.upper(
                )]]
            except AttributeError:
                # single missing value
                if len(self.previous_positions) > ind and self.previous_positions:
                    position = self.previous_positions[name]

            finally:
                positions.append(position)

        assert len(positions) == len(self.keys)
        #self.previous_positions = self.current
        return positions

    def _calc_landmarks(self, positions):
        """listify positions

        Returns:
            [list]: [list of lists of self.current_position [x,y,z]]
        """
        
        # werte von der Sichtbarkeit abhängig:
        # position.visibility<self.position_visibility_threshold setzt einen default wert 
        if not positions:
            return
        points = list()
        for i,position in enumerate(positions):
            if position:
                if self.use_visibility_threshold and position.visibility < self.position_visible_threshold:
                    points.append(self.defaultPosition)
                else: points.append([position.x, position.y, position.z])#, self.dist(position,index=i)])
            else:
                continue
        self.previous_position = points
        return points
    
    def dist(self,position,index)->list:
        """ calculate the distance of movement for each landmark"""
        if not self.previous_position:
            return 0
        p1 = np.array([position.x, position.y, position.z])
        p2 = np.array([self.previous_position[index][:3]])

        dist = np.linalg.norm(p1 - p2)
        print(dist)
        return dist

    def _prepare_data(self, landmarks: list):
        """fills in points for keys

        Args:
            landmarks ([list]): [list of points: x,y,z]
        """
        for ind, key in enumerate(self.keys):
            if landmarks:
                #print(ind,key)
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
