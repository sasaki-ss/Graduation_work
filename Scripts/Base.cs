using System.Collections;
using System.Collections.Generic;
using TapStateManager;
using UnityEngine;
using UnityEngine.AI;

public class Base : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] GameObject[] chara;

    //タッチ時に使う処理
    Vector2 touch;
    Vector3 touchPosition;

    //タップ処理を実行するためのやつ
    TouchManager tMger;
    public TouchManager touch_state;

    float flightTime;
    float speed;

    void Start()
    {
        //数値リセット
        flightTime = 0;
        speed      = 0;

        tMger = new TouchManager(); //初期化

        //それぞれのキャラを読み込む
        chara = new GameObject[2];
        chara[(int)User.User1] = GameObject.Find("Player");
        chara[(int)User.User2] = GameObject.Find("Player2");

        //ボールの読み込み
        ball = GameObject.Find("Ball").GetComponent<Ball>();
    }

    void Update()
    {
        tMger.update();                    //更新
        touch_state = tMger.getTouch();    //タッチ取得
    }

    //共通処理部分の初期化
    public void Init()
    {
        //それぞれのInitを呼び出す
        CharacterMove player = chara[0].GetComponent<CharacterMove>();
        player.Init();

        AI ai = chara[1].GetComponent<AI>();
        ai.Init();

        //数値のリセット
        flightTime = 0;
        speed = 0;

        //Debug.Log("BaseのInit処理の実行");
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
            _CircleScale = 15;
        }
        else
        if (_scale >= 5 && _scale < 20)
        {
            _CircleScale = 10;
        }
        else
        if (_scale >= 40 && _scale < 60)
        {
            _CircleScale = 8;
        }
        else
        if (_scale >= 60 && _scale < 80)
        {
            _CircleScale = 6;
        }
        else
        if (_scale >= 80 && _scale < 100)
        {
            _CircleScale = 4;
        }
        else
        if (_scale >= 100)
        {
            _CircleScale = 2;
        }
        else
        {
            _CircleScale = 0;
        }

        //Debug.Log(_scale);
        //Debug.Log(_CircleScale);

        return _CircleScale;
    }

    //共通処理　スイング
    public void Swing(double _power, double _flight, double _taptime,User _user)
    {
        //滞空時間　タップ時間から　
        //速度　　　パワーから

        //この二つ変数をProjectileMotion関数に渡す

        //とりあえず最低4秒滞空時間があるとしてます　
        flightTime = (float)_flight / 60 + (float)_taptime / 20 + 2.5f;

        //とりあえず6で割ってます
        speed = (float)_power / 6;

        //Debug.Log(flightTime);
        //Debug.Log(speed);

        if (GameManager.instance.isServe == true)
        {
            //Debug.Log("サーブ");
            GameManager.instance.isServe = false;
            ball.Serve(flightTime, speed,_user);
        }
        else
        {
            //Debug.Log("ラリー");
            ball.Strike(flightTime, speed,_user);
        }
    }
}

