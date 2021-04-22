using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapStateManager;

public class Shot : MonoBehaviour
{
    TouchManager tMger;      //TapStateManager内のTouchManager

    public float tapTime;   //タップしている時間
    public Vector2 tapStart;//タップし始めた場所
    public Vector2 tapEnd;  //タップを離した場所

    public float Power
    {
        get { return tapTime/30; }  //タップ時間によるパワー
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;   //フレームレート制御
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

        if (touch_state.touch_flg)  //タッチされていた場合
        {
            if(touch_state.touch_phase == TouchPhase.Began)
            {   //タッチ開始
                tapTime = 0;
                tapStart = new Vector2(0, 0);
                tapEnd = new Vector2(0, 0);
            }

            if (touch_state.touch_phase == TouchPhase.Moved)
            {   //タッチ中
                if(tapTime == 0.0f)
                {
                    tapStart = tMger.touch_position;
                }
                tapTime += 0.1f;

            }

            if (touch_state.touch_phase == TouchPhase.Ended)
            {   //タッチ終了
                tapEnd = tMger.touch_position;
            }

            if(tapTime > 120.0f)
            {
                tapTime = 120.0f;   //上限(タップしてから2秒で最大になる)
            }
            Debug.Log("start"+tapStart);
            Debug.Log("end"+tapEnd);
            //Debug.Log(tapTime);
        }
    }
}
