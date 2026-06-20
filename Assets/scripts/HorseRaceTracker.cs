//各馬のGameObjectに1つずつ付ける前提のスクリプト
using UnityEngine;
using System.Collections.Generic;

public class HorseRaceTracker : MonoBehaviour
{
    public string horseName;
    public bool isPlayer;
    public bool hasFinished = false;
    public float finishTime;
}
