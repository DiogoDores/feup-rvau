import cv2
import numpy as np

INDEX = 1
MAX_POINTS = 8

def print_instructions():
    global INDEX

    if (INDEX <= MAX_POINTS):
        print(f'[{INDEX}]:', end = ' ', flush=True)
        INDEX = INDEX + 1

    else:
        print('All points registered. Please press ENTER.')

def mouse_handler(event, x, y, flags, data):
    if event == cv2.EVENT_LBUTTONDOWN:
        cv2.circle(data['im'], (x, y), 3, (0, 0, 255), 5, 16)
        cv2.imshow("Image", data['im'])
        data['points'].append([x, y])

        print(f'Selected. Positioned at ({x}, {y}).')
        print_instructions()

    if event == cv2.EVENT_RBUTTONDOWN:
        data['points'].append([-1, -1])

        print('Ignored.')
        print_instructions()

def get_field_points(im):
    # Set up data to send to mouse handler
    data = {}

    data['im'] = im.copy()
    data['points'] = []

    # Set the callback function for any mouse event
    cv2.imshow("Image", im)
    
    print_instructions()

    cv2.setMouseCallback("Image", mouse_handler, data)
    cv2.waitKey(0)

    # Convert array to np.array
    points = np.vstack(data['points']).astype(float)

    return points


def get_player(im):

    # Set up data to send to mouse handler
    data = {}
    data['im'] = im.copy()
    data['points'] = []

    # Set the callback function for any mouse event
    cv2.imshow("Image", im)
    cv2.setMouseCallback("Image", mouse_handler, data)
    cv2.waitKey(0)

    # Convert array to np.array
    point = np.vstack(data['points']).astype(float)

    return point
