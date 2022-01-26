import cv2
from ModulePipe.pipe_client import NamedPipe
from ModulePipe.positions_dataclass import Positions
import sys
import argparse
import time

def WriteToStd(message):
    print(message)
    sys.stdout.flush() 

if __name__ == "__main__":
    time.sleep(0.4)
    
    parser = argparse.ArgumentParser(description='Start a Webcam Stream')
    parser.add_argument('-w', dest='webcam', type=int,
                    help='device number of the webcam to use', default=0)
    args = parser.parse_args()
    pipe = NamedPipe()
    WriteToStd("Create Pipe!\n")
    
   
    WriteToStd(f"Using Webcam {args.webcam} \n")
    cap = cv2.VideoCapture(args.webcam)


    
    if not cap.isOpened():
        WriteToStd("Cant open Webcam!")
    while cap.isOpened():
        success, image = cap.read()
        if not success:
           WriteToStd("Ignoring empty camera frame.")
        pipe.SendPositions(Positions(),image)

            
        