using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Item : MonoBehaviour
{
    //�X�e�[�^�X�擾�p
    public CharaStatus CharaStatus;

    void OnTriggerEnter(Collider Collider)
    {
        //Debug.Log("�ڐG");
        CharaStatus.CharaStamina += 10;
    }
    void OnTriggerStay(Collider Collider)
    {
        //Debug.Log("�ڐG��");
        // �ԐF�ɕύX����
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
    void OnTriggerExit(Collider Collider)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.black;
        //Debug.Log("���E");
    }
}