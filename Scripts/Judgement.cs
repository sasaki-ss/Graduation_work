using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : MonoBehaviour
{
    //ラケットが当たったフラグ
    private bool hitFlg = false;
    //ラケットが当たったフラグ
    private bool hitFlg2 = false;
    //ラケットフラグのプロパティー
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
        // 物体がトリガーに接触しとき、１度だけ呼ばれる

        //プレイヤー側のラケットと当たったら
        if (this.gameObject.name == "PlayerRacket" && collision.name == "Ball")
        {
            //Debug.Log("aaaaaaaaaaaaaaaaaaa");

            hitFlg = true;
        }
        //AI側のラケットと当たったら
        if (this.gameObject.name == "AIRacket" && collision.name == "Ball") 
        {
            //Debug.Log("aaa");

            hitFlg2 = true;
        }
    }
}
