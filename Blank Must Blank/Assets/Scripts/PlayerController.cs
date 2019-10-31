using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private CanvasController canvasController;
    private int cogs;
    private Dictionary<Trap, int> markers;

    private ShopController shopController;

    public enum Trap {
        SPIKES, ACID, TURRET
    }

    private void Start() {
        this.markers = new Dictionary<Trap, int>();

        List<Trap> traps = new List<Trap>{ Trap.SPIKES, Trap.ACID, Trap.TURRET };
        traps.ForEach(trap => this.markers.Add(trap, 0));

        this.canvasController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>();
        this.shopController = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopController>();
    }

    public void ShopTrap(Trap trap, bool isPurchase) {
        if (isPurchase && this.markers[trap] < 5) 
            this.markers[trap]++;

        if (!isPurchase && this.markers[trap] > 0)
            this.markers[trap]--;

        this.shopController.UpdateInventoryLabel(trap, this.markers[trap]);
    }

    public void CollectCogs(int amount) {
        this.cogs += amount;
        this.canvasController.UpdateCogs(this.cogs);
    }
}
