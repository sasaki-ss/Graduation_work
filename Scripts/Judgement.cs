using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : MonoBehaviour
{
    [SerializeField] AI al;
    //���P�b�g�����������t���O
    private bool hitFlg = false;
    //���P�b�g�����������t���O
    private bool hitFlg2 = false;
    //���P�b�g�t���O�̃v���p�e�B�[
    public bool HitFlg
    {
        get { return this.hitFlg; }
        set { this.hitFlg = value; }
    }

    public bool HitFlg2
    {
        get { return this.hitFlg2; }
        set { this.hitFlg2 = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {

        // ���̂��g���K�[�ɐڐG���Ƃ��A�P�x�����Ă΂��

        //���P�b�g�Ɠ���������
        if (gameObject.name == "Cube")
        {
            hitFlg = true;
        }
        //���P�b�g�Ɠ���������
        if (gameObject.name == "Cube2")
        {
            hitFlg2 = true;
        }
    }
}
