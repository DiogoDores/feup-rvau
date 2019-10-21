using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
    private List<GameObject> targets = new List<GameObject>();
    public GameObject headObject;

    public Transform partToRotate;

    private void Start() {
        StartCoroutine(Fire());
    }

    private void Update() {
        if (this.targets.Count > 0) {
            /*Transform target = this.targets[0].transform;
            Vector3 direction = (target.position - headObject.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            headObject.transform.rotation = Quaternion.Slerp(headObject.transform.rotation, lookRotation, Time.deltaTime * 2); */

            Vector3 dir = this.targets[0].transform.position - this.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = lookRotation.eulerAngles;
            this.partToRotate.rotation =  Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Robot")) {
            this.targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Robot")) {
            this.targets.Remove(other.gameObject);
        }
    }

    private IEnumerator Fire() {
        while (true) {
            if (this.targets.Count > 0) {
                this.targets[0].GetComponent<RobotController>().robotHP--;
                Debug.Log(this.targets[0].GetComponent<RobotController>().robotHP);
                if (this.targets[0].GetComponent<RobotController>().robotHP == 0){
                    Destroy(this.targets[0]);
                    this.targets.RemoveAt(0);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
