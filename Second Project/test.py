#!/usr/bin/env python

import cv2
import numpy as np
from utils import *

import sys

def get_blue_points():
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
        blue_points.append([int(round(x + w/2)), int(round(y + h/2))])
        cv2.rectangle(blue, (x, y), (x + w, y + h), (0, 0, 255), 1)

    return blue_points


if __name__ == '__main__':

    # Read source image.
    im_src = cv2.imread('field.jpg')
    size = im_src.shape

    # Create a vector of source points.

    # Fetch blue points.
    blue_points = get_blue_points()
    pts_src = np.array(blue_points, dtype=float)

    # Read destination image
    im_dst = cv2.imread('field.jpg')

    # Get four corners of the field
    print('Select the 4 corners of the field and then press ENTER')
    pts_dst = get_four_points(im_dst)
    print(pts_dst)

    """# Get offside player
    print('Select the offside player and then press ENTER')
    point = get_player(im_dst)
    print(point)"""

    # Create a vector of source points.
    """
    pts_dst = np.array(
        [
            [pts_field[0][0] - 0.5, pts_field[0] - 0.5, pts_field[0][1]],
            [size[1] - 1, 0],
            [size[1] - 1, size[0] - 1],
            [0, size[0] - 1]
        ], dtype=float
    )
    """

    # Calculate Homography between source and destination points
    h, status = cv2.findHomography(pts_src, pts_dst)

    # Warp source image
    im_temp = cv2.warpPerspective(
        im_src, h, (im_dst.shape[1], im_dst.shape[0]))

    # Black out polygonal area in destination image.
    cv2.fillConvexPoly(im_dst, pts_dst.astype(int), 0, 16)

    # Add warped source image to destination image.
    im_dst = im_dst + im_temp

    # Display image.
    cv2.imshow("Image", im_dst)
    cv2.waitKey(0)
