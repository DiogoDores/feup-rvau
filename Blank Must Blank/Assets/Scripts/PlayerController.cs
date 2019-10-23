using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private CanvasController canvasController;

    private int cogs;

    private void Start() {
        this.canvasController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>();
    }

    public void CollectCogs(int amount) {
        this.cogs += amount;
        this.canvasController.UpdateCogs(this.cogs);
    }
}
