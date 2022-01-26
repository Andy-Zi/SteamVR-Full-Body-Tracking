import win32file, json
import cv2
import numpy as np
from win32pipe import error
from utils.positions_dataclass import Positions
import sys

#8192 Json -> rest 0
#8193-Ende Image
class NamedPipe():
    def __init__(self, pipe_name : str = r'\\.\pipe\PTSCModulePipe') -> None:
        self.pipe_name = pipe_name
        self.handle = win32file.CreateFile(self.pipe_name, win32file.GENERIC_WRITE,
                        0, None, win32file.OPEN_EXISTING, win32file.FILE_ATTRIBUTE_NORMAL, None)

    def __del__(self):
        if hasattr(self, 'handle'):
            win32file.CloseHandle(self.handle)

    def SendPositions(self, positions: Positions, image: np.ndarray = None):
        self.__PipeData(json.dumps(positions.serialize()), image)


    def SendJSONString(self, json_data : str, image: np.ndarray = None):
        self.__PipeData(json_data, image)


    def __PipeData(self, string : str, image : np.ndarray):

        
        try:
            arr = np.zeros(8192, dtype=np.uint8)
            dat = np.frombuffer(string.encode("utf-8"), dtype=np.uint8)
            arr[:len(dat)] = dat
            
            if image is not None:
                arr = np.append(arr,cv2.imencode('.jpg' , image)[1])

            win32file.WriteFile(self.handle, arr.tobytes())
            win32file.FlushFileBuffers(self.handle)

        except error as e:
            raise Exception("Pipe {} konnte nicht beschrieben werden! {}".format(self.pipe_name, e)) from None