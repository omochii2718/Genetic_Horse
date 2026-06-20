using UnityEngine;

SupabaseLoader.LoadAllHorses(horseList => {
    foreach (var data in horseList)
    {
        var horseObj = SpawnHorse(data.horse_name);
        horseObj.GetComponent<HorseGeneController>().ApplyGenome(data.genome);
    }
    RaceManager.Instance.StartRace();
});