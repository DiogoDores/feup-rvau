using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
    private PlayerController playerController;
    public Text invLabelSpikes, invLabelTurret, invLabelAcid;

    private void Start() {
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void UpdateInventoryLabel(PlayerController.Trap trap, int value) {
        if (trap == PlayerController.Trap.ACID) {
            this.invLabelAcid.text = value.ToString() + "/5";
        }
        else if (trap == PlayerController.Trap.TURRET) {
            this.invLabelTurret.text = value.ToString() + "/5";
        }
        else if (trap == PlayerController.Trap.SPIKES) {
            this.invLabelSpikes.text = value.ToString() + "/5";
        }
    }

    public void PurchaseTrap(string trap) {
        System.Enum.TryParse(trap, out PlayerController.Trap trapEnum);
        playerController.ShopTrap(trapEnum, true);
    }

    public void SellTrap(string trap) {
        System.Enum.TryParse(trap, out PlayerController.Trap trapEnum);
        playerController.ShopTrap(trapEnum, false);
    }
}
