using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour {
    public Transform spawnPoint;

    private void Start() {
        this.Spawn();
    }

    public void Spawn() {
        gameObject.transform.position = spawnPoint.transform.position;
    }
}
