import argparse
import numpy as np
import cv2
#import matplotlib.pylab as plt

img = cv2.imread('field.jpg')
hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

# Masks out any green color
green_mask = cv2.inRange(hsv, (25, 100, 100), (70, 255, 255))
player_mask = cv2.bitwise_not(green_mask)

players = cv2.bitwise_and(img, img, mask=player_mask)
cv2.line(img, (10, 10), (800, 300), (255, 0, 0), 5)
final = cv2.bitwise_or(img, players)

cv2.imshow('image', players)
cv2.imshow('green', green_mask)
cv2.imshow('final', final)
cv2.waitKey(0)
