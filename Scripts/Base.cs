using System.Collections;
using System.Collections.Generic;
using TapStateManager;
using UnityEngine;
using UnityEngine.AI;

public class Base : MonoBehaviour
{
    [SerializeField] Ball ball;
    private User user;

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
            //�T�[�u�t���O���I�t��
            if (GameManager.instance.isServe == true)
            {
                GameManager.instance.isServe = false;
                ball.Serve(flightTime, speed);
            }
            //�����[�Ƃ��ł̃X�C���O�̎�
            else
            {
                ball.Strike(flightTime, speed);
            }

            flg = false;
        }
    }

    //���ʏ��������̏�����
    public void Init()
    {
        if (user == User.User1)
        {
            CharacterMove player = this.GetComponent<CharacterMove>();
            player.Init();
        }
        else
        {
            AI ai = this.GetComponent<AI>();
            ai.Init();
        }

        flightTime = 0;
        speed = 0;
        flg = false;

        Debug.Log("Base��Init�����̎��s");
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

    //���ʏ����@�X�C���O
    public void Swing(double _power, double _flight, double _taptime)
    {
        //�؋󎞊ԁ@�^�b�v���Ԃ���@
        //���x�@�@�@�p���[����

        //���̓�ϐ���ProjectileMotion�֐��ɓn��
        //�Ƃ肠�����Œ�4�b�؋󎞊Ԃ�����Ƃ��Ă܂��@
        flightTime = (float)_flight / 60 + (float)_taptime / 5;
        //Debug.Log(flightTime);
        //Debug.Log("a" + flightTime);
        //�Ƃ肠����5�Ŋ����Ă܂�
        speed = (float)_power / 6;

        flg = true;    
    }
}

