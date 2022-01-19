

# https://github.com/IntelRealSense/librealsense/blob/master/wrappers/python/examples/align-depth2color.py
import matplotlib.pyplot as plt
import tensorflow as tf
import pyrealsense2.pyrealsense2 as rs
import numpy as np
from .draw_on_image import draw_prediction_on_image
import cv2
import os


class RealSenseStream:
    
    def __init__(self,sample_size:int=5):
    
        self.sample_size = sample_size
        self.depth_scale:float = 0.0
        self.align = self.align_stream()
        
    def start_streaming(self):
        # Start streaming
        self.pipeline = rs.pipeline()
        self.pipeline_wrapper = rs.pipeline_wrapper(self.pipeline)
        
  
    def get_profile(self,config):
        profile = self.pipeline.start(config)
        return profile
        
    def get_format_of_stream(self):
        """find product line and format"""
        config = rs.config()
        
        config.enable_all_streams()
            # config.enable_stream(rs.stream.depth, 640, 480, rs.format.bgr8, 30)
            # config.enable_stream(rs.stream.depth, 640, 480, rs.format.z16, 30)
 
        return config
  
    def get_depth_sensor(self,profile):  
        device = profile.get_device()
        found_rgb = False
        for s in device.sensors:
            print("Name of Cameras:", s.get_info(rs.camera_info.name))
            if s.get_info(rs.camera_info.name) == 'RGB Camera':
                found_rgb = True
                break
        if not found_rgb:
            print("The demo requires Depth camera with Color sensor")
            raise Exception("No RGB frame found")
        depth_sensor = device.first_depth_sensor()
        return depth_sensor
    
    def get_depth_scale(self, depth_sensor):# Getting the depth sensor's depth scale (see rs-align example for explanation)

        depth_scale = depth_sensor.get_depth_scale()
        return depth_scale
    
    def set_clipping_distance(self,clipping_distance_in_meters:float)->float:
        clipping_distance = clipping_distance_in_meters / self.depth_scale
        return clipping_distance

    def align_stream(self):
        align_to = rs.stream.color
        align = rs.align(align_to)  
        return align
    
    def configure_divice(self):
        self.start_streaming()
        config = self.get_format_of_stream()
        profile = self.get_profile(config)
        depth_sensor = self.get_depth_sensor(profile)
        return depth_sensor
    
    def loop(self, classifier, pipe, output, cut_off_distance:float= 10.0, remove_background:bool = False):
        # Streaming loop
        depth_sensor = self.configure_divice()

        self.depth_scale = self.get_depth_scale(depth_sensor)
        self.clipping_distance = self.set_clipping_distance(cut_off_distance)
        try:
            while True:
                print("looping")
                # Get frameset of color and depth
                frames = self.pipeline.wait_for_frames()
                
                # frames.get_depth_frame() is a 640x360 depth image

                # Align the depth frame to color frame
                aligned_frames = self.align.process(frames)

                # Get aligned frames
                aligned_depth_frame = aligned_frames.get_depth_frame() # aligned_depth_frame is a 640x480 depth image
                color_frame = aligned_frames.get_color_frame()

                # Validate that both frames are valid
                if not aligned_depth_frame or not color_frame:
                    continue
                
                depth_image = np.asanyarray(aligned_depth_frame.get_data())
                color_image = np.asanyarray(color_frame.get_data())
                
                
                assert color_image.shape[2] == 3
                assert depth_image.shape[2] == 1
                ########################################################################
                #set to zero for testing
                depth_image = np.zeros(depth_image.shape)
                color_image = np.zeros(color_image.shape)
                
                ########################################################################
                
                
                # Apply colormap on depth image (image must be converted to 8-bit per pixel first)
                #depth_colormap = cv2.applyColorMap(cv2.convertScaleAbs(depth_image, alpha=0.03), cv2.COLORMAP_JET)

                depth_map_dim = depth_image.shape
                color_map_dim = color_image.shape

                # If depth and color resolutions are different, resize color image to match depth image for display
                if depth_map_dim != color_map_dim:
                    resized_color_image = cv2.resize(color_image, dsize=(depth_map_dim[1], depth_map_dim[0]), interpolation=cv2.INTER_AREA)
                    images = np.dstack((resized_color_image, depth_image))
                else:
                    images = np.dstack((color_image, depth_image))
                    
                #stacked images with depthmap in images[:,:,3]    
                assert images.shape[2] == 4

                # Show images
                cv2.namedWindow('RealSense', cv2.WINDOW_AUTOSIZE)
                cv2.imshow('RealSense', images)
                cv2.waitKey(1)    
                
                #new            
                # Remove background - Set pixels further than clipping_distance to grey
                grey_color = 153
                if remove_background:
                    depth_image = np.where((depth_image > self.clipping_distance) | (depth_image <= 0), grey_color, color_image)
                images = np.dstack((color_image,depth_image))
                
                
                results = classifier.classify_image(images)
                plt.show()
                if results is not None and images is not None:
                    if pipe:
                        pipe.SendPositions(results, images)
                
                
                cv2.namedWindow('Align Example', cv2.WINDOW_NORMAL)
                cv2.imshow('Align Example', image)
                if output:
                    key = cv2.waitKey(1)
                    # Press esc or 'q' to close the image window
                    if key & 0xFF == ord('q') or key == 27:
                        cv2.destroyAllWindows()
                        break
        finally:
            self.pipeline.stop()

    def predict_distance(self,aligned_depth_frame):
        """calculates mean distance in area of x_depth and y_depth +- sample_size"""
        depth = np.asanyarray(aligned_depth_frame.get_data())
        # Crop depth data:
       
        #cv2.imshow(depth)
        depth = depth[(self.x_depth-self.sample_size):(self.x_depth+self.sample_size),(self.y_depth-self.sample_size):(self.y_depth+self.sample_size)].astype(float)

        # Get data scale from the device and convert to meters
        depth_scale = self.profile.get_device().first_depth_sensor().get_depth_scale()
        depth = depth * depth_scale
        dist,_,_,_ = cv2.mean(depth)
        return dist




def visualize(image, keypoint_with_scores):

    # Visualize the predictions with image.
    display_image = tf.expand_dims(image, axis=0)
    display_image = tf.cast(tf.image.resize_with_pad(
        display_image, 480,480), dtype=tf.int32)
    output_overlay = draw_prediction_on_image(
        np.squeeze(display_image.numpy(), axis=0), keypoint_with_scores)

    plt.figure(figsize=(5, 5))
    plt.imshow(output_overlay)
    _ = plt.axis('off')
    
    
def main():
    a = RealSencePipeline()
    a.run()
    # with RealSencePipeline() as pipe:
    #     pipe.run()
    
if __name__ == "__main__":
    
    main()