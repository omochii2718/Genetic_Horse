//Œ‹‰Êƒf[ƒ^‚Ìó‚¯“n‚µ
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RaceResultData:MonoBehaviour
{
    public static RaceResultData instance;
    public List<HorseResultEntry> results = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void SetResult(List<HorseRaceTracker> finishOrder)
    {
        results.Clear();
        for (int i = 0; i < finishOrder.Count; i++)
        {
            results.Add(new HorseResultEntry
            {
                rank = i + 1,
                horseName = finishOrder[i].horseName,
                finishTime = finishOrder[i].finishTime
            });
        }
    }
}

[System.Serializable]
public class HorseResultEntry
{
    public int rank;
    public string horseName;
    public float finishTime;
}
