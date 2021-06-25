using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定数クラス
public class Define
{
    public const float  NEXT_ROUNDTIME = 2.0f;  //ラウンド間の時間
    public const int    USER_NUM = 2;           //ユーザーの数
    public const int    SERVE_CHANGECNT = 2;    //サーブ切り替えの数
    public const int    MAX_BOUNDCNT = 2;       //最大のバウンド回数
}

//ユーザースコアデータ
public class UserScoreData
{
    /*プロパティ関連*/
    public int  score { get; private set; }     //スコア
    public bool isMatchP { get; private set; }  //マッチポイントフラグ

    //コンストラクタ
    public UserScoreData(int _score,bool _isMatchP)
    {
        score = _score;
        isMatchP = _isMatchP;
    }
}

//ゲーム状態
public enum GameState
{
   Serve,           //サーブ
   DuringRound,     //ラウンド
   GameSet          //ゲーム終了
}

//サーブユーザー
public enum User
{
    User1,      //ユーザー1
    User2       //ユーザー2
}

//フォルト状態
public enum FaultState
{
    None,
    Fault,
    DoubleFault,
}

//ゲームマネージャー
public class GameManager : MonoBehaviour
{
    /*外部参照に使う変数*/
    public static GameManager instance;               //インスタンス

    /*このスクリプトでのみ使う変数*/
    private Score           score;              //スコアクラス
    private GameObject      serveAreaObj;       //サーブエリアオブジェクト
    private GameObject      safetyAreaObj;      //セーフティエリアオブジェクト
    private GameObject[]    serveOutAreaObj;    //サーブアウトエリアオブジェクト
    private Vector3[]       serveAreaPos;       //セーブエリアの座標
    private int             changeCount;        //ラウンドカウント
    private string[]        userObjTag;         //プレイヤーオブジェクトのタグ

    /*プロパティ関連*/
    public bool         isDeuce { get; set; }              //デュースフラグ
    public bool         isAddScore { get; set; }           //スコアフラグ
    public bool         isNextRound { get;  private set; } //次のラウンドフラグ
    public bool         isServe { get; set; }              //サーブフラグ
    public bool         isFault { get; private set; }      //フォルトフラグ
    public bool         isGameSet { get; private set; }    //ゲーム終了フラグ
    public GameState    gameState { get; set; }            //ゲームの状態
    public User         serveUser { get; private set; }    //サーブするユーザー
    public FaultState   faultState { get; set; }           //フォルト状態

    /*インスペクターに表示又は設定する変数*/
    [SerializeField]
    private int winScore;   //勝利スコア

    //初期化処理
    private void Start()
    {
        score = this.GetComponentInChildren<Score>();
        serveAreaObj = GameObject.Find("ServeArea");
        safetyAreaObj = GameObject.Find("SafetyArea");

        serveOutAreaObj = new GameObject[3];
        serveOutAreaObj[0] = GameObject.Find("Area1");
        serveOutAreaObj[1] = GameObject.Find("Area2");
        serveOutAreaObj[2] = GameObject.Find("Area3");

        serveAreaPos = new Vector3[4]
        {
            new Vector3( 32f,0f, 21f), //ユーザー1側右
            new Vector3( 32f,0f,-21f), //ユーザー1側左
            new Vector3(-32f,0f, 21f), //ユーザー2側右
            new Vector3(-32f,0f,-21f), //ユーザー2側左
        };

        userObjTag = new string[Define.USER_NUM]
        {
            "Player",
            "Player2"
        };

        instance = this;
        Init();
    }

    //更新処理
    private void Update()
    {
        //スコアが追加された際
        if (isAddScore && !isNextRound && !isGameSet)
        {
            StartCoroutine(NextRound());
        }

        //得点処理
        ScoreProc();

        //勝利判定
        VictoryJudgment();
    }

    #region 得点処理
    private void ScoreProc()
    {
        //ボールを取得
        Ball ball = GameObject.Find("Ball").GetComponent<Ball>();

        //アウトの場合
        if (ball.isNet && !isAddScore)
        {
            score.AddScore(InversionTag(ball.nowUserTag));
        }

        //バウンド回数が2回以上の場合
        //※正確なバウンド数が取れないため
        if ((ball.boundCount >= Define.MAX_BOUNDCNT && !isAddScore) ||
            ball.isOut)
        {
            score.AddScore(ball.nowUserTag);
        }
    }
    #endregion

    #region 勝利判定処理
    private void VictoryJudgment()
    {
        //ユーザーデータを取得
        UserScoreData[] userSData = new UserScoreData[Define.USER_NUM];
        userSData[0] = new UserScoreData(score.user1Score, score.isUser1MatchP);
        userSData[1] = new UserScoreData(score.user2Score, score.isUser2MatchP);
        int j = 1;  //もう一つのユーザー指定用

        for (int i = 0; i < Define.USER_NUM; i++)
        {
            //ユーザーiが勝利スコアに到達かつユーザーjがマッチポイントでない場合
            if (userSData[i].score == winScore && !userSData[j].isMatchP)
            {
                gameState = GameState.GameSet;
                isGameSet = true;
            }

            //両ユーザーがマッチポイントの場合
            if (userSData[i].isMatchP && userSData[j].isMatchP)
            {
                isDeuce = true;
                score.MatchPReset();
                winScore++;
                break;
            }

            j--;
        }
    }
    #endregion

    #region 初期化処理
    private void Init()
    {
        safetyAreaObj.SetActive(false);

        isDeuce = false;
        isFault = false;
        isServe = true;
        isAddScore = false;
        isNextRound = false;
        isGameSet = false;

        changeCount = 1;

        faultState = FaultState.None;
        gameState = GameState.Serve;
        serveUser = User.User1;

        ServeAreaPosChange();
    }

    private void ObjInit()
    {
        //ボール、プレイヤー、着地地点の初期化
        Ball            iBall = GameObject.Find("Ball").GetComponent<Ball>();
        LandingForecast iLandForecast =
            GameObject.Find("RandingPointControl").GetComponent<LandingForecast>();
        Base[]          iPBase = new Base[Define.USER_NUM];
        for (int i = 0; i < Define.USER_NUM; i++)
            iPBase[i] = GameObject.Find(userObjTag[i]).GetComponent<Base>();

        iBall.Init();
        iLandForecast.Init();
        foreach (var pBase in iPBase) pBase.Init();
    }

    #endregion

    #region フィールド関連の処理
    //サーブエリア変更処理
    private void ServeAreaPosChange()
    {
        foreach (var obj in serveOutAreaObj)
        {
            obj.SetActive(true);
        }

        serveAreaObj.SetActive(true);
        safetyAreaObj.SetActive(false);

        //サーブユーザーがプレイヤー1の場合
        if (serveUser == User.User1)
        {
            //スコアが偶数の場合
            if (score.user1Score % 2 == 0)
            {
                serveAreaObj.transform.position = serveAreaPos[2];
                serveOutAreaObj[1].transform.position = new Vector3(-32.27f, 0f, -22f);
            }
            //スコアが奇数の場合
            else
            {
                serveAreaObj.transform.position = serveAreaPos[3];
                serveOutAreaObj[1].transform.position = new Vector3(-32.27f, 0f, 22f);
            }
            serveOutAreaObj[0].transform.position = new Vector3(59.7f, 0f, 0f);
            serveOutAreaObj[2].transform.position = new Vector3(-91.7f, 0f, 0f);
        }
        //サーブユーザーがプレイヤー2の場合
        else
        {
            //スコアが偶数の場合
            if (score.user2Score % 2 == 0)
            {
                serveAreaObj.transform.position = serveAreaPos[1];
                serveOutAreaObj[1].transform.position = new Vector3(32.27f, 0f, 22f);
            }
            //スコアが奇数の場合
            else
            {
                serveAreaObj.transform.position = serveAreaPos[0];
                serveOutAreaObj[1].transform.position = new Vector3(32.27f, 0f, -22f);
            }
            serveOutAreaObj[0].transform.position = new Vector3(-59.7f, 0f, 0f);
            serveOutAreaObj[2].transform.position = new Vector3(91.7f, 0f, 0f);
        }
    }
    public void ChangeField()
    {
        foreach (var obj in serveOutAreaObj)
        {
            obj.SetActive(false);
        }

        serveAreaObj.SetActive(false);
        safetyAreaObj.SetActive(true);
    }
    #endregion

    #region タグ関連
    private User InversionTag(User _user)
    {
        User returnUesr = User.User1;

        //タグを反転させる
        if (_user == User.User1)
        {
            returnUesr = User.User2;
        }

        return returnUesr;
    }
    #endregion

    #region フォルトの状態関連
    public void FaultProc()
    {

        //ボールを取得
        Ball ball = GameObject.Find("Ball").GetComponent<Ball>();

        switch (faultState)
        {
            case FaultState.None:
                faultState = FaultState.Fault;
                isFault = true;
                break;
            case FaultState.Fault:
                faultState = FaultState.DoubleFault;
                score.AddScore(InversionTag(ball.nowUserTag));
                break;
        }

        if (gameState != GameState.GameSet) StartCoroutine(NextRound());
    }
    #endregion

    #region ゲームの進行関連

    //次のラウンド
    private IEnumerator NextRound()
    {
        float timeCnt = Define.NEXT_ROUNDTIME;  //待機時間

        //ダブルフォルトまたはスコアが加算された際
        if(faultState == FaultState.DoubleFault || isAddScore)
        {
            isNextRound = true;
        }

        //サーブ切り替えカウントを加算
        if (isNextRound) changeCount++;

        //切り替えカウントが2の場合
        if (changeCount == Define.SERVE_CHANGECNT)
        {
            //サーブユーザーを切り替える
            serveUser = InversionTag(serveUser);

            //切り替えカウントを初期化
            changeCount = 0;
        }

        //サーブエリアの座標を適用
        ServeAreaPosChange();

        //ボール、プレイヤー、着地地点の初期化
        ObjInit();

        //処理待ち
        while (timeCnt >= 0f)
        {
            timeCnt -= Time.deltaTime;
            yield return null;
        }

        //ゲーム終了でない場合
        if (gameState != GameState.GameSet)
        {
            //ゲーム状態をサーブに変更
            gameState = GameState.Serve;

            //次のラウンドの際フォルトの状態をNoneに変更
            if (isNextRound)
            {
                faultState = FaultState.None;
            }

            //各フラグをオフにする
            isNextRound = false;
            isAddScore = false;
            isFault = false;

            //サーブフラグをオンに
            isServe = true;
        }
    }

    private void NextGame()
    {
        score.Init();
        Init();
        ObjInit();
    }

    #endregion
}
