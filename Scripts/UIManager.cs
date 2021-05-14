using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TapStateManager;
using System;

public class UIManager : MonoBehaviour
{
    //タップ判定取得
    TouchManager tMger;                 //TapStateManager内のTouchManager
    private bool createFlg;             //タップ中のUIの生成フラグ

    //UIの要素数
    private const int UINum = 8;        //生成するUIの要素数

    private int i = 0;                  //UIの要素数のカウント
    private int j = 0;                  //ゲーム中生成したり削除するオブジェクトの要素数

    //プレハブの格納
    private GameObject textPref;        //テキスト
    private GameObject lgPref;          //ラインゲージ
    private GameObject pgPref1;         //パワーゲージ(枠線)
    private GameObject pgPref2;         //パワーゲージ(青い部分)
    private GameObject arrowPref;       //矢印

    //UIのインスタンスを格納する配列
    private GameObject[] instances;

    //得点を格納する変数
    private int pScore;                 //プレイヤーのスコア
    private int oScore;                 //相手のスコア

    //テキスト
    private Text textScore;             //スコアの表示
    private Text textPlayerName;        //プレイヤー名
    private Text textOpponentName;      //相手名

    //ゲージ
    private Slider lgPlayer;            //プレイヤーのスタミナゲージ
    private Slider lgOpponent;          //相手のスタミナゲージ
    private Image pGauge1;              //パワーゲージ(枠線)
    private Image pGauge2;              //パワーゲージ(青い部分)

    //矢印
    private Image arrow;                //飛ばす方向の矢印

    //座標
    private Vector2 scorePos;           //スコアの座標
    private Vector2 pNamePos;           //プレイヤー名の座標
    private Vector2 oNamePos;           //相手名の座標
    private Vector2 plgPos;             //プレイヤーのスタミナゲージの座標
    private Vector2 olgPos;             //相手のスタミナゲージの座標
    private Vector3 pgPos;              //パワーゲージの座標
    private Vector3 plViewPos;          //プレイヤーのカメラ上の座標
    private Vector3 pgViewPos;          //plViewPosと同じ位置に表示するための座標
    private Vector2 arrowPos;           //矢印の座標

    //カメラ
    public Camera mainCam;              //メインカメラ
    public Camera uiCam;                //サブカメラ

    //プレイヤーのオブジェクト格納
    private GameObject Player;          //プレイヤーオブジェクトを格納する変数
    CharaStatus pcStatus;               //プレイヤーオブジェクトのスクリプトを格納する変数

    //ショットのオブジェクト格納
    private GameObject Shot;            //ショットオブジェクトを格納する変数
    Shot shot;                          //ショットオブジェクトのスクリプトを格納する変数

    // Start is called before the first frame update
    void Start()
    {
        //タップ関連の初期化
        tMger = new TouchManager();
        createFlg = true;

        //プレハブの読み込み
        textPref = (GameObject)Resources.Load("TextPref");
        lgPref = (GameObject)Resources.Load("LineGaugePref");
        pgPref1 = (GameObject)Resources.Load("PowerGaugePref1");
        pgPref2 = (GameObject)Resources.Load("PowerGaugePref2");
        arrowPref = (GameObject)Resources.Load("ArrowPref"); 

        //座標設定
        scorePos = new Vector2(-100.0f, Screen.height/2);
        pNamePos = new Vector2(-Screen.width / 2, Screen.height / 2);
        oNamePos = new Vector2((Screen.width / 2 )- 140, Screen.height / 2);
        plgPos = pNamePos + new Vector2(210.0f, -100.0f);
        olgPos = oNamePos + new Vector2(-70.0f, -100.0f);

        //オブジェクトおよびスクリプトの格納
        Player = GameObject.Find("Player");
        pcStatus = Player.GetComponent<CharaStatus>();

        Shot = GameObject.Find("Shot");
        shot = Shot.GetComponent<Shot>();

        CreateInit();                   //初期化と生成

    }

    // Update is called once per frame
    void Update()
    {
        lgPlayer.value = (float)pcStatus.CharaStamina;  //スタミナ取得

        tMger.update();                                 //更新(タップ監視)
        TapDoing();                                     //タップ中のUIの生成管理

    }

    private void CreateInit()
    {
        #region 基本表示されるUIの生成

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

        #endregion
    }

    void TapDoing()
    {   //タップしている最中に行う処理

        TouchManager touch_state = tMger.getTouch();    //タッチ取得

        if (touch_state._touch_flag)                    //タッチされていた場合
        {
            if (touch_state._touch_phase == TouchPhase.Began)
            {   //タッチ開始
                //ここの処理が実行されない
            }

            if (touch_state._touch_phase == TouchPhase.Moved)
            {
                if (createFlg)
                {   //Beganの代わり
                    #region タップ中に表示されるUIの生成

                    j = i;                                                                              //現在のinstances配列の続きからカウントする

                    //座標設定
                    plViewPos = mainCam.WorldToViewportPoint(Player.transform.position);                //プレイヤーのカメラ上の座標
                    pgViewPos = uiCam.ViewportToWorldPoint(plViewPos);                                  //UIカメラでplViewPosと同じ位置に表示されるようにワールド座標を取得
                    pgViewPos.z = 0;                                                                    //z軸の設定
                    pgPos = pgViewPos + new Vector3(-200.0f, 100.0f, 0.0f);                             //pgViewPosに更に補正した値を設定
                    arrowPos = shot.GetTapStart;                                                        //タップを開始した座標に設定
                    arrowPos.x -= Screen.width / 2;
                    arrowPos.y -= Screen.height / 2;
                    Debug.Log(arrowPos);

                    //パワーゲージ(枠組み)
                    instances[j] = (GameObject)Instantiate(pgPref1, pgPos, Quaternion.identity);        //インスタンス生成
                    instances[j].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
                    instances[j].name = "pGauge1";                                                      //オブジェクト名変更
                    pGauge1 = instances[j].GetComponent<Image>();                                       //イメージ
                    j++;

                    //パワーゲージ(青い部分)
                    instances[j] = (GameObject)Instantiate(pgPref2, pgPos, Quaternion.identity);        //インスタンス生成
                    instances[j].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
                    instances[j].name = "pGauge2";                                                      //オブジェクト名変更
                    pGauge2 = instances[j].GetComponent<Image>();                                       //イメージ
                    j++;

                    //矢印
                    instances[j] = (GameObject)Instantiate(arrowPref, arrowPos, Quaternion.identity);   //インスタンス生成
                    instances[j].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
                    instances[j].name = "arrow";                                                        //オブジェクト名変更
                    arrow = instances[j].GetComponent<Image>();                                         //イメージ
                    j++;

                    createFlg = false;                                                                  //生成しました

                    #endregion
                }

                //タッチ中
                pGauge2.fillAmount = 1.0f - ((float)shot.GetTapTime / 60.0f) / 2.0f;                    //タッチ中のパワーゲージ設定
                
            }

            if (touch_state._touch_phase == TouchPhase.Ended)
            {   //タッチ終了
                for (int n = i; n < j; n++)
                {
                    Destroy(instances[n], 0.0f);                                                        //タップ中に生成されたオブジェクトの削除
                }
                createFlg = true;
            }
        }
    }
}
