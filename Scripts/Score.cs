using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//スコアクラス
public class Score : MonoBehaviour
{
    /*プロパティ関連*/
    public int  user1Score { get; private set; }        //user1のスコア
    public int  user2Score { get; private set; }        //user2のスコア
    public bool isUser1MatchP { get; private set; }     //user1のマッチポイントフラグ
    public bool isUser2MatchP { get; private set; }     //user2のマッチポイントフラグ

    /*インスペクターに表示又は設定する変数*/
    [SerializeField]
    private int matchPScore;    //マッチポイントの得点

    //初期化処理
    private void Start()
    {
        Init();
    }

    //初期化処理
    private void Init()
    {
        user1Score = 0;
        user2Score = 0;
        isUser1MatchP = false;
        isUser2MatchP = false;
    }

    //マッチポイントをリセットする処理(デュースの際に使う)
    public void MatchPReset()
    {
        isUser1MatchP = false;
        isUser2MatchP = false;
    }

    //スコアを加算する
    public void AddScore(User _playerTag)
    {
        //デュースの際
        if (GameManager.instance.isDeuce)
        {
            matchPScore++;
            GameManager.instance.isDeuce = false;
        }

        //タグが"Player"の場合
        if (_playerTag == User.User1)
        {
            user1Score++;
            if (user1Score == matchPScore) isUser1MatchP = true;
        }
        //タグが"Player2"の場合
        else
        {
            user2Score++;
            if (user2Score == matchPScore) isUser2MatchP = true;
        }
        
        //スコア追加フラグをオンにする
        GameManager.instance.isAddScore = true;

        GameManager.instance.isFault = false;
        GameManager.instance.faultState = FaultState.None;

        GameManager.instance.changeCount++;

        Debug.Log("User1 : " + user1Score + " " + "User2 : " + user2Score);

    }
}
