using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingForecast : MonoBehaviour
{
    [SerializeField]
    private GameObject randingPoint;    //着地地点オブジェクト
    [SerializeField]
    private GameObject shotObject;      //座標受け取り用
    [SerializeField]
    private Shot shot;                  //座標受け取り用

    [SerializeField]
    private float radius;               //半径
    [SerializeField]
    private float diameter;             //直径

    //初期化処理
    private void Start()
    {
        radius = 0.5f;
        diameter = radius * 2;
        randingPoint.SetActive(false);

        shot = shotObject.GetComponent<Shot>();
    }

    //更新処理
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
