using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //�\������UI�̗v�f��
    private const int UINum = 1;

    //�v���n�u�̊i�[
    private GameObject textPref;        //�e�L�X�g

    //UI�̃C���X�^���X���i�[����z��
    private GameObject[] instances;

    //���_���i�[����ϐ�
    private int pScore;                 //�v���C���[�̃X�R�A
    private int oScore;                 //����̃X�R�A

    //�e�L�X�g
    private Text textScore;             //�X�R�A�̕\��
    
    //���W
    private Vector2 scorePos;           //�X�R�A�̍��W

    // Start is called before the first frame update
    void Start()
    {
        //�v���n�u�̓ǂݍ���
        textPref = (GameObject)Resources.Load("TextPref");

        //���W�ݒ�
        scorePos = new Vector2(-100.0f, Screen.height/2);

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
        pScore = 0;
        oScore = 0;

        //����
        instances[0] = (GameObject)Instantiate(textPref, scorePos, Quaternion.identity);    //�C���X�^���X����
        instances[0].transform.SetParent(gameObject.transform, false);                      //�e�I�u�W�F�N�g
        instances[0].name = "TextScore";                                                    //�I�u�W�F�N�g���ύX
        textScore = instances[0].GetComponent<Text>();                                      //�e�L�X�g
        textScore.text = pScore + " - " + oScore;                                           //�e�L�X�g���e�ݒ�
    }
}
