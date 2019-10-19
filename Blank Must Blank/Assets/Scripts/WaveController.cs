using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {
    public int currentWave = 1;
    public Transform spawnPoint;
    public List<WaveSettings> waves = new List<WaveSettings>();
    private List<Coroutine> activeCoroutines = new List<Coroutine>();

    private float preparationPrecise;
    [SerializeField] private int preparationTime;

    private void Start() {
        this.preparationPrecise = (float) this.preparationTime;
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
        if (this.preparationPrecise > 0.0f) {
            this.preparationPrecise -= Time.deltaTime;
            this.preparationTime = (int) this.preparationPrecise % 60;

            if (this.preparationPrecise <= 0.0f)
                this.activeCoroutines = StartWave();
        }
        else {
            if (this.waves[this.currentWave - 1].remainingTime > 0.0f) {
                this.waves[this.currentWave - 1].remainingTime -= Time.deltaTime;
            } else {
                this.activeCoroutines.ForEach(c => StopCoroutine(c));
                this.activeCoroutines.Clear();

                if (this.currentWave < this.waves.Count) {
                    this.EnablePreparationPhase();
                }
            }
        }
    }

    public List<Coroutine> StartWave() {
        List<Coroutine> crtn = new List<Coroutine>();

        this.waves[this.currentWave - 1].robotSettings.ForEach(robot => {
            crtn.Add(StartCoroutine(StartSpawningRobot(robot)));
        });

        return crtn;
    }

    public IEnumerator StartSpawningRobot(RobotSettings robot) {
        while (true) {
            yield return new WaitForSeconds(robot.spawnRate);
            robot.obj.tag = "Robot";
            Instantiate(robot.obj, this.spawnPoint);
        }
    } 

    public void EnablePreparationPhase() {
        this.currentWave++;
        this.preparationPrecise = 5.0f;
    }
}
