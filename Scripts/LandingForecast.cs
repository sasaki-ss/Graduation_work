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
    private CharaStatus[] cStatus;
    [SerializeField]
    private float radius;               //半径
    [SerializeField]
    private float diameter;             //直径
    [SerializeField,Range(0f,100f)]
    private float correctionVal;        //補正値

    //初期化処理
    private void Start()
    {
        radius = 2;
        diameter = radius * 2;

        cStatus = new CharaStatus[2];

        GameObject userObj = GameObject.Find("Player");
        GameObject userObj2 = GameObject.Find("Player2");

        cStatus[0] = userObj.GetComponent<CharaStatus>();
        cStatus[1] = userObj2.GetComponent<CharaStatus>();

        //オブジェクト非表示
        randingPoint.SetActive(false);

        shot = shotObject.GetComponent<Shot>();
    }

    public void PointSetting()
    {
        Ball ball = GameObject.Find("Ball").GetComponent<Ball>();
        int nowUser = 0;

        if (ball.nowUserTag == "Player2") nowUser = 1;

        float rad = (float)cStatus[nowUser].Rad;                                //ラジアン値
        float distance = (float)cStatus[nowUser].Distance / correctionVal;      //到達距離

        //ボールScriptを取得

        //到達距離と三角関数を使ってx,zを算出
        float x = Mathf.Sin(rad) * distance;
        float z = Mathf.Cos(rad) * distance;

        //User2だった場合数値を反転させる
        if (ball.nowUserTag == "Player2")
        {
            z = -z;
        }
        else if(ball.nowUserTag == "Player")
        {
            x = -x;
        }

        //オブジェクトを表示する
        randingPoint.SetActive(true);

        //到達地点の座標を変更
        randingPoint.transform.position = new Vector3(x, 0.5f, z);

        //スケールを修正
        diameter = radius * 2;
        randingPoint.transform.localScale = new Vector3(diameter, diameter, diameter);

        Vector3 basePoint = randingPoint.transform.position;
        GameObject pointB = GameObject.Find("pointB");

        //マイナスか否かを判定
        int isMinusX = Random.Range(0, 1 + 1);
        int isMinusZ = Random.Range(0, 1 + 1);

        //加算値を乱数
        float addX = Random.Range(0f, radius);
        float addZ = Random.Range(0f, radius);

        //マイナスだった場合
        if (isMinusX == 1) addX = -addX;
        if (isMinusZ == 1) addZ = -addZ;

        //ボールの着地地点を設定
        pointB.transform.position = new Vector3(
            basePoint.x + addX, pointB.transform.position.y,
            basePoint.z + addZ);
    }
}
