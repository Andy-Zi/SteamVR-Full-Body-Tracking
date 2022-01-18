
import cv2
from MediaPipe.classifier.pose import PoseMP
from MediaPipe.camera_stream import CameraStream
from MoveNet.classify import MoveNetModel
from MoveNet.camera_stream import RealSenseStream
import sys
from typing import Union,Callable
from utils.positions_dataclass import Positions
try:
    from ModulePipe.pipe_client import NamedPipe
except:
    pass


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
        parsed_options["video_stream"] = CameraStream
        
    if "-mv" in opts:
        parsed_options["classifier"] = MoveNetModel
        parsed_options["video_stream"] = RealSenseStream
    
        
    return parsed_options


if __name__ == "__main__":

    run_media_pipeline()
