#!/usr/bin/env python

import cv2
import numpy as np
import argparse
import math
from utils import *

import sys
font = cv2.FONT_HERSHEY_SIMPLEX

def get_player_mask(img):
    hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
    mask = cv2.inRange(hsv, (25, 20, 100), (70, 255, 255))
    return cv2.bitwise_not(mask)

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
        
        cv2.putText(img, str(index), (int(round(x + w/2)), int(round(y + h/2))),
                    font, 0.5, (255, 255, 255), 2, cv2.LINE_AA)
        cv2.rectangle(blue, (x, y), (x + w, y + h), (0, 0, 255), 1)
        index += 1

    cv2.imshow("Field", img)

    return blue_points

if __name__ == '__main__':

    # construct the argument parser and parse the arguments
    ap = argparse.ArgumentParser()
    ap.add_argument("-m", "--mode", type=int, required=True,
                    help="Which mode to use: 1 - Offside Player; 2 - Free kick; 3 - Distance to goal;")
    ap.add_argument("-i", "--image", required=True,
                    help="Source Image Path")
    args = vars(ap.parse_args())

    mode = args["mode"]
    im_src = args["image"]

    # Fetch blue points.
    im_dst = cv2.imread('plane_field.png')
    blue_points = get_blue_points(im_dst)
    pts_dst = np.array(blue_points, dtype=float)

    # Read source image.
    im_src = cv2.imread(im_src)
    size = im_src.shape

    # Get four corners of the field
    print('Select the homography points of the field and then press ENTER')
    pts_src = get_field_points(im_src)

    # Get offside player
    print('Select the offside player and then press ENTER')
    player = get_player(im_src)

    #Get Goal if mode is 3
    if mode == 3:
        print('Select the middle of the goal line')
        goal = get_player(im_src)

    height = int(im_dst.shape[0] * 20 / 100)
    width = int(im_dst.shape[1] * 20 / 100)
    dim = (width, height)
    im_dst = cv2.resize(im_dst, dim, interpolation=cv2.INTER_AREA)

    # Calculate Homography between source and destination points
    h, status = cv2.findHomography(pts_src, pts_dst)

    # Warp source image to fit the Illustrator one.
    im_temp = cv2.warpPerspective(
        im_src, h, (im_dst.shape[1], im_dst.shape[0]))

    # Black out polygonal area in destination image.
    cv2.fillConvexPoly(im_dst, np.array([[0, 0], [size[1], 0], [
                       size[1], size[0]], [0, size[0]]], dtype=float).astype(int), 0)

    # Add warped source image to destination image.
    im_dst = im_dst + im_temp

    # Convert player based on perspective transform.
    player_new = cv2.perspectiveTransform(np.array([player]), h)

    # Get player mask.
    player_mask = get_player_mask(im_src)

    if mode == 1:
        player_new = [[(player_new[0][0][0]), 0.0], [
            (player_new[0][0][0]), size[0]]]
    elif mode == 2:
        x = (player_new[0][0][0])
        y = (player_new[0][0][1])

        delta = abs(pts_dst[6][0] - pts_dst[7][0])
        radius_px = delta * 9.15 / 16.5
        player_new = [[x - radius_px, y - radius_px], [x + radius_px, y - radius_px],
                      [x + radius_px, y + radius_px], [x - radius_px, y + radius_px]]
    elif mode == 3:
        x = (player_new[0][0][0])
        y = (player_new[0][0][1])

        player_new = [[x,y]]
        goal_new = cv2.perspectiveTransform(np.array([goal]), h)
        
        x2 = (goal_new[0][0][0])
        y2 = (goal_new[0][0][1])

        goal_new = [[x2,y2]]

        dist = math.sqrt((x-x2)**2 + (y-y2)**2)
        delta = abs(pts_dst[6][0] - pts_dst[7][0])
        real_dist = dist * 16.5 / delta

    h_inv, status = cv2.findHomography(pts_dst, pts_src)
    player_best = cv2.perspectiveTransform(np.array([player_new]), h_inv)

    if mode == 3:
        goal_best = cv2.perspectiveTransform(np.array([goal_new]), h_inv)

    #player_new = cv2.perspectiveTransform(np.array([player]), h_inv)

    # Warp source image
    #im_temp = cv2.warpPerspective(im_dst, h_inv, (im_src.shape[1], im_src.shape[0]))
    #im_src = im_temp

    fg_back_inv = cv2.bitwise_and(im_src, im_src, mask=player_mask)

    if mode == 1:
        player_p1, player_p2 = player_best[0].astype(int)
        im_name = "Offside"

        im_src = cv2.line(im_src, (player_p1[0], player_p1[1]), (player_p2[0], player_p2[1]), (255, 0, 0), 2)

    elif mode == 2:
        player_best = player_best[0].astype(int)
        box = cv2.minAreaRect(player_best)
        im_name = "Free kick"

        im_src = cv2.ellipse(im_src, box, (255, 0, 0), 2)

    elif mode == 3:
        player_p1 = player_best[0].astype(int)
        player_p2 = goal_best[0].astype(int)
        im_name = "Distance"

        im_dst = cv2.arrowedLine(im_src, (player_p1[0][0], player_p1[0][1]), (player_p2[0][0], player_p2[0][1]), (255,0,0), 2, cv2.LINE_AA, 0, 0.05)
        im_dst = cv2.arrowedLine(im_src, (player_p2[0][0], player_p2[0][1]), (player_p1[0][0], player_p1[0][1]), (255,0,0), 2, cv2.LINE_AA, 0, 0.05)
        cv2.putText(im_src, "{} meters".format(round(float(real_dist),1)), (120,120),
                    font, 0.5, (255, 255, 255), 2, cv2.LINE_AA)

    im_src = cv2.bitwise_or(im_src, fg_back_inv)
    cv2.imshow(im_name, im_src)

    cv2.waitKey(0)
