using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public Canvas canvas, shop;
    private CanvasController canvasController;
    private ShopController shopController;

    private int health = 100;
    private int cogs;

    public List<Trap> traps = new List<Trap>();

    [System.Serializable]
    public class Trap {
        public enum Type {
            Spikes, Acid, Turret
        }

        public Type type;
        public int cost;
        public List<GameObject> targets;
        [HideInInspector] public int lastMarker;

        public Trap(Type type, int cost) {
            this.targets = new List<GameObject>();
            this.type = type;
            this.cost = cost;
            this.lastMarker = 0;
        }
    }

    private void Start() {
        this.canvasController = this.canvas.GetComponent<CanvasController>();
        this.shopController = this.shop.GetComponent<ShopController>();

        StartCoroutine(DisableImageTargets());
    }

    public void ShopTrap(Trap.Type trapType, bool isPurchase) {
        Trap trap = this.traps.Find(t => t.type == trapType);

        if (isPurchase && this.cogs < 0) {
            Debug.Log("Not enough money!");
            return;
        }

        if (isPurchase && trap.lastMarker < 5) {
            trap.lastMarker++;
            this.UpdateCogs(-trap.cost);
            trap.targets[trap.lastMarker - 1].GetComponent<ImageTargetBehaviour>().enabled = true;
        }

        if (!isPurchase && trap.lastMarker > 0) {
            trap.lastMarker--;
            this.UpdateCogs(trap.cost);
            trap.targets[trap.lastMarker].GetComponent<ImageTargetBehaviour>().enabled = false;
        }

        this.shopController.UpdateInventoryLabel(trap.type, trap.lastMarker);
    }

    private IEnumerator DisableImageTargets() {
        yield return new WaitForSeconds(0.1f);
        this.traps.ForEach(trap => trap.targets.ForEach(target => {
            target.GetComponent<ImageTargetBehaviour>().enabled = false;
        }));
    }

    public int GetCogs() {
        return this.cogs;
    }

    public void UpdateCogs(int amount) {
        if (this.cogs + amount < 0) {
            this.cogs = 0;
        } else {
            this.cogs += amount;
        }
    }

    public void UpdateHealth(int value) {
        
        // Handle negative values.
        if (this.health - value < 0) {
            this.health = 0;
        } else {
            this.health -= value;
        }
        
        // Update rift points label on scene.
        GameObject.Find("RiftPoints").GetComponent<TextMesh>().text = this.health.ToString();
    }
}
