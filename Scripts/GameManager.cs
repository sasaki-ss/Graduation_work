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

public enum GameState
{
   Serve,
   DuringRound
}

//�Q�[���}�l�[�W���[
public class GameManager : MonoBehaviour
{
    /*�O���Q�ƂɎg���ϐ�*/
    public static GameManager   instance;       //�C���X�^���X

    /*���̃X�N���v�g�ł̂ݎg���ϐ�*/
    private Score               score;              //�X�R�A�N���X
    private GameObject          serveAreaObj;       //�T�[�u�G���A�I�u�W�F�N�g
    private GameObject          safetyAreaObj;      //�Z�[�t�e�B�G���A�I�u�W�F�N�g
    [SerializeField]
    private GameObject[]        serveOutAreaObj;    //�T�[�u�A�E�g�G���A�I�u�W�F�N�g
    private Vector3[]           serveAreaPos;       //�Z�[�u�G���A�̍��W
    private int                 serveUser;          //�T�[�u���郆�[�U�[
    private int                 changeCount;        //���E���h�J�E���g

    /*�v���p�e�B�֘A*/
    public bool isDeuce { get; set; }           //�f���[�X�t���O
    public bool isAddScore { get; set; }        //�X�R�A�t���O
    public bool isNextRound { get; set; }       //���̃��E���h�t���O
    public GameState gameState { get; set; }    //�Q�[���̏��

    /*�C���X�y�N�^�[�ɕ\�����͐ݒ肷��ϐ�*/
    [SerializeField]
    private int winScore;   //�����X�R�A

    //����������
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
        isAddScore = false;
        isNextRound = false;

        gameState = GameState.Serve;

        serveAreaPos = new Vector3[4]
        {
            new Vector3( 32f,0f, 21f), //���[�U�[1���E
            new Vector3( 32f,0f,-21f), //���[�U�[1����
            new Vector3(-32f,0f, 21f), //���[�U�[2���E
            new Vector3(-32f,0f,-21f), //���[�U�[2����
        };

        serveUser = 0;
        changeCount = 2;

        ServeAreaPosChange();
    }

    //�X�V����
    private void Update()
    {
        //�X�R�A���ǉ����ꂽ��
        if (isAddScore && !isNextRound)
        {
            StartCoroutine(NextRound());
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
                break;
            }

            j--;
        }
        #endregion
    }

    //�T�[�o�[���[�U�[��؂�ւ���
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
        //�T�[�u���[�U�[���v���C���[1�̏ꍇ
        if (serveUser == 0)
        {
            //�X�R�A�������̏ꍇ
            if (score.user1Score % 2 == 0)
            {
                serveAreaObj.transform.position = serveAreaPos[2];
                serveOutAreaObj[1].transform.position = new Vector3(-32.27f, 0f, -22f);
            }
            //�X�R�A����̏ꍇ
            else
            {
                serveAreaObj.transform.position = serveAreaPos[3];
                serveOutAreaObj[1].transform.position = new Vector3(-32.27f, 0f, 22f);
            }
            serveOutAreaObj[0].transform.position = new Vector3(59.7f, 0f, 0f);
            serveOutAreaObj[2].transform.position = new Vector3(-91.7f, 0f, 0f);
        }
        //�T�[�u���[�U�[���v���C���[2�̏ꍇ
        else
        {
            //�X�R�A�������̏ꍇ
            if (score.user2Score % 2 == 0)
            {
                serveAreaObj.transform.position = serveAreaPos[1];
                serveOutAreaObj[1].transform.position = new Vector3(32.27f, 0f, 22f);
            }
            //�X�R�A����̏ꍇ
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
        float timeCnt = 10.0f;
        isNextRound = true;

        //�Q�[�������̃��E���h��
        Ball iBall = GameObject.Find("Ball").GetComponent<Ball>();
        
        //iBall.Init();


        #region �T�[�u���[�U�[�؂�ւ�����
        //�؂�ւ�����
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
        //isAddScore = false;
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
}
