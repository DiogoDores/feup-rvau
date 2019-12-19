import argparse
import numpy as np
import cv2

# construct the argument parser and parse the arguments
"""
ap = argparse.ArgumentParser()
ap.add_argument("-i", "--image", required=True, help="path to input image")
args = vars(ap.parse_args())
"""

img = cv2.imread('plane_field.png')

scale_percent = 20
height = int(img.shape[0] * scale_percent / 100)
width = int(img.shape[1] * scale_percent / 100)
dim = (width, height)

img = cv2.resize(img, dim, interpolation = cv2.INTER_AREA)


hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

# Masks out any green color
lower_blue = np.array([100,150,0])
upper_blue = np.array([140,255,255])

mask = cv2.inRange(hsv, lower_blue, upper_blue)
blue = cv2.bitwise_and(img, img, mask=mask)

blue_points = []

contours, hierarchy = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
for c in contours:
    x, y, w, h = cv2.boundingRect(c)
    blue_points.append((int(round(x + w/2)), int(round(y + h/2))))
    cv2.rectangle(blue, (x, y), (x + w, y + h), (0, 0, 255), 1)

print(blue_points)

cv2.imshow('image', mask)
cv2.imshow('blue', blue)
cv2.waitKey(0)
