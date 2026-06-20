using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager:MonoBehaviour{
    public static RaceManager Instance;
    public List<HorseRaceTracker> finishOrder = new List<HorseRaceTracker>();
    public int totalHorseCount;

    public void OnHorseFinished(HorseRaceTracker horse)
    {
        horse.hasFinished = true;
        horse.finishTime = Time.time;
        finishOrder.Add(horse);

        if (finishOrder.Count >= totalHorseCount)
        {
            EndRace();
        }
    }

    private void EndRace()
    {
        RaceResultData.Instance.SetResult(finishOrder);
        SceneLoader.Instance.LoadResultScene();
    }
    
}
