using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RiftController : MonoBehaviour {
    public int riftDamage;
    private PlayerController player;

    private void Start() {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Robot") {
            this.player.UpdateHealth(this.riftDamage);
        }
    }
}
