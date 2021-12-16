
import json
import time
import os
from matplotlib import pyplot as plt

def read_file(filename):
    names,x,y,z = [],[],[],[]
    with open(filename, 'r') as f:
        data = json.load(f)
        
        for landmark in data.items():
            
            x.append(landmark[1][0])
            y.append(landmark[1][1])
            z.append(landmark[1][2])
            names.append(landmark[0])
            
    return x,y,z,names


def plot_landmarks(x,y,z,names):
    
    fig = plt.figure()
    ax = fig.add_subplot(projection="3d")
    #ax.scatter(x,y,z,names)
    for name,xi,yi,zi in zip(names,x,y,z):
        ax.text(xi,yi,zi,name,color="red")
        ax.scatter(xi,yi,zi,color="blue")
    plt.show()
    #plt.savefig("")
    
    
def main():
    file ="/Users/macbook/Documents/KI_Master/AR-VR/arvr-projekt-modularbeit/src/Modules/PoseClassifier/landmarks.txt"
    x,y,z,names = read_file(file)
    plot_landmarks(x,y,z,names)
    str = [f"{name}: {xi},{yi},{zi}" for xi,yi,zi,name in zip(x,y,z,names)]
    print("\n".join(str))
if __name__== "__main__":
    main()