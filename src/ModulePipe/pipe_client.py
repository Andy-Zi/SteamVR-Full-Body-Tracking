import win32file, json

from win32pipe import error

class NamedPipe():
    def __init__(self) -> None:
        self.pipe_name = r'\\.\pipe\ModulePipe'
    

    def SendAcceleration(self, left_shoulder = None, right_shoulder = None, left_elbow = None, right_elbow = None, 
                            left_wrist = None, right_wrist = None, left_hip = None, right_hip = None, left_knee = None, 
                            right_knee = None, left_ankle = None, right_ankle = None, left_heel = None, right_heel = None, 
                            left_foot = None, right_foot = None):
        data = { 'LEFT_SHOULDER': left_shoulder, 'RIGHT_SHOULDER': right_shoulder, 'LEFT_ELBOW': left_elbow, 'RIGHT_ELBOW': right_elbow,
                 'LEFT_WRIST': left_wrist, 'RIGHT_WRIST': right_wrist, 'LEFT_HIP': left_hip, 'RIGHT_HIP': right_hip, 'LEFT_KNEE': left_knee,
                 'RIGHT_KNEE': right_knee, 'LEFT_ANKLE': left_ankle, 'RIGHT_ANKLE': right_ankle, 'LEFT_HEEL': left_heel, 'RIGHT_HEEL': right_heel,
                 'LEFT_FOOT': left_foot, 'RIGHT_FOOT': right_foot }
        self.__PipeData(json.dumps(data))


    def SendJSONString(self, json_data : str):
        self.__PipeData(json_data)


    def __PipeData(self, string : str):

        try:
            handle = win32file.CreateFile(self.pipe_name, win32file.GENERIC_WRITE,
                        0, None, win32file.OPEN_EXISTING, win32file.FILE_ATTRIBUTE_NORMAL, None)
            dat = string.encode("utf-8")
            win32file.WriteFile(handle, dat)

            win32file.CloseHandle(handle)
        except error as e:
            raise Exception("Pipe {} konnte nicht beschrieben werden! {}".format(self.pipe_name, e)) from None


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