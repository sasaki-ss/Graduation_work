using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapStateManager;

public class Shot : MonoBehaviour
{
    TouchManager tMger;      //TapStateManager����TouchManager

    private static int FRAME = 60;  //�t���[�����[�g
    private static float MAX_TAPTIME = FRAME * 2;   //�^�b�v���Ԃ̍ő�

    public float tapTime;   //�^�b�v���Ă��鎞��
    public float power;     //�v���C���[�ɓn���p�̕ϐ�
    public Vector2 tapStart;//�^�b�v���n�߂��ꏊ
    public Vector2 tapEnd;  //�^�b�v�𗣂����ꏊ
    public Vector2 direction;//�����̃x�N�^�[

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
        Application.targetFrameRate = FRAME;   //�t���[�����[�g����
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

        if (touch_state._touch_flag)  //�^�b�`����Ă����ꍇ
        {
            if(touch_state._touch_phase == TouchPhase.Began)
            {   //�^�b�`�J�n
                //�Ȃ��������̏��������s����Ȃ�
            }

            if (touch_state._touch_phase == TouchPhase.Moved)
            {   //�^�b�`��
                if (tapTime == 0.0f)
                {   //Began�̑���
                    tapStart = tMger._touch_position;   //���W�擾
                    Debug.Log("start" + tapStart);
                }
                tapTime += 1.0f;
            }

            if (touch_state._touch_phase == TouchPhase.Ended)
            {   //�^�b�`�I��
                tapEnd = tMger._touch_position;         //���W�擾
                Debug.Log("end" + tapEnd);
                PowerConversion();  //�p���[�̌v�Z
                VectorCalculation();//�x�N�g���̌v�Z
                tapTime = 0;        //���Ԃ̏�����
            }

            if(tapTime > MAX_TAPTIME)
            {
                tapTime = MAX_TAPTIME;   //���(�^�b�v���Ă���2�b�ōő�ɂȂ�)
            }


            //Debug.Log(tapTime);
        }
    }

    void PowerConversion()
    {   //���͎��Ԃ��p���[�ɕϊ�����
        power = tapTime / (FRAME / 2);  //���Ԃɂ��p���[�̌v�Z��   
        Debug.Log(power);
    }

    void VectorCalculation()
    {   //���͏ꏊ���N�_�Ƃ��ē��͋��������߂�
        direction = tapEnd - tapStart;

        if(tapStart.y > tapEnd.y)
        {   //�����ɃX���C�v�����ꍇ(���]��)
            direction *= new Vector2(-1, -1);
        }
        Debug.Log("direction" + direction);
    }
}
