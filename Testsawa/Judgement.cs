using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : MonoBehaviour
{
    //ラケットを振ったフラグ
    private bool hitFlg = false;

    //ラケットフラグのプロパティー
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
        
    }

    void OnCollisionEnter(Collision collision)
    {

        //Debug.Log("衝突したオブジェクト：" + gameObject.name);
        //Debug.Log("衝突されたオブジェクト：" + collision.gameObject.name);

        //ボールのオブジェクトと当たったら
        if(gameObject.name == "Ball")
        {
            hitFlg = true;
        }
        else
        {
            hitFlg = false;
        }
    }
}
