using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VirtualButtonController : MonoBehaviour, IVirtualButtonEventHandler {
    public GameObject vb;

    // Start is called before the first frame update
    void Start() {
        vb.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb) {
        Debug.Log("Clicked.");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb) {
        Debug.Log("Released.");
    }

    // Update is called once per frame
    void Update() {
        
    }
}
