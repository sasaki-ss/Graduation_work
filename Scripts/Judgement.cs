using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : MonoBehaviour
{
    //���P�b�g�����������t���O
    private bool hitFlg = false;

    //���P�b�g�t���O�̃v���p�e�B�[
    public bool HitFlg
    {
        get { return this.hitFlg; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hitFlg);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("�Փ˂����I�u�W�F�N�g�F" + gameObject.name);
        //Debug.Log("�Փ˂��ꂽ�I�u�W�F�N�g�F" + collision.gameObject.name);

        //���P�b�g�Ɠ���������
        if(gameObject.name == "Cube")
        {
            hitFlg = true;
        }
    }

    //���ꂽ��
    void OnCollisionExit(Collision collision)
    {
        //���P�b�g�Ɨ��ꂽ��
        if (gameObject.name == "Cube")
        {
            hitFlg = false;
        }
    }
}
