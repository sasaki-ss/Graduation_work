using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public enum GameState
{
   Serve,
   DuringRound
}

//ゲームマネージャー
public class GameManager : MonoBehaviour
{
    /*外部参照に使う変数*/
    public static GameManager   instance;       //インスタンス

    /*このスクリプトでのみ使う変数*/
    private Score               score;          //スコアクラス
    private GameObject          serveAreaObj;   //サーブエリアオブジェクト
    private GameObject          safetyAreaObj;  //セーフティエリアオブジェクト
    private Vector3[]           serveAreaPos;   //セーブエリアの座標
    private int                 serveUser;      //サーブするユーザー
    private int                 changeCount;    //ラウンドカウント

    /*プロパティ関連*/
    public bool isDeuce { get; set; }           //デュースフラグ
    public bool isAddScore { get; set; }        //スコアフラグ
    public bool isNextRound { get; set; }       //次のラウンドフラグ
    public GameState gameState { get; set; }    //ゲームの状態

    /*インスペクターに表示又は設定する変数*/
    [SerializeField]
    private int winScore;   //勝利スコア

    //初期化処理
    private void Start()
    {
        score = this.GetComponentInChildren<Score>();
        serveAreaObj = GameObject.Find("ServeArea");
        safetyAreaObj = GameObject.Find("SafetyArea");

        safetyAreaObj.SetActive(false);

        instance = this;
        isDeuce = false;
        isAddScore = false;
        isNextRound = false;

        gameState = GameState.Serve;

        serveAreaPos = new Vector3[4]
        {
            new Vector3( 32,0, 21), //ユーザー1側右
            new Vector3( 32,0,-21), //ユーザー1側左
            new Vector3(-32,0, 21), //ユーザー2側右
            new Vector3(-32,0,-21)  //ユーザー2側左
        };

        serveUser = 0;
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
            //isAddScore = false;
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
            string addScoreUser = ball.nowUserTag;

            //タグを反転させる
            if (addScoreUser == "Player")
            {
                addScoreUser = "Player2";
            }
            else
            {
                addScoreUser = "Player";
            }

            score.AddScore(addScoreUser);
        }

        //バウンド回数が2回以上の場合
        //※正確なバウンド数が取れないため
        if(ball.boundCount >= 2 && !isAddScore)
        {
            score.AddScore(ball.nowUserTag);
        }
        #endregion

        #region 勝利判定
        for (int i = 0; i < 2; i++)
        {
            //ユーザーiが勝利スコアに到達かつユーザーjがマッチポイントでない場合
            if(userSData[i].score == winScore && !userSData[j].isMatchP)
            {
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
        if(serveUser == 1)
        {
            serveUser = 0;
        }
        else
        {
            serveUser = 1;
        }
    }

    private void ServeAreaPosChange()
    {
        //サーブユーザーがプレイヤー1の場合
        if (serveUser == 0)
        {
            //スコアが偶数の場合
            if (score.user1Score % 2 == 0)
            {
                serveAreaObj.transform.position = serveAreaPos[2];
            }
            //スコアが奇数の場合
            else
            {
                serveAreaObj.transform.position = serveAreaPos[3];
            }
        }
        //サーブユーザーがプレイヤー2の場合
        else
        {
            //スコアが偶数の場合
            if (score.user2Score % 2 == 0)
            {
                serveAreaObj.transform.position = serveAreaPos[1];
            }
            //スコアが奇数の場合
            else
            {
                serveAreaObj.transform.position = serveAreaPos[0];
            }
        }
    }

    private IEnumerator NextRound()
    {
        float timeCnt = 10.0f;
        isNextRound = true;

        //ゲームを次のラウンドへ
        Ball iBall = GameObject.Find("Ball").GetComponent<Ball>();
        
        iBall.Init();


        #region サーブユーザー切り替え処理
        //切り替え処理
        if (changeCount == 2)
        {
            ServeUserChange();
            changeCount = 0;

            ServeAreaPosChange();
        }

        changeCount++;
        #endregion



        while (timeCnt <= 0f)
        {
            yield return null;
        }

        isNextRound = false;
    }

    public void ChangeField()
    {
        serveAreaObj.SetActive(false);
        safetyAreaObj.SetActive(true);
    }
}
