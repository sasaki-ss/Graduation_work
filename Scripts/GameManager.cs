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

//ゲームマネージャー
public class GameManager : MonoBehaviour
{
    /*このスクリプトでのみ使う変数*/
    public static GameManager   instance;   //インスタンス
    private Score               score;      //スコアクラス

    /*プロパティ関連*/
    public bool isDeuce { get; set; }    //デュースフラグ
    public bool isAddScore { get; set; } //スコアフラグ

    /*インスペクターに表示又は設定する変数*/
    [SerializeField]
    private int winScore;   //勝利スコア

    //初期化処理
    private void Start()
    {
        score = this.GetComponentInChildren<Score>();
        instance = this;
        isDeuce = false;
        isAddScore = false;
    }

    //更新処理
    private void Update()
    {
        //スコアが追加された際
        if (isAddScore)
        {
            //ゲームを次のラウンドへ
            Ball iBall = GameObject.Find("Ball").GetComponent<Ball>();

            iBall.Init();
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

                Debug.Log("デュースになりました！！");
                break;
            }

            j--;
        }
        #endregion
    }
}
