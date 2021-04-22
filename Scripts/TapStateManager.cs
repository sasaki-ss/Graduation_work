using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 �Q�l�T�C�g
 https://techblog.gracetory.co.jp/entry/2018/06/04/000000
 */

/**
 * ��ԊǗ�
 */
namespace TapStateManager
{
    /**
     * �^�b�`�Ǘ��N���X
     */
    public class TouchManager
    {
        public bool touch_flg;      // �^�b�`�L��
        public Vector2 touch_position;   // �^�b�`���W
        public TouchPhase touch_phase;   // �^�b�`���

        /**
         * �R���X�g���N�^
         *
         * @param bool flag �^�b�`�L��
         * @param Vector2 position �^�b�`���W(�����̏ȗ����s����悤��Null���e�^��)
         * @param Touchphase phase �^�b�`���
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
         * �X�V
         *
         * @access public
         */
        public void update()
        {
            this.touch_flg = false;

            // �G�f�B�^
            if (Application.isEditor)
            {
                // �������u��
                if (Input.GetMouseButtonDown(0))
                {
                    this.touch_flg = true;
                    this.touch_phase = TouchPhase.Began;
                    Debug.Log("�������u��");
                }

                // �������u��
                if (Input.GetMouseButtonUp(0))
                {
                    this.touch_flg = true;
                    this.touch_phase = TouchPhase.Ended;
                    Debug.Log("�������u��");
                }

                // �������ςȂ�
                if (Input.GetMouseButton(0))
                {
                    this.touch_flg = true;
                    this.touch_phase = TouchPhase.Moved;
                    Debug.Log("�������ςȂ�");
                }

                // ���W�擾
                if (this.touch_flg) this.touch_position = Input.mousePosition;

                // �[��
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
         * �^�b�`��Ԏ擾
         *
         * @access public
         */
        public TouchManager getTouch()
        {
            return new TouchManager(this.touch_flg, this.touch_position, this.touch_phase);
        }
    }
}