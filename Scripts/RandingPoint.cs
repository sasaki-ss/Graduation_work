using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandingPoint : MonoBehaviour
{
    [SerializeField]
    private float alpha;        //α値
    [SerializeField]
    private float red;          //赤
    [SerializeField]
    private float green;        //緑
    [SerializeField]
    private float blue;         //青
    [SerializeField]
    private float maxTime;      //最大時間
    [SerializeField]
    private float fedeOutSpeed; //フェイドアウトする速度

    private void Start()
    {
        //各値を取得する
        red = GetComponent<SpriteRenderer>().color.r;
        green = GetComponent<SpriteRenderer>().color.g;
        blue = GetComponent<SpriteRenderer>().color.b;
        alpha = GetComponent<SpriteRenderer>().color.a;

        //フェイドアウトする速度
        fedeOutSpeed = maxTime / 255f;
    }

    private void Update()
    {
        //α値をフェイドアウトする速度分減らす
        alpha -= fedeOutSpeed;

        //α値が0以下の場合このオブジェクトを削除
        if(alpha <= 0)
        {
            Destroy(this.gameObject);
        }

        //色を適用する
        GetComponent<SpriteRenderer>().color = new Color(red, green, blue, alpha);
    }
}
