# external modules
from cv2 import VideoCapture, VideoWriter, VideoWriter_fourcc, FONT_HERSHEY_SIMPLEX, flip, transpose, cvtColor, COLOR_RGB2BGR
import time
import mediapipe as mp
import win32pipe
import win32file
import pywintypes
import os
import sys
from threading import Thread
from tkinter import Tk, Frame, Label, Button  # fix later
from PIL import ImageTk, Image

# posevr modules
import posevr_client

# vars
h_x = 0
h_y = 0
h_z = 0
w_x = 0
w_y = 0
w_z = 0
l_x = 0
l_y = 0
l_z = 0
l_x = 0
l_y = 0
l_z = 0

# setup pipe
pipe_name = 'PTSCDriverPipe'
pipe_connected = False


# function for calibration


def calibrate():
    if pipe_connected:
        global calibrating
        calibrating = True


# tkinter app stuff
root = Tk()
root.title('Airpose')
# Create a frame
app = Frame(root, bg="white")
app.grid()
# Create a label for video stream
video_label = Label(app)
video_label.grid(row=0, column=0, columnspan=1)
# Create calibration button
calibration_button = Button(root, text="Calibrate",
                            command=calibrate, width=50, height=5, bg='green')
calibration_button.grid(row=1, column=0)
calibrating = False
initial_calibrating = False

# setup mediapipe
mp_drawing = mp.solutions.drawing_utils
mp_pose = mp.solutions.pose
pose = mp_pose.Pose(min_detection_confidence=0.8,
                    min_tracking_confidence=0.8, smooth_landmarks=True)

# recorded positions
positions = ['LEFT_HIP', 'RIGHT_HIP', 'LEFT_ANKLE', 'RIGHT_ANKLE',
             'LEFT_WRIST', 'RIGHT_WRIST', 'NOSE', 'LEFT_FOOT_INDEX', 'RIGHT_FOOT_INDEX']
heel_height = .8
hand_height = .3
prev_time = 0
prev_landmarks = {}
min_visibility = 0.7


def get_landmarks(positions, results):
    # returns a dict of {position : (position.x, position.y, position.z)}
    res = {}
    for position in positions:
        r = results.pose_landmarks.landmark[getattr(
            mp_pose.PoseLandmark, position)]
        res[position] = (r.x, r.y, r.z)
    return res


def start_pipe(pipe):
    """Waits until pipe server connects to pipe client

    Args:
        pipe ([type]): [description]
    """
    global pipe_connected
    global initial_calibrating
    print('Waiting for client to connect...')
    win32pipe.ConnectNamedPipe(pipe, None)
    print('Client is connected')
    if posevr_client.pipe_ended:
        return
    pipe_connected = True
    time.sleep(8)  # wait for driver to get set up TODO: fix
    initial_calibrating = True


def video_stream():
    global calibrating
    global initial_calibrating
    global pipe_connected
    global prev_time
    global prev_landmarks
    try:  # try is attempt to cleanup app after steamvr disconnects

        send = f"head;{h_x};{h_y};{h_z};waist;{w_x};{w_y};{w_z};left_foot;{l_x};{l_y};{l_z};right_foot;{l_x};{l_y};{l_z}"
        # about 150 chars for 4 positions, rounded 5 decimal points
        some_data = str.encode(str(send), encoding="ascii")
        # print(send)

        if pipe_connected:
            # about 150 chars for 4 positions, rounded 5 decimal points
            some_data = str.encode(str(send), encoding="ascii")
            # Send the encoded string to client
            err, bytes_written = win32file.WriteFile(
                pipe,
                some_data,
                pywintypes.OVERLAPPED()
            )

        # loops back
        video_label.after(1, video_stream)
    except:
        root.quit()
        return


if __name__ == '__main__':
    # starts pipe in seperate thread
    pipe = win32pipe.CreateNamedPipe(f'\\\\.\\pipe\\{pipe_name}', win32pipe.PIPE_ACCESS_DUPLEX | win32file.FILE_FLAG_OVERLAPPED,
                                     win32pipe.PIPE_TYPE_MESSAGE | win32pipe.PIPE_READMODE_MESSAGE | win32pipe.PIPE_WAIT,
                                     1, 65536, 65536, 0, None)
    print(f'Pipe {pipe_name} created.')
    t1 = Thread(target=start_pipe, args=(pipe,))
    t1.start()

    prev_time = time.time()

    root.after(0, video_stream)
    root.mainloop()

    # ends pipe server by connecting a dummy client pipe and closing
    if not pipe_connected:
        posevr_client.named_pipe_client()


# while True:
#    if keyboard.read_key() == "p":
#        print("You pressed p")
#        break
#
# while True:
#    if keyboard.is_pressed("q"):
#        print("You pressed q")
#        break
#
#keyboard.on_press_key("r", lambda _: print("You pressed r"))
