using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //表示するUIの要素数
    private const int UINum = 1;

    //プレハブの格納
    private GameObject textPref;        //テキスト

    //UIのインスタンスを格納する配列
    private GameObject[] instances;

    //得点を格納する変数
    private int pScore;                 //プレイヤーのスコア
    private int oScore;                 //相手のスコア

    //テキスト
    private Text textScore;             //スコアの表示
    
    //座標
    private Vector2 scorePos;           //スコアの座標

    // Start is called before the first frame update
    void Start()
    {
        //プレハブの読み込み
        textPref = (GameObject)Resources.Load("TextPref");

        //座標設定
        scorePos = new Vector2(-100.0f, Screen.height/2);

        CreateInit();                   //初期化と生成
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateInit()
    {
        //初期化
        instances = new GameObject[UINum];
        textScore = null;
        pScore = 0;
        oScore = 0;

        //生成
        instances[0] = (GameObject)Instantiate(textPref, scorePos, Quaternion.identity);    //インスタンス生成
        instances[0].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
        instances[0].name = "TextScore";                                                    //オブジェクト名変更
        textScore = instances[0].GetComponent<Text>();                                      //テキスト
        textScore.text = pScore + " - " + oScore;                                           //テキスト内容設定
    }
}
