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

    //����������
    private void Start()
    {
        radius = 0.5f;
        diameter = radius * 2;
        randingPoint.SetActive(false);

        shot = shotObject.GetComponent<Shot>();
    }

    //�X�V����
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            randingPoint.SetActive(true);
            diameter = radius * 2;
            randingPoint.transform.localScale = new Vector3(diameter, diameter, diameter);
        }
    }

    void PointSetting()
    {
        Vector2 direction = shot.GetDirection;


    }
}
