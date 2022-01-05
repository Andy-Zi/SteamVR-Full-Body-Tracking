
import cv2
from MediaPipe.classifier.pose import PoseMP
from MoveNet.classify import MoveNetModel
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
    except:
        pass

    classifier = parsed_options["classifier"](
        default_value=default_value, options=None)

    count = 0
    cap = cv2.VideoCapture(0)
    while cap.isOpened():
        count += 1
        success, image = cap.read()

        if not success:
            # ignore empty frames
            continue
        
        image.flags.writeable = False
        results, image = classifier.classify_image(image)

        if results is not None and image is not None:
            if pipe:
                pipe.SendPositions(results, image)
            
            # show prediction
            if output:
                cv2.imshow('Pose', cv2.flip(image, 1))
                if cv2.waitKey(5) & 0xFF == 27:
                    break
                
                #print results
                if (count%40) == 0:
                    print(f"{results=}")
        else:
            print("no results")


def parse_options():
    opts = [opt for opt in sys.argv[1:] if opt.startswith("-")]
    #args = [arg for arg in sys.argv[1:] if not arg.startswith("-")]
    parsed_options: dict[str, Union[bool,Callable]] = {
        "default-position-value": False, "commandline-output": False, "classifier": PoseMP}

    if "-dv" in opts:
        parsed_options["default-position-value"] = True
    if "-o" in opts:
        parsed_options["commandline-output"] = True
    if "-mp" in opts:
        parsed_options["classifier"] = PoseMP
    if "-mv" in opts:
        parsed_options["classifier"] = MoveNetModel
    return parsed_options


if __name__ == "__main__":
    #TODO  landmarks overlay picture
    # Head as 0,0,0
    run_media_pipeline()
