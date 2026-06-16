using GeneticSharp.Domain.Populations;
using JetBrains.Annotations;
using System;
using System.CodeDom.Compiler;
using UnityEngine;
public class GeneticManager : MonoBehaviour
{
    struct Chromosome
    {
        public float[] genes;
        public float fitness;

    }

    public GameObject agentPrefab;
    public bool alive = true;
    public float cooltime;
    static int populationSize = 50;
    Chromosome[] population = new Chromosome[populationSize];
    GameObject[] agents = new GameObject[populationSize];
    void Start()
    {
        for (int i = 0; i < population.Length; i++)
        {
            population[i].genes = new float[2];
            for (int j = 0; j < 2; j++)
            {
                population[i].genes[j] = UnityEngine.Random.Range(-10f, 10f);
            }
        }
        for (int i = 0; i < population.Length; i++)
        {
            agents[i] = Instantiate(agentPrefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
            agents[i].GetComponent<Agent>().assignv(population[i].genes);
        }
        cooltime = Time.time; // Set the initial cooltime to 5 seconds from the start
    }

    void selection()
    {
        var bestFitness = float.MinValue;
        int indexElite = 0;
        var elite = new Chromosome[populationSize / 5];
        for (int i = 0; i < populationSize; i++)
        {
            if (population[i].fitness > bestFitness)
            {
                bestFitness = population[i].fitness;
                elite[0] = population[i];
                indexElite = i;
            }
            // 最も優れた個体のindexを保存
        }
        population[indexElite].fitness = float.MinValue; 
        for (int i = 0; i < elite.Length; i++)
        {
            for (int j = 0; j < populationSize; j++)
            {
                if (population[j].fitness > elite[i].fitness)
                {
                    elite[i] = population[j];
                    indexElite = j;
                }
            }
            population[indexElite].fitness = float.MinValue;
            // 上位１０体のindexを保存
        }

        for (int i = 0; i < populationSize; i++)
        {
            var parent1 = elite[UnityEngine.Random.Range(0, elite.Length)];
            var parent2 = elite[UnityEngine.Random.Range(0, elite.Length)];
            for (int j = 0; j < population[i].genes.Length; j++)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    population[i].genes[j] = parent1.genes[j];
                }
                else
                {
                    population[i].genes[j] = parent2.genes[j];
                }
            }
        }

    }

    void generate()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Rigidbody rb = agents[i].GetComponent<Rigidbody>();
            agents[i].GetComponent<Agent>().assignv(population[i].genes);
            rb.transform.eulerAngles = Vector3.zero; // Reset the alive status for the new generation
            rb.angularVelocity = Vector3.zero; // Reset the alive status for the new generation
            rb.linearVelocity = Vector3.zero; // Reset the alive status for the new generation
            agents[i].GetComponent<Agent>().transform.position = new Vector3(i * 2.0f, 0, 0); // Reset position for the new generation
        }
    }


    void Update()
    {
        if (Time.time - cooltime > 3f)
        {
            cooltime = Time.time;
            for (int i = 0; i < populationSize; i++)
            {
                population[i].fitness = population[i].genes[0] + population[i].genes[1];
            }
            selection();
            generate();

            // Here you would implement the logic to evaluate the fitness of each agent, select parents, perform crossover and mutation, and create the next generation of agents.
        }
    }
}

