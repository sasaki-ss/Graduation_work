using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : MonoBehaviour
{
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
        Debug.Log("ssssssss");
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaa");
        // ���̂��g���K�[�ɐڐG���Ƃ��A�P�x�����Ă΂��

        //�v���C���[���̃��P�b�g�Ɠ���������
        if (gameObject.name == "PlayerRacket")
        {
            hitFlg = true;
        }
        //AI���̃��P�b�g�Ɠ���������
        if (gameObject.name == "AIRacket")
        {
            hitFlg2 = true;
        }
    }
}
