using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool active = true;
    public int damage = 50;
    public float resetTime = 2.0f;
    private float counter = 0.0f;
    private float trapCounter = 0.0f;
    public Animator trapAnimation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.active) {
            counter++;
            if (counter >= resetTime * 60)
                this.active = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(trapAnimation.GetBool("trigger"));
        if (other.tag == "Robot") {
            if (this.active) {
                other.gameObject.GetComponent<RobotController>().TakeDamage(this.damage, other.gameObject);
                trapAnimation.SetBool("trigger", true);
                this.counter++;
            }
            if (this.counter >= 5) {
                trapAnimation.SetBool("trigger", false);
            }
        }
    }
}
