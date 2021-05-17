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
        randingPoint.SetActive(false);

        shot = shotObject.GetComponent<Shot>();
    }

    //�X�V����
    private void Update()
    {


    }

    public void PointSetting()
    {
        float rad = (float)shot.GetRadian;
        float distance = (float)shot.GetDistance / correctionVal;

        Ball ball = GameObject.Find("Ball").GetComponent<Ball>();

        Debug.Log("rad=" + rad * (180 / 3.14f));
        Debug.Log("distance=" + distance);

        float x = Mathf.Sin(rad) * distance;
        float z = Mathf.Cos(rad) * distance;

        //User2�������ꍇ���l�𔽓]������
        if (ball.Tag == "Player2")
        {
            x = -x;
            z = -z;
        }

        randingPoint.SetActive(true);
        randingPoint.transform.position = new Vector3(x, 0.5f, z);
        diameter = radius * 2;
        randingPoint.transform.localScale = new Vector3(diameter, diameter, diameter);

        Vector3 basePoint = randingPoint.transform.position;
        GameObject pointB = GameObject.Find("pointB");

        int isMinusX = Random.Range(0, 1 + 1);
        int isMinusZ = Random.Range(0, 1 + 1);
        float addX = Random.Range(0f, radius);
        float addZ = Random.Range(0f, radius);

        if (isMinusX == 1) addX = -addX;
        if (isMinusZ == 1) addZ = -addZ;

        pointB.transform.position = new Vector3(
            randingPoint.transform.position.x + addX,
            pointB.transform.position.y,
            randingPoint.transform.position.z + addZ);
    }
}
