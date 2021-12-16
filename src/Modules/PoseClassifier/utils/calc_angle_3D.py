import numpy as np


def calculate_angle_3D(a:np.ndarray,b:np.ndarray,c:np.ndarray):
    assert len(p1) == len(p2) == 3
    
    ba = a-b
    bc = c-b
    
    cosine_angle = np.dot(ba, bc) / (np.linalg.norm(ba) * np.linalg.norm(bc))
    angle = np.arccos(cosine_angle)

    return np.degrees(angle)