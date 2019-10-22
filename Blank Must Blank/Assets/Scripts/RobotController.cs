using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RobotController : MonoBehaviour {
    public Transform riftTransform;
    public float health = 100;

    [Header("UI")]
    public Image healthBar;

    private void Start() {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = this.riftTransform.position;
    }

    private void Update() {
        if (this.health <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Rift") {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount) {
        this.health -= amount;
        this.healthBar.fillAmount = this.health / 100.0f;
    }

    public float GetHealth() => this.health;
}
