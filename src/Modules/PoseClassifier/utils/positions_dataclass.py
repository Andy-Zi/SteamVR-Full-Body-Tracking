from dataclasses import dataclass, asdict

@dataclass
class Positions:
    NOSE            : list[float] = None
    LEFT_EYE_INNER  : list[float] = None
    LEFT_EYE        : list[float] = None
    LEFT_EYE_OUTER  : list[float] = None
    RIGHT_EYE_INNER : list[float] = None
    RIGHT_EYE       : list[float] = None
    RIGHT_EYE_OUTER : list[float] = None
    LEFT_EAR        : list[float] = None
    RIGHT_EAR       : list[float] = None
    MOUTH_LEFT      : list[float] = None    
    MOUTH_RIGHT     : list[float] = None
    LEFT_SHOULDER   : list[float] = None
    RIGHT_SHOULDER  : list[float] = None
    LEFT_ELBOW      : list[float] = None
    RIGHT_ELBOW     : list[float] = None
    LEFT_WRIST      : list[float] = None
    RIGHT_WRIST     : list[float] = None
    LEFT_PINKY      : list[float] = None
    RIGHT_PINKY     : list[float] = None
    LEFT_INDEX      : list[float] = None
    RIGHT_INDEX     : list[float] = None
    LEFT_THUMB      : list[float] = None
    RIGHT_THUMB     : list[float] = None
    LEFT_HIP        : list[float] = None
    RIGHT_HIP       : list[float] = None
    LEFT_KNEE       : list[float] = None
    RIGHT_KNEE      : list[float] = None
    LEFT_ANKLE      : list[float] = None
    RIGHT_ANKLE     : list[float] = None
    LEFT_HEEL       : list[float] = None
    RIGHT_HEEL      : list[float] = None
    LEFT_FOOT_INDEX : list[float] = None
    RIGHT_FOOT_INDEX: list[float] = None

    def serialize(self):
        return asdict(self)
    
    def get_angle(self,p1,p2,p3)->float:
        pass
    
    def get_distance(self,p1,p2)->float:
        pass