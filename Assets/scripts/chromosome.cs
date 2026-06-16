using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Chromosome
{
    public float[] MakeChromosome()
    {
        float[] genes = new float[2];
        for (int i = 0; i < 2; i++)
        {
            genes[i] = Random.Range(0f, 1f); // Randomly initialize genes between 0 and 1
        }
        return genes;
    }
}