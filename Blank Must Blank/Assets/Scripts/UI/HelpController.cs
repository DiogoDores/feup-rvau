using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class HelpController : MonoBehaviour, ITrackableEventHandler {
    private TrackableBehaviour mTrackableBehaviour;
    public Canvas helpCanvas, mainCanvas;

    private void Start() {
        this.mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        if (this.mTrackableBehaviour) {
            this.mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus) {
        Debug.Log("penis");

        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED) {
            this.helpCanvas.gameObject.SetActive(false);
            this.mainCanvas.gameObject.SetActive(true);
        }
    }
}
