from dataclasses import dataclass
import numpy as np
import time
import matplotlib.pyplot as plt


@dataclass
class KalmanFilter:
    dt      : float = 0.0                # delta t (Zeit zwischen Messungen) 0 => set automatic
    std_X   : float = 0.005                # Messfehler X Achse
    std_Y   : float = std_X              # Messfehler Y Achse
    std_Z   : float = std_Y              # Messfehler Z Achse
    std_V   : float = 1                    # Standardabweichung Geschwindigkeit (Rauschen / Messfehler)
    plot    : bool  = False              # Plot Measure 

    def __post_init__(self):
        # state transition Matrix
        #                    x,  x'     , y, y', z, z'
        self.F = np.array( [[ 1, self.dt, 0, 0, 0, 0 ], # x
                            [ 0, 1,       0, 0, 0, 0 ], # x'
                            [ 0, 0, 1, self.dt, 0, 0 ], # y
                            [ 0, 0, 0, 1,       0, 0 ], # y'
                            [ 0, 0, 0, 0, 1, self.dt],  # z
                            [ 0, 0, 0, 0, 0, 1 ]] )      # z'

        # process noise Matrix
        #                    x,  x'     , y, y', z, z'
        self.Q = np.array( [[ self.dt**4/4, self.dt**3/2, 0, 0, 0, 0 ], # x
                            [ self.dt**3/2, self.dt**2,   0, 0, 0, 0 ], # x'
                            [ 0, 0, self.dt**4/4, self.dt**3/2, 0, 0 ], # y
                            [ 0, 0, self.dt**3/2, self.dt**2,   0, 0 ], # y'
                            [ 0, 0, 0, 0, self.dt**4/4, self.dt**3/2 ], # z
                            [ 0, 0, 0, 0, self.dt**3/2, self.dt**2   ]] )# z'
        self.Q = self.Q * self.std_V**2
        
        # Measurement Covariance Matrix
        #                    x, y, z
        self.R = np.array( [[ self.std_X**2, 0, 0 ], 
                            [ 0, self.std_Y**2, 0 ], 
                            [ 0, 0, self.std_Z**2 ]] )

        # Start Initial Position (Unknown)
        self.x = np.transpose( np.array(  [ 0, 0, 0, 0, 0, 0 ] ) )

        # estimate uncertainty (covariance) matrix of the current state (predicted at the previous state)

        self.P = np.array( [[ 500,   0,   0,   0,   0,   0],
                            [   0, 500,   0,   0,   0,   0],
                            [   0,   0, 500,   0,   0,   0],
                            [   0,   0,   0, 500,   0,   0],
                            [   0,   0,   0,   0, 500,   0],
                            [   0,   0,   0,   0,   0, 500]] )

        # Measurement mapping (Observation Matrix)
        #                     x     y     z        Measurement
        self.H = np.array( [[ 1, 0, 0, 0, 0, 0 ],
                            [ 0, 0, 1, 0, 0, 0 ],
                            [ 0, 0, 0, 0, 1, 0 ]] )

        self.__predict()
        
        plt.ion()
        self.figure,ax= plt.subplots(figsize=(10,8))
        self.plt_m, = ax.plot([], [], color='g')
        self.plt_f, = ax.plot([], [], color='r')
        ax.set_xlim([-1, 1])
        ax.set_ylim([-1, 1])
    
    def __predict(self) -> None:
        """[summary]
        """
        print("PREDICT P VORHER")
        print(self.P)
        self.P = self.F @ self.P @ np.transpose(self.F) + self.Q
        print("PREDICT P")
        print(self.P)
        print("F")
        print(self.F)
        print("Q")
        print(self.Q)
        
    
    def update(self, measure: np.ndarray) -> np.ndarray:
        if self.dt > 1:
            self.__set_delta_t()
        
        temp_measure = np.copy(measure)
        measure = np.resize(measure, (3,))

        #calculate Kalman Gain
        self.K = self.P @ np.transpose(self.H) @ np.linalg.inv( self.H @ self.P @ np.transpose(self.H) + self.R )

        print("K")
        print(self.K)
        print("P")
        print(self.P)
        print("P @ H.T")
        print(self.P @ self.H.T)
        print("inv(H @ P @ H.T +R)")
        print(np.linalg.inv(self.H @ self.P @ self.H.T + self.R))
        


        # estimate new Value
        self.x = self.x + self.K @ ( measure - self.H @ self.x )

        # I identity matrix
        I = np.eye(np.shape(self.H)[1])
        
        self.P = (I - self.K @ self.H) @ self.P @ np.transpose(I - self.K @ self.H) + self.K @ self.R @ np.transpose(self.K)
        #print("K")
        #print(self.K)
        
        #print("I - k @ H")
        #print( I - self.K @ self.H)
        #print("Transpose ( I - K @ H")
        #print( np.transpose(I - self.K @ self.H))
        #print("k @ R @ k.T")
        #print(self.K @ self.R @ np.transpose(self.K))
        #print("P")
        #print(self.P)
        #print("TEIL")
        #print((I - self.K @ self.H) @ self.P @ np.transpose(I - self.K @ self.H))

        self.__predict()
        
        if self.dt == 0:
            self.__set_delta_t()
           
        if self.plot:
            self.__plot_data(measure, self.x)
        
        return np.append(self.x[::2], temp_measure[-1])


    def __set_delta_t(self) -> None:
        if self.dt == 0:
            self.dt = time.time()
        elif self.dt > 1:
            self.dt = time.time() - self.dt

    def __plot_data(self, measure: np.ndarray, filtered: np.ndarray) -> None:
        print(np.shape(measure))
        print(np.shape(filtered))
        print(measure[0])
        print(measure[1])
        print(filtered[0])
        print(filtered[2])
        self.plt_m.set_xdata(np.append(self.plt_m.get_xdata(), measure[0]))
        self.plt_m.set_ydata(np.append(self.plt_m.get_ydata(), measure[1]))
        self.plt_f.set_xdata(np.append(self.plt_f.get_xdata(), filtered[0]))
        self.plt_f.set_ydata(np.append(self.plt_f.get_ydata(), filtered[2]))
        
        self.figure.canvas.draw()
        self.figure.canvas.flush_events()
        time.sleep(0.1)
        
       
    

if __name__ == "__main__":
    K = np.array([[0.9921, 0], [0.6614, 0], [0.2205, 0], [0, 0.9921], [0, 0.6614], [0, 0.2205]])
    P = np.array([[1125, 750, 250, 0, 0, 0], [750, 1000, 500, 0, 0, 0], [250, 500, 500, 0, 0, 0], [0, 0, 0, 1125, 750, 250], [0, 0, 0, 750, 1000, 500], [0, 0, 0, 250, 500, 500]])
    H = np.array([[1, 0, 0, 0, 0, 0], [0, 0, 0, 1, 0, 0]])
    R = np.array([[9, 0], [0, 9]])
    I = np.eye(np.shape(P)[0])

    T1 = np.dot(np.dot(K, R), np.transpose(K))
   
    T2 = (I - np.dot(K, H))
 
    T3 = np.transpose(T2)

    PP = np.dot ( np.dot(T2, P), T3) + T1
    print(PP)
    PP = T2 @ P @ T3 + T1
    print(PP)
    print(np.shape(H)[1])

    