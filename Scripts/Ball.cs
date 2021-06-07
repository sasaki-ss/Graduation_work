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
    private int         colCoolTime;    //当たり判定クールタイム
    private float       flightTime;     //滞空時間
    private float       speedRate;      //滞空時間を基準とした移動速度倍率
    private float       e;              //反発係数
    private bool        isBound;        //バウンドフラグ
    private bool        isProjection;   //投射フラグ
    private bool        isCoolTime;     //クールタイムフラグ
    private bool        isSafetyArea;   //セーフティエリアフラグ

    /*プロパティ関連*/
    public string       nowUserTag { get; private set; }    //タグ
    public int          boundCount { get; private set; }    //バウンド回数
    public bool         isOut { get; private set; }         //アウトフラグを取得
    public bool         isNet { get; private set; }         //ネットフラグ

    /*インスペクターに表示又は設定する変数*/
    private GameObject[]    userObj;        //ユーザーオブジェクト
    [SerializeField]
    private GameObject      randingPoint;   //着地地点オブジェクト

    private void Awake()
    {
        //Rigidbodyを取得
        rb = this.GetComponent<Rigidbody>();

        //Userオブジェクトを取得
        userObj = new GameObject[2];
        userObj[0] = GameObject.Find("Player");
        userObj[1] = GameObject.Find("Player2");

        //PhysiceMaterialを取得
        SphereCollider sc = this.GetComponent<SphereCollider>();
        PhysicMaterial bound = sc.material;

        BoxCollider bc = GameObject.Find("SafetyArea").GetComponent<BoxCollider>();
        PhysicMaterial field = bc.material;

        //取得したboundとfieldから反発係数を算出
        e = (bound.bounciness + field.bounciness) / 2f;
    }

    //初期化処理
    private void Start()
    {
        //打っているユーザーの初期化
        nowShotUser = 0;
        TagChange();

        Init();
    }

    //物理演算が行われる際の処理
    private void FixedUpdate()
    {
        if(colCoolTime == 10)
        {
            isCoolTime = false;
            colCoolTime = 0;
        }
        Debug.Log("バウンド回数 : " + boundCount);

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

        if (isCoolTime) colCoolTime++;
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
        if (isCoolTime) return;
        if (GameManager.instance.gameState == GameState.DuringRound)
        {
            //ボールがネットに当たった際
            if (other.gameObject.CompareTag("Net"))
            {
                isNet = true;

                Debug.Log("当たったよ");
            }

            //ボールがフィールドに着地した際の処理
            if (other.gameObject.CompareTag("SafetyArea") ||
                other.gameObject.CompareTag("OutArea"))
            {
                //着地地点を生成する
                GenerateRandingPoint();
                boundCount++;

                //セーフエリアの場合
                if (other.gameObject.CompareTag("SafetyArea")) isSafetyArea = true;

                //アウトエリアの場合
                if (!isSafetyArea && boundCount == 1) isOut = true;

                isCoolTime = true;
            }
        }

        //ゲームの状態がサーブの場合
        if (GameManager.instance.gameState == GameState.Serve)
        {
            if (other.gameObject.CompareTag("ServeArea"))
            {
                Debug.Log("サーブ成功");
                GameManager.instance.gameState = GameState.DuringRound;
                GameManager.instance.ChangeField();
                //boundCount++;

                isCoolTime = true;
            }
            else if (!other.gameObject.CompareTag("SwingArea"))
            {
                Debug.Log("サーブ失敗");
            }
        }
    }

    //打つ処理
    public void Strike(float _flightTime, float _speedRate)
    {
        if(GameManager.instance.gameState == GameState.Serve &&
            boundCount == 0 && isProjection && !isBound)
        {
            isOut = true;
        }

        //タグを切り替える
        TagChange();

        //到達地点を更新する
        LandingForecast lf = 
            GameObject.Find("RandingPointControl").GetComponent<LandingForecast>();
        lf.PointSetting();

        //バウンドフラグをオフに
        isBound = false;

        //セーフティフラグをオフに
        isSafetyArea = false;

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
    }

    public void Init()
    {
        boundCount = 0;
        colCoolTime = 0;
        isBound = false;
        isProjection = false;
        isOut = false;
        isNet = false;
        isSafetyArea = false;
        isCoolTime = false;
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

        Debug.Log("バウンド回数をリセットしました " + Time.time);
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
