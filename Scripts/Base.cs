using System.Collections;
using System.Collections.Generic;
using TapStateManager;
using UnityEngine;
using UnityEngine.AI;

public class Base : MonoBehaviour
{

    [SerializeField] Ball ball;


    //タッチ時に使う処理
    Vector2 touch;
    Vector3 touchPosition;

    //タップ処理を実行するためのやつ
    TouchManager tMger;      //TapStateManager内のTouchManager
    public TouchManager touch_state;

    float flightTime;
    float speed;

    bool flg;

    // Start is called before the first frame update
    void Start()
    {
        flightTime = 0;
        speed = 0;
        flg = false;

        tMger = new TouchManager(); //初期化
        ball = GameObject.Find("Ball").GetComponent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        tMger.update(); //更新

        touch_state = tMger.getTouch();    //タッチ取得
    }

    void FixedUpdate()
    {
        //ここにProjectileMotion関数で使うための情報を入れる関数が来る
        if (flg == true)
        {
            ball.Strike(flightTime, speed);
            flg = false;
        }
    }

    //共通処理　移動
    public Vector3 Move(Vector3 _pos, RaycastHit _hit)
    {
        //カメラの場所とマウスのClick座標からZ座標を求める
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //タッチ時の処理　確認はしてない
        //Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        bool hasHit = Physics.Raycast(ray, out _hit);

        return _hit.point;
    }

    //共通処理　移動判定
    public bool PositionJudge(Vector3 _now, Vector3 _old)
    {
        //現座標と前座標に違う場合
        if (_now != _old)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //共通処理　自動移動
    public void AutoMove()
    {
        //移動させる
        GetComponent<NavMeshAgent>().destination = ball.transform.position;
    }

    //共通処理　円の大きさ
    public int CircleScale(double _scale)
    {
        int _CircleScale;

        //スライドの長さによって円の大きさが変わる
        if (_scale >= 0 && _scale < 5)
        {
            _CircleScale = 50;
        }
        else
        if (_scale >= 5 && _scale < 10)
        {
            _CircleScale = 42;
        }
        else
        if (_scale >= 10 && _scale < 15)
        {
            _CircleScale = 34;
        }
        else
        if (_scale >= 15 && _scale < 20)
        {
            _CircleScale = 26;
        }
        else
        if (_scale >= 20 && _scale < 25)
        {
            _CircleScale = 18;
        }
        else
        if (_scale >= 25 && _scale < 30)
        {
            _CircleScale = 10;
        }
        else
        {
            _CircleScale = 2;
        }

        return _CircleScale;
    }

    //共通処理　スイング
    public void Swing(double _power, double _flight)
    {
        //滞空時間　タップ時間から　
        //速度　　　パワーから

        //この二つ変数をProjectileMotion関数に渡す
        //とりあえず最低5秒滞空時間があるとしてます　(5～15秒)
        flightTime = (float)_flight / 12 + 3;
        //Debug.Log("a" + flightTime);
        //とりあえず7で割ってます
        speed = (float)_power / 7;

        flg = true;
    }
}

