using Unity.VisualScripting;
using UnityEngine;

public class BoneBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float v_x, v_y, v_z, radius, phase;

    private Rigidbody rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        phase = 0;
    }


    public void SetVelocity(float[] genes,int index_num)
    {
        v_x = genes[index_num];
        v_y = genes[index_num+1];
        v_z = genes[index_num+2];
        radius = genes[index_num+3];
        phase = 0;

        if (index_num +4 < 8)
        {
            BoneBehavior child = transform.GetChild(0).gameObject.GetComponent<BoneBehavior>();
            child.SetVelocity(genes, index_num + 4*GeneticManager.BoneLength);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.AddTorque(new Vector3(v_x, v_y, v_z) *Mathf.Cos(phase + radius*Time.deltaTime));
    }
}
