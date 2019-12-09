using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    private ShopController shop;
    private float hold = 1.0f;
    private bool isDown = false, isWaiting = true;

    private void Start() {
        this.shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopController>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        StartCoroutine(CountHoldSeconds(this.hold));
        this.isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        this.isDown = false;

        if (this.isWaiting) {
            StopCoroutine(CountHoldSeconds(this.hold));
            this.shop.PurchaseTrap(name.Substring(0, name.Length -  5));
        }
    }

    private IEnumerator CountHoldSeconds(float hold) {
        this.isWaiting = true;
        yield return new WaitForSeconds(hold);
        this.isWaiting = false;
        
        if (this.isDown) {
            this.shop.SellTrap(name.Substring(0, name.Length - 5));
        }
    }
}
