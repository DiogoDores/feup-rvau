using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftController : MonoBehaviour {
    public int playerHP = 10;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Robot" && this.playerHP > 0) {
            this.playerHP--;
        }
    }
}
