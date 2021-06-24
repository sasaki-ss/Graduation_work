using UnityEngine;
using UnityEngine.UI;
using TapStateManager;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    //�^�b�v����擾
    TouchManager tMger;                 //TapStateManager����TouchManager
    private bool createFlg;             //�^�b�v����UI�̐����t���O

    //���E���h���̐���
    private bool rCreateFlg;            //���E���h���̐����t���O

    //�T�[�u���̐���
    private bool sCreateFlg;            //�T�[�u���̐����t���O

    //UI�̗v�f��
    private const int UINum = 8;        //��������UI�̗v�f��
    private const int RoundUINum = 5;   //���E���h�Ԃɕ\������UI�̗v�f��

    private int i = 0;                  //UI�̗v�f���̃J�E���g
    private int j = 0;                  //�Q�[��������������폜����I�u�W�F�N�g�̗v�f��
    private int r = 0;                  //���E���h�؂�ւ����ɐ���������폜����v�f��

    //�v���n�u�̊i�[
    private GameObject textPref;        //�e�L�X�g
    private GameObject lgPref;          //���C���Q�[�W
    private GameObject pgPref1;         //�p���[�Q�[�W(�g��)
    private GameObject pgPref2;         //�p���[�Q�[�W(������)
    private GameObject linePref;        //��
    private GameObject panelPref;       //�p�l��
    private GameObject buttonPref;      //�{�^��
    private GameObject trianglePref;    //�O�p�`

    //UI�̃C���X�^���X���i�[����ϐ�
    private GameObject[] instances;
    private GameObject[] roundInstances;
    private GameObject serveInstance;
    

    //�e�L�X�g
    private Text textScore;             //�X�R�A�̕\��
    private Text textPlayerName;        //�v���C���[��
    private Text textOpponentName;      //���薼
    private Text textRoundBetween;      //���E���h�Ԃɕ\������

    //�Q�[�W
    private Slider lgPlayer;            //�v���C���[�̃X�^�~�i�Q�[�W
    private Slider lgOpponent;          //����̃X�^�~�i�Q�[�W
    private Image pGauge1;              //�p���[�Q�[�W(�g��)
    private Image pGauge2;              //�p���[�Q�[�W(������)

    //��
    private LineRenderer line;          //��΂������̐�
    private AnimationCurve lineCurve;   //���̕��̃A�j���[�V�����J�[�u

    //�p�l��
    private Image panel;                //���E���h�Ԃɕ\������p�l��(��ʂ��Â����邱�ƂŃe�L�X�gUI�����₷������)

    //�{�^��
    private Button retryB;              //���g���C�{�^��
    private Text retryBtext;            //���g���C�{�^���̃e�L�X�g

    private Image triangle;             //�O�p�`

    //�ێ��p
    private float gaugeKeep;            //�Q�[�W�̃L�[�v
    private Vector3 vertexKeep;         //���_�̍��W�L�[�v

    //���W
    private Vector2 scorePos;           //�X�R�A�̍��W
    private Vector2 pNamePos;           //�v���C���[���̍��W
    private Vector2 oNamePos;           //���薼�̍��W
    private Vector2 plgPos;             //�v���C���[�̃X�^�~�i�Q�[�W�̍��W
    private Vector2 olgPos;             //����̃X�^�~�i�Q�[�W�̍��W
    private Vector3 rTextPos;           //���E���h�Ԃɕ\������e�L�X�g�̍��W
    private Vector3 pgPos;              //�p���[�Q�[�W�̍��W
    private Vector3 plViewPos;          //�v���C���[�̃J������̍��W
    private Vector3 pgViewPos;          //plViewPos�Ɠ����ʒu�ɕ\�����邽�߂̍��W
    private Vector3 linePos;            //���̌��_
    private Vector3 lineEndPos;         //���̈ړ����钸�_
    private Vector3 panelPos;           //�p�l���̍��W
    private Vector2 gSetTextPos;        //�Q�[���Z�b�g���̃e�L�X�g�̍��W
    private Vector2 buttonPos;          //�{�^���̍��W 
    private Vector3 triPos;             //�O�p�`�̍��W

    //�J����
    private GameObject mCam;            //�J�����i�[�I�u�W�F�N�g
    private GameObject uCam;
    private Camera mainCam;             //���C���J����
    private Camera uiCam;               //�T�u�J����

    //�v���C���[�̃I�u�W�F�N�g�i�[
    private GameObject Player;          //�v���C���[�I�u�W�F�N�g���i�[����ϐ�
    CharaStatus pcStatus;               //�v���C���[�I�u�W�F�N�g�̃X�N���v�g���i�[����ϐ�

    //����v���C���[�I�u�W�F�N�g�̊i�[
    private GameObject opponentPlayer;  //����v���C���[�I�u�W�F�N�g
    CharaStatus opcStatus;              //����̃X�e�[�^�X�X�N���v�g�̊i�[

    //�V���b�g�̃I�u�W�F�N�g�i�[
    private GameObject Shot;            //�V���b�g�I�u�W�F�N�g���i�[����ϐ�
    Shot shot;                          //�V���b�g�I�u�W�F�N�g�̃X�N���v�g���i�[����ϐ�

    //�X�R�A�̃I�u�W�F�N�g�i�[
    private GameObject Score;           //�X�R�A
    private Score score;                //�X�R�A�̃X�N���v�g

    //�p�l���X�N���v�g�̊i�[
    private DestroyPanel destroyPanel;  

    void Start()
    {
        #region �������Ə�������

        //�^�b�v�֘A�̏�����
        tMger = new TouchManager();
        createFlg = true;
        sCreateFlg = true;


        //�v���n�u�̓ǂݍ���
        textPref = (GameObject)Resources.Load("TextPref");
        lgPref = (GameObject)Resources.Load("LineGaugePref");
        pgPref1 = (GameObject)Resources.Load("PowerGaugePref1");
        pgPref2 = (GameObject)Resources.Load("PowerGaugePref2");
        linePref = (GameObject)Resources.Load("linePref");
        panelPref = (GameObject)Resources.Load("PanelPref");
        buttonPref = (GameObject)Resources.Load("ButtonPref");
        trianglePref = (GameObject)Resources.Load("TrianglePref");

        //�A�j���[�V�����J�[�u�̏�����
        lineCurve = new AnimationCurve();

        //���W�ݒ�
        scorePos = new Vector2(0.0f, Screen.height/2);
        pNamePos = new Vector2(-Screen.width / 2, Screen.height / 2);
        oNamePos = new Vector2(Screen.width / 2 , Screen.height / 2);
        rTextPos = new Vector2(0.0f,0.0f);
        plgPos = pNamePos + new Vector2(215.0f, -100.0f);
        olgPos = oNamePos + new Vector2(-215.0f, -100.0f);
        panelPos = new Vector3(0.0f, 0.0f, 0.0f);
        gSetTextPos = new Vector2(0.0f, 500.0f);
        buttonPos = new Vector2(0.0f, -500.0f);

        //�J�����̎擾
        mCam = GameObject.Find("Main Camera");
        uCam = GameObject.Find("UICamera");
        mainCam = mCam.GetComponent<Camera>();
        uiCam = uCam.GetComponent<Camera>();

        //�I�u�W�F�N�g����уX�N���v�g�̊i�[
        Player = GameObject.Find("Player");
        pcStatus = Player.GetComponent<CharaStatus>();

        opponentPlayer = GameObject.Find("Player2");
        opcStatus = opponentPlayer.GetComponent<CharaStatus>();

        Shot = GameObject.Find("Shot");
        shot = Shot.GetComponent<Shot>();

        Score = GameObject.Find("Score");
        score = Score.GetComponent<Score>();

        CreateInit();                                   //�������Ɛ���

        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        tMger.update();                                                 //�X�V(�^�b�v�Ď�)

        textScore.text = score.user1Score + " - " + score.user2Score;   //���_�擾
        lgPlayer.value = (float)pcStatus.CharaStamina;                  //�X�^�~�i�擾
        lgOpponent.value = (float)opcStatus.CharaStamina;               //����X�^�~�i�擾�@
        TapDoing();                                                     //�^�b�v����UI�̐����Ǘ�
        if (GameManager.instance.isAddScore || GameManager.instance.isFault)
        {
            if (GameManager.instance.gameState != GameState.GameSet)
            {//�I�����Ă��Ȃ��ꍇ
                if (rCreateFlg) RoundBetween();                                             //���E���h�̊Ԃ�UI�̐����Ǘ�
            }
            else
            {
                if (rCreateFlg) GameSet();          //�I������
            }
        }
        else { rCreateFlg = true; }

        if (sCreateFlg) ServeDisplay();
        
    }

    private void CreateInit()
    {
        #region ��{�\�������UI�̐���

        //������
        instances = new GameObject[UINum];
        textScore = null;
        textPlayerName = null;
        textOpponentName = null;
        lgPlayer = null;
        lgOpponent = null;

        //�_���̃e�L�X�g
        instances[i] = (GameObject)Instantiate(textPref, scorePos, Quaternion.identity);    //�C���X�^���X����
        instances[i].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
        instances[i].name = "TextScore";                                                    //�I�u�W�F�N�g���ύX
        textScore = instances[0].GetComponent<Text>();                                      //�e�L�X�g
        textScore.text = score.user1Score + " - " + score.user2Score;                       //�e�L�X�g���e�ݒ�
        textScore.alignment = TextAnchor.UpperCenter;                                       //�e�L�X�g�A���J�[
        i++;

        //�v���C���[���̕\��
        instances[i] = (GameObject)Instantiate(textPref, pNamePos, Quaternion.identity);    //�C���X�^���X����
        instances[i].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
        instances[i].name = "TextPlayerName";                                               //�I�u�W�F�N�g���ύX
        textPlayerName = instances[i].GetComponent<Text>();                                 //�e�L�X�g
        textPlayerName.text = "�v���C���[";                                                 //�e�L�X�g���e�ݒ�
        textPlayerName.fontSize = 60;                                                       //�t�H���g�T�C�Y�̕ύX
        textPlayerName.alignment = TextAnchor.UpperLeft;                                   //�e�L�X�g�A���J�[
        i++;

        //���薼�̕\��
        instances[i] = (GameObject)Instantiate(textPref, oNamePos, Quaternion.identity);    //�C���X�^���X����
        instances[i].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
        instances[i].name = "TextOpponentName";                                             //�I�u�W�F�N�g���ύX
        textOpponentName = instances[i].GetComponent<Text>();                               //�e�L�X�g
        textOpponentName.text = "����";                                                     //�e�L�X�g���e�ݒ�
        textOpponentName.fontSize = 60;                                                     //�t�H���g�T�C�Y�̕ύX
        textOpponentName.alignment = TextAnchor.UpperRight;                                //�e�L�X�g�A���J�[
        i++;

        //�v���C���[�̃X�^�~�i�Q�[�W
        instances[i] = (GameObject)Instantiate(lgPref, plgPos, Quaternion.identity);        //�C���X�^���X����
        instances[i].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
        instances[i].name = "lgPlayer";                                                     //�I�u�W�F�N�g���ύX
        lgPlayer = instances[i].GetComponent<Slider>();                                     //�X���C�_�[
        lgPlayer.minValue = 0;                                                              //�ŏ��l
        lgPlayer.maxValue = (float)pcStatus.CharaStamina;                                   //�ő�l
        lgPlayer.value = lgPlayer.maxValue;                                                 //���݂̒l�̐ݒ�
        i++;

        //����̃X�^�~�i�Q�[�W
        instances[i] = (GameObject)Instantiate(lgPref, olgPos, Quaternion.identity);        //�C���X�^���X����
        instances[i].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
        instances[i].name = "lgOpponent";                                                   //�I�u�W�F�N�g���ύX
        lgOpponent = instances[i].GetComponent<Slider>();                                   //�X���C�_�[
        lgOpponent.minValue = 0;                                                            //�ŏ��l
        lgOpponent.maxValue = (float)opcStatus.CharaStamina;                                //�ő�l
        lgOpponent.value = lgOpponent.maxValue;                                             //���݂̒l�̐ݒ�
        lgOpponent.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);               //�Q�[�W��](���S���猸��悤�ɂ���)
        i++;

        #endregion
    }

    void TapDoing()
    {   
        #region �^�b�v���Ă���Œ��ɍs������

        TouchManager touch_state = tMger.getTouch();    //�^�b�`�擾

        if (touch_state._touch_flag)                    //�^�b�`����Ă����ꍇ
        {
            if (touch_state._touch_phase == TouchPhase.Began)
            {   //�^�b�`�J�n
                //�����̏��������s����Ȃ�
            }

            if (touch_state._touch_phase == TouchPhase.Moved)
            {
                //Debug.Log("tap time" + shot.GetTapTime);
                if (10 <= shot.GetTapTime)
                {
                    if (GameManager.instance.gameState == GameState.Serve || GameManager.instance.gameState == GameState.DuringRound)
                    {   //���_�t���O�I�t(�Q�[����)�̎��ɏ������s��
                        if (createFlg)
                        {
                            //�^�b�v���ɕ\�������UI�̐���
                            //Began�̑���
                            j = i;                                                                              //���݂�instances�z��̑�������J�E���g����

                            //�Q�[�W�̍��W�ݒ�
                            //plViewPos = mainCam.WorldToViewportPoint(Player.transform.position);                //�v���C���[�̃J������̍��W
                            //pgViewPos = uiCam.ViewportToWorldPoint(plViewPos);                                  //UI�J������plViewPos�Ɠ����ʒu�ɕ\�������悤�Ƀ��[���h���W���擾
                            //pgViewPos.z = 0;                                                                    //z���̐ݒ�
                            //pgPos = pgViewPos + new Vector3(-200.0f, 100.0f, 0.0f);                             //pgViewPos�ɍX�ɕ␳�����l��ݒ�
                            pgPos = RectTransformUtility.WorldToScreenPoint(mainCam, Player.transform.position);
                            pgPos = pgPos + new Vector3(-430, -800, 0);

                            //���̍��W�ݒ�
                            linePos = shot.GetTapStart;                                                         //�^�b�v���J�n�������W�ɐݒ�
                            linePos.z = 10;
                            linePos = uiCam.ScreenToWorldPoint(linePos);
                            lineEndPos = new Vector3(shot.GetTapWhile.x, shot.GetTapWhile.y, 10.0f);              //�^�b�v���Ă���Ԉړ����钸�_�̍��W
                            lineEndPos = uiCam.ScreenToWorldPoint(lineEndPos);

                            //�p���[�Q�[�W(�g�g��)
                            instances[j] = (GameObject)Instantiate(pgPref1, pgPos, Quaternion.identity);        //�C���X�^���X����
                            instances[j].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
                            instances[j].name = "pGauge1";                                                      //�I�u�W�F�N�g���ύX
                            pGauge1 = instances[j].GetComponent<Image>();                                       //�C���[�W
                            j++;

                            //�p���[�Q�[�W(������)
                            instances[j] = (GameObject)Instantiate(pgPref2, pgPos, Quaternion.identity);        //�C���X�^���X����
                            instances[j].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
                            instances[j].name = "pGauge2";                                                      //�I�u�W�F�N�g���ύX
                            pGauge2 = instances[j].GetComponent<Image>();                                       //�C���[�W
                            j++;

                            //��
                            instances[j] = (GameObject)Instantiate(linePref, linePos, Quaternion.identity);     //�C���X�^���X����
                            instances[j].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
                            instances[j].name = "line";                                                         //�I�u�W�F�N�g���ύX
                            line = instances[j].GetComponent<LineRenderer>();                                   //�C���[�W
                            lineCurve.AddKey(0.05f, 0.05f);                                                     //���̑���(�_1)
                            lineCurve.AddKey(0.3f, 0.3f);                                                       //���̑���2(�_2)
                            line.numCapVertices = 10;                                                           //�p���ۂ�����
                            line.numCornerVertices = 10;                                                        //���̊e�Z�O�����g���ۂ�����
                            line.widthCurve = lineCurve;                                                        //�ݒ肵���A�j���[�V�����J�[�u��K�p
                            line.SetPosition(0, linePos);                                                       //���_�̍��W�ݒ�(���_)
                            line.SetPosition(1, lineEndPos);                                                    //���_�̍��W�ݒ�(�ړ�����_)
                            j++;

                            createFlg = false;                                                                  //�������܂���
                            
                        }

                        //�^�b�`��
                        pGauge2.fillAmount = 1.0f - ((float)shot.GetTapTime / 60.0f) / 2.0f;                    //�^�b�`���̃p���[�Q�[�W�ݒ�
                        gaugeKeep = pGauge2.fillAmount;                                                         //�Q�[�W�L�[�v

                        lineEndPos.x = shot.GetTapWhile.x;                                                      //���_��x���W�̕ύX
                        lineEndPos.y = shot.GetTapWhile.y;                                                      //���_��y���W�̕ύX
                        lineEndPos.z = 10;
                        lineEndPos = uiCam.ScreenToWorldPoint(lineEndPos);
                        vertexKeep = lineEndPos;                                                                //���_�̍��W�L�[�v
                        line.SetPosition(1, lineEndPos);                                                        //���_�̐ݒ�
                    }
                }
            }

            if (touch_state._touch_phase == TouchPhase.Ended)
            {   //�^�b�`�I��
                for (int n = i; n < j; n++)
                {
                    pGauge2.fillAmount = gaugeKeep;                                                     //�ێ�
                    lineEndPos = vertexKeep;

                    Destroy(instances[n], 0.3f);                                                        //�^�b�v���ɐ������ꂽ�I�u�W�F�N�g�̍폜
                }
                createFlg = true;
            }
        }
        #endregion
    }
    
    void RoundBetween()
    {
        #region ���E���h�؂�ւ����ɍs������

        roundInstances = new GameObject[RoundUINum];
        r = 0;      //���ݐ�������Ă���UI�̗v�f������r�ɑ��

        //�p�l��
        roundInstances[r] = (GameObject)Instantiate(panelPref, panelPos, Quaternion.identity);       //�C���X�^���X����
        roundInstances[r].transform.SetParent(gameObject.transform, false);                          //�e�I�u�W�F�N�g
        roundInstances[r].name = "panel";                                                            //�I�u�W�F�N�g���ύX
        panel = roundInstances[r].GetComponent<Image>();                                             //�C���[�W
        destroyPanel = roundInstances[r].GetComponent<DestroyPanel>();
        destroyPanel.onDestroyed.AddListener(() => sCreateFlg = true);                               //�p�l�����폜�����Ƃ��ɌĂяo����鏈��
        r++;

        //���E���h�Ԃɕ\������e�L�X�g
        roundInstances[r] = (GameObject)Instantiate(textPref, rTextPos, Quaternion.identity);        //�C���X�^���X����
        roundInstances[r].transform.SetParent(gameObject.transform, false);                          //�e�I�u�W�F�N�g
        roundInstances[r].name = "TextRoundBetween";                                                 //�I�u�W�F�N�g���ύX
        textRoundBetween = roundInstances[r].GetComponent<Text>();                                   //�e�L�X�g
        textRoundBetween.color = Color.white;                                                   //�F�ύX
        textRoundBetween.fontSize = 80;                                                         //�t�H���g�T�C�Y
        textRoundBetween.alignment = TextAnchor.MiddleCenter;                                   //�A���J�[(���S�ʒu)�̕ύX
        r++;

        

        if (GameManager.instance.isAddScore)
        {   //���_�t���O�I�����ɍs������
            if (GameManager.instance.isDeuce)
            {   //�f���[�X�ɂȂ����ꍇ
                textRoundBetween.text = "�f���[�X";
            }
            else
            {   //�ʏ펞
                if(score.isUser1MatchP || score.isUser2MatchP)
                {
                    textRoundBetween.text = score.user1Score + " - " + score.user2Score+
                        "\n�}�b�`�|�C���g";            //���_�擾
                }
                else
                {
                    textRoundBetween.text = score.user1Score + " - " + score.user2Score;            //���_�擾
                }
            }

        }
        else
        {
            textRoundBetween.text = "";
        }
        
        if(GameManager.instance.faultState == FaultState.Fault)
        {
            textRoundBetween.text = "�t�H���g";         //�t�H���g
        }

        else if(GameManager.instance.faultState == FaultState.DoubleFault)
        {
            if(score.isUser1MatchP || score.isUser2MatchP)
            {
                textRoundBetween.text = "�_�u���t�H���g\n" +
                score.user1Score + " - " + score.user2Score+
                "\n�}�b�`�|�C���g";   //�_�u���t�H���g
            }
            else
            {
                textRoundBetween.text = "�_�u���t�H���g\n" +
                score.user1Score + " - " + score.user2Score;   //�_�u���t�H���g
            }
        }

        
        for(int n = 0; n < r; n++)
        {
            Destroy(roundInstances[n],Define.NEXT_ROUNDTIME);                                                   //�^�b�v���ɐ������ꂽ�I�u�W�F�N�g�̍폜
        }

        rCreateFlg = false;
        #endregion
    }

    void ServeDisplay()
    {
        #region �T�[�u���̕\��

        serveInstance = null;

        if(GameManager.instance.serveUser == User.User1)
        {
            triPos = RectTransformUtility.WorldToScreenPoint(mainCam, Player.transform.position);
            triPos = triPos + new Vector3(-560, -640, 0);

        }
        else
        {
            triPos = RectTransformUtility.WorldToScreenPoint(mainCam, opponentPlayer.transform.position);
            triPos = triPos + new Vector3(-540, -810, 0);
        }

        serveInstance = (GameObject)Instantiate(trianglePref, triPos, Quaternion.identity);      //�C���X�^���X����
        serveInstance.transform.SetParent(gameObject.transform, false);                          //�e�I�u�W�F�N�g
        serveInstance.name = "triangle";                                                         //�I�u�W�F�N�g���ύX
        triangle = serveInstance.GetComponent<Image>();                                          //�C���[�W

        Destroy(serveInstance, Define.NEXT_ROUNDTIME);                                           //�폜

        sCreateFlg = false;
        #endregion
    }

    void GameSet()
    {
        #region �Q�[���Z�b�g���ɍs������

        roundInstances = new GameObject[RoundUINum];
        r = 0;      //���ݐ�������Ă���UI�̗v�f������r�ɑ��

        //�p�l��
        roundInstances[r] = (GameObject)Instantiate(panelPref, panelPos, Quaternion.identity);       //�C���X�^���X����
        roundInstances[r].transform.SetParent(gameObject.transform, false);                          //�e�I�u�W�F�N�g
        roundInstances[r].name = "panel";                                                            //�I�u�W�F�N�g���ύX
        panel = roundInstances[r].GetComponent<Image>();                                             //�C���[�W
        r++;

        //���E���h�Ԃɕ\������e�L�X�g
        roundInstances[r] = (GameObject)Instantiate(textPref, gSetTextPos, Quaternion.identity);        //�C���X�^���X����
        roundInstances[r].transform.SetParent(gameObject.transform, false);                             //�e�I�u�W�F�N�g
        roundInstances[r].name = "GameSetText";                                                         //�I�u�W�F�N�g���ύX
        textRoundBetween = roundInstances[r].GetComponent<Text>();                                      //�e�L�X�g
        textRoundBetween.color = Color.white;                                                   //�F�ύX
        textRoundBetween.fontSize = 80;                                                         //�t�H���g�T�C�Y
        textRoundBetween.alignment = TextAnchor.MiddleCenter;                                   //�A���J�[(���S�ʒu)�̕ύX
        if(score.user1Score > score.user2Score) textRoundBetween.text = "�Q�[���Z�b�g\n�v���C���[�̏���";
        else textRoundBetween.text = "�Q�[���Z�b�g\n����̏���";
        r++;

        //���g���C�{�^��
        roundInstances[r] = (GameObject)Instantiate(buttonPref, buttonPos, Quaternion.identity);    //�C���X�^���X����
        roundInstances[r].transform.SetParent(gameObject.transform, false);                         //�e�I�u�W�F�N�g
        roundInstances[r].name = "retryButton";                                                     //�I�u�W�F�N�g��
        retryB = roundInstances[r].GetComponent<Button>();                                          //�{�^��
        //retryB.onClick.AddListener(() => ) ;                                                      //OnClick�̏����ǉ�
        retryBtext = roundInstances[r].GetComponentInChildren<Text>();                              //�e�L�X�g
        retryBtext.fontSize = 80;                                                                   //�t�H���g�T�C�Y
        retryBtext.text = "���g���C";
        r++;

        rCreateFlg = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        #endregion
    }

}
