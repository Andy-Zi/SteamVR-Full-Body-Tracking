from dataclasses import dataclass, asdict
from typing import List
@dataclass
class Positions:
    NOSE            : List[float] = None
    LEFT_EYE        : List[float] = None
    RIGHT_EYE       : List[float] = None
    LEFT_EAR        : List[float] = None
    RIGHT_EAR       : List[float] = None
    LEFT_SHOULDER   : List[float] = None
    RIGHT_SHOULDER  : List[float] = None
    LEFT_ELBOW      : List[float] = None
    RIGHT_ELBOW     : List[float] = None
    LEFT_WRIST      : List[float] = None
    RIGHT_WRIST     : List[float] = None
    LEFT_HIP        : List[float] = None
    RIGHT_HIP       : List[float] = None
    LEFT_KNEE       : List[float] = None
    RIGHT_KNEE      : List[float] = None
    LEFT_ANKLE      : List[float] = None
    RIGHT_ANKLE     : List[float] = None
    LEFT_FOOT_INDEX : List[float] = None
    RIGHT_FOOT_INDEX: List[float] = None

    def serialize(self):
        return asdict(self)
    
    def get_angle(self,p1,p2,p3)->float:
        pass
    
    def get_distance(self,p1,p2)->float:
        pass