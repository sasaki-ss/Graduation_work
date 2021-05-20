using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : MonoBehaviour
{
    [SerializeField] AI al;
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

        //ラケットと当たったら
        if (gameObject.name == "Cube")
        {
            hitFlg = true;
        }
        //ラケットと当たったら
        if (gameObject.name == "Cube2")
        {
            hitFlg2 = true;
        }
    }
}
