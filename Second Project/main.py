#!/usr/bin/env python

import cv2
import numpy as np
from utils import *

import sys


def get_blue_points(img):

    scale_percent = 20
    height = int(img.shape[0] * scale_percent / 100)
    width = int(img.shape[1] * scale_percent / 100)
    dim = (width, height)
    index = 1

    img = cv2.resize(img, dim, interpolation=cv2.INTER_AREA)

    hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
    rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

    # Masks out any green color
    lower_blue = np.array([100, 150, 0])
    upper_blue = np.array([140, 255, 255])

    mask = cv2.inRange(hsv, lower_blue, upper_blue)
    blue = cv2.bitwise_and(img, img, mask=mask)

    blue_points = []

    contours, hierarchy = cv2.findContours(
        mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    for c in contours:
        x, y, w, h = cv2.boundingRect(c)
        blue_points.append([int(round(x + w/2)), int(round(y + h/2))])
        font = cv2.FONT_HERSHEY_SIMPLEX
        cv2.putText(img, str(index), (int(round(x + w/2)), int(round(y + h/2))),
                    font, 0.5, (255, 255, 255), 2, cv2.LINE_AA)
        cv2.rectangle(blue, (x, y), (x + w, y + h), (0, 0, 255), 1)
        index += 1

    cv2.imshow("Field", img)

    return blue_points


if __name__ == '__main__':

    # Fetch blue points.
    im_dst = cv2.imread('plane_field.png')
    blue_points = get_blue_points(im_dst)
    pts_dst = np.array(blue_points, dtype=float)
    print(pts_dst)

    # Read source image.
    im_src = cv2.imread('field.jpg')
    size = im_src.shape

    # Get four corners of the field
    print('Select the homography points of the field and then press ENTER')
    pts_src = get_field_points(im_src)
    print(pts_src)

    # Get offside player
    print('Select the offside player and then press ENTER')
    point = get_player(im_src)
    print(point[0][0])

    height = int(im_dst.shape[0] * 20 / 100)
    width = int(im_dst.shape[1] * 20 / 100)
    dim = (width, height)
    im_dst = cv2.resize(im_dst, dim, interpolation=cv2.INTER_AREA)

    # Calculate Homography between source and destination points
    h, status = cv2.findHomography(pts_src, pts_dst)

    # Warp source image
    im_temp = cv2.warpPerspective(
        im_src, h, (im_dst.shape[1], im_dst.shape[0]))

    # Black out polygonal area in destination image.
    cv2.fillConvexPoly(im_dst, np.array([[0, 0], [size[1], 0], [
                       size[1], size[0]], [0, size[0]]], dtype=float).astype(int), 0)

    # Add warped source image to destination image.
    im_dst = im_dst + im_temp

    # Display image.
    cv2.imshow("Image", im_dst)

    # TODO - ADAPT PLAYER POINT TO NEW HOMOGRAPHY
    im_dst = cv2.line(im_dst, (point[0][1].astype(int), 0), (
                      point[0][1].astype(int), size[0]), (255, 0, 0), 4)
    cv2.imshow("Image", im_dst)

    # TODO - INVERSE HOMOGRAPHY AKA GET FINAL IMAGE
    H_inv, status = cv2.findHomography(pts_dst, pts_src)

    # Warp source image
    im_temp = cv2.warpPerspective(
        im_dst, H_inv, (im_src.shape[1], im_src.shape[0]))

    im_src = im_temp

    cv2.imshow("Image", im_src)

    cv2.waitKey(0)
