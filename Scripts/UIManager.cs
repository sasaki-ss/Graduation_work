using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //�\������UI�̗v�f��
    private const int UINum = 2;

    //�v���n�u�̊i�[
    private GameObject textPref;        //�e�L�X�g

    //UI�̃C���X�^���X���i�[����z��
    private GameObject[] instances;

    //���_���i�[����ϐ�
    private int pScore;                 //�v���C���[�̃X�R�A
    private int oScore;                 //����̃X�R�A

    //�e�L�X�g
    private Text textScore;             //�X�R�A�̕\��
    private Text textPlayerName;        //�v���C���[��
    private Text textOpponentName;      //���薼
    
    //���W
    private Vector2 scorePos;           //�X�R�A�̍��W
    private Vector2 pNamePos;           //�v���C���[���̍��W
    private Vector2 oNamePos;           //���薼�̍��W

    // Start is called before the first frame update
    void Start()
    {
        //�v���n�u�̓ǂݍ���
        textPref = (GameObject)Resources.Load("TextPref");

        //���W�ݒ�
        scorePos = new Vector2(-100.0f, Screen.height/2);
        pNamePos = new Vector2(-Screen.width / 2, Screen.height / 2);

        CreateInit();                   //�������Ɛ���
    }

    // Update is called once per frame
    void Update()
    {

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

        //����
        instances[0] = (GameObject)Instantiate(textPref, scorePos, Quaternion.identity);    //�C���X�^���X����
        instances[0].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
        instances[0].name = "TextScore";                                                    //�I�u�W�F�N�g���ύX
        textScore = instances[0].GetComponent<Text>();                                      //�e�L�X�g
        textScore.text = pScore + " - " + oScore;                                           //�e�L�X�g���e�ݒ�

        instances[1] = (GameObject)Instantiate(textPref, pNamePos, Quaternion.identity);    //�C���X�^���X����
        instances[1].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
        instances[1].name = "TextPlayerName";                                               //�I�u�W�F�N�g���ύX
        textPlayerName = instances[1].GetComponent<Text>();                                 //�e�L�X�g
        textPlayerName.text = "�v���C���[";                                                 //�e�L�X�g���e�ݒ�
        textPlayerName.fontSize = 60;                                                       //�t�H���g�T�C�Y�̕ύX
    }
}
