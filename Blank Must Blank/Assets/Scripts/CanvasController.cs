using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    public Text cogsText, waveTime, waveName;

    private PlayerController player;

    private void Start() {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update() {
        this.cogsText.text = this.player.GetCogs().ToString();
    }

    public void UpdateWaveName(int wave) {
        this.waveName.text = "Wave " + wave.ToString();
        Debug.Log(this.waveName.text);
    }

    public void UpdateWaveTime(float time, bool isPreparation) {
        if (isPreparation)
            this.waveTime.color = Color.red;
        else
            this.waveTime.color = Color.white;

        this.waveTime.text = (Mathf.Ceil(time * 100) / 100).ToString("0.00");
    }
}
