using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�萔�N���X
public class Define
{
    public const float NEXT_ROUNDTIME = 2.0f;  //���E���h�Ԃ̎���
}

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

//�Q�[�����
public enum GameState
{
   Serve,           //�T�[�u
   DuringRound,     //���E���h
   GameSet          //�Q�[���I��
}

//�T�[�u���[�U�[
public enum User
{
    User1,      //���[�U�[1
    User2       //���[�U�[2
}

//�t�H���g���
public enum FaultState
{
    None,
    Fault,
    DoubleFault,
}

//�Q�[���}�l�[�W���[
public class GameManager : MonoBehaviour
{
    /*�O���Q�ƂɎg���ϐ�*/
    public static GameManager instance;               //�C���X�^���X

    /*���̃X�N���v�g�ł̂ݎg���ϐ�*/
    private Score           score;               //�X�R�A�N���X
    private GameObject      serveAreaObj;        //�T�[�u�G���A�I�u�W�F�N�g
    private GameObject      safetyAreaObj;       //�Z�[�t�e�B�G���A�I�u�W�F�N�g
    private GameObject[]    serveOutAreaObj;     //�T�[�u�A�E�g�G���A�I�u�W�F�N�g
    private Vector3[]       serveAreaPos;        //�Z�[�u�G���A�̍��W
    private int             changeCount;         //���E���h�J�E���g

    /*�v���p�e�B�֘A*/
    public bool         isDeuce { get; set; }              //�f���[�X�t���O
    public bool         isAddScore { get; set; }           //�X�R�A�t���O
    public bool         isNextRound { get; set; }          //���̃��E���h�t���O
    public bool         isServe { get; set; }              //�T�[�u�t���O
    public bool         isFault { get; private set; }      //�t�H���g�t���O
    public GameState    gameState { get; set; }            //�Q�[���̏��
    public User         serveUser { get; private set; }    //�T�[�u���郆�[�U�[
    public FaultState   faultState { get; private set; }   //�t�H���g���

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
        isFault = false;
        isServe = true;
        isAddScore = false;
        isNextRound = false;

        faultState = FaultState.None;
        gameState = GameState.Serve;
        serveUser = User.User1;

        serveAreaPos = new Vector3[4]
        {
            new Vector3( 32f,0f, 21f), //���[�U�[1���E
            new Vector3( 32f,0f,-21f), //���[�U�[1����
            new Vector3(-32f,0f, 21f), //���[�U�[2���E
            new Vector3(-32f,0f,-21f), //���[�U�[2����
        };

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
            score.AddScore(InversionTag(ball.nowUserTag));
        }

        //�o�E���h�񐔂�2��ȏ�̏ꍇ
        //�����m�ȃo�E���h�������Ȃ�����
        if(ball.boundCount >= 2 && !isAddScore)
        {
            score.AddScore(ball.nowUserTag);
            Debug.Log("���_������܂����I " + ball.nowUserTag);
        }
        #endregion

        #region ��������
        for (int i = 0; i < 2; i++)
        {
            //���[�U�[i�������X�R�A�ɓ��B�����[�U�[j���}�b�`�|�C���g�łȂ��ꍇ
            if(userSData[i].score == winScore && !userSData[j].isMatchP)
            {
                gameState = GameState.GameSet;
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
        if(serveUser == User.User2)
        {
            serveUser = User.User1;
        }
        else
        {
            serveUser = User.User2;
        }
    }

    //�T�[�u�G���A�ύX����
    private void ServeAreaPosChange()
    {
        //�T�[�u���[�U�[���v���C���[1�̏ꍇ
        if (serveUser == User.User1)
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
        float timeCnt = Define.NEXT_ROUNDTIME;
        isNextRound = true;

        //�Q�[�������̃��E���h��
        Ball iBall = GameObject.Find("Ball").GetComponent<Ball>();
        Base[] iPBase = new Base[2];
        iPBase[(int)User.User1] = GameObject.Find("Player").GetComponent<Base>();
        iPBase[(int)User.User2] = GameObject.Find("Player2").GetComponent<Base>();

        iBall.Init();
        foreach (var pBase in iPBase) pBase.Init();

        gameState = GameState.Serve;
        isServe = true;

        #region �T�[�u���[�U�[�؂�ւ�����
        //�؂�ւ�����
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
        //�^�O�𔽓]������
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

        //�{�[�����擾
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
