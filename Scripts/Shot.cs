using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapStateManager;

public class Shot : MonoBehaviour
{
    TouchManager tMger;      //TapStateManager����TouchManager

    public float tapTime;   //�^�b�v���Ă��鎞��
    public Vector2 tapStart;//�^�b�v���n�߂��ꏊ
    public Vector2 tapEnd;  //�^�b�v�𗣂����ꏊ

    public float Power
    {
        get { return tapTime/30; }  //�^�b�v���Ԃɂ��p���[
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;   //�t���[�����[�g����
        tMger = new TouchManager(); //������
        tapTime = 0.0f;
        tapStart = new Vector2(0,0);
        tapEnd = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        tMger.update(); //�X�V

        TouchManager touch_state = tMger.getTouch();    //�^�b�`�擾

        if (touch_state.touch_flg)  //�^�b�`����Ă����ꍇ
        {
            if(touch_state.touch_phase == TouchPhase.Began)
            {   //�^�b�`�J�n
                tapTime = 0;
                tapStart = new Vector2(0, 0);
                tapEnd = new Vector2(0, 0);
            }

            if (touch_state.touch_phase == TouchPhase.Moved)
            {   //�^�b�`��
                if(tapTime == 0.0f)
                {
                    tapStart = tMger.touch_position;
                }
                tapTime += 0.1f;

            }

            if (touch_state.touch_phase == TouchPhase.Ended)
            {   //�^�b�`�I��
                tapEnd = tMger.touch_position;
            }

            if(tapTime > 120.0f)
            {
                tapTime = 120.0f;   //���(�^�b�v���Ă���2�b�ōő�ɂȂ�)
            }
            Debug.Log("start"+tapStart);
            Debug.Log("end"+tapEnd);
            //Debug.Log(tapTime);
        }
    }
}
