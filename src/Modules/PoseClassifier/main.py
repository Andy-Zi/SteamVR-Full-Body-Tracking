
import cv2
from MediaPipe.classifier.pose import PoseMP
import sys
import os
from inspect import getsourcefile

current_path = os.path.abspath(getsourcefile(lambda:0))
current_dir = os.path.dirname(current_path)
parent_dir = current_dir[:current_dir.rfind(os.path.sep)]

sys.path.insert(0, parent_dir)

from ModulePipe.positions_dataclass import Positions
from ModulePipe.pipe_client import NamedPipe



def run_media_pipeline():
    """ runs pose detection with mediapipe (scalable) and return image and results"""
    
    parsed_options = parse_options()
    
    output = parsed_options["commandline-output"]
    default_value = parsed_options["default-position-value"]
    
    classifier = parsed_options["classifier"](default_value=default_value,options=None)
    
    # get video stream
    count = 0
    cap = cv2.VideoCapture(0)
    while cap.isOpened():
        count += 1
        success, image = cap.read()

        if not success:
            # ignore empty frames
            continue

    
    # calssify positions
        image.flags.writeable = False
        image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        results = classifier.classify_image(image, image_id=str(count))
        if results is not None:   
            positions = Positions({key.upper():value for key,value in results.items()})
            
            pipe.SendPositions(positions,image)
            if output:
                print(positions)
            
        
    
    
def parse_options():
    opts = [opt for opt in sys.argv[1:] if opt.startswith("-")]
    #args = [arg for arg in sys.argv[1:] if not arg.startswith("-")]
    parsed_options:dict[str,any] = {"default-position-value":False,"commandline-output":False,"classifier":PoseMP}
    
    if "-default-position-value" in opts:
        parsed_options["default-position-value"] = True
    elif "-commandline-output" in opts:
        parsed_options["commandline-output"] = True
    elif "-mediapipe" in opts:
        parsed_options["classifier"] = PoseMP  

    return parsed_options

def main():
    runs = 100
    pose = PoseMP()

    mp_drawing = mp.solutions.drawing_utils
    mp_drawing_styles = mp.solutions.drawing_styles
    mp_pose = mp.solutions.pose

    cap = cv2.VideoCapture(0)

    while cap.isOpened():
        count += 1
        success, image = cap.read()

        if not success:
            # ignore empty frames
            continue

        image.flags.writeable = False
        image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        
        results = pose.classify_image(image, image_id=count)
        
        
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



if __name__ == "__main__":
    ans = run_media_pipeline()
    