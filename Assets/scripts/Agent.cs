using GeneticSharp.Domain.Chromosomes;
using UnityEngine;

public class Agent: MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public double speedX, speedY;
    public Chromosome chromosome;
    public bool isAlive = true;

    public BoneBehavior[] bones;
    public void assignv(float[] genes)
    {   
        for(int i = 0; i < GeneticManager.BoneLength; i++)
        {
            bones[i] = transform.GetChild(i).gameObject.GetComponent<BoneBehavior>();
            bones[i].GetComponent<BoneBehavior>().SetVelocity(genes, i * 4);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if (isAlive)
        //{
        //    this.transform.Translate(new Vector3((float)speedX, 0, (float)speedY) * Time.deltaTime);
        //}
    }
}
