
import numpy as np
from typing import Union


def dist(p1: Union[np.ndarray,list[float]], p2: Union[np.ndarray,list[float]]) -> float:
    """ calculate the distance of movement for each landmark"""
    if type(p1) ==list[float]:
        p1 = np.ndarray(p1)
    if type(p2) == list[float]:
        p2 = np.ndarray(p2)
        
    dist = np.linalg.norm(p1 - p2)

    return dist