using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool active = true;
    public int damage = 50;
    public float resetTime = 1.0f;
    private float counter = 0.0f;
    private float trapCounter = 0.0f;
    public Animator trapAnimation;

    private bool coroutineRunning = false;

    // Update is called once per frame
    void Update()
    {
        if (trapAnimation.GetBool("trigger") && !this.coroutineRunning) {
           StartCoroutine(activateTrap());
           this.coroutineRunning = true;
        }
    }

    private IEnumerator activateTrap(){
        yield return new WaitForSeconds(2.6f);
        this.active = true;
        this.trapAnimation.SetBool("trigger", false);
        this.coroutineRunning = false;
        GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Robot") {
            if (this.active) {
                trapAnimation.SetBool("trigger", true);
                other.gameObject.GetComponent<RobotController>().TakeDamage(this.damage, other.gameObject);
                GetComponent<BoxCollider>().enabled = false;
                this.active = false;
            }
        }
    }
}
