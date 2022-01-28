
import cv2


import sys
from typing import Union,Callable
try:
    from ModulePipe.pipe_client import NamedPipe
except:
    pass

import sys

print(sys.executable)

def run_media_pipeline():
    """ runs pose detection with mediapipe (scalable) and return image and results"""

    parsed_options = parse_options()

    output = parsed_options["commandline-output"]
    default_value = parsed_options["default-position-value"]
    pipe = None
    try:
        pipe = NamedPipe()
    except Exception as e:
        pass
        

    classifier = parsed_options["classifier"](
        default_value=default_value, options=None)

    streamer = parsed_options["video_stream"]()
    streamer.loop(classifier=classifier,pipe=pipe,output=output)


def parse_options():
    from MediaPipe.classifier.pose import PoseMP
    from MediaPipe.camera_stream import CameraStream
    opts = [opt for opt in sys.argv[1:] if opt.startswith("-")]
    parsed_options: dict[str, Union[bool,Callable]] = {
        "default-position-value": False, "commandline-output": False, "classifier": PoseMP,
        "video_stream": CameraStream}

    if "-dv" in opts:
        parsed_options["default-position-value"] = True
        
    if "-o" in opts:
        parsed_options["commandline-output"] = True
        
    if "-mp" in opts:
        
        parsed_options["classifier"] = PoseMP
        
    if "-mv" in opts:
        from MoveNet.classify import MoveNetModel
        parsed_options["classifier"] = MoveNetModel
    
    if "-rs" in opts: # Realsense
        from MoveNet.camera_stream import RealSenseStream
        parsed_options["video_stream"] = RealSenseStream
        
    if "-kin" in opts: #Kinect
        parsed_options["video_stream"] = not_implemented
        
    if "-wc" in opts: #webcam
        parsed_options["video_stream"] = CameraStream
<<<<<<< HEAD
    
    
=======
        
>>>>>>> 22c4d19e20eb9da935d82498c1b512577f738b20
    return parsed_options

def not_implemented():
    raise NotImplementedError

if __name__ == "__main__":

    run_media_pipeline()
