using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SupabaseHorseLoader : MonoBehaviour
{

    public void LoadAllHorses(System.Action<List<HorseData>> onLoaded)
    {
        StartCoroutine(GetHorses(onLoaded));
    }

    IEnumerator GetHorses(System.Action<List<HorseData>> onLoaded)
    {
        string url = $"{SupabaseConfig.Url}/rest/v1/horses?select=*";

        var request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", SupabaseConfig.AnonKey);
        request.SetRequestHeader("Authorization", $"Bearer {SupabaseConfig.AnonKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string wrappedJson = "{\"items\":" + request.downloadHandler.text + "}";
            var wrapper = JsonUtility.FromJson<HorseDataListWrapper>(wrappedJson);
            onLoaded(wrapper.items);
        }
        else
        {
            Debug.LogError("ì«Ç›çûÇ›é∏îs: " + request.error);
        }
    }
}

[System.Serializable]
public class HorseDataListWrapper
{
    public List<HorseData> items;
}
