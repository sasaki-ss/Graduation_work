using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボールクラス
public class Ball : MonoBehaviour
{
    /*このスクリプトでのみ使う変数*/
    private Rigidbody       rb;             //Rigidbody
    private TrailRenderer   tr;             //TrailRenderer
    private Coroutine       coroutine;      //コルーチン
    private Vector3         endPoint;       //終点地点
    private Vector3         diff;           //距離
    private User            nowShotUser;    //現在打っているユーザー
    private int             colCoolTime;    //当たり判定クールタイム
    private float           flightTime;     //滞空時間
    private float           speedRate;      //滞空時間を基準とした移動速度倍率
    private float           e;              //反発係数
    private bool            isBound;        //バウンドフラグ
    private bool            isProjection;   //投射フラグ
    private bool            isCoolTime;     //クールタイムフラグ
    private bool            isSafetyArea;   //セーフティエリアフラグ

    /*プロパティ関連*/
    public User         nowUserTag { get; private set; }    //タグ
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
        //TrailRendererを取得
        tr = this.GetComponent<TrailRenderer>();

        tr.time = 0f;

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
        Init();
    }

    //物理演算が行われる際の処理
    private void FixedUpdate()
    {
        Debug.Log("現在のユーザータグ : "+nowUserTag);

        if (GameManager.instance.isServe)
        {
            Vector3 pPos = userObj[(int)GameManager.instance.serveUser].transform.position;
            Vector3 pForwardVec = userObj[(int)GameManager.instance.serveUser].transform.forward;

            this.transform.position = new Vector3(pPos.x, 10f, pPos.z) + (pForwardVec * 5);
        }
        else
        {
            if (colCoolTime == 60)
            {
                isCoolTime = false;
                colCoolTime = 0;
            }

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

        tr.time = 0.5f;

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

        tr.time = 0f;
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
                Debug.Log("バウンド回数 : " + boundCount + " " + Time.time);
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
                boundCount++;
                Debug.Log("バウンド回数 : " + boundCount + " " + Time.time);

                isCoolTime = true;
            }
            else if (!other.gameObject.CompareTag("SwingArea"))
            {
                Debug.Log("サーブ失敗");
                GameManager.instance.FaultProc();
            }
        }
    }

    #region サーブ処理
    public void Serve(float _flightTime, float _speedRate, User _user)
    {
        //サーブ用のコルーチンを開始
        coroutine = StartCoroutine(ThrowBall(_flightTime, _speedRate, _user));
    }

    private IEnumerator ThrowBall(float _flightTime, float _speedRate, User _user)
    {
        Debug.Log("=======上昇開始=======");
        flightTime = _flightTime;
        speedRate = _speedRate;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(new Vector3(0f, 10f, 0f), ForceMode.Impulse);

        while (this.transform.position.y >= 8.0f)
        {
            yield return null;
        }

        Debug.Log("=======発射！！=======");

        Strike(flightTime, speedRate, _user);
    }
    #endregion

    #region 打つ処理
    public void Strike(float _flightTime, float _speedRate,User _user)
    {
        Debug.Log("=======Shot　Now=======");

        if (GameManager.instance.gameState == GameState.Serve &&
            boundCount == 0 && isProjection && !isBound)
        {
            isOut = true;
        }

        //タグを切り替える
        if(GameManager.instance.gameState != GameState.Serve)TagChange();

        //到達地点を更新する
        LandingForecast lf = 
            GameObject.Find("RandingPointControl").GetComponent<LandingForecast>();
        lf.PointSetting(_user);

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
    #endregion

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

        nowShotUser = GameManager.instance.serveUser;

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    //タグ切り替え処理
    private void TagChange()
    {
        //ユーザーが0番目のとき
        if(nowShotUser == User.User1)
        {
            nowShotUser = User.User2;
        }
        //ユーザーが1番目のとき
        else
        {
            nowShotUser = User.User1;
        }

        //タグを指定したユーザーへ変更する
        nowUserTag = nowShotUser;
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
