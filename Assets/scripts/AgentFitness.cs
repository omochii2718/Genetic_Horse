using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;  
using UnityEngine;

public class AgentFitness:IFitness
{
    public double Evaluate(IChromosome chromosome)
    {
        var fc = chromosome as FloatingPointChromosome;
        var genes = fc.ToFloatingPoints();

        // Implement your fitness evaluation logic here
        return (float)genes[0] + (float)genes[1]; // Example: maximize the sum of the two genes
    }
}