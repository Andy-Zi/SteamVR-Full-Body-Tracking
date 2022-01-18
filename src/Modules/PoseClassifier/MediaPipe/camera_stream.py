import cv2
import numpy as np
from MediaPipe.utils_mp.plot_landmarks import plot_landmarks

class CameraStream:
    
    def loop(self, classifier, pipe, output:bool = False):
        count = 0
        cap = cv2.VideoCapture(0)
        while cap.isOpened():
            
            count += 1
            success, image = cap.read()

            if not success:
                # ignore empty frames
                continue
            
            image.flags.writeable = False
            results, image = classifier.classify_image(image)
            # chance = np.random.randint(100)
            # if chance < 2:
            #     x,y,z,keys = [],[],[],[]
            #     for key,values in results.serialize().items():
            #         x.append(values[0])
            #         y.append(values[1])
            #         z.append(values[2])
            #         keys.append(key)
            #     plot_landmarks(x,y,z,keys)

            if results is not None and image is not None:
                if pipe:
                    
                    pipe.SendPositions(results, image)
                
                # show prediction
                if output:
                    cv2.imshow('Pose', cv2.flip(image, 1))
                    if cv2.waitKey(5) & 0xFF == 27:
                        break
                    
                    # #print results
                    # if (count%40) == 0:
                    #     print(f"{results=}")
