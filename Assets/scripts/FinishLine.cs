using UnityEngine;
using System.Collections.Generic;

public class FinishLine : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HorseRaceTracker>(out var horse)){
            //‚±‚±‚ÅŽæ“¾‚µ‚½horse‚ª‚»‚Ì”nŒÂ‘Ì‚ÌHorseRaceTracker
            if (!horse.hasFinished)
            {
                RaceManager.Instance.OnHorseFinished(horse);
            }
        }
    }
}
