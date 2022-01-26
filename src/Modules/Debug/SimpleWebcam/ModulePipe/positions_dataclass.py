from dataclasses import dataclass, asdict

@dataclass
class Positions:
    LEFT_SOULDER    : list = None
    RIGHT_SHOULDER  : list = None
    LEFT_ELBOW      : list = None
    RIGHT_ELBOW     : list = None
    LEFT_WRIST      : list = None
    RIGHT_WRIST     : list = None
    LEFT_HIP        : list = None
    RIGHT_HIP       : list = None
    LEFT_KNEE       : list = None
    RIGHT_KNEE      : list = None
    LEFT_ANKLE      : list = None
    RIGHT_ANKLE     : list = None
    LEFT_HEEL       : list = None
    RIGHT_HEEL      : list = None
    LEFT_FOOT       : list = None
    RIHT_FOOT       : list = None

    def serialize(self):
        return asdict(self)
