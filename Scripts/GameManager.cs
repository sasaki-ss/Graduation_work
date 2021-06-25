using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�萔�N���X
public class Define
{
    public const float  NEXT_ROUNDTIME = 2.0f;  //���E���h�Ԃ̎���
    public const int    USER_NUM = 2;           //���[�U�[�̐�
    public const int    SERVE_CHANGECNT = 2;    //�T�[�u�؂�ւ��̐�
    public const int    MAX_BOUNDCNT = 2;       //�ő�̃o�E���h��
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
    private Score           score;              //�X�R�A�N���X
    private GameObject      serveAreaObj;       //�T�[�u�G���A�I�u�W�F�N�g
    private GameObject      safetyAreaObj;      //�Z�[�t�e�B�G���A�I�u�W�F�N�g
    private GameObject[]    serveOutAreaObj;    //�T�[�u�A�E�g�G���A�I�u�W�F�N�g
    private Vector3[]       serveAreaPos;       //�Z�[�u�G���A�̍��W
    private int             changeCount;        //���E���h�J�E���g
    private string[]        userObjTag;         //�v���C���[�I�u�W�F�N�g�̃^�O

    /*�v���p�e�B�֘A*/
    public bool         isDeuce { get; set; }              //�f���[�X�t���O
    public bool         isAddScore { get; set; }           //�X�R�A�t���O
    public bool         isNextRound { get;  private set; } //���̃��E���h�t���O
    public bool         isServe { get; set; }              //�T�[�u�t���O
    public bool         isFault { get; private set; }      //�t�H���g�t���O
    public bool         isGameSet { get; private set; }    //�Q�[���I���t���O
    public GameState    gameState { get; set; }            //�Q�[���̏��
    public User         serveUser { get; private set; }    //�T�[�u���郆�[�U�[
    public FaultState   faultState { get; set; }           //�t�H���g���

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

        serveAreaPos = new Vector3[4]
        {
            new Vector3( 32f,0f, 21f), //���[�U�[1���E
            new Vector3( 32f,0f,-21f), //���[�U�[1����
            new Vector3(-32f,0f, 21f), //���[�U�[2���E
            new Vector3(-32f,0f,-21f), //���[�U�[2����
        };

        userObjTag = new string[Define.USER_NUM]
        {
            "Player",
            "Player2"
        };

        instance = this;
        Init();
    }

    //�X�V����
    private void Update()
    {
        //�X�R�A���ǉ����ꂽ��
        if (isAddScore && !isNextRound && !isGameSet)
        {
            StartCoroutine(NextRound());
        }

        //���_����
        ScoreProc();

        //��������
        VictoryJudgment();
    }

    #region ���_����
    private void ScoreProc()
    {
        //�{�[�����擾
        Ball ball = GameObject.Find("Ball").GetComponent<Ball>();

        //�A�E�g�̏ꍇ
        if (ball.isNet && !isAddScore)
        {
            score.AddScore(InversionTag(ball.nowUserTag));
        }

        //�o�E���h�񐔂�2��ȏ�̏ꍇ
        //�����m�ȃo�E���h�������Ȃ�����
        if ((ball.boundCount >= Define.MAX_BOUNDCNT && !isAddScore) ||
            ball.isOut)
        {
            score.AddScore(ball.nowUserTag);
        }
    }
    #endregion

    #region �������菈��
    private void VictoryJudgment()
    {
        //���[�U�[�f�[�^���擾
        UserScoreData[] userSData = new UserScoreData[Define.USER_NUM];
        userSData[0] = new UserScoreData(score.user1Score, score.isUser1MatchP);
        userSData[1] = new UserScoreData(score.user2Score, score.isUser2MatchP);
        int j = 1;  //������̃��[�U�[�w��p

        for (int i = 0; i < Define.USER_NUM; i++)
        {
            //���[�U�[i�������X�R�A�ɓ��B�����[�U�[j���}�b�`�|�C���g�łȂ��ꍇ
            if (userSData[i].score == winScore && !userSData[j].isMatchP)
            {
                gameState = GameState.GameSet;
                isGameSet = true;
            }

            //�����[�U�[���}�b�`�|�C���g�̏ꍇ
            if (userSData[i].isMatchP && userSData[j].isMatchP)
            {
                isDeuce = true;
                score.MatchPReset();
                winScore++;
                break;
            }

            j--;
        }
    }
    #endregion

    #region ����������
    private void Init()
    {
        safetyAreaObj.SetActive(false);

        isDeuce = false;
        isFault = false;
        isServe = true;
        isAddScore = false;
        isNextRound = false;
        isGameSet = false;

        changeCount = 1;

        faultState = FaultState.None;
        gameState = GameState.Serve;
        serveUser = User.User1;

        ServeAreaPosChange();
    }

    private void ObjInit()
    {
        //�{�[���A�v���C���[�A���n�n�_�̏�����
        Ball            iBall = GameObject.Find("Ball").GetComponent<Ball>();
        LandingForecast iLandForecast =
            GameObject.Find("RandingPointControl").GetComponent<LandingForecast>();
        Base[]          iPBase = new Base[Define.USER_NUM];
        for (int i = 0; i < Define.USER_NUM; i++)
            iPBase[i] = GameObject.Find(userObjTag[i]).GetComponent<Base>();

        iBall.Init();
        iLandForecast.Init();
        foreach (var pBase in iPBase) pBase.Init();
    }

    #endregion

    #region �t�B�[���h�֘A�̏���
    //�T�[�u�G���A�ύX����
    private void ServeAreaPosChange()
    {
        foreach (var obj in serveOutAreaObj)
        {
            obj.SetActive(true);
        }

        serveAreaObj.SetActive(true);
        safetyAreaObj.SetActive(false);

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
    public void ChangeField()
    {
        foreach (var obj in serveOutAreaObj)
        {
            obj.SetActive(false);
        }

        serveAreaObj.SetActive(false);
        safetyAreaObj.SetActive(true);
    }
    #endregion

    #region �^�O�֘A
    private User InversionTag(User _user)
    {
        User returnUesr = User.User1;

        //�^�O�𔽓]������
        if (_user == User.User1)
        {
            returnUesr = User.User2;
        }

        return returnUesr;
    }
    #endregion

    #region �t�H���g�̏�Ԋ֘A
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

        if (gameState != GameState.GameSet) StartCoroutine(NextRound());
    }
    #endregion

    #region �Q�[���̐i�s�֘A

    //���̃��E���h
    private IEnumerator NextRound()
    {
        float timeCnt = Define.NEXT_ROUNDTIME;  //�ҋ@����

        //�_�u���t�H���g�܂��̓X�R�A�����Z���ꂽ��
        if(faultState == FaultState.DoubleFault || isAddScore)
        {
            isNextRound = true;
        }

        //�T�[�u�؂�ւ��J�E���g�����Z
        if (isNextRound) changeCount++;

        //�؂�ւ��J�E���g��2�̏ꍇ
        if (changeCount == Define.SERVE_CHANGECNT)
        {
            //�T�[�u���[�U�[��؂�ւ���
            serveUser = InversionTag(serveUser);

            //�؂�ւ��J�E���g��������
            changeCount = 0;
        }

        //�T�[�u�G���A�̍��W��K�p
        ServeAreaPosChange();

        //�{�[���A�v���C���[�A���n�n�_�̏�����
        ObjInit();

        //�����҂�
        while (timeCnt >= 0f)
        {
            timeCnt -= Time.deltaTime;
            yield return null;
        }

        //�Q�[���I���łȂ��ꍇ
        if (gameState != GameState.GameSet)
        {
            //�Q�[����Ԃ��T�[�u�ɕύX
            gameState = GameState.Serve;

            //���̃��E���h�̍ۃt�H���g�̏�Ԃ�None�ɕύX
            if (isNextRound)
            {
                faultState = FaultState.None;
            }

            //�e�t���O���I�t�ɂ���
            isNextRound = false;
            isAddScore = false;
            isFault = false;

            //�T�[�u�t���O���I����
            isServe = true;
        }
    }

    private void NextGame()
    {
        score.Init();
        Init();
        ObjInit();
    }

    #endregion
}
