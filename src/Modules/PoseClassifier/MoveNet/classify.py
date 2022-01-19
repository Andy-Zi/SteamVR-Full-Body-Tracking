

from re import I
from typing import Optional, List
import tensorflow as tf
import tensorflow_hub as hub
import numpy as np
import cv2
from MoveNet.crop import run_inference,determine_crop_region,init_crop_region
from MoveNet.draw_on_image import draw_prediction_on_image
from utils.positions_dataclass import Positions
from MoveNet.config import MoveNetConfig as cfg
from MoveNet.camera_stream import RealSenseStream
import matplotlib.pyplot as plt

class MoveNetModel:
    image_size:int #only square pictures
    module = None
    model = None
    depth_values:dict[str, float] # a depth value for every position
    accepted_input_size:int
    
    def __init__(self, model_name:str = "movenet_lightning",default_value = False,options = None):
        
        self.load_movenet_model(model_name)
        self.use_default_value = default_value
        self.depth_values = None
        if options is not None:
            raise NotImplementedError
        
    def load_movenet_model(self,model_name) -> None:
        """select from two models and load them"""
        
        if model_name == "movenet_lightning":
            try:
                self.module = hub.load("/Users/macbook/Documents/KI_Master/AR-VR/arvr-projekt-modularbeit/src/Modules/PoseClassifier/MoveNet/models/movenet_singlepose_lightning_4")
                self.accepted_input_size:int = 192
            except Exception as e:
                print("Error loading movenet_singlepose_lightning: ", e)
                
                self.module = hub.load("https://tfhub.dev/google/movenet/singlepose/lightning/4")
                self.accepted_input_size:int = 192
        elif "movenet_thunder" in model_name:
            self.module = hub.load("https://tfhub.dev/google/movenet/singlepose/thunder/4")
            self.accepted_input_size = 256
        else:
            raise ValueError("Unsupported model name: %s" % model_name)

    def movenet(self,input_image:tf.Tensor):
        """Runs detection on an input image.

        Args:
        input_image: A [1, height, width, 3] tensor represents the input image
            pixels. Note that the height/width should already be resized and match the
            expected input resolution of the model before passing into this function.

        Returns:
        A [1, 1, 17, 3] float numpy array representing the predicted keypoint
        coordinates and scores.
        
        """
        
       
        self.model = self.module.signatures['serving_default']
        # SavedModel format expects tensor type of int32.
        #print("type cast")
        input_image = tf.cast(input_image, dtype=tf.int32)
        # Run model inference.
        assert input_image.shape == (1,192,192,3)
        outputs = self.model(input_image)
        #print("output: ", outputs)
        # Output is a [1, 1, 17, 3] tensor.
        keypoints_with_scores = outputs['output_0'].numpy()
        return keypoints_with_scores

    # def convert_to_formated_points(self, keypoints_with_scores)->Optional[Positions]:
    #     """"""
    #     positions:dict[str,list[float]] = {}
    #     for key,val in cfg.KEYPOINT_DICT.items():
    #         positions[key.upper()] = keypoints_with_scores[0,0,val,:]
    #     if positions is not None and len(positions.keys()) == 17:
    #         return Positions(**positions)
        
    def _look_up_depth_values_for_keys(self, key_point_locs:List):
        """extract values from depthmap where keypoints are"""
        
                #self.depth_values = {key.upper() : 0 for key,_ in cfg.KEYPOINT_DICT.items()}
        self.depth_values = {}
        for key,val in cfg.KEYPOINT_DICT.items():
            x, y, _ = key_point_locs[0,0,val,:]
            assert isinstance(x, int)
            assert isinstance(y, int)
            self.depth_values[key.upper()] = self.depth_map[x, y]
            
    def insert_depth_value(self, key_point_locs:List)->List:
        """inserts depth value in predictions of keypoints from a depth-map of the picture"""
        #find out values from depth-map
        self._look_up_depth_values_for_keys(key_point_locs)
        
        for key,val in cfg.KEYPOINT_DICT.items():
            key_point_locs[0,0,val,:].tolist().insert(2, self.depth_values[key.upper()]) # insert depth value
        return key_point_locs
   
    def calculate_positions(self,points_3d):
        positions:dict[str,list[float]] = {}
        list_of_body_parts = list(KEYPOINT_DICT.keys())
        nose = points_3d[0,0,KEYPOINT_DICT["nose"],:]
        for ind,val in enumerate(points_3d[0,0,:,:]):
            positions[list_of_body_parts[ind].upper()] = val - nose #scale to nose

        return Positions(**positions)
    
    
    #@profile
    def classify_image(self,rawimage:np.ndarray):
        """classify image and crop"""
        # Resize and pad the image to keep the aspect ratio and fit the expected size.
        print(rawimage.shape[2])
        
        #check if shape[3]==4 -> depth map was appended
        if rawimage.shape[2] == 4:
            image = rawimage[:,:,:3]
            self.depth_map = rawimage[:,:,3]
        else: 
            self.depth_map = None
            image = rawimage
        
        input_image = tf.expand_dims(image, axis=0)
        
        input_image = tf.image.resize_with_pad(input_image, self.accepted_input_size, self.accepted_input_size)
        #assert input_image.shape == (1,192,192,4)
        # Run model inference.
        keypoints_with_scores = self.movenet(input_image)
        output_overlay = None
        
        # # Visualize the predictions with image.
        display_image = tf.expand_dims(image, axis=0) #expand image to image.shape[0]==1 (1,192,192,3)
        display_image = tf.cast(tf.image.resize_with_pad(
                                image, 1280, 1280), dtype=tf.int32)
        if display_image.shape[0]==1:
            output_overlay, key_point_locs = draw_prediction_on_image(
                np.squeeze(display_image.numpy(), axis=0), keypoints_with_scores) #get rid of first dim 
        else:
            output_overlay, key_point_locs = draw_prediction_on_image(
                display_image.numpy(), keypoints_with_scores)
        #if self.depth_map[0] != None:
        keypoints_with_scores = self.insert_depth_value(key_point_locs)
    
        positions = self.calculate_positions(keypoints_with_scores)
        return positions,output_overlay
        
#@profile
def input_stream():
    mn = MoveNetModel()
    cap = cv2.VideoCapture(0)
    while True:
        # Read frame from camera
        ret, image_np = cap.read()

        # Expand dimensions since the model expects images to have shape: [1, size, size, 3]
        
        if not ret:
            print("no success")
            continue
        
        # Flip horizontally
        # image_np = np.fliplr(image_np).copy()

        detection, crop_region = mn.classify_image(image_np)
        print("crop_region:", crop_region)
        # Display output
        cv2.imshow('object detection', cv2.resize(detection, (800, 600)))
        if cv2.waitKey(25) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()
        
if __name__ == "__main__":
    
    move = MoveNetModel()