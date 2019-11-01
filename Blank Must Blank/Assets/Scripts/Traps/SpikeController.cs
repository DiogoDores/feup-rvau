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

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(trapAnimation.GetBool("trigger"));
        if (!this.active) {
            Debug.Log(this.counter);
            this.counter++;
            if (this.counter >= resetTime * 60) {
                this.active = true;
                this.counter = 0;
            }
        }
        else if (trapAnimation.GetBool("trigger")) {
            this.trapCounter++;
            if (this.trapCounter >= 150) {
                this.active = false;
                this.trapAnimation.SetBool("trigger", false);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Robot") {
            if (this.active) {
                other.gameObject.GetComponent<RobotController>().TakeDamage(this.damage, other.gameObject);
                trapAnimation.SetBool("trigger", true);
            }
        }
    }
}
