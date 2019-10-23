using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RobotController : MonoBehaviour {
    public Transform riftTransform;
    public int health = 100;
    public int bounty = 5;

    private PlayerController playerController;

    [Header("UI")]
    public Image healthBar;

    private void Start() {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = this.riftTransform.position;
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void TakeDamage(int amount, GameObject source) {
        this.health -= amount;
        this.healthBar.fillAmount = this.health / 100.0f;

        if (this.health <= 0) {
            if (!source.CompareTag("Rift")) this.playerController.CollectCogs(this.bounty);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Rift") {
            this.TakeDamage(int.MaxValue, other.gameObject);
        }
    }


    public float GetHealth() => this.health;
}
