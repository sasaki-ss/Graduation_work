using System.Collections;
using System.Collections.Generic;
using TapStateManager;
using UnityEngine;
using UnityEngine.AI;

public class Base : MonoBehaviour
{

    [SerializeField] Ball ball;


    //�^�b�`���Ɏg������
    Vector2 touch;
    Vector3 touchPosition;

    //�^�b�v���������s���邽�߂̂��
    TouchManager tMger;      //TapStateManager����TouchManager
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

        tMger = new TouchManager(); //������
        ball = GameObject.Find("Ball").GetComponent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        tMger.update(); //�X�V

        touch_state = tMger.getTouch();    //�^�b�`�擾
    }

    void FixedUpdate()
    {
        //������ProjectileMotion�֐��Ŏg�����߂̏�������֐�������
        if (flg == true)
        {
            ball.Strike(flightTime, speed);
            flg = false;
        }
    }

    //���ʏ����@�ړ�
    public Vector3 Move(Vector3 _pos, RaycastHit _hit)
    {
        //�J�����̏ꏊ�ƃ}�E�X��Click���W����Z���W�����߂�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //�^�b�`���̏����@�m�F�͂��ĂȂ�
        //Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        bool hasHit = Physics.Raycast(ray, out _hit);

        return _hit.point;
    }

    //���ʏ����@�ړ�����
    public bool PositionJudge(Vector3 _now, Vector3 _old)
    {
        //�����W�ƑO���W�ɈႤ�ꍇ
        if (_now != _old)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //���ʏ����@�����ړ�
    public void AutoMove()
    {
        //�ړ�������
        GetComponent<NavMeshAgent>().destination = ball.transform.position;
    }

    //���ʏ����@�~�̑傫��
    public int CircleScale(double _scale)
    {
        int _CircleScale;

        //�X���C�h�̒����ɂ���ĉ~�̑傫�����ς��
        if (_scale >= 0 && _scale < 5)
        {
            _CircleScale = 50;
        }
        else
        if (_scale >= 5 && _scale < 10)
        {
            _CircleScale = 42;
        }
        else
        if (_scale >= 10 && _scale < 15)
        {
            _CircleScale = 34;
        }
        else
        if (_scale >= 15 && _scale < 20)
        {
            _CircleScale = 26;
        }
        else
        if (_scale >= 20 && _scale < 25)
        {
            _CircleScale = 18;
        }
        else
        if (_scale >= 25 && _scale < 30)
        {
            _CircleScale = 10;
        }
        else
        {
            _CircleScale = 2;
        }

        return _CircleScale;
    }

    //���ʏ����@�X�C���O
    public void Swing(double _power, double _flight)
    {
        //�؋󎞊ԁ@�^�b�v���Ԃ���@
        //���x�@�@�@�p���[����

        //���̓�ϐ���ProjectileMotion�֐��ɓn��
        //�Ƃ肠�����Œ�5�b�؋󎞊Ԃ�����Ƃ��Ă܂��@(5�`15�b)
        flightTime = (float)_flight / 12 + 3;
        //Debug.Log("a" + flightTime);
        //�Ƃ肠����7�Ŋ����Ă܂�
        speed = (float)_power / 7;

        flg = true;
    }
}

