using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TapStateManager;
using System;

public class UIManager : MonoBehaviour
{
    //�^�b�v����擾
    TouchManager tMger;                 //TapStateManager����TouchManager
    private bool createFlg;             //�^�b�v����UI�̐����t���O

    //UI�̗v�f��
    private const int UINum = 8;        //��������UI�̗v�f��

    private int i = 0;                  //UI�̗v�f���̃J�E���g
    private int j = 0;                  //�Q�[��������������폜����I�u�W�F�N�g�̗v�f��

    //�v���n�u�̊i�[
    private GameObject textPref;        //�e�L�X�g
    private GameObject lgPref;          //���C���Q�[�W
    private GameObject pgPref1;         //�p���[�Q�[�W(�g��)
    private GameObject pgPref2;         //�p���[�Q�[�W(������)

    //UI�̃C���X�^���X���i�[����z��
    private GameObject[] instances;

    //���_���i�[����ϐ�
    private int pScore;                 //�v���C���[�̃X�R�A
    private int oScore;                 //����̃X�R�A

    //�e�L�X�g
    private Text textScore;             //�X�R�A�̕\��
    private Text textPlayerName;        //�v���C���[��
    private Text textOpponentName;      //���薼

    //�Q�[�W
    private Slider lgPlayer;            //�v���C���[�̃X�^�~�i�Q�[�W
    private Slider lgOpponent;          //����̃X�^�~�i�Q�[�W
    private Image pGauge1;              //�p���[�Q�[�W(�g��)
    private Image pGauge2;              //�p���[�Q�[�W(������)

    //���W
    private Vector2 scorePos;           //�X�R�A�̍��W
    private Vector2 pNamePos;           //�v���C���[���̍��W
    private Vector2 oNamePos;           //���薼�̍��W
    private Vector2 plgPos;             //�v���C���[�̃X�^�~�i�Q�[�W�̍��W
    private Vector2 olgPos;             //����̃X�^�~�i�Q�[�W�̍��W
    private Vector3 pgPos;              //�p���[�Q�[�W�̍��W
    private Vector3 plViewPos;          //�v���C���[�̃J������̍��W
    private Vector3 pgViewPos;          //plViewPos�Ɠ����ʒu�ɕ\�����邽�߂̍��W

    //�J����
    public Camera mainCam;
    public Camera uiCam;

    //�v���C���[�̃I�u�W�F�N�g�i�[
    private GameObject Player;          //�v���C���[�I�u�W�F�N�g���i�[����ϐ�
    CharaStatus pcStatus;               //�v���C���[�I�u�W�F�N�g�̃X�N���v�g���i�[����ϐ�

    //�V���b�g�̃I�u�W�F�N�g�i�[
    private GameObject Shot;            //�V���b�g�I�u�W�F�N�g���i�[����ϐ�
    Shot shot;                          //�V���b�g�I�u�W�F�N�g�̃X�N���v�g���i�[����ϐ�

    // Start is called before the first frame update
    void Start()
    {
        //�v���n�u�̓ǂݍ���
        textPref = (GameObject)Resources.Load("TextPref");
        lgPref = (GameObject)Resources.Load("LineGaugePref");
        pgPref1 = (GameObject)Resources.Load("PowerGaugePref1");
        pgPref2 = (GameObject)Resources.Load("PowerGaugePref2");

        //���W�ݒ�
        scorePos = new Vector2(-100.0f, Screen.height/2);
        pNamePos = new Vector2(-Screen.width / 2, Screen.height / 2);
        oNamePos = new Vector2((Screen.width / 2 )- 140, Screen.height / 2);
        plgPos = pNamePos + new Vector2(210.0f, -100.0f);
        olgPos = oNamePos + new Vector2(-70.0f, -100.0f);
        pgPos = new Vector3(0, 0, 0);
        //�I�u�W�F�N�g����уX�N���v�g�̊i�[
        Player = GameObject.Find("Player");
        pcStatus = Player.GetComponent<CharaStatus>();

        Shot = GameObject.Find("Shot");
        shot = Shot.GetComponent<Shot>();

        //�^�b�v�֘A�̏�����
        tMger = new TouchManager();
        createFlg = true;

        CreateInit();                   //�������Ɛ���

    }

    // Update is called once per frame
    void Update()
    {
        lgPlayer.value = (float)pcStatus.CharaStamina;  //�X�^�~�i�擾

        TapDoing();                     //�^�b�v����UI�̐����Ǘ�
        tMger.update();                                              //�X�V(�^�b�v�Ď�)

    }

    private void CreateInit()
    {
        #region ��{�\�������UI�̐���

        //������
        instances = new GameObject[UINum];
        textScore = null;
        textPlayerName = null;
        textOpponentName = null;
        pScore = 0;
        oScore = 0;

        //�_���̃e�L�X�g
        instances[i] = (GameObject)Instantiate(textPref, scorePos, Quaternion.identity);    //�C���X�^���X����
        instances[i].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
        instances[i].name = "TextScore";                                                    //�I�u�W�F�N�g���ύX
        textScore = instances[0].GetComponent<Text>();                                      //�e�L�X�g
        textScore.text = pScore + " - " + oScore;                                           //�e�L�X�g���e�ݒ�
        i++;

        //�v���C���[���̕\��
        instances[i] = (GameObject)Instantiate(textPref, pNamePos, Quaternion.identity);    //�C���X�^���X����
        instances[i].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
        instances[i].name = "TextPlayerName";                                               //�I�u�W�F�N�g���ύX
        textPlayerName = instances[i].GetComponent<Text>();                                 //�e�L�X�g
        textPlayerName.text = "�v���C���[";                                                 //�e�L�X�g���e�ݒ�
        textPlayerName.fontSize = 60;                                                       //�t�H���g�T�C�Y�̕ύX
        i++;

        //���薼�̕\��
        instances[i] = (GameObject)Instantiate(textPref, oNamePos, Quaternion.identity);    //�C���X�^���X����
        instances[i].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
        instances[i].name = "TextOpponentName";                                             //�I�u�W�F�N�g���ύX
        textOpponentName = instances[i].GetComponent<Text>();                               //�e�L�X�g
        textOpponentName.text = "����";                                                     //�e�L�X�g���e�ݒ�
        textOpponentName.fontSize = 60;                                                     //�t�H���g�T�C�Y�̕ύX
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
        lgOpponent.maxValue = 1;                                                            //�ő�l
        lgOpponent.value = lgOpponent.maxValue;                                             //���݂̒l�̐ݒ�
        i++;

        #endregion
    }

    void TapDoing()
    {   //�^�b�v���Ă���Œ��ɍs������

        TouchManager touch_state = tMger.getTouch();                 //�^�b�`�擾

        if (touch_state._touch_flag)                    //�^�b�`����Ă����ꍇ
        {
            if (touch_state._touch_phase == TouchPhase.Began)
            {   //�^�b�`�J�n
                //�����̏��������s����Ȃ�
            }

            if (touch_state._touch_phase == TouchPhase.Moved)
            {
                if (createFlg)
                {   //Began�̑���

                    j = i;                                                                              //���݂�instances�z��̑�������J�E���g����

                    instances[j] = (GameObject)Instantiate(pgPref1, pgPos, Quaternion.identity);        //�C���X�^���X����
                    instances[j].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
                    instances[j].name = "pGauge1";                                                      //�I�u�W�F�N�g���ύX
                    pGauge1 = instances[j].GetComponent<Image>();                                       //�C���[�W
                    j++;

                    instances[j] = (GameObject)Instantiate(pgPref2, pgPos, Quaternion.identity);        //�C���X�^���X����
                    instances[j].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
                    instances[j].name = "pGauge2";                                                      //�I�u�W�F�N�g���ύX
                    pGauge2 = instances[j].GetComponent<Image>();                                       //�C���[�W
                    j++;

                    createFlg = false;                                                                  //�������܂���
                }
                //�^�b�`��
                pGauge2.fillAmount = 1.0f - ((float)shot.GetTapTime / 60.0f) / 2.0f;                    //�Q�[�W�ݒ�
                
            }

            if (touch_state._touch_phase == TouchPhase.Ended)
            {   //�^�b�`�I��
                for (int n = i; n < j; n++)
                {
                    Destroy(instances[n], 0.0f);                                                        //�폜
                }
                createFlg = true;
            }
        }
    }
}