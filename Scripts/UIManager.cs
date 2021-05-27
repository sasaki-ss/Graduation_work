using UnityEngine;
using UnityEngine.UI;
using TapStateManager;

public class UIManager : MonoBehaviour
{
    //タップ判定取得
    TouchManager tMger;                 //TapStateManager内のTouchManager
    private bool createFlg;             //タップ中のUIの生成フラグ

    //UIの要素数
    private const int UINum = 10;        //生成するUIの要素数

    private int i = 0;                  //UIの要素数のカウント
    private int j = 0;                  //ゲーム中生成したり削除するオブジェクトの要素数
    private int r = 0;                  //ラウンド切り替え時に生成したり削除する要素数

    //プレハブの格納
    private GameObject textPref;        //テキスト
    private GameObject lgPref;          //ラインゲージ
    private GameObject pgPref1;         //パワーゲージ(枠線)
    private GameObject pgPref2;         //パワーゲージ(青い部分)
    private GameObject linePref;        //線

    //UIのインスタンスを格納する配列
    private GameObject[] instances;

    //テキスト
    private Text textScore;             //スコアの表示
    private Text textPlayerName;        //プレイヤー名
    private Text textOpponentName;      //相手名

    //ゲージ
    private Slider lgPlayer;            //プレイヤーのスタミナゲージ
    private Slider lgOpponent;          //相手のスタミナゲージ
    private Image pGauge1;              //パワーゲージ(枠線)
    private Image pGauge2;              //パワーゲージ(青い部分)

    //線
    private LineRenderer line;          //飛ばす方向の線
    private AnimationCurve lineCurve;   //線の幅のアニメーションカーブ

    //保持用
    private float gaugeKeep;            //ゲージのキープ
    private Vector3 vertexKeep;         //頂点の座標キープ

    //座標
    private Vector2 scorePos;           //スコアの座標
    private Vector2 pNamePos;           //プレイヤー名の座標
    private Vector2 oNamePos;           //相手名の座標
    private Vector2 plgPos;             //プレイヤーのスタミナゲージの座標
    private Vector2 olgPos;             //相手のスタミナゲージの座標
    private Vector3 pgPos;              //パワーゲージの座標
    private Vector3 plViewPos;          //プレイヤーのカメラ上の座標
    private Vector3 pgViewPos;          //plViewPosと同じ位置に表示するための座標
    private Vector3 linePos;            //線の原点
    private Vector3 lineEndPos;         //線の移動する頂点

    //カメラ
    private GameObject mCam;            //カメラ格納オブジェクト
    private GameObject uCam;
    private Camera mainCam;             //メインカメラ
    private Camera uiCam;               //サブカメラ

    //プレイヤーのオブジェクト格納
    private GameObject Player;          //プレイヤーオブジェクトを格納する変数
    CharaStatus pcStatus;               //プレイヤーオブジェクトのスクリプトを格納する変数

    //相手プレイヤーオブジェクトの格納
    private GameObject opponentPlayer;  //相手プレイヤーオブジェクト
    CharaStatus opcStatus;              //相手のステータススクリプトの格納

    //ショットのオブジェクト格納
    private GameObject Shot;            //ショットオブジェクトを格納する変数
    Shot shot;                          //ショットオブジェクトのスクリプトを格納する変数

    private GameObject Score;           //スコア
    private Score score;                //スコアのスクリプト

    void Start()
    {
        #region 初期化と初期生成

        //タップ関連の初期化
        tMger = new TouchManager();
        createFlg = true;

        //プレハブの読み込み
        textPref = (GameObject)Resources.Load("TextPref");
        lgPref = (GameObject)Resources.Load("LineGaugePref");
        pgPref1 = (GameObject)Resources.Load("PowerGaugePref1");
        pgPref2 = (GameObject)Resources.Load("PowerGaugePref2");
        linePref = (GameObject)Resources.Load("linePref");

        //アニメーションカーブの初期化
        lineCurve = new AnimationCurve();

        //座標設定
        scorePos = new Vector2(-100.0f, Screen.height/2);
        pNamePos = new Vector2(-Screen.width / 2, Screen.height / 2);
        oNamePos = new Vector2((Screen.width / 2 )- 140, Screen.height / 2);
        plgPos = pNamePos + new Vector2(210.0f, -100.0f);
        olgPos = oNamePos + new Vector2(-70.0f, -100.0f);

        //カメラの取得
        mCam = GameObject.Find("Main Camera");
        uCam = GameObject.Find("UICamera");
        mainCam = mCam.GetComponent<Camera>();
        uiCam = uCam.GetComponent<Camera>();

        //オブジェクトおよびスクリプトの格納
        Player = GameObject.Find("Player");
        pcStatus = Player.GetComponent<CharaStatus>();

        opponentPlayer = GameObject.Find("Player2");
        opcStatus = opponentPlayer.GetComponent<CharaStatus>();

        Shot = GameObject.Find("Shot");
        shot = Shot.GetComponent<Shot>();

        Score = GameObject.Find("Score");
        score = Score.GetComponent<Score>();

        CreateInit();                                   //初期化と生成

        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        tMger.update();                                                 //更新(タップ監視)

        textScore.text = score.user1Score + " - " + score.user2Score;   //得点取得
        lgPlayer.value = (float)pcStatus.CharaStamina;                  //スタミナ取得
        lgOpponent.value = (float)opcStatus.CharaStamina;               //相手スタミナ取得　
        TapDoing();                                                     //タップ中のUIの生成管理
        RoundBetween();                                                 //ラウンドの間のUIの生成管理

    }

    private void CreateInit()
    {
        #region 基本表示されるUIの生成

        //初期化
        instances = new GameObject[UINum];
        textScore = null;
        textPlayerName = null;
        textOpponentName = null;
        lgPlayer = null;
        lgOpponent = null;

        //点数のテキスト
        instances[i] = (GameObject)Instantiate(textPref, scorePos, Quaternion.identity);    //インスタンス生成
        instances[i].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
        instances[i].name = "TextScore";                                                    //オブジェクト名変更
        textScore = instances[0].GetComponent<Text>();                                      //テキスト
        textScore.text = score.user1Score + " - " + score.user2Score;                       //テキスト内容設定
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
        lgOpponent.maxValue = (float)opcStatus.CharaStamina;                                //最大値
        lgOpponent.value = lgOpponent.maxValue;                                             //現在の値の設定
        i++;

        #endregion
    }

    void TapDoing()
    {   
        #region タップしている最中に行う処理

        TouchManager touch_state = tMger.getTouch();    //タッチ取得

        if (touch_state._touch_flag)                    //タッチされていた場合
        {
            if (touch_state._touch_phase == TouchPhase.Began)
            {   //タッチ開始
                //ここの処理が実行されない
            }

            if (touch_state._touch_phase == TouchPhase.Moved)
            {
                if (10 <= shot.GetTapTime)
                {
                    if (!GameManager.instance.isAddScore)
                    {   //得点フラグオフ(ゲーム中)の時に処理を行う
                        if (createFlg)
                        {
                            #region タップ中に表示されるUIの生成
                            //Beganの代わり
                            j = i;                                                                              //現在のinstances配列の続きからカウントする

                            //ゲージの座標設定
                            //plViewPos = mainCam.WorldToViewportPoint(Player.transform.position);                //プレイヤーのカメラ上の座標
                            //pgViewPos = uiCam.ViewportToWorldPoint(plViewPos);                                  //UIカメラでplViewPosと同じ位置に表示されるようにワールド座標を取得
                            //pgViewPos.z = 0;                                                                    //z軸の設定
                            //pgPos = pgViewPos + new Vector3(-200.0f, 100.0f, 0.0f);                             //pgViewPosに更に補正した値を設定
                            pgPos = RectTransformUtility.WorldToScreenPoint(mainCam, Player.transform.position);
                            pgPos = pgPos + new Vector3(-430, -800, 0);

                            //線の座標設定
                            linePos = shot.GetTapStart;                                                         //タップを開始した座標に設定
                            linePos.z = 10;
                            linePos = uiCam.ScreenToWorldPoint(linePos);
                            lineEndPos = new Vector3(shot.GetTapWhile.x, shot.GetTapWhile.y, 10.0f);              //タップしている間移動する頂点の座標
                            lineEndPos = uiCam.ScreenToWorldPoint(lineEndPos);

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

                            //線
                            instances[j] = (GameObject)Instantiate(linePref, linePos, Quaternion.identity);     //インスタンス生成
                            instances[j].transform.SetParent(gameObject.transform, false);                      //親オブジェクト
                            instances[j].name = "line";                                                         //オブジェクト名変更
                            line = instances[j].GetComponent<LineRenderer>();                                   //イメージ
                            lineCurve.AddKey(0.05f, 0.05f);                                                     //線の太さ(点1)
                            lineCurve.AddKey(0.3f, 0.3f);                                                       //線の太さ2(点2)
                            line.numCapVertices = 10;                                                           //角を丸くする
                            line.numCornerVertices = 10;                                                        //線の各セグメントを丸くする
                            line.widthCurve = lineCurve;                                                        //設定したアニメーションカーブを適用
                            line.SetPosition(0, linePos);                                                       //頂点の座標設定(原点)
                            line.SetPosition(1, lineEndPos);                                                    //頂点の座標設定(移動する点)
                            j++;

                            createFlg = false;                                                                  //生成しました

                            #endregion
                        }

                        //タッチ中
                        pGauge2.fillAmount = 1.0f - ((float)shot.GetTapTime / 60.0f) / 2.0f;                    //タッチ中のパワーゲージ設定
                        gaugeKeep = pGauge2.fillAmount;                                                         //ゲージキープ

                        lineEndPos.x = shot.GetTapWhile.x;                                                      //頂点のx座標の変更
                        lineEndPos.y = shot.GetTapWhile.y;                                                      //頂点のy座標の変更
                        lineEndPos.z = 10;
                        lineEndPos = uiCam.ScreenToWorldPoint(lineEndPos);
                        vertexKeep = lineEndPos;                                                                //頂点の座標キープ
                        line.SetPosition(1, lineEndPos);                                                        //頂点の設定
                    }
                }
            }

            if (touch_state._touch_phase == TouchPhase.Ended)
            {   //タッチ終了
                for (int n = i; n < j; n++)
                {
                    pGauge2.fillAmount = gaugeKeep;                                                     //保持
                    lineEndPos = vertexKeep;

                    Destroy(instances[n], 0.3f);                                                        //タップ中に生成されたオブジェクトの削除
                }
                createFlg = true;
            }
        }
        #endregion
    }
    void RoundBetween()
    {   //ラウンドの間に行う処理
        #region ラウンド切り替え時に行う処理
        if (GameManager.instance.isAddScore)
        {   //得点フラグオン(ラウンド切り替え時)に行う処理
            
        }


        #endregion
    }
}
