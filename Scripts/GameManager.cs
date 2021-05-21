using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ユーザースコアデータ
public class UserScoreData
{
    /*プロパティ関連*/
    public int score { get; private set; }
    public bool isMatchP { get; private set; }

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
    public static GameManager instance;     //インスタンス
    private Score               score;      //スコアクラス

    /*プロパティ関連*/
    public bool isDeuce { get; set; }    //デュースフラグ

    /*インスペクターに表示又は設定する変数*/
    [SerializeField]
    private int winScore;   //勝利スコア

    //初期化処理
    private void Start()
    {
        score = this.GetComponentInChildren<Score>();
        instance = this;
        isDeuce = false;
    }

    //更新処理
    private void Update()
    {
        /*後で消します*/
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            score.AddScore("Player");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            score.AddScore("Player2");
        }

        //ボールを取得
        Ball ball = GameObject.Find("Ball").GetComponent<Ball>();

        //バウンド回数が2回の場合
        if(ball.boundCount == 2)
        {
            score.AddScore(ball.tag);
        }

        //ユーザーデータを取得
        UserScoreData[] userSData = new UserScoreData[2];
        userSData[0] = new UserScoreData(score.user1Score, score.isUser1MatchP);
        userSData[1] = new UserScoreData(score.user2Score, score.isUser2MatchP);

        int j = 1;  //もう一つのユーザー指定用

        for(int i = 0; i < 2; i++)
        {
            if(userSData[i].score == winScore && !userSData[j].isMatchP)
            {
                Debug.Log("勝ちました！！");
            }

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
    }
}
