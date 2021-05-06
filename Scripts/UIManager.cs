using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //UIの要素数
    private const int UINum = 7;        //生成するUIの要素数

    private int i = 0;                  //UIの要素数のカウント

    //プレハブの格納
    private GameObject textPref;        //テキスト
    private GameObject lgPref;          //ラインゲージ

    //UIのインスタンスを格納する配列
    private GameObject[] instances;

    //得点を格納する変数
    private int pScore;                 //プレイヤーのスコア
    private int oScore;                 //相手のスコア

    //テキスト
    private Text textScore;             //スコアの表示
    private Text textPlayerName;        //プレイヤー名
    private Text textOpponentName;      //相手名
    private Slider lgPlayer;            //プレイヤーのスタミナゲージ
    private Slider lgOpponent;          //相手のスタミナゲージ
    
    //座標
    private Vector2 scorePos;           //スコアの座標
    private Vector2 pNamePos;           //プレイヤー名の座標
    private Vector2 oNamePos;           //相手名の座標
    private Vector2 plgPos;             //プレイヤーのスタミナゲージの座標
    private Vector2 olgPos;             //相手のスタミナゲージの座標

    //プレイヤーのオブジェクト格納
    private GameObject Player;          //プレイヤーオブジェクトを格納する変数
    CharaStatus pcStatus;               //プレイヤーオブジェクトのスクリプトを格納する変数

    // Start is called before the first frame update
    void Start()
    {
        //プレハブの読み込み
        textPref = (GameObject)Resources.Load("TextPref");
        lgPref = (GameObject)Resources.Load("LineGaugePref");

        //座標設定
        scorePos = new Vector2(-100.0f, Screen.height/2);
        pNamePos = new Vector2(-Screen.width / 2, Screen.height / 2);
        oNamePos = new Vector2((Screen.width / 2 )- 140, Screen.height / 2);
        plgPos = pNamePos + new Vector2(210.0f, -100.0f);
        olgPos = oNamePos + new Vector2(-70.0f, -100.0f);

        Player = GameObject.Find("Player");
        pcStatus = Player.GetComponent<CharaStatus>();

        CreateInit();                   //初期化と生成
    }

    // Update is called once per frame
    void Update()
    {
        lgPlayer.value = (float)pcStatus.CharaStamina;
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

        //点数のテキスト
        instances[i] = (GameObject)Instantiate(textPref, scorePos, Quaternion.identity);    //インスタンス生成
        instances[i].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
        instances[i].name = "TextScore";                                                    //オブジェクト名変更
        textScore = instances[0].GetComponent<Text>();                                      //テキスト
        textScore.text = pScore + " - " + oScore;                                           //テキスト内容設定
        i++;

        //プレイヤー名の表示
        instances[i] = (GameObject)Instantiate(textPref, pNamePos, Quaternion.identity);    //インスタンス生成
        instances[i].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
        instances[i].name = "TextPlayerName";                                               //オブジェクト名変更
        textPlayerName = instances[i].GetComponent<Text>();                                 //テキスト
        textPlayerName.text = "プレイヤー";                                                 //テキスト内容設定
        textPlayerName.fontSize = 60;                                                       //フォントサイズの変更
        i++;

        //相手名の表示
        instances[i] = (GameObject)Instantiate(textPref, oNamePos, Quaternion.identity);    //インスタンス生成
        instances[i].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
        instances[i].name = "TextOpponentName";                                             //オブジェクト名変更
        textOpponentName = instances[i].GetComponent<Text>();                               //テキスト
        textOpponentName.text = "相手";                                                     //テキスト内容設定
        textOpponentName.fontSize = 60;                                                     //フォントサイズの変更
        i++;

        //プレイヤーのスタミナゲージ
        instances[i] = (GameObject)Instantiate(lgPref, plgPos, Quaternion.identity);        //インスタンス生成
        instances[i].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
        instances[i].name = "lgPlayer";                                                     //オブジェクト名変更
        lgPlayer = instances[i].GetComponent<Slider>();                                     //スライダー
        lgPlayer.minValue = 0;                                                              //最小値
        lgPlayer.maxValue = (float)pcStatus.CharaStamina;                                   //最大値
        lgPlayer.value = lgPlayer.maxValue;                                                 //現在の値の設定
        i++;

        //相手のスタミナゲージ
        instances[i] = (GameObject)Instantiate(lgPref, olgPos, Quaternion.identity);        //インスタンス生成
        instances[i].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
        instances[i].name = "lgOpponent";                                                   //オブジェクト名変更
        lgOpponent = instances[i].GetComponent<Slider>();                                   //スライダー
        lgOpponent.minValue = 0;                                                            //最小値
        lgOpponent.maxValue = 1;                                                            //最大値
        lgOpponent.value = lgOpponent.maxValue;                                             //現在の値の設定
        i++;

    }


}
