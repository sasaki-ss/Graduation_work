using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapStateManager;
using System;

public class Shot : MonoBehaviour
{
    TouchManager tMger;      //TapStateManager内のTouchManager

    private static int FRAME = 60;  //フレームレート
    private static double MAX_TAPTIME = FRAME * 2;   //タップ時間の最大

    private double tapTime;   //タップしている時間
    private double power;     //プレイヤーに渡す用の変数
    
    private Vector2 tapStart; //タップし始めた場所    
    private Vector2 tapEnd;   //タップを離した場所
    private Vector2 direction;//始点と終点を計算した方向のベクター
    private double radian;     //2点間の角度
    private double distance;   //2点間の距離

    public double GetTapTime
    {
        get { return tapTime; }
    }
    public double GetPower
    {
        get { return power; }
    }
    public Vector2 GetDirection
    {
        get { return direction; }
    }
    public double GetDistance
    {
        get { return distance; }
    }
    public double GetRadian
    {
        get { return radian; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = FRAME;   //フレームレート制御
        tMger = new TouchManager(); //初期化
        tapTime = 0.0f;
        tapStart = new Vector2(0,0);
        tapEnd = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        tMger.update(); //更新

        TouchManager touch_state = tMger.getTouch();    //タッチ取得

        if (touch_state._touch_flag)  //タッチされていた場合
        {
            if(touch_state._touch_phase == TouchPhase.Began)
            {   //タッチ開始
                //なぜかここの処理が実行されない
            }

            if (touch_state._touch_phase == TouchPhase.Moved)
            {   //タッチ中
                if (tapTime == 0.0f)
                {   //Beganの代わり
                    tapStart = tMger._touch_position;   //座標取得
                    //Debug.Log("開始座標" + tapStart);
                }
                tapTime += 1.0f;
            }

            if (touch_state._touch_phase == TouchPhase.Ended)
            {   //タッチ終了
                tapEnd = tMger._touch_position;         //座標取得
                //Debug.Log("終了座標" + tapEnd);
                PowerConversion();  //パワーの計算
                VectorCalculation();//ベクトルの計算
                tapTime = 0;        //時間の初期化
            }

            if(tapTime > MAX_TAPTIME)
            {
                tapTime = MAX_TAPTIME;   //上限(タップしてから2秒で最大になる)
            }


            //Debug.Log("入力時間"+tapTime);
        }
    }

    void PowerConversion()
    {   //入力時間をパワーに変換する
        power = tapTime / (FRAME / 2);  //時間によるパワーの計算式 tapTime/30 で最大値4になる  
        //Debug.Log("入力時間"+power);
    }

    void VectorCalculation()
    {   //入力場所を起点として入力距離を求める
        direction = tapEnd - tapStart;

        if(tapStart.y > tapEnd.y)
        {   //下側にスワイプした場合(反転化)
            direction *= new Vector2(-1, -1);
        }
        //Debug.Log("入力距離の座標" + direction);

        //2点間の距離を求める
        distance = Math.Sqrt((direction.x * direction.x) +
            (direction.y * direction.y));

        //2点間の角度を求める
        radian = Math.Atan2(direction.y, direction.x);

        //Debug.Log("距離"+distance);
        //Debug.Log("角度"+radian);
    }
}
