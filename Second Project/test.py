#!/usr/bin/env python

import cv2
import numpy as np
from utils import *

import sys


if __name__ == '__main__':

    # Read source image.
    im_src = cv2.imread('bar.png')
    size = im_src.shape

    # Create a vector of source points.
    pts_src = np.array(
        [
            [0, 0],
            [size[1] - 1, 0],
            [size[1] - 1, size[0] - 1],
            [0, size[0] - 1]
        ], dtype=float
    )

    # Read destination image
    im_dst = cv2.imread('field.jpg')

    # Get four corners of the field
    print('Select the 4 corners of the field and then press ENTER')
    pts_field = get_four_points(im_dst)
    print(pts_field)

    # Get offside player
    print('Select the offside player and then press ENTER')
    point = get_player(im_dst)
    print(point)

    # Create a vector of source points.
    pts_dst = np.array(
        [
            [point[0][0] - 0.5, point[0] - 0.5, pts_field[0][1]],
            [size[1] - 1, 0],
            [size[1] - 1, size[0] - 1],
            [0, size[0] - 1]
        ], dtype=float
    )

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
