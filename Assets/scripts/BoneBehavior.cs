using Unity.VisualScripting;
using UnityEngine;

public class BoneBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float v_x, v_y, v_z, radius, phase;

    private Rigidbody rb;
    public float downwardForce = 10f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        phase = 0;
    }


    public void SetVelocity(float[] genes,int index_num)
    {
        v_x = genes[index_num];
        v_y = genes[index_num+1];
        v_z = genes[index_num+2];//����킴�킴����Ă�̂�agent�̎��s�Ɛ��F�̂�n�����ے��𕪗������邽��
        radius = genes[index_num+3];
        phase = 0;//�O�p�֐��̈ʑ�����Z�b�g
        this.transform.eulerAngles = new Vector3(0, 0, 0);

        if (index_num +GeneticManager.Legnum*4 < GeneticManager.Legnum*GeneticManager.Bonenum*4)//���F�̂̏���͑��̐��~���ꂼ��̃{�[�����~4(�O�����{�����j
        {
            BoneBehavior child = transform.GetChild(0).gameObject.GetComponent<BoneBehavior>();
            child.SetVelocity(genes, index_num + 4*GeneticManager.Legnum);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
<<<<<<< Updated upstream
        rb.AddTorque(new Vector3(v_x, v_y, v_z) *Mathf.Cos(phase + radius)*3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ぶつかった相手のタグが "Ground"（地面）かどうかを確認
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Vector3.down（真下）に向かって力を加える
            rb.AddForce(Vector3.down * downwardForce, ForceMode.Impulse);
        }
=======
        phase += Time.deltaTime;
        rb.transform.eulerAngles = new Vector3(v_x,v_y,0)*(Mathf.Cos(phase));
>>>>>>> Stashed changes
    }
}
