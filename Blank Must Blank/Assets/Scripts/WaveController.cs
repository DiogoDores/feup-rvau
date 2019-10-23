using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {
    public int currentWave = 1;
    public Transform spawnPoint;
    public List<WaveSettings> waves = new List<WaveSettings>();
    private List<Coroutine> activeCoroutines = new List<Coroutine>();
    private CanvasController canvasController;

    public float preparationTime;
    private float copyPreparationTime;

    private void Start() {
        this.canvasController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>();
        this.copyPreparationTime = this.preparationTime;
    }

    [System.Serializable]
    public class WaveSettings {
        public float remainingTime;
        public List<RobotSettings> robotSettings;

        public WaveSettings(float remainingTime, List<RobotSettings> robotSettings) {
            this.remainingTime = remainingTime;
            this.robotSettings = robotSettings;
        }
    }

    [System.Serializable]
    public class RobotSettings {
        public GameObject obj;
        public float spawnRate;

        public RobotSettings(GameObject obj, float spawnRate) {
            this.obj = obj;
            this.spawnRate = spawnRate;
        }
    }

    private void Update() {
        // Elapse time if there's any preparation time available.
        if (this.preparationTime > 0.0f) {
            this.preparationTime -= Time.deltaTime;
            this.canvasController.UpdateWaveTime(this.preparationTime, true);

            if (this.preparationTime <= 0.0f)
                this.activeCoroutines = StartWave();
        }
        else {
            float delta = this.waves[this.currentWave - 1].remainingTime;
            

            if (delta > 0.0f) {
                this.waves[this.currentWave - 1].remainingTime -= Time.deltaTime;
                
                // Handle rounding and negative issues.
                if (this.waves[this.currentWave - 1].remainingTime < 0.0f) {
                    delta = this.waves[this.currentWave - 1].remainingTime = 0.0f;
                }

                this.canvasController.UpdateWaveTime(delta, false);
            } else {
                this.activeCoroutines.ForEach(c => StopCoroutine(c));
                this.activeCoroutines.Clear();

                if (this.currentWave < this.waves.Count && GameObject.FindGameObjectsWithTag("Robot").Length == 0) {
                    this.EnablePreparationPhase();
                }
            }
        }
    }

    public List<Coroutine> StartWave() {
        List<Coroutine> crtn = new List<Coroutine>();

        this.waves[this.currentWave - 1].robotSettings.ForEach(robot => {
            crtn.Add(StartCoroutine(StartSpawningRobots(robot)));
        });

        return crtn;
    }

    public IEnumerator StartSpawningRobots(RobotSettings robot) {
        while (true) {
            Instantiate(robot.obj, this.spawnPoint);
            yield return new WaitForSeconds(robot.spawnRate);
        }
    } 

    public void EnablePreparationPhase() {
        this.currentWave++;
        this.canvasController.UpdateWaveName(this.currentWave);
        this.preparationTime = this.copyPreparationTime;
    }
}
