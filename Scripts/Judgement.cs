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
        Debug.Log("ssssssss");
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaa");
        // 物体がトリガーに接触しとき、１度だけ呼ばれる

        //プレイヤー側のラケットと当たったら
        if (gameObject.name == "PlayerRacket")
        {
            hitFlg = true;
        }
        //AI側のラケットと当たったら
        if (gameObject.name == "AIRacket")
        {
            hitFlg2 = true;
        }
    }
}
