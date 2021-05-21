using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボールクラス
public class Ball : MonoBehaviour
{
    /*このスクリプトでのみ使う変数*/
    private Rigidbody   rb;             //Rigidbody
    private Coroutine   coroutine;      //コルーチン
    private Vector3     endPoint;       //終点地点
    private Vector3     diff;           //距離
    private int         nowShotUser;    //現在打っているユーザー
    private float       flightTime;     //滞空時間
    private float       speedRate;      //滞空時間を基準とした移動速度倍率
    private float       e;              //反発係数
    private bool        isNet;          //ネットフラグ
    private bool        isBound;        //バウンドフラグ
    private bool        isProjection;   //投射フラグ
    private bool        isSafetyArea;   //セーフティエリアフラグ

    /*プロパティ関連*/
    public string       nowUserTag { get; private set; }    //タグ
    public int          boundCount { get; private set; }    //バウンド回数

    /*インスペクターに表示又は設定する変数*/
    private GameObject[]    userObj;        //ユーザーオブジェクト
    [SerializeField]
    private GameObject      randingPoint;   //着地地点オブジェクト

    //初期化処理
    private void Start()
    {
        //Rigidbodyを取得
        rb = this.GetComponent<Rigidbody>();

        //Userオブジェクトを取得
        userObj = new GameObject[2];
        userObj[0] = GameObject.Find("Player");
        userObj[1] = GameObject.Find("Player2");

        //打っているユーザーの初期化
        nowShotUser = 0;
        TagChange();

        //バウンドカウントを初期化
        boundCount = 0;

        //PhysiceMaterialを取得
        SphereCollider sc = this.GetComponent<SphereCollider>();
        PhysicMaterial bound = sc.material;

        MeshCollider mc = GameObject.Find("Court_Base").GetComponent<MeshCollider>();
        PhysicMaterial field = mc.material;

        //取得したboundとfieldから反発係数を算出
        e = (bound.bounciness + field.bounciness) / 2f;

        isNet = false;
        isProjection = false;
        isSafetyArea = true;
    }

    //物理演算が行われる際の処理
    private void FixedUpdate()
    {
        //ネットに当たってるとき
        if (isNet)
        {
            //コルーチンを停止する
            StopCoroutine(coroutine);
        }

        //滞空時間が0.005f以下の場合バウンドを停止させる
        if (isBound && flightTime < 1f)
        {
            isBound = false;
        }

        //バウンドの処理
        if (isBound && !isProjection)
        {
            //到達地点、滞空時間、速度倍率に反発係数をかける
            //物理法則は多分無視してる
            endPoint += diff * e;
            flightTime *= e;
            speedRate *= e;
            coroutine = StartCoroutine(ProjectileMotion(endPoint, flightTime,
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
        isSafetyArea = false;

        for (float t = 0f; t < _flightTime; t += (Time.deltaTime * _speedRate))
        {
            Vector3 p = Vector3.Lerp(startPoint, _endPoint,
                t / _flightTime);                                    //水平方向の座標を求める(x,z座標)
            p.y = startPoint.y + vn * t + 0.5f * _gravity * t * t;  //鉛直方向の座標 y

            //座標を更新する
            rb.MovePosition(p);

            yield return null;
        }
        isBound = true;
        isProjection = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Net"))
        {
            Debug.Log("当たったよ");
            isNet = true;
        }

        //ボールがフィールドに着地した際の処理
        if (other.gameObject.CompareTag("SafetyArea"))
        {
            //着地地点を生成する
            GenerateRandingPoint();
            boundCount++;
            isSafetyArea = true;

            Debug.Log("澤村りょう");
        }

        if (other.gameObject.CompareTag("OutSide") && !isSafetyArea)
        {
            //着地地点を生成する
            GenerateRandingPoint();

            Debug.Log("浦部ひろかず");
        }
    }

    //打つ処理
    public void Strike(float _flightTime, float _speedRate)
    {
        //到達地点を更新する
        LandingForecast lf = GameObject.Find("RandingPointControl").GetComponent<LandingForecast>();
        lf.PointSetting();

        //バウンドフラグをオフに
        isBound = false;

        //斜方投射フラグをオフに
        if (isProjection)
        {
            isProjection = false;
            //コルーチンを停止する
            StopCoroutine(coroutine);
        }

        //到達地点を取得
        endPoint = GameObject.Find("pointB").transform.position;

        //滞空時間をBallクラスに格納
        flightTime = _flightTime;

        //滞空時間を基準とした移動速度倍率をBallクラスに格納
        speedRate = _speedRate;

        //斜方投射コルーチンを開始
        coroutine = StartCoroutine(ProjectileMotion(endPoint, flightTime,
            speedRate, Physics.gravity.y));

        //タグを切り替える
        TagChange();
    }

    private void Init()
    {
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
        nowUserTag = userObj[nowShotUser].name;
        //バウンド回数もリセットする
        boundCount = 0;
    }

    //着地地点生成処理
    private void GenerateRandingPoint()
    {
        //着地地点を生成
        GameObject instObject = Instantiate(randingPoint);

        //着地地点を設定
        instObject.transform.position = new Vector3(this.transform.position.x, 0.02f,
            this.transform.position.z);
    }
}
