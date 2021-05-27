using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���[�U�[�X�R�A�f�[�^
public class UserScoreData
{
    /*�v���p�e�B�֘A*/
    public int  score { get; private set; }     //�X�R�A
    public bool isMatchP { get; private set; }  //�}�b�`�|�C���g�t���O

    //�R���X�g���N�^
    public UserScoreData(int _score,bool _isMatchP)
    {
        score = _score;
        isMatchP = _isMatchP;
    }
}

//�Q�[���}�l�[�W���[
public class GameManager : MonoBehaviour
{
    /*���̃X�N���v�g�ł̂ݎg���ϐ�*/
    public static GameManager   instance;   //�C���X�^���X
    private Score               score;      //�X�R�A�N���X

    /*�v���p�e�B�֘A*/
    public bool isDeuce { get; set; }    //�f���[�X�t���O
    public bool isAddScore { get; set; } //�X�R�A�t���O

    /*�C���X�y�N�^�[�ɕ\�����͐ݒ肷��ϐ�*/
    [SerializeField]
    private int winScore;   //�����X�R�A

    //����������
    private void Start()
    {
        score = this.GetComponentInChildren<Score>();
        instance = this;
        isDeuce = false;
        isAddScore = false;
    }

    //�X�V����
    private void Update()
    {
        //�X�R�A���ǉ����ꂽ��
        if (isAddScore)
        {
            //�Q�[�������̃��E���h��
            Ball iBall = GameObject.Find("Ball").GetComponent<Ball>();

            iBall.Init();
        }

        //�{�[�����擾
        Ball ball = GameObject.Find("Ball").GetComponent<Ball>();

        //���[�U�[�f�[�^���擾
        UserScoreData[] userSData = new UserScoreData[2];
        userSData[0] = new UserScoreData(score.user1Score, score.isUser1MatchP);
        userSData[1] = new UserScoreData(score.user2Score, score.isUser2MatchP);
        int j = 1;  //������̃��[�U�[�w��p

        #region ���_����
        //�A�E�g�̏ꍇ
        if ((ball.isOut || ball.isNet) && !isAddScore)
        {
            string addScoreUser = ball.nowUserTag;

            //�^�O�𔽓]������
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

        //�o�E���h�񐔂�2��ȏ�̏ꍇ
        //�����m�ȃo�E���h�������Ȃ�����
        if(ball.boundCount >= 2 && !isAddScore)
        {
            score.AddScore(ball.nowUserTag);
        }
        #endregion

        #region ��������
        for (int i = 0; i < 2; i++)
        {
            //���[�U�[i�������X�R�A�ɓ��B�����[�U�[j���}�b�`�|�C���g�łȂ��ꍇ
            if(userSData[i].score == winScore && !userSData[j].isMatchP)
            {
                Debug.Log("�����܂����I�I");
            }

            //�����[�U�[���}�b�`�|�C���g�̏ꍇ
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
        #endregion
    }
}
