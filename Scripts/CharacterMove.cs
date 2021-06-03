using TapStateManager;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Touch;
public class CharacterMove : MonoBehaviour
{
    //���ʂ̂��
    [SerializeField] Base Base;

    //�e�X�̂��
    [SerializeField] Animator animator;
    [SerializeField] CharaStatus CharaStatus;
    [SerializeField] Transform player;
    [SerializeField] public Shot Shot;
    [SerializeField] GameObject net;
    [SerializeField] Ball ball;
    [SerializeField] GameObject pointB;
    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

     int motionCnt = 0;
    bool autoFlg   = false;
    bool swingFlg  = false;
    bool hitFlg = false;

    void Start()
    {
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    void Init()
    {
        player.transform.position = new Vector3(110, 0, 0);
        motionCnt = 0;
        autoFlg = false;
        swingFlg = false;
        hitFlg = false;

        //����̈ړ��w��n���폜
        GetComponent<NavMeshAgent>().ResetPath();

        //if���ł��������T�[�u�Ȃ̂����肵�Ă���

        if(ball.nowUserTag =="Player2")
        {
           //�Ίp����ɔz�u����\��
           player.transform.position = new Vector3(105,0,-25);
        }
        else
        {
            //�Ίp����ɔz�u����\��
            player.transform.position = new Vector3(105, 0, 25);
        }
   
        Shot = GameObject.Find("Shot").GetComponent<Shot>();

        Base.InitCnt += 1;
        Debug.Log("Player");
    }

    void Update()
    {
        if (Base.InitCnt == 2) 
        {
            Init();
        }

        //�R�����g�A�E�g
        {
            /*
            //�^�b�`���̏����@�m�F�͂��ĂȂ�
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                touch = Input.GetTouch(0).deltaPosition;

                touchPosition = new Vector3(touch.x, touch.y,0);
            }
            */
        }

        //�^�b�`���̏���
        TapMove();

        //�����ړ����̏���
        AutoMove();

        //�ړ��������肷�鏈��
        JudgeMove();

        //���P�b�g��U�������̏���
        Swing();

        //���݂̍��W���擾
        nowPosition = player.position;
    }






    void TapMove()
    {
        //�N���b�N
        if (Base.touch_state._touch_flag == true && Base.touch_state._touch_phase == TouchPhase.Ended)
        {
            //����̈ړ��w��n���폜
            GetComponent<NavMeshAgent>().ResetPath();

            //�N���b�N���Ԃɂ���ď����𕪂���
            if (Shot.GetTapTime <= 10)
            {
                //�ړ��̏���
                Vector3 xyz = Base.Move(Input.mousePosition, hit);

                //�l�b�g�z���Ȃ��悤��
                if (xyz.x >= 20)
                {
                    GetComponent<NavMeshAgent>().destination = Base.Move(Input.mousePosition, hit);
                }
            }
            //������
            else
            {
                //�X�C���OAnimation�ɂ���\��
                animator.SetBool("is_RightShake", true);

                //�~�̑傫���𑪂�
                CharaStatus.CharaCircle = Base.CircleScale(Shot.GetDistance);

                //�v���C���[��Ԃ�U��ɕύX
                CharaStatus.NowState = 2;
            }
        }
    }

    void AutoMove()
    {
        //�I�[�g�ړ�����
        if (ball.nowUserTag == "Player2" && ball.transform.position.x >= 7)
        {
            if (autoFlg == true)
            {

                Vector3 xyz = new Vector3(0, 0, 0);

                //�v���C���[�̃R�[�g�̍���
                if (pointB.transform.position.z < -40 && pointB.transform.position.z > -0)
                {
                    xyz = new Vector3(110, 0, -25);
                }

                //�v���C���[�̃R�[�g�̉E�� 
                if (pointB.transform.position.z > -9 && pointB.transform.position.z < 9)
                {
                    xyz = new Vector3(110, 0, -2);

                }

                //�v���C���[�̃R�[�g�̒���
                if (pointB.transform.position.z > 10 && pointB.transform.position.z < 40)
                {
                    xyz = new Vector3(110, 0, 25);

                }

                //�v���C���[�̃R�[�g�̑O����
                if (pointB.transform.position.x < 0 && pointB.transform.position.x > 40)
                {
                    xyz = new Vector3(40, 0, 25);
                }

                //�v���C���[�̃R�[�g�̒���
                if (pointB.transform.position.x < 41 && pointB.transform.position.x > 80)
                {
                    xyz = new Vector3(80, 0, 25);

                }

                //�v���C���[�̃R�[�g�̌㒆��
                if (pointB.transform.position.x < 81 && pointB.transform.position.x > 120)
                {
                    xyz = new Vector3(110, 0, 25);
                }

                //�ړ�������
                GetComponent<NavMeshAgent>().destination = xyz;

                //�����ړ��͈��̂�
                autoFlg = false;
            }
        }
    }

    void JudgeMove()
    {
        //�ړ������ǂ���
        if (Base.PositionJudge(player.position, nowPosition))
        {
            //�v���C���[�𑖂郂�[�V�����ɂ���
            animator.SetBool("is_Run", true);

            //�v���C���[��Ԃ��ړ��ɕύX
            CharaStatus.NowState = 1;

            //�v���C���[�̃X�^�~�i�����炷
            CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
        }
        else
        {
            //�v���C���[��ҋ@���[�V�����ɂ���
            animator.SetBool("is_Run", false);

            //�v���C���[��Ԃ�ҋ@�ɕύX
            CharaStatus.NowState = 0;
        }

        if (animator.GetBool("is_Run") == false)
        {
            //�X�s�[�h�����߂�
            float speed = 0.1f;

            //�����Ƒ���Ƃ̃x�N�g�������擾
            Vector3 relativePos = net.transform.position - player.transform.position;

            //��������]���ɂ���
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            //�ΏۂɌ�������
            player.rotation = Quaternion.Slerp(this.transform.rotation, rotation, speed);
        }
    }

    void Swing()
    {
        //�U���Ԏ��Ȃ�50�J�E���g��ɑҋ@��Ԃɖ߂�
        if (animator.GetBool("is_RightShake") == true)
        {
            motionCnt++;

            if (motionCnt > 30)
            {
                swingFlg = true;
            }

            if (motionCnt > 40)
            {
                motionCnt = 0;

                //�v���C���[��Ԃ�ҋ@�ɕύX
                CharaStatus.NowState = 0;

                //�v���C���[��ҋ@���[�V�����ɂ���
                animator.SetBool("is_RightShake", false);

                swingFlg = false;
            }
        }

        //�U�������P�b�g������������
        if (ball.nowUserTag == "Player2" && hitFlg == true && swingFlg == true)
        {
            CharaStatus.Rad = (float)Shot.GetRadian;          //���W�A���l
            CharaStatus.Distance = (float)Shot.GetDistance;   //����

            CharaStatus.Distance *=2;

            //�U��
            Base.Swing(CharaStatus.CharaPower, Shot.GetPower);

            //�����ړ��t���O������
            autoFlg = true;

            //���P�b�g�Ƃ�Hit�t���O�������瑤�ŃI�t(�����瑤�����Ŋ����������炱�����̃t���O���ƈႢ��������������)
            hitFlg = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // ���̂��g���K�[�ɐڐG���Ƃ��A�P�x�����Ă΂��

        //�v���C���[���̃��P�b�g�Ɠ���������
        if (collision.name == "Ball")
        {
            hitFlg = true;
        }
    }
}