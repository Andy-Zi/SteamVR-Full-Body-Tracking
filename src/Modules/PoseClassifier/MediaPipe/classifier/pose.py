import mediapipe as mp
import cv2
from ..utils_mp.positions import PositionHandler
import numpy as np
from typing import Union, Optional


class PoseMP:

    def __init__(self, default_value:bool = False, options: Optional[dict[str,Union[str,float,bool]]] = None):
        self.points = PositionHandler(ignore_hidden_points=default_value)

        if options is not None:
            estimator_options = options
        else:
            estimator_options = {"static_image_mode": True,
                                 "min_detection_confidence": 0.5,
                                 "min_tracking_confidence": 0.5,
                                 "enable_segmentation": False,
                                 "smooth_landmarks": False,
                                 "smooth_segmentation": False,
                                 "model_complexity": 1}

        # Pose Model
        self.mp_pose = mp.solutions.pose.Pose(estimator_options)

    def classify_image(self, image: np.ndarray, image_id: str = "000"):
        
        pose = self._get_pose(image)

        if pose is not None and pose.pose_world_landmarks is not None:
            
            result = self.points.manage_points(pose)
            #self.points.previous_positions = self.points.points
            return result
        else:
            return None

    def _get_pose(self, image: np.ndarray):
        """Uses mediapipe to detect pose on image and retrun a Pose object"""
        return self.mp_pose.process(cv2.cvtColor(image, cv2.COLOR_BGR2RGB))
