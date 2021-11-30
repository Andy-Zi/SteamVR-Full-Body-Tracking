
from functools import lru_cache
import mediapipe as mp
import cv2
import numpy as np
import math
import json
import time

class PoseMP:

    previous_positions: list[float]
    data: list
    image: np.ndarray
    missing_counter: int

    def __init__(self, visualize=False, static_image_mode=False, smooth_landmarks=True,
                 smooth_segmentation=True, min_detection_confidence=0.5,
                 min_tracking_confidence=0.5, enable_segmentation=False, model_complexity=2,
                 landmark_names=['left_shoulder', 'right_shoulder',
                                 'left_elbow', 'right_elbow',
                                 'left_wrist', 'right_wrist', 'left_hip', 'right_hip',
                                 'left_knee', 'right_knee',
                                 'left_ankle', 'right_ankle',
                                 'left_heel', 'right_heel',
                                 'left_foot_index', 'right_foot_index'
                                 ]):

        # mp.Pose settings
        # treat pics undependent from the past pic
        self.static_image_mode = static_image_mode
        self.smooth_segmentation = smooth_segmentation
        self.min_detection_confidence = min_detection_confidence
        self.min_tracking_confidence = min_tracking_confidence
        self.model_complexity = model_complexity
        self.enable_segmentation = enable_segmentation
        self.smooth_landmarks = smooth_landmarks

        self.landmark_names = landmark_names

        # visualizing
        self.mp_drawing = mp.solutions.drawing_utils
        self.mp_drawing_styles = mp.solutions.drawing_styles

        # Pose Model
        self.mp_pose = mp.solutions.pose

        # visualize
        self.visualize = visualize

        # initialize values
        self.missing_counter = 0
        self.previous_positions = []
        self.data = []
        self.results = None


    def classify_image(self, image: np.ndarray, image_id: str = "000", reshape: bool = False):
        self.image_id = image_id
        self.image = image
        self.image, h, w = self.resize(reshape)

        self.get_pose()
        self.prepare_data(w, h)
        self.dump_data()
        # if self.visualize:
        #     self.visualize_pose()

    #@lru_cache
    def prepare_data(self, width, height):

        # list world postions and fill in missing values
        
        positions = self.lisify_and_fill_missing_landmarks()

        if positions:
            landmarks = [[position.x, position.y, position.z]
                        for position in positions]
            names = [name.upper()for name in self.landmark_names]
            self.data = [{names[i]: landmark}
                        for i, landmark in enumerate(landmarks)]

        #print(len(self.data))
        #print(self.data)
        #self.data = [{name: self.results.pose_world_landmarks.landmark[mp_pose.PoseLandmark[name.upper()]]} for name in self.landmark_names]
        self.data.append({"image_id": self.image_id})
        self.data.append({"image_width": width})
        self.data.append({"image_height": height})


    def resize(self, reshape):
        DESIRED_HEIGHT = 480
        DESIRED_WIDTH = 480

        h, w = self.image.shape[:2]
        if not reshape:
            return self.image, h, w

        if h < w:
            img = cv2.resize(self.image, (DESIRED_WIDTH,
                             math.floor(h/(w/DESIRED_WIDTH))))
        else:
            img = cv2.resize(self.image, (math.floor(
                w/(h/DESIRED_HEIGHT)), DESIRED_HEIGHT))
        return img, h, w

    
    def get_pose(self):
        with self.mp_pose.Pose(
                static_image_mode=self.static_image_mode,
                min_detection_confidence=self.min_detection_confidence,
                min_tracking_confidence=self.min_tracking_confidence,
                enable_segmentation=self.enable_segmentation,
                smooth_landmarks=self.smooth_landmarks,
                smooth_segmentation=self.smooth_segmentation,
                model_complexity=self.model_complexity) as pose:

            self.results = pose.process(
                cv2.cvtColor(self.image, cv2.COLOR_BGR2RGB))

    
    def dump_data(self) -> None:
        with open("../landmarks.txt", "w+") as outfile:
            json.dump(self.data, outfile, indent=4)

    
    def lisify_and_fill_missing_landmarks(self) -> list:
        """
        Wenn man die Thresholds min_detection_confidence und min_tracking_confidence
        weiter erhöht riskiert man das fehlen von Landmarks, erhält aber eine genauere Position
        Für diesen fall könnte man die fehlenden Einträge durch die vergangenen ersetzen.
        """
        positions = []
        if not self.results.pose_landmarks:
            if self.previous_positions:
                return self.previous_positions
            else:
                return None 
        
        position = None
        for ind, name in enumerate(self.landmark_names):
            
            #try:
            position = self.results.pose_landmarks.landmark[self.mp_pose.PoseLandmark[name.upper(
                )]]
            
            # except AttributeError:
            #     #single missing value
            #     if len(self.previous_positions)>ind and self.previous_positions:
            #         position = self.previous_positions[ind]

            # fill in missing values with old value
            if position is None:
                self.missing_counter += 1
                print(f"Missing Counter {self.missing_counter}")
                position = self.previous_positions[ind]
            positions.append(position)
        assert len(positions)==len(self.landmark_names)
        self.previous_positions = positions
        return positions

 
def main(use_img):

    mp_drawing = mp.solutions.drawing_utils
    mp_drawing_styles = mp.solutions.drawing_styles
    mp_pose = mp.solutions.pose
    pose = PoseMP(visualize=False, static_image_mode=False, smooth_landmarks=True,
                 smooth_segmentation=True, min_detection_confidence=0.5,
                 min_tracking_confidence=0.5, enable_segmentation=False, model_complexity=2,
                 landmark_names=['left_shoulder', 'right_shoulder',
                                 'left_elbow', 'right_elbow',
                                 'left_wrist', 'right_wrist', 'left_hip', 'right_hip',
                                 'left_knee', 'right_knee',
                                 'left_ankle', 'right_ankle',
                                 'left_heel', 'right_heel',
                                 'left_foot_index', 'right_foot_index'
                                 ])
    
    count = 0
    start = 0

    if not use_img:

        cap = cv2.VideoCapture(0)
        start = time.perf_counter()
        while cap.isOpened(): 

            success, image = cap.read()
        
            if not success:
                # ignore empty frames
                continue
            
            count += 1
            image.flags.writeable = False
            image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
            pose.classify_image(image,image_id=count)
            
            image.flags.writeable = True
            image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
            mp_drawing.draw_landmarks(
            image,
            pose.results.pose_landmarks,
            mp_pose.POSE_CONNECTIONS,
            landmark_drawing_spec=mp_drawing_styles.get_default_pose_landmarks_style())
            # Flip the image horizontally for a selfie-view display.
            
            cv2.imshow('MediaPipe Pose', cv2.flip(image, 1))
            if cv2.waitKey(5) & 0xFF == 27:
                return
            
        cap.release()
        end = time.perf_counter()
        
        print(f"performance: {count/end-start}")



if __name__ == "__main__":
    main(use_img=False)