using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //UI�̗v�f��
    private const int UINum = 7;        //��������UI�̗v�f��

    private int i = 0;                  //UI�̗v�f���̃J�E���g

    //�v���n�u�̊i�[
    private GameObject textPref;        //�e�L�X�g
    private GameObject lgPref;          //���C���Q�[�W

    //UI�̃C���X�^���X���i�[����z��
    private GameObject[] instances;

    //���_���i�[����ϐ�
    private int pScore;                 //�v���C���[�̃X�R�A
    private int oScore;                 //����̃X�R�A

    //�e�L�X�g
    private Text textScore;             //�X�R�A�̕\��
    private Text textPlayerName;        //�v���C���[��
    private Text textOpponentName;      //���薼
    private Slider lgPlayer;            //�v���C���[�̃X�^�~�i�Q�[�W
    private Slider lgOpponent;          //����̃X�^�~�i�Q�[�W
    
    //���W
    private Vector2 scorePos;           //�X�R�A�̍��W
    private Vector2 pNamePos;           //�v���C���[���̍��W
    private Vector2 oNamePos;           //���薼�̍��W
    private Vector2 plgPos;             //�v���C���[�̃X�^�~�i�Q�[�W�̍��W
    private Vector2 olgPos;             //����̃X�^�~�i�Q�[�W�̍��W

    //�v���C���[�̃I�u�W�F�N�g�i�[
    private GameObject Player;          //�v���C���[�I�u�W�F�N�g���i�[����ϐ�
    CharaStatus pcStatus;               //�v���C���[�I�u�W�F�N�g�̃X�N���v�g���i�[����ϐ�

    // Start is called before the first frame update
    void Start()
    {
        //�v���n�u�̓ǂݍ���
        textPref = (GameObject)Resources.Load("TextPref");
        lgPref = (GameObject)Resources.Load("LineGaugePref");

        //���W�ݒ�
        scorePos = new Vector2(-100.0f, Screen.height/2);
        pNamePos = new Vector2(-Screen.width / 2, Screen.height / 2);
        oNamePos = new Vector2((Screen.width / 2 )- 140, Screen.height / 2);
        plgPos = pNamePos + new Vector2(210.0f, -100.0f);
        olgPos = oNamePos + new Vector2(-70.0f, -100.0f);

        Player = GameObject.Find("Player");
        pcStatus = Player.GetComponent<CharaStatus>();

        CreateInit();                   //�������Ɛ���
    }

    // Update is called once per frame
    void Update()
    {
        lgPlayer.value = (float)pcStatus.CharaStamina;
    }

    private void CreateInit()
    {
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

    }


}
