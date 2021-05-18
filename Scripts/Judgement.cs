using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : MonoBehaviour
{
    //ラケットが当たったフラグ
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
        //Debug.Log(hitFlg);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("衝突したオブジェクト：" + gameObject.name);
        //Debug.Log("衝突されたオブジェクト：" + collision.gameObject.name);

        //ラケットと当たったら
        if(gameObject.name == "Cube")
        {
            hitFlg = true;
        }
    }

    //離れたら
    void OnCollisionExit(Collision collision)
    {
        //ラケットと離れたら
        if (gameObject.name == "Cube")
        {
            hitFlg = false;
        }
    }
}
