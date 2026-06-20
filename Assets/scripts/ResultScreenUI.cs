//Œ‹‰Ê‰æ–Ê‘¤‚Ì•\Ž¦
using UnityEngine;
using System.Collections.Generic;

public class ResultScreenUI : MonoBehaviour
{
    public Transform resultListParent;
    public GameObject resultRowPrefab;

    private void Start()
    {
        foreach(var entry in RaceResultData.Instance.results)
        {
            var row = Instantiate(resultRowPrefab, resultListParent);
            row.GetComponent<ResultRowUI>().Setup(entry.rank, entry.horseName, entry.finishTime);
        }
    }
}
