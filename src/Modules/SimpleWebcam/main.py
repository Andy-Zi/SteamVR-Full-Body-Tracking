import cv2
from ModulePipe.pipe_client import NamedPipe
from ModulePipe.positions_dataclass import Positions
import sys

import time

def WriteToStd(message):
    print(message)
    sys.stdout.flush() 

if __name__ == "__main__":
    time.sleep(0.4)
    args = sys.argv[1:]
    pipe = NamedPipe()
    WriteToStd("Create Pipe!\n")
    
    if(len(args) > 0):
        WriteToStd(f"Using Webcam {args[0]} \n")
        cap = cv2.VideoCapture(args[0])

    else:
        WriteToStd(f"Using Webcam 0")
        cap = cv2.VideoCapture(0)
    
    if not cap.isOpened():
        WriteToStd("Cant open Webcam!")
    while cap.isOpened():
        success, image = cap.read()
        if not success:
           WriteToStd("Ignoring empty camera frame.")
        pipe.SendPositions(Positions(),image)

            
        