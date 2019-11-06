using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
    public int damage;
    public Transform head;

    private GameObject target;
    private float lockSpeed = 2.0f;

    private void Start() {
        StartCoroutine(Fire());
    }

    private void Update() {

        if (target != null) {
            Vector3 dir = this.target.transform.position - this.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = lookRotation.eulerAngles;
            head.rotation = Quaternion.Slerp(this.head.rotation, Quaternion.Euler(180f, rotation.y, 0f), Time.deltaTime * this.lockSpeed);
        }
    }

    private GameObject FindClosestRobot() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 7.0f);
        float minDistance = Mathf.Infinity;
        GameObject nearest = null;

        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject.tag != "Robot") {
                continue;
            }

            float thisDistance = (transform.position - colliders[i].transform.position).sqrMagnitude;
            
            if (thisDistance < minDistance) {
                minDistance = thisDistance;
                nearest = colliders[i].gameObject;
            }
        }  

        return nearest;
    }

    private IEnumerator Fire() {
        while (true) {
            this.target = FindClosestRobot();
            if (this.target != null) {
                this.target.GetComponent<RobotController>().TakeDamage(this.damage, gameObject);
            }
            yield return new WaitForSeconds(2.9f);
        }
    }
}
