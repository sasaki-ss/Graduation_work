using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定数クラス
public class Define
{
    public const float NEXT_ROUNDTIME = 2.0f;  //ラウンド間の時間
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
    private Score           score;               //スコアクラス
    private GameObject      serveAreaObj;        //サーブエリアオブジェクト
    private GameObject      safetyAreaObj;       //セーフティエリアオブジェクト
    private GameObject[]    serveOutAreaObj;     //サーブアウトエリアオブジェクト
    private Vector3[]       serveAreaPos;        //セーブエリアの座標
    private int             changeCount;         //ラウンドカウント

    /*プロパティ関連*/
    public bool         isDeuce { get; set; }              //デュースフラグ
    public bool         isAddScore { get; set; }           //スコアフラグ
    public bool         isNextRound { get; set; }          //次のラウンドフラグ
    public bool         isServe { get; set; }              //サーブフラグ
    public bool         isFault { get; private set; }      //フォルトフラグ
    public GameState    gameState { get; set; }            //ゲームの状態
    public User         serveUser { get; private set; }    //サーブするユーザー
    public FaultState   faultState { get; private set; }   //フォルト状態

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

        safetyAreaObj.SetActive(false);

        instance = this;
        isDeuce = false;
        isFault = false;
        isServe = true;
        isAddScore = false;
        isNextRound = false;

        faultState = FaultState.None;
        gameState = GameState.Serve;
        serveUser = User.User1;

        serveAreaPos = new Vector3[4]
        {
            new Vector3( 32f,0f, 21f), //ユーザー1側右
            new Vector3( 32f,0f,-21f), //ユーザー1側左
            new Vector3(-32f,0f, 21f), //ユーザー2側右
            new Vector3(-32f,0f,-21f), //ユーザー2側左
        };

        changeCount = 2;

        ServeAreaPosChange();
    }

    //更新処理
    private void Update()
    {
        //スコアが追加された際
        if (isAddScore && !isNextRound)
        {
            StartCoroutine(NextRound());
        }

        //ボールを取得
        Ball ball = GameObject.Find("Ball").GetComponent<Ball>();

        //ユーザーデータを取得
        UserScoreData[] userSData = new UserScoreData[2];
        userSData[0] = new UserScoreData(score.user1Score, score.isUser1MatchP);
        userSData[1] = new UserScoreData(score.user2Score, score.isUser2MatchP);
        int j = 1;  //もう一つのユーザー指定用

        #region 得点処理
        //アウトの場合
        if ((ball.isOut || ball.isNet) && !isAddScore)
        {
            score.AddScore(InversionTag(ball.nowUserTag));
        }

        //バウンド回数が2回以上の場合
        //※正確なバウンド数が取れないため
        if(ball.boundCount >= 2 && !isAddScore)
        {
            score.AddScore(ball.nowUserTag);
            Debug.Log("得点が入りました！ " + ball.nowUserTag);
        }
        #endregion

        #region 勝利判定
        for (int i = 0; i < 2; i++)
        {
            //ユーザーiが勝利スコアに到達かつユーザーjがマッチポイントでない場合
            if(userSData[i].score == winScore && !userSData[j].isMatchP)
            {
                gameState = GameState.GameSet;
                Debug.Log("勝ちました！！");
            }

            //両ユーザーがマッチポイントの場合
            if(userSData[i].isMatchP && userSData[j].isMatchP)
            {
                isDeuce = true;
                score.MatchPReset();
                winScore++;
                break;
            }

            j--;
        }
        #endregion
    }


    //サーバーユーザーを切り替える
    private void ServeUserChange()
    {
        if(serveUser == User.User2)
        {
            serveUser = User.User1;
        }
        else
        {
            serveUser = User.User2;
        }
    }

    //サーブエリア変更処理
    private void ServeAreaPosChange()
    {
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

    private IEnumerator NextRound()
    {
        float timeCnt = Define.NEXT_ROUNDTIME;
        isNextRound = true;

        //ゲームを次のラウンドへ
        Ball iBall = GameObject.Find("Ball").GetComponent<Ball>();
        Base[] iPBase = new Base[2];
        iPBase[(int)User.User1] = GameObject.Find("Player").GetComponent<Base>();
        iPBase[(int)User.User2] = GameObject.Find("Player2").GetComponent<Base>();

        iBall.Init();
        foreach (var pBase in iPBase) pBase.Init();

        gameState = GameState.Serve;
        isServe = true;

        #region サーブユーザー切り替え処理
        //切り替え処理
        if (changeCount == 2 && !isFault)
        {
            ServeUserChange();
            changeCount = 0;

            ServeAreaPosChange();
        }

        if (gameState == GameState.DuringRound ||
            faultState == FaultState.DoubleFault) changeCount++;
        #endregion

        while (timeCnt >= 0f)
        {
            timeCnt -= Time.deltaTime;
            yield return null;
        }

        isNextRound = false;
        isAddScore = false;
        isFault = false;
        if (faultState == FaultState.DoubleFault)faultState = FaultState.None;
    }

    public void ChangeField()
    {
        foreach(var obj in serveOutAreaObj)
        {
            obj.SetActive(false);
        }

        serveAreaObj.SetActive(false);
        safetyAreaObj.SetActive(true);
    }

    private User InversionTag(User _user)
    {
        //タグを反転させる
        if (_user == User.User1)
        {
            _user = User.User2;
        }
        else
        {
            _user = User.User1;
        }

        return _user;
    }

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

        StartCoroutine(NextRound());
    }
}
