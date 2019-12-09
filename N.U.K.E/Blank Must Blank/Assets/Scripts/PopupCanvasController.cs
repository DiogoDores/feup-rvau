using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCanvasController : MonoBehaviour {
    public Text hintText;
    private int currentHint = 0;
    public List<string> hints;

    private void Awake() {
        this.currentHint = 0;
        this.hintText.text = this.hints[this.currentHint];
    }

    public void NextHint() {
        if (this.currentHint < this.hints.Count - 1) {
            this.hintText.text = this.hints[++this.currentHint];
        } else {
            this.currentHint = 0;
            gameObject.SetActive(false);
        }
    }
}
