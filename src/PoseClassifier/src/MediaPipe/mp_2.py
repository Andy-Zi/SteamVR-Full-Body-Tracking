import mediapipe as mp
import cv2
import time
from utils_mp.positions import Positions
import numpy as np


class PoseMP:

    def __init__(self, options: dict = None):
        self.points = Positions()

        if options is not None:
            estimator_options = options
        else:
            estimator_options = {"static_image_mode": True,
                                 "min_detection_confidence": 0.5,
                                 "min_tracking_confidence": 0.5,
                                 "enable_segmentation": False,
                                 "smooth_landmarks": False,
                                 "smooth_segmentation": False,
                                 "model_complexity": 2}

        # Pose Model
        self.mp_pose = mp.solutions.pose.Pose(estimator_options)

    def classify_image(self, image: np.ndarray, image_id: str = "000"):
        
        pose = self._get_pose(image)

        if pose is not None and pose.pose_world_landmarks is not None:
            
            self.points.manage_points(pose)
            #self.points.previous_positions = self.points.points
            return pose
        else:
            return None

    def _get_pose(self, image: np.ndarray):
        return self.mp_pose.process(cv2.cvtColor(image, cv2.COLOR_BGR2RGB))


def main():
    runs = 100
    pose = PoseMP()

    mp_drawing = mp.solutions.drawing_utils
    mp_drawing_styles = mp.solutions.drawing_styles
    mp_pose = mp.solutions.pose

    import cv2

    cap = cv2.VideoCapture(0)
    count = 0

    while cap.isOpened():
        runs -= 1
        success, image = cap.read()

        if not success:
            # ignore empty frames
            continue

        image.flags.writeable = False
        image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        start = time.time()
        results = pose.classify_image(image, image_id=count)
        end = time.time()
        print(f"FPS: {1/(end-start)}")
        if results is None:
            continue
        if results.pose_landmarks is not None:
            image.flags.writeable = True
            image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
            mp_drawing.draw_landmarks(
                image,
                results.pose_landmarks,
                mp_pose.POSE_CONNECTIONS,
                landmark_drawing_spec=mp_drawing_styles.get_default_pose_landmarks_style())
            # Flip the image horizontally for a selfie-view display.

            cv2.imshow('MediaPipe Pose', cv2.flip(image, 1))
            if cv2.waitKey(5) & 0xFF == 27:
                return
        if runs < 10:
            return


if __name__ == "__main__":
    main()
