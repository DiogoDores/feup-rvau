import argparse
import numpy as np
import cv2
#import matplotlib.pylab as plt

# construct the argument parser and parse the arguments
ap = argparse.ArgumentParser()
ap.add_argument("-i", "--image", required=True,
                help="path to input image")
args = vars(ap.parse_args())

img = cv2.imread('field.jpg')
hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

# Masks out any green color
low_green = np.array([25, 52, 72])
high_green = np.array([102, 255, 255])
green_mask = cv2.inRange(hsv, low_green, high_green)
green = cv2.bitwise_and(img, img, mask=green_mask)

cv2.imshow('image', img)
cv2.imshow('green', green_mask)
cv2.waitKey(0)
