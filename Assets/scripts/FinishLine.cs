//FinishLineにトリガーを置き、馬が通過したら順位を記録する
using UnityEngine;
using System.Collections.Generic;

public class FinishLine : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HorseRaceTracker>(out var horse)){
            //ここで取得したhorseがその馬個体のHorseRaceTracker
            if (!horse.hasFinished)
            {
                RaceManager.Instance.OnHorseFinished(horse);
            }
        }
    }
}
