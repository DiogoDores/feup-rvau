using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftController : MonoBehaviour {
    public int playerHP = 10;
    private PlayerController player;

    private void Start() {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Robot") {
            this.player.UpdateHealth(10);
        }
    }
}
