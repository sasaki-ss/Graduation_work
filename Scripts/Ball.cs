using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボールクラス
public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;           //Rigidbody
    [SerializeField]
    private Vector3 endPoint;       //終点地点
    [SerializeField]
    private float flightTime;       //滞空時間
    [SerializeField]
    private float speedRate;        //滞空時間を基準とした移動速度倍率

    [SerializeField]
    private float e;  //反発係数

    [SerializeField]
    private Vector3 diff;       //距離

    private bool isNet;
    [SerializeField]
    private bool isShot;
    [SerializeField]
    private bool isBound;       //バウンドフラグ
    [SerializeField]
    private bool isProjection;  //投射フラグ

    [SerializeField]
    private GameObject[] userObj;   //ユーザーオブジェクト
    [SerializeField]
    private int nowShotUser;        //現在打っているユーザー
    [SerializeField]
    private string tag = "";        //タグ

    //tagのゲッター
    public string Tag
    {
        get { return this.tag; }
    }

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        userObj = new GameObject[2];
        userObj[0] = GameObject.Find("Player");
        userObj[1] = GameObject.Find("Player2");

        nowShotUser = 0;
        TagChange();

        SphereCollider sc = this.GetComponent<SphereCollider>();
        PhysicMaterial bound = sc.material;

        MeshCollider mc = GameObject.Find("Court_Base").GetComponent<MeshCollider>();
        PhysicMaterial field = mc.material;

        e = (bound.bounciness + field.bounciness) / 2f;

        isNet = false;
        isShot = false;
        isProjection = false;
    }

    //物理演算が行われる際の処理
    private void FixedUpdate()
    {
        if(isBound && flightTime < 0.005f)
        {
            isBound = false;
        }

        if (Input.GetMouseButtonDown(0) && !isShot)
        {
            isBound = false;
            isShot = true;

            endPoint = GameObject.Find("pointB").transform.position;

            StartCoroutine(ProjectileMotion(endPoint, flightTime,
                speedRate, Physics.gravity.y));

            TagChange();
        }

        if (isBound && !isProjection)
        {
            endPoint += diff * e;
            flightTime *= e;
            speedRate *= e;
            StartCoroutine(ProjectileMotion(endPoint, flightTime,
                speedRate, Physics.gravity.y));
        }
    }

    private IEnumerator ProjectileMotion(Vector3 _endPoint, float _flightTime,
        float _speedRate, float _gravity)
    {
        Vector3 startPoint = this.transform.position;               //初期位置
        float diffY = (_endPoint - startPoint).y;                   //視点と終点のy成分の差分
        diff.x = (_endPoint - startPoint).x;
        diff.z = (_endPoint - startPoint).z;
        float vn = (diffY - _gravity * 0.5f *
            _flightTime * _flightTime) / _flightTime;               //鉛直方向の初速度vn

        isProjection = true;

        for(float t = 0f; t < _flightTime; t += (Time.deltaTime * _speedRate))
        {
            if (isNet) yield break;

            Vector3 p = Vector3.Lerp(startPoint, _endPoint,
                t / _flightTime);                                    //水平方向の座標を求める(x,z座標)
            p.y = startPoint.y + vn * t + 0.5f * _gravity * t * t;  //鉛直方向の座標 y

            rb.MovePosition(p);
            //transform.position = p;

            yield return null;
        }
        isShot = false;
        isBound = true;
        isProjection = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Net"))
        {
            Debug.Log("当たったよ");
            isNet = true;
        }

        if(other.gameObject.CompareTag("Field"))
        {
            Debug.Log("地面着地");
        }
    }

    private void Init()
    {
        isShot = false;
        isBound = false;
        isProjection = false;
    }

    //タグ切り替え処理
    private void TagChange()
    {
        //ユーザーが0番目のとき
        if(nowShotUser == 0)
        {
            nowShotUser = 1;
        }
        //ユーザーが1番目のとき
        else
        {
            nowShotUser = 0;
        }

        //タグを指定したユーザーへ変更する
        tag = userObj[nowShotUser].name;
    }
}
