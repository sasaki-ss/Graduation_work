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
        if (ball.nowUserTag == "Player2")
        {
            // Debug.Log("x:"+pointB.transform.position.x+ "y:" + pointB.transform.position.y+ "z:" + pointB.transform.position.z );

            //x7�`x119���R�[�g�̓���
            //z55�`z-55���R�[�g�̓���

            int patternX = 0;
            int patternZ = 0;

            //X�̏ꍇ
            if (pointB.transform.position.x > 7 && pointB.transform.position.x <= 30)
            {
                // Debug.Log("7�`30");
                patternX = 1;
            }
            else
            if (pointB.transform.position.x > 30 && pointB.transform.position.x <= 60)
            {
                //Debug.Log("30�`60");
                patternX = 2;
            }
            else
            if (pointB.transform.position.x > 60 && pointB.transform.position.x <= 90)
            {
                //Debug.Log("60�`90");
                patternX = 3;
            }
            else
            if (pointB.transform.position.x > 90 && pointB.transform.position.x <= 119)
            {
                //Debug.Log("90�`119");
                patternX = 4;
            }
            else
            {
                //Debug.Log("119�`or7>x");
                patternX = 0;
            }

            //Z�̏ꍇ
            if (pointB.transform.position.z > -55 && pointB.transform.position.z <= -27.5)
            {
                //Debug.Log("-55�`-27.5");
                patternZ = 1;
            }
            else
            if (pointB.transform.position.z > -27.5 && pointB.transform.position.z <= 0)
            {
                //Debug.Log("-27.5�`0");
                patternZ = 2;
            }
            else
            if (pointB.transform.position.z > 0 && pointB.transform.position.z <= 27.5)
            {
                //Debug.Log("0�`27.5");
                patternZ = 3;
            }
            else
            if (pointB.transform.position.z > 27.5 && pointB.transform.position.z <= 55)
            {
                //Debug.Log("27.5�`55");
                patternZ = 4;
            }
            else
            {
                //Debug.Log("55�`or-55�`");
                patternZ = 0;
            }

            //Debug.Log(patternX +":::"+ patternZ);

            Vector3 xyz = new Vector3(0, 0, 0);

            //�ꏊ�ɉ����Ĉړ�(X���W)
            switch (patternX)
            {
                case 1:
                    xyz.x = 40;
                    break;
                case 2:
                    xyz.x = 70;
                    break;
                case 3:
                    xyz.x = 100;
                    break;
                case 4:
                    xyz.x = 130;
                    break;
                default:
                    break;
            }

            //�ꏊ�ɉ����Ĉړ�(Z���W)
            switch (patternZ)
            {
                case 1:
                    xyz.z = -40;
                    break;
                case 2:
                    xyz.z = -13;
                    break;
                case 3:
                    xyz.z = 13;
                    break;
                case 4:
                    xyz.z = 40;
                    break;
                default:
                    break;
            }

            if(xyz.x!=0&& xyz.z!=0)
            {
                //�ړ�������
                GetComponent<NavMeshAgent>().destination = xyz;
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