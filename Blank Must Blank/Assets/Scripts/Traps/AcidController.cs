using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidController : MonoBehaviour {
    public ParticleSystem particles;
    private float activation = 1.6f, delay = 1.0f;
    public int damage = 8;

    private void Start() {
        StartCoroutine(Enable(this.delay));
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Robot")) {
            other.gameObject.GetComponent<RobotController>().TakeDamage(this.damage, gameObject);
        }
    }

    public IEnumerator Enable(float delay) {
        while (true) {
            GetComponent<BoxCollider>().enabled = true;
            this.particles.Play();
            yield return new WaitForSeconds(activation);
            GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(delay);
        }
    }
}
