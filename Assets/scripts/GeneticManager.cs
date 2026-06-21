using JetBrains.Annotations;
using System;
using System.CodeDom.Compiler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    public int simulationnum = 100;
    public int current_simulationnum = 0;
    static int populationSize = 50;//並列にシミュレーションさせるエージェント数
    public static int Legnum =4;//足の数
    public static int Bonenum = 2;

    public Slider Slider;
    public float mutationRate = 0.02f; // 突然変異率 (0.0 〜 1.0)
    public GameObject canvasObject; // シミュレーション終了時に再表示するCanvas
    public TextMeshProUGUI maxFitnessText; // 最大評価値表示用テキスト
    private float overallBestFitness = float.MinValue; // シミュレーション全体の最大評価値
    Chromosome[] population = new Chromosome[populationSize];
    GameObject[] agents = new GameObject[populationSize];
    //agentsと染色体は
    void Start()
    {
        if (Slider != null)
        {
            // スライダーの値(0〜100%)を確率(0.0〜1.0)に変換
            mutationRate = Slider.value / 100f;
            Debug.Log($"Mutation Rate set to: {mutationRate} (Slider Value: {Slider.value}%)");
        }

        current_simulationnum++;
        for (int i = 0; i < population.Length; i++)
        {
            population[i].genes = new float[Legnum*Bonenum*3];
            for (int j = 0; j < Legnum*Bonenum*3; j++)
            {   
                    population[i].genes[j] = UnityEngine.Random.Range(-90f, 90f);
            }
        }
        for (int i = 0; i < population.Length; i++)
        {
            agents[i] = Instantiate(agentPrefab, new Vector3(i * 10f, 0, 0), Quaternion.identity);
            agents[i].GetComponent<Agent>().assignv(population[i].genes);
        }
        cooltime = Time.time;
    }

    void selection()//いろいろな操作が混ざっているのでリファクタ必須
    {
        var bestFitness = float.MinValue;
        int indexElite = 0;
        var elite = new Chromosome[populationSize / 2];
        float elite_weight = 0;

        //最優良個体を選別、終了時にはこいつを返り値にしたい
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

        if (bestFitness > overallBestFitness)
        {
            overallBestFitness = bestFitness;
        }

        elite_weight += population[indexElite].fitness;
        population[indexElite].fitness = float.MinValue;//次のエリート選別で同じ個体が選ばれないように

        //selection
        for (int i = 1; i < elite.Length; i++)
        {
            elite[i].fitness = float.MinValue;
            for (int j = 0; j < populationSize; j++)
            {
                if (population[j].fitness > elite[i].fitness)
                {  
                    elite[i] = population[j];
                    indexElite = j;
                }
            }
            elite_weight += population[indexElite].fitness;
            population[indexElite].fitness = float.MinValue;
            // 上位25体のindexを保存
        }
        Debug.Log(String.Join(",", elite[0].genes));
        population[0] = elite[0];//最優良個体を次世代に引き継

        for (int i = 1; i < populationSize; i++)//crossover
        {
            //
            var lot = UnityEngine.Random.Range(0, elite_weight);
            var lot2 = UnityEngine.Random.Range(0, elite_weight);
            var current_weight = 0f;
            Chromosome  parent1 = elite[0], parent2 = elite[0];
            for (int j= elite.Length-1;j >=0; j--)
            {
                current_weight += elite[j].fitness;
                if(lot < current_weight)
                {
                    parent1 = elite[j];
                    break;
                }
                parent1 = elite[0];
                    }
            for (int j = elite.Length-1; j >= 0; j--)
            {
                current_weight += elite[j].fitness;
                if (lot2 < current_weight)
                {
                    parent2 = elite[j];
                    break;
                }
            }

            for (int j = 0; j < population[i].genes.Length/3; j++)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)//50%の確率でどちらかの親の遺伝子を引き継ぎ
                {
                    for (int k = 0; k < 3; k++)
                    {
                        population[i].genes[j+k] = parent1.genes[j+k];
                    }
                }
                else
                {
                    for (int k = 0; k < 3; k++)
                    {
                        population[i].genes[j + k] = parent2.genes[j + k];
                    }
                }
            }
        }

        //mutation
        for (int i = 1; i < populationSize; ++i)
        {
            for (int j = 0; j < population[i].genes.Length; j++)
            {
                if (UnityEngine.Random.value <= mutationRate)
                {
                    population[i].genes[j] = UnityEngine.Random.Range(-90f, 90f);
                }
            }
        }

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
            agents[i].GetComponent<Agent>().transform.position = new Vector3(i * 10f, 0, 0); // 元の場所に呼び戻す
        }
    }


    void Update()
    {
        if (Slider != null)
        {
            // スライダーの値(0〜100%)を確率(0.0〜1.0)にリアルタイム反映
            mutationRate = Slider.value / 100f;
        }

        if (Time.time - cooltime > 10f)//三秒ごとに適宜評価する
        {   
            if(current_simulationnum>=simulationnum)
            {
                //SupabaseHorseUploader.Instance.SaveHorce(population[0].genes)
                foreach  (GameObject agent in agents) {
                    Destroy(agent);
                }
                if (maxFitnessText != null)
                {
                    maxFitnessText.text = overallBestFitness.ToString("F2");
                }
                if (canvasObject != null)
                {
                    canvasObject.SetActive(true);
                }
                Destroy(this.gameObject);
            }
            current_simulationnum++;
            cooltime = Time.time;
            for (int i = 0; i < populationSize; i++)
            {
                population[i].fitness = agents[i].GetComponent<Agent>().transform.position.z -Mathf.Abs(agents[i].GetComponent<Agent>().transform.position.x - 5.0f*i)* Mathf.Abs(agents[i].GetComponent<Agent>().transform.position.x - 5.0f * i);//z座標でどれだけ進んだかを評価の基準にする
            }
            selection();
            generate();

        }

    }
}

