using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftController : MonoBehaviour
{
    public int playerHP = 10;
    public GameObject rift;
    // Start is called before the first frame update
    void Start()
    {
        rift = GameObject.Find("Rift");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Robot" && this.playerHP > 0) {
            this.playerHP--;
            Debug.Log(playerHP);
        }
    }
}
