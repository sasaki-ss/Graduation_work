using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapStateManager;

public class Shot : MonoBehaviour
{
    TouchManager tMger;      //TapStateManager内のTouchManager

    private static int FRAME = 60;  //フレームレート
    private static float MAX_TAPTIME = FRAME * 2;   //タップ時間の最大

    public float tapTime;   //タップしている時間
    public float power;     //プレイヤーに渡す用の変数
    public Vector2 tapStart;//タップし始めた場所
    public Vector2 tapEnd;  //タップを離した場所
    public Vector2 direction;//方向のベクター

    public float GetPower
    {
        get { return power; }
    }
    public Vector2 GetDirection
    {
        get { return direction; }
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
                    Debug.Log("start" + tapStart);
                }
                tapTime += 1.0f;
            }

            if (touch_state._touch_phase == TouchPhase.Ended)
            {   //タッチ終了
                tapEnd = tMger._touch_position;         //座標取得
                Debug.Log("end" + tapEnd);
                PowerConversion();  //パワーの計算
                VectorCalculation();//ベクトルの計算
                tapTime = 0;        //時間の初期化
            }

            if(tapTime > MAX_TAPTIME)
            {
                tapTime = MAX_TAPTIME;   //上限(タップしてから2秒で最大になる)
            }


            //Debug.Log(tapTime);
        }
    }

    void PowerConversion()
    {   //入力時間をパワーに変換する
        power = tapTime / (FRAME / 2);  //時間によるパワーの計算式   
        Debug.Log(power);
    }

    void VectorCalculation()
    {   //入力場所を起点として入力距離を求める
        direction = tapEnd - tapStart;

        if(tapStart.y > tapEnd.y)
        {   //下側にスワイプした場合(反転化)
            direction *= new Vector2(-1, -1);
        }
        Debug.Log("direction" + direction);
    }
}
