using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingForecast : MonoBehaviour
{
    [SerializeField]
    private GameObject randingPoint;    //���n�n�_�I�u�W�F�N�g
    [SerializeField]
    private GameObject shotObject;      //���W�󂯎��p
    [SerializeField]
    private Shot shot;                  //���W�󂯎��p
    [SerializeField]
    private CharaStatus[] cStatus;
    [SerializeField]
    private float radius;               //���a
    [SerializeField]
    private float diameter;             //���a
    [SerializeField,Range(0f,100f)]
    private float correctionVal;        //�␳�l

    //����������
    private void Start()
    {
        radius = 2;
        diameter = radius * 2;

        cStatus = new CharaStatus[2];

        GameObject userObj = GameObject.Find("Player");
        GameObject userObj2 = GameObject.Find("Player2");

        cStatus[0] = userObj.GetComponent<CharaStatus>();
        cStatus[1] = userObj2.GetComponent<CharaStatus>();

        //�I�u�W�F�N�g��\��
        randingPoint.SetActive(false);

        shot = shotObject.GetComponent<Shot>();
    }

    public void PointSetting()
    {
        Ball ball = GameObject.Find("Ball").GetComponent<Ball>();
        int nowUser = 0;

        if (ball.nowUserTag == "Player2") nowUser = 1;

        float rad = (float)cStatus[nowUser].Rad;                                //���W�A���l
        float distance = (float)cStatus[nowUser].Distance / correctionVal;      //���B����

        //�{�[��Script���擾

        //���B�����ƎO�p�֐����g����x,z���Z�o
        float x = Mathf.Sin(rad) * distance;
        float z = Mathf.Cos(rad) * distance;

        //User2�������ꍇ���l�𔽓]������
        if (ball.nowUserTag == "Player2")
        {
            z = -z;
        }
        else if(ball.nowUserTag == "Player")
        {
            x = -x;
        }

        //�I�u�W�F�N�g��\������
        randingPoint.SetActive(true);

        //���B�n�_�̍��W��ύX
        randingPoint.transform.position = new Vector3(x, 0.5f, z);

        //�X�P�[�����C��
        diameter = radius * 2;
        randingPoint.transform.localScale = new Vector3(diameter, diameter, diameter);

        Vector3 basePoint = randingPoint.transform.position;
        GameObject pointB = GameObject.Find("pointB");

        //�}�C�i�X���ۂ��𔻒�
        int isMinusX = Random.Range(0, 1 + 1);
        int isMinusZ = Random.Range(0, 1 + 1);

        //���Z�l�𗐐�
        float addX = Random.Range(0f, radius);
        float addZ = Random.Range(0f, radius);

        //�}�C�i�X�������ꍇ
        if (isMinusX == 1) addX = -addX;
        if (isMinusZ == 1) addZ = -addZ;

        //�{�[���̒��n�n�_��ݒ�
        pointB.transform.position = new Vector3(
            basePoint.x + addX, pointB.transform.position.y,
            basePoint.z + addZ);
    }
}
