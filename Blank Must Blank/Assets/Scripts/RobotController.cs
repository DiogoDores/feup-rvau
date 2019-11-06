using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RobotController : MonoBehaviour {
    public Transform riftTransform;
    public float health = 100;
    private int currentHealth;
    public int bounty = 5;

    private PlayerController playerController;
    public ParticleSystem boltParticles;

    [Header("UI")]
    public Image healthBar;

    private void OnEnable() {
        this.currentHealth = (int) this.health;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = this.riftTransform.position;
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void TakeDamage(int amount, GameObject source) {
        this.health -= amount;
        this.healthBar.fillAmount = this.health / this.currentHealth;

        if (this.health <= 0) {
            PlayDeathParticleEffect();
            if (!source.CompareTag("Rift")) this.playerController.UpdateCogs(this.bounty);
            gameObject.SetActive(false);
        }
    }

    private void PlayDeathParticleEffect() {
        ParticleSystem particles = Instantiate(this.boltParticles, transform.position, Quaternion.identity);
        particles.transform.parent = transform.parent;
        Destroy(particles.gameObject, particles.main.duration);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Rift") {
            this.TakeDamage(int.MaxValue, other.gameObject);
        }
    }

    public void Revive() {
        this.currentHealth = (int) this.health;
        this.healthBar.fillAmount = this.currentHealth / this.health;
    }

    public float GetHealth() => this.health;
}
