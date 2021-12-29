
import cv2
from MediaPipe.classifier.pose import PoseMP

import sys
from typing import Union,Callable
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
        results = classifier.classify_image(image, image_id=str(count))
        
        if output:
            
            
            cv2.imshow('MediaPipe Pose', cv2.flip(image, 1))
            if cv2.waitKey(5) & 0xFF == 27:
                break

        #image_rgb.flags.writeable = False
        if results is not None and image is not None:
            if pipe:
                pipe.SendPositions(results, image)
            
            #print results every 2 seconds
            if output and (count%40) == 0:
                print(results)


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
    #else:
        #raise ValueError("Select a classifier (e.g. '-mp')")

    return parsed_options


if __name__ == "__main__":
    #TODO  landmarks overlay picture
    # Head as 0,0,0
    run_media_pipeline()
