using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class HorseData
{
    public string horse_name;
    public float[] genome;
}

public class SupabaseHorseUploader : MonoBehaviour
{

    public void SaveHorse(HorseData data)
    {
        StartCoroutine(PostHorse(data));
    }

    IEnumerator PostHorse(HorseData data)
    {
        string json = JsonUtility.ToJson(data);
        string url = $"{SupabaseConfig.Url}/rest/v1/horses";

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", SupabaseConfig.AnonKey);
        request.SetRequestHeader("Authorization", $"Bearer {SupabaseConfig.AnonKey}");
        request.SetRequestHeader("Prefer", "return=representation");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("ï€ë∂ê¨å˜: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("ï€ë∂é∏îs: " + request.error + " / " + request.downloadHandler.text);
        }
    }
}
