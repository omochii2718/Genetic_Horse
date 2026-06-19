using GeneticSharp.Domain.Populations;
using JetBrains.Annotations;
using System;
using System.CodeDom.Compiler;
using UnityEngine;
public class GeneticManager : MonoBehaviour
{
    struct Chromosome//染色体のデータ構造、float配列は遺伝子の配列、fitnessは評価値
    {
        public float[] genes;
        public float fitness;

    }

    public GameObject agentPrefab;
    public bool alive = true;
    public float cooltime;
    static int populationSize = 50;
    public static int BoneLength = 1;

    Chromosome[] population = new Chromosome[populationSize];
    GameObject[] agents = new GameObject[populationSize];
    void Start()
    {
        for (int i = 0; i < population.Length; i++)
        {
            population[i].genes = new float[20];
            for (int j = 0; j < 8; j++)
            {   
                population[i].genes[j] = UnityEngine.Random.Range(-10f, 10f);
            }
        }
        for (int i = 0; i < population.Length; i++)
        {
            agents[i] = Instantiate(agentPrefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
            agents[i].GetComponent<Agent>().assignv(population[i].genes);
        }
        cooltime = Time.time;
    }

    void selection()//いろいろな操作が混ざっているのでリファクタ必須
    {
        var bestFitness = float.MinValue;
        int indexElite = 0;
        var elite = new Chromosome[populationSize / 5];

        //最優良個体を選別、終了時にはこいつを返り値にしたい
        for (int i = 0; i < populationSize; i++)
        {
            if (population[i].fitness > bestFitness)
            {
                bestFitness = population[i].fitness;
                elite[0] = population[i];
                indexElite = i;
            }
            Debug.Log(elite[0].genes[0]);
            // 最も優れた個体のindexを保存
        }
        population[indexElite].fitness = float.MinValue;//次のエリート選別で同じ個体が選ばれないように

        //selection
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

        population[0] = elite[0];//最優良個体を次世代に引き継ぐ

        for (int i = 1; i < populationSize; i++)//crossover
        {
            var parent1 = elite[UnityEngine.Random.Range(0, elite.Length)];
            var parent2 = elite[UnityEngine.Random.Range(0, elite.Length)];
            for (int j = 0; j < population[i].genes.Length; j++)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)//50%の確率でどちらかの親の遺伝子を引き継ぎ
                {
                    population[i].genes[j] = parent1.genes[j];
                }
                else
                {
                    population[i].genes[j] = parent2.genes[j];
                }
            }
        }

        //mutationはあとから実装
    }

    void generate()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Rigidbody rb = agents[i].GetComponent<Rigidbody>();
            agents[i].GetComponent<Agent>().assignv(population[i].genes);//更新された遺伝子の割り当て
            rb.transform.eulerAngles = Vector3.zero; // 傾きリセット
            rb.angularVelocity = Vector3.zero; // 角速度をリセット
            rb.linearVelocity = Vector3.zero; //速度をリセット
            agents[i].GetComponent<Agent>().transform.position = new Vector3(i * 2.0f, 0, 0); // 元の場所に呼び戻す
        }
    }


    void Update()
    {
        if (Time.time - cooltime > 10f)//三秒ごとに適宜評価する
        {
            cooltime = Time.time;
            for (int i = 0; i < populationSize; i++)
            {
                population[i].fitness = agents[i].GetComponent<Agent>().transform.position.z;//z座標でどれだけ進んだかを評価の基準にする
            }
            selection();
            generate();

        }
    }
}

