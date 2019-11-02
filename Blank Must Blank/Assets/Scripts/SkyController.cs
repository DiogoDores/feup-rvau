using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Coord {
    public double lon ;
    public double lat ;
}

[Serializable]
public class Weather {
    public int id ;
    public string main ;
    public string description ;
    public string icon ;
}

[Serializable]
public class Main {
    public double temp ;
    public int pressure ;
    public int humidity ;
    public double temp_min ;
    public double temp_max ;
}

[Serializable]
public class Wind {
    public double speed ;
    public int deg ;
}

[Serializable]
public class Rain {
    public double __invalid_name__1h ;
}

[Serializable]
public class Clouds {
    public int all ;
}

[Serializable]
public class Sys {
    public int type ;
    public int id ;
    public string country ;
    public int sunrise ;
    public int sunset ;
}

[Serializable]
public class RootObject {
    public Coord coord ;
    public List<Weather> weather ;
    public string @base ;
    public Main main ;
    public int visibility ;
    public Wind wind ;
    public Rain rain ;
    public Clouds clouds ;
    public int dt ;
    public Sys sys ;
    public int timezone ;
    public int id ;
    public string name ;
    public int cod ;
    
    public static RootObject CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<RootObject>(jsonString);
    }
}

public class SkyController : MonoBehaviour {
    public float pollingRate;
    private string weatherURL;

    private void Start() {
        this.weatherURL = "https://api.openweathermap.org/data/2.5/weather?q=Porto&appid=1d53d1f033b31b103f3e91ac47f9c580";
        StartCoroutine(GetRequest(this.weatherURL));
    }

    private IEnumerator GetRequest(string uri) {
        while (true) {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError) {
                    Debug.LogError("Error: " + webRequest.error);
                    yield return null;
                }

                RootObject obj = JsonUtility.FromJson<RootObject>(webRequest.downloadHandler.text);
                Debug.Log(webRequest.downloadHandler.text);
                Debug.Log(obj.weather[0].main);
            }
            
            yield return new WaitForSeconds(this.pollingRate);
        }
    }
}
