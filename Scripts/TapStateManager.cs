using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public bool _touch_flag;      // �^�b�`�L��
        public Vector2 _touch_position;   // �^�b�`���W
        public TouchPhase _touch_phase;   // �^�b�`���

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
            this._touch_flag = flag;
            if (position == null)
            {
                this._touch_position = new Vector2(0, 0);
            }
            else
            {
                this._touch_position = (Vector2)position;
            }
            this._touch_phase = phase;
        }

        /**
         * �X�V
         *
         * @access public
         */
        public void update()
        {
            this._touch_flag = false;

            // �G�f�B�^
            if (Application.isEditor)
            {
                // �������u��
                if (Input.GetMouseButtonDown(0))
                {
                    this._touch_flag = true;
                    this._touch_phase = TouchPhase.Began;
                    Debug.Log("�������u��");
                }

                // �������u��
                if (Input.GetMouseButtonUp(0))
                {
                    this._touch_flag = true;
                    this._touch_phase = TouchPhase.Ended;
                    Debug.Log("�������u��");
                }

                // �������ςȂ�
                if (Input.GetMouseButton(0))
                {
                    this._touch_flag = true;
                    this._touch_phase = TouchPhase.Moved;
                    Debug.Log("�������ςȂ�");
                }

                // ���W�擾
                if (this._touch_flag) this._touch_position = Input.mousePosition;

                // �[��
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    this._touch_position = touch.position;
                    this._touch_phase = touch.phase;
                    this._touch_flag = true;
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
            return new TouchManager(this._touch_flag, this._touch_position, this._touch_phase);
        }
    }
}