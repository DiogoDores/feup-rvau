using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour {
    public Transform riftTransform;

    private void Start() {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = this.riftTransform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Rift")
            Destroy(gameObject);
    }
}
