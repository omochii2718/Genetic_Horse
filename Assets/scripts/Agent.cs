using System;
using UnityEngine;
public class Agent: MonoBehaviour
{
    public BoneBehavior[] bones;
    public void assignv(float[] genes)//“n‚³‚ê‚½õF‘Ì‚ğ‚»‚ê‚¼‚ê‚Ìƒ{[ƒ“‚Éˆø‚«“n‚·
    {
        for (int i = 0; i < 1; i++)
        {
            bones[i] = transform.GetChild(i).gameObject.GetComponent<BoneBehavior>();
            bones[i].GetComponent<BoneBehavior>().SetVelocity(genes, i * 4);
        }
    }
}
