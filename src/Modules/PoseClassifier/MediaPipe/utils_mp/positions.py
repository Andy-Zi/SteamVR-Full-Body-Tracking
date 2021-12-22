from typing import Optional,List,Dict
import json
import mediapipe as mp
from utils.positions_dataclass import Positions
from dataclasses import fields


class PositionHandler:
    """ use keys from Positions-Class to extract positions from classification

    Returns:
        [Postisions]: [Dataclass with elemets as landmarks ans values of type List[float]]
    """
    defaultPosition: List[float] = [.0, .0, .0]
    position_visible_threshold: float = 0.5
    positions:Dict[str,List[float]] = {field.name:[] for field in fields(Positions())}
    previous_positions: Positions = Positions()
    current_positions: Positions = Positions()
    

    def __init__(self, ignore_hidden_points: Optional[bool] = False, outputfile: str = "/Users/macbook/Documents/KI_Master/AR-VR/arvr-projekt-modularbeit/src/Modules/PoseClassifier/landmarks.txt", inputfile: str = "/Users/macbook/Documents/KI_Master/AR-VR/arvr-projekt-modularbeit/src/Modules/PoseClassifier/keypoints.txt"):
        # settings
        self.use_visibility_threshold = ignore_hidden_points
        if self.use_visibility_threshold:
            print("set small values to 0")
        self.input_file = inputfile
        self.export_file = outputfile

        # keep track of not visible points (error message in VR: "body part is not visible from camera")
        self.not_visible_names: List[str] = []

        self.mp_pose = mp.solutions.pose

    def manage_points(self, results) -> Positions:
        """loads keypoint names from dataclass Positions and fills them with values"""
        self.last_positions = self.current_positions
        landmarks = self._load_positions(results=results)

        self._calc_landmarks(landmarks)
        if self.positions:
            self.current_positions = Positions(**self.positions)
            return self.current_positions

    def _load_positions(self, results):
        """ extracts the landmarks from the result"""
        positions: ict[str,List[float]] = {}
        for field in fields(Positions):
            positions[field.name] =  results.pose_world_landmarks.landmark[self.mp_pose.PoseLandmark[field.name]]
        return positions

    def _set_name_warning(self, name: str):
        """keep track of landmarks that cause trouble"""
        self.not_visible_names.append(name)
        print(f"{name=} caused an AttributeError \n")

    def _calc_landmarks(self, landmarks):
        """Listify positions

        Returns:
            [List]: [List of Lists of self.current_position [x,y,z]]
        """

        # werte von der Sichtbarkeit abhängig:
        # position.visibility<self.position_visibility_threshold setzt einen default wert
        
        if not landmarks:
            return
        
        for key, value in landmarks.items():
            if value:
                if self.use_visibility_threshold and value.visibility < self.position_visible_threshold:
                    self.positions[key] = self.defaultPosition
                else:
                    self.positions[key] = [value.x, value.y, value.z, value.visibility]
            else:
                continue
            

    def write_file(self):
        with open(self.export_file, "w+") as f:
            json.dump(self.current_positions, f, indent=4)

    def _read_point_names(self):
        """
        read landmark names to keep track of
        """

        with open(self.input_file, "r") as f:
            lines = f.read().split("\n")
            return lines
