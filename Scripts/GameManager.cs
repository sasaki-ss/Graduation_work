using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserScoreData
{
    public int score { get; private set; }
    public bool isMatchP { get; private set; }

    public UserScoreData(int _score,bool _isMatchP)
    {
        score = _score;
        isMatchP = _isMatchP;
    }
}

//ゲームマネージャー
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    Score score;            //スコアクラス
    [SerializeField]
    private int winScore;   //勝利スコア

    public bool isDeuce { get; set; }    //デュースフラグ

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
