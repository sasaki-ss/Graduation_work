using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //表示するUIの要素数
    private const int UINum = 2;

    //プレハブの格納
    private GameObject textPref;        //テキスト

    //UIのインスタンスを格納する配列
    private GameObject[] instances;

    //得点を格納する変数
    private int pScore;                 //プレイヤーのスコア
    private int oScore;                 //相手のスコア

    //テキスト
    private Text textScore;             //スコアの表示
    private Text textPlayerName;        //プレイヤー名
    private Text textOpponentName;      //相手名
    
    //座標
    private Vector2 scorePos;           //スコアの座標
    private Vector2 pNamePos;           //プレイヤー名の座標
    private Vector2 oNamePos;           //相手名の座標

    // Start is called before the first frame update
    void Start()
    {
        //プレハブの読み込み
        textPref = (GameObject)Resources.Load("TextPref");

        //座標設定
        scorePos = new Vector2(-100.0f, Screen.height/2);
        pNamePos = new Vector2(-Screen.width / 2, Screen.height / 2);

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
        textPlayerName = null;
        textOpponentName = null;
        pScore = 0;
        oScore = 0;

        //生成
        instances[0] = (GameObject)Instantiate(textPref, scorePos, Quaternion.identity);    //インスタンス生成
        instances[0].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
        instances[0].name = "TextScore";                                                    //オブジェクト名変更
        textScore = instances[0].GetComponent<Text>();                                      //テキスト
        textScore.text = pScore + " - " + oScore;                                           //テキスト内容設定

        instances[1] = (GameObject)Instantiate(textPref, pNamePos, Quaternion.identity);    //インスタンス生成
        instances[1].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
        instances[1].name = "TextPlayerName";                                               //オブジェクト名変更
        textPlayerName = instances[1].GetComponent<Text>();                                 //テキスト
        textPlayerName.text = "プレイヤー";                                                 //テキスト内容設定
        textPlayerName.fontSize = 60;                                                       //フォントサイズの変更
    }
}
