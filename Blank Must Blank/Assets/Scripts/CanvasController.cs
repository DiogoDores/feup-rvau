using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    public Text cogsText, waveTime, waveName;

    public void UpdateCogs(int amount) {
        this.cogsText.text = amount.ToString();
    }

    public void UpdateWaveName(int wave) {
        this.waveName.text = "Wave " + wave.ToString();
    }

    public void UpdateWaveTime(float time, bool isPreparation) {
        if (isPreparation)
            this.waveTime.color = Color.red;
        else
            this.waveTime.color = Color.white;

        this.waveTime.text = (Mathf.Ceil(time * 10) / 10).ToString("0.0");
    }
}
