
import cv2
from PoseClassifier.MediaPipe.classifier.pose import PoseMP
import sys
from typing import Union,Callable
#from ModulePipe.pipe_client import NamedPipe


def run_media_pipeline():
    """ runs pose detection with mediapipe (scalable) and return image and results"""

    parsed_options = parse_options()

    output = parsed_options["commandline-output"]
    default_value = parsed_options["default-position-value"]
    #pipe = NamedPipe()

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
        image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        results = classifier.classify_image(image, image_id=str(count))
        
        if results is not None and image is not None:
            #pipe.SendPositions(results, image)
            
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
    run_media_pipeline()
