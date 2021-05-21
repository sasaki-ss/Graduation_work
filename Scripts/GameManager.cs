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

//�Q�[���}�l�[�W���[
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    Score score;            //�X�R�A�N���X
    [SerializeField]
    private int winScore;   //�����X�R�A

    public bool isDeuce { get; set; }    //�f���[�X�t���O

    private void Start()
    {
        score = this.GetComponentInChildren<Score>();
        instance = this;
        isDeuce = false;
    }

    //�X�V����
    private void Update()
    {
        /*��ŏ����܂�*/
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            score.AddScore("Player");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            score.AddScore("Player2");
        }

        //���[�U�[�f�[�^���擾
        UserScoreData[] userSData = new UserScoreData[2];
        userSData[0] = new UserScoreData(score.user1Score, score.isUser1MatchP);
        userSData[1] = new UserScoreData(score.user2Score, score.isUser2MatchP);

        int j = 1;  //������̃��[�U�[�w��p

        for(int i = 0; i < 2; i++)
        {
            if(userSData[i].score == winScore && !userSData[j].isMatchP)
            {
                Debug.Log("�����܂����I�I");
            }

            if(userSData[i].isMatchP && userSData[j].isMatchP)
            {
                isDeuce = true;
                score.MatchPReset();
                winScore++;

                Debug.Log("�f���[�X�ɂȂ�܂����I�I");
                break;
            }

            j--;
        }
    }
}
