using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting.FullSerializer;
using System;

public class OpenWeatherApi : MonoBehaviour
{
    [SerializeField]
    string Longitute, Latitute;
    string url = "http://api.weatherapi.com/v1/current.json?key=33f9766800b54ba08df95607241701&q=Antalya";

    public Animator animator;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 100), "Check weather"))
        {
            OnlineWeatherUpdate();

        }
    }

    void OnlineWeatherUpdate() => StartCoroutine(ShowandLoadWeatherDate());
    IEnumerator ShowandLoadWeatherDate()
    {
        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            var JsonData = JsonUtility.FromJson<WeatherResponseModel>(www.downloadHandler.text); 
            Debug.Log("Antalyada Hava Sýcaklýðý: " + JsonData.current.temp_c);
            Debug.Log("Antalyada Nem Oraný: " + JsonData.current.humidity);

            if(JsonData.current.cloud != 0)
            {
                animator.SetBool("cloud", true);
            }
            else
            {
                animator.SetBool("cloud", false);
            }



            if (JsonData.current.uv != 4)
            {
                animator.SetBool("uv", true);
            }
            else
            {
                animator.SetBool("uv", false);
            }
        }
    }
}

[Serializable]
public class WeatherResponseModel
{
    public Current current;
    public int id;
}

[Serializable]
public class Current
{
    public float temp_c; // hava sýcaklýðý celcius olarak
    public float humidity; // nem oraný
    public float cloud; // bulut durumu
    public float uv; //uv durumu

    //public float degerAdi; // Buraya havadurumundna okuduðun deðer ve türü gelecek.
}