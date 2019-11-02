using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Coord
{
    public int lon;
    public int lat;
}

[Serializable]
public class Weather
{
    public int id;
    public string main;
    public string description;
    public string icon;
}

[Serializable]
public class Main
{
    public double temp;
    public int pressure;
    public int humidity;
    public double temp_min;
    public double temp_max;
}

[Serializable]
public class Wind
{
    public double speed;
    public int deg;
    public double gust;
}

[Serializable]
public class Clouds
{
    public int all;
}

[Serializable]
public class Sys
{
    public int type;
    public int id;
    public string country;
    public int sunrise;
    public int sunset;
}

[Serializable]
public class RootObject
{
    public Coord coord;
    public List<Weather> weather;
    public string @base;
    public Main main;
    public Wind wind;
    public Clouds clouds;
    public int dt;
    public Sys sys;
    public int timezone;
    public int id;
    public string name;
    public int cod;
}

public class SkyController : MonoBehaviour {
    public float pollingRate;
    private string baseURL, apiKey;
    public ParticleSystem rainParticles;

    private void Start() {
        this.baseURL = "https://api.openweathermap.org/data/2.5/weather?";
        this.apiKey = "&appid=1d53d1f033b31b103f3e91ac47f9c580";

        StartCoroutine(GetCoordinates());
    }

    private IEnumerator GetCoordinates() {
        if (!Input.location.isEnabledByUser) {
            Debug.LogWarning("Location isn't enabled by the user.");
            StartCoroutine(GetRequest(41.1f, 8.6f));
            yield break;
        }

        Input.location.Start();

        // Wait until service initializes.
        int maxWait = 5;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1) {
            Debug.LogWarning("Device location request timed out.");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed) {
            Debug.LogWarning("Unable to determine device location.");
            yield break;
        }

        float latitude = Input.location.lastData.latitude;
        float longitude = Input.location.lastData.longitude;

        Input.location.Stop();
        StartCoroutine(GetRequest(latitude, longitude));
    }

    private IEnumerator GetRequest(float latitude, float longitude) {
        while (true) {
            string uri = this.baseURL + "lat=" + latitude.ToString() + "&lon=" + longitude.ToString() + this.apiKey;

            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError) {
                    Debug.LogError("Error: " + webRequest.error);
                    yield return null;
                }
                Debug.Log(webRequest.downloadHandler.text);

                RootObject obj = JsonUtility.FromJson<RootObject>(webRequest.downloadHandler.text);
                Debug.Log(obj.weather[0].main);

                if (obj.weather[0].main.Equals("Rain")) {
                    this.rainParticles.Play();
                } else {
                    this.rainParticles.Stop();
                }
            }

            yield return new WaitForSeconds(this.pollingRate);
        }
    }
}
