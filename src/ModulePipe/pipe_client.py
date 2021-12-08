import win32file, json
import cv2
import numpy as np
from win32pipe import error

from positions_dataclass import Positions
#8192 Json -> rest 0
class NamedPipe():
    def __init__(self, pipe_name : str = r'\\.\pipe\ModulePipe') -> None:
        self.pipe_name = pipe_name
        self.handle = win32file.CreateFile(self.pipe_name, win32file.GENERIC_WRITE,
                        0, None, win32file.OPEN_EXISTING, win32file.FILE_ATTRIBUTE_NORMAL, None)

    def __del__(self):
        if self.handle:
            win32file.CloseHandle(self.handle)

    def SendPositions(self, positions: Positions, image: np.ndarray = None):
        self.__PipeData(json.dumps(positions.serialize()), image)


    def SendJSONString(self, json_data : str, image: np.ndarray = None):
        self.__PipeData(json_data)


    def __PipeData(self, string : str, image : np.ndarray):

        
        try:
            dat = np.frombuffer(string.encode("utf-8"), dtype=np.uint8)
            arr = np.zeros(8192, dtype=np.uint8)
            arr[:len(dat)] = dat

            win32file.WriteFile(self.handle, arr.tobytes())
            win32file.FlushFileBuffers(self.handle)

        except error as e:
            raise Exception("Pipe {} konnte nicht beschrieben werden! {}".format(self.pipe_name, e)) from None


NamedPipe().SendPositions(Positions(LEFT_ANKLE=[1,3,4], RIGHT_ANKLE=[8,5,4]))

"""
{
    "LEFT_SHOULDER": [
        0.7273658514022827,
        1.0285296440124512,
        -0.21030668914318085
    ],
    "RIGHT_SHOULDER": [
        0.35100454092025757,
        1.0084062814712524,
        -0.17886438965797424
    ],
    "LEFT_ELBOW": [
        0.8172922134399414,
        1.488473892211914,
        -0.28326284885406494
    ],
    "RIGHT_ELBOW": [
        0.2874842882156372,
        1.5419394969940186,
        -0.507495105266571
    ],
    "LEFT_WRIST": [
        0.806119441986084,
        1.813567042350769,
        -0.6427343487739563
    ],
    "RIGHT_WRIST": [
        0.2983761727809906,
        1.5431828498840332,
        -1.1420397758483887
    ],
    "LEFT_HIP": [
        0.6399475336074829,
        1.8952288627624512,
        0.030724581331014633
    ],
    "RIGHT_HIP": [
        0.4227457642555237,
        1.9086651802062988,
        -0.03130394220352173
    ],
    "LEFT_KNEE": [
        0.6080567240715027,
        2.573002576828003,
        0.022683147341012955
    ],
    "RIGHT_KNEE": [
        0.4384058117866516,
        2.4637489318847656,
        -0.4311080574989319
    ],
    "LEFT_ANKLE": [
        0.616787314414978,
        3.149784564971924,
        0.6966956853866577
    ],
    "RIGHT_ANKLE": [
        0.40265512466430664,
        3.103792667388916,
        0.07355707883834839
    ],
    "LEFT_HEEL": [
        0.628137469291687,
        3.2517828941345215,
        0.7448744773864746
    ],
    "RIGHT_HEEL": [
        0.3925572633743286,
        3.2179369926452637,
        0.11300840228796005
    ],
    "LEFT_FOOT_INDEX": [
        0.5668566823005676,
        3.280980348587036,
        0.3927505910396576
    ],
    "RIGHT_FOOT_INDEX": [
        0.441722571849823,
        3.2266597747802734,
        -0.26048514246940613
    ]
}
"""