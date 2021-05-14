using System.Collections;
using System.Collections.Generic;
using TapStateManager;
using UnityEngine;
using UnityEngine.AI;

public class Base : MonoBehaviour
{
    [SerializeField] Shot Shot;

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
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
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
    public int CircleScale()
    {
        int _CircleScale;

        //スライドの長さによって円の大きさが変わる
        if (Shot.GetDistance >= 0 && Shot.GetDistance < 5)
        {
            _CircleScale = 50;
        }
        else
        if (Shot.GetDistance >= 5 && Shot.GetDistance < 10)
        {
            _CircleScale = 42;
        }
        else
        if (Shot.GetDistance >= 10 && Shot.GetDistance < 15)
        {
            _CircleScale = 34;
        }
        else
        if (Shot.GetDistance >= 15 && Shot.GetDistance < 20)
        {
            _CircleScale = 26;
        }
        else
        if (Shot.GetDistance >= 20 && Shot.GetDistance < 25)
        {
            _CircleScale = 18;
        }
        else
        if (Shot.GetDistance >= 25 && Shot.GetDistance < 30)
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
    public void Swing(double _power)
    {
        //滞空時間　タップ時間から　
        //速度　　　パワーから

        //この二つ変数をProjectileMotion関数に渡す

        //とりあえず最低5秒滞空時間があるとしてます　(5～7秒)
        flightTime = (float)Shot.GetTapTime * 2 + 5;
        //とりあえず6で割ってます数値的には1.6666666
        speed = (float)_power / 6;

        flg = true;
    }
}
