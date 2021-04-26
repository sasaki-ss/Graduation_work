using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Item : MonoBehaviour
{
    //ステータス取得用
    public CharaStatus CharaStatus;

    void OnTriggerEnter(Collider Collider)
    {
        //Debug.Log("接触");
        CharaStatus.CharaStamina += 10;
    }
    void OnTriggerStay(Collider Collider)
    {
        //Debug.Log("接触中");
        // 赤色に変更する
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
    void OnTriggerExit(Collider Collider)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.black;
        //Debug.Log("離脱");
    }
}