using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 参考サイト
 https://techblog.gracetory.co.jp/entry/2018/06/04/000000
 */

/**
 * 状態管理
 */
namespace TapStateManager
{
    /**
     * タッチ管理クラス
     */
    public class TouchManager
    {
        public bool touch_flg;      // タッチ有無
        public Vector2 touch_position;   // タッチ座標
        public TouchPhase touch_phase;   // タッチ状態

        /**
         * コンストラクタ
         *
         * @param bool flag タッチ有無
         * @param Vector2 position タッチ座標(引数の省略が行えるようにNull許容型に)
         * @param Touchphase phase タッチ状態
         * @access public
         */
        public TouchManager(bool flag = false, Vector2? position = null, TouchPhase phase = TouchPhase.Began)
        {
            this.touch_flg = flag;
            if (position == null)
            {
                this.touch_position = new Vector2(0, 0);
            }
            else
            {
                this.touch_position = (Vector2)position;
            }
            this.touch_phase = phase;
        }

        /**
         * 更新
         *
         * @access public
         */
        public void update()
        {
            this.touch_flg = false;

            // エディタ
            if (Application.isEditor)
            {
                // 押した瞬間
                if (Input.GetMouseButtonDown(0))
                {
                    this.touch_flg = true;
                    this.touch_phase = TouchPhase.Began;
                    Debug.Log("押した瞬間");
                }

                // 離した瞬間
                if (Input.GetMouseButtonUp(0))
                {
                    this.touch_flg = true;
                    this.touch_phase = TouchPhase.Ended;
                    Debug.Log("離した瞬間");
                }

                // 押しっぱなし
                if (Input.GetMouseButton(0))
                {
                    this.touch_flg = true;
                    this.touch_phase = TouchPhase.Moved;
                    Debug.Log("押しっぱなし");
                }

                // 座標取得
                if (this.touch_flg) this.touch_position = Input.mousePosition;

                // 端末
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    this.touch_position = touch.position;
                    this.touch_phase = touch.phase;
                    this.touch_flg = true;
                }
            }
        }

        /**
         * タッチ状態取得
         *
         * @access public
         */
        public TouchManager getTouch()
        {
            return new TouchManager(this.touch_flg, this.touch_position, this.touch_phase);
        }
    }
}