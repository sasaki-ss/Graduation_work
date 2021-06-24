using TapStateManager;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Touch;
public class CharacterMove : MonoBehaviour
{
    //�O���̂�̓ǂݍ��ݗp
    [SerializeField] Base Base;
    [SerializeField] Animator animator;
    [SerializeField] CharaStatus CharaStatus;
    [SerializeField] Transform player;
    [SerializeField] Ball ball;
    [SerializeField] GameObject net;
    [SerializeField] GameObject pointB;
    [SerializeField] Score score;
    [SerializeField] public Shot Shot;

    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

    //

    int   motionCnt    = 0;        //���[�V�����Ǘ��p�̃J�E���g�ϐ�
    int   serveMoveCnt = 0;        //�T�[�u��������Ɏ����ړ��������Ȃ����߂̃J�E���g
    float dis          = 0;        //�����𑪂�ׂ̕ϐ�
    bool  hitFlg       = false;    //���P�b�g�ɓ��������Ƃ���t���O
    bool  onceFlg      = true;     //�X�C���O��������񂾂��s�����߂̃t���O
    bool  serveMoveFlg = true;     //�T�[�u��������Ɏ����ړ��������Ȃ����߂̃t���O

    //

    void Start()
    {
        //�ǂݍ���
        Base        = GameObject.Find("Player").GetComponent<Base>();
        animator    = GameObject.Find("Player").GetComponent<Animator>();
        CharaStatus = GameObject.Find("Player").GetComponent<CharaStatus>();
        player      = GameObject.Find("Player").GetComponent<Transform>();
        ball        = GameObject.Find("Ball").GetComponent<Ball>();
        net         = GameObject.Find("Net");
        pointB      = GameObject.Find("PointB");
        score       = GameObject.Find("Score").GetComponent<Score>();
        Shot        = GameObject.Find("Shot").GetComponent<Shot>();

        //�������T�[�u�̎�
        if (GameManager.instance.serveUser == User.User1) 
        {
            if (score.user1Score % 2 == 0)
            {
                //�
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(125, 0, -25);
            }
            else
            {
                //����
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(125, 0, 25);
            }
        }
        else
        {
            if (score.user2Score % 2 == 0)
            {
                //�
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(125, 0, -25);
            }
            else
            {
                //����
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(125, 0, 25);
            }
        }
    }

    public void Init()
    {
        //����̈ړ��w��n���폜
        GetComponent<NavMeshAgent>().ResetPath();

        //�ǂݍ���
        Base        = GameObject.Find("Player").GetComponent<Base>();
        animator    = GameObject.Find("Player").GetComponent<Animator>();
        CharaStatus = GameObject.Find("Player").GetComponent<CharaStatus>();
        player      = GameObject.Find("Player").GetComponent<Transform>();
        ball        = GameObject.Find("Ball").GetComponent<Ball>();
        net         = GameObject.Find("Net");
        pointB      = GameObject.Find("PointB");
        score       = GameObject.Find("Score").GetComponent<Score>();
        Shot        = GameObject.Find("Shot").GetComponent<Shot>();

        //���l�̃��Z�b�g
        this.CharaStatus.Rad = 0;
        this.CharaStatus.Distance = 0;
        player.transform.position = new Vector3(125, 0, 0);
        dis = 0;
        motionCnt = 0;
        serveMoveCnt = 0;
        onceFlg = true;
        hitFlg = false;
        serveMoveFlg = true;
        //�U�郂�[�V������false��
        this.animator.SetBool("is_RightShake", false);

        //�������T�[�u�̎�
        if (GameManager.instance.serveUser == User.User1)
        {
            if (score.user1Score % 2 == 0)
            {
                //�
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(125, 0, -25);
            }
            else
            {
                //����
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(125, 0, 25);
            }
        }
        //�T�[�u����Ȃ����Ȃ�
        else
        {
            if (score.user2Score % 2 == 0)
            {
                //����
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(125, 0, -25);
            }
            else
            {
                //�
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(125, 0, 25);
            }
        }

        //Debug.Log("Player��Init�����̎��s");
    }

    void Update()
    {
        //�L�����ƃ{�[���̋����𑪂�
        dis = Vector3.Distance(this.transform.position, ball.transform.position);

        //Debug.Log(GameManager.instance.serveUser);
        //Debug.Log(ball.nowUserTag);

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
        //�����炪�T�[�u���鑤�ł͂Ȃ��Ƃ�
        if (GameManager.instance.isServe != true) 
        {
            serveMoveFlg = false;
            serveMoveCnt = 300;

            //�N���b�N
            if (Base.touch_state._touch_flag == true && Base.touch_state._touch_phase == TouchPhase.Ended)
            {
                //Debug.Log("�T�[�u���ł͂Ȃ��s��");

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
                //������(�X�C���O���s�����Ƃ���)
                else
                {
                    //�X�C���OAnimation�ɂ���\��
                    //�X�C���O�t���O(���[�V�����t���O�����˂Ă�t���O���I��)
                    animator.SetBool("is_RightShake", true);

                    //�~�̑傫���𑪂�
                    CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                    //�v���C���[��Ԃ�U��ɕύX
                    CharaStatus.NowState = 2;
                }
            }
        }
        //���ړ��̂�
        //�������T�[�u�̎�
        else
        if (GameManager.instance.serveUser == User.User1)
        {
            //�N���b�N
            if (Base.touch_state._touch_flag == true && Base.touch_state._touch_phase == TouchPhase.Ended)
            {
                //�N���b�N���Ԃɂ���ď����𕪂���
                if (Shot.GetTapTime <= 10)
                {
                    //Debug.Log("�T�[�u���̉��ړ�");

                    //�ړ��̏���
                    Vector3 xyz = Base.Move(Input.mousePosition, hit);

                    xyz.x = 125;

                    //�v���C���[���_���炾�Ǝ��R�[�g�̉E��
                    if (this.transform.position.z >= 0)
                    {
                        if (xyz.z <= 0)
                        {
                            xyz.z = 3;
                        }
                        else
                        if (xyz.z > 45)
                        {
                            xyz.z = 42;
                        }
                    }
                    //�v���C���[���_���炾�Ǝ��R�[�g�̍���
                    else
                    {
                        if (xyz.z > 0)
                        {
                            xyz.z = -3;
                        }
                        else
                        if (xyz.z < -45)
                        {
                            xyz.z = -42;
                        }
                    }

                    GetComponent<NavMeshAgent>().destination = xyz;
                }
                else
                if (GameManager.instance.serveUser == User.User1)
                {
                    //�X�C���OAnimation�ɂ���\��
                    animator.SetBool("is_RightShake", true);

                    //�~�̑傫���𑪂�
                    CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                    //�v���C���[��Ԃ�U��ɕύX
                    CharaStatus.NowState = 2;

                    serveMoveFlg = false;
                }
            }
        }
    }

    void AutoMove()
    {
        //

        //Debug.Log(serveMoveCnt);

        if (serveMoveFlg == false)
        {
            serveMoveCnt++;
        }

        //

        //�I�[�g�ړ�����
        if (GameManager.instance.isServe != true && ball.nowUserTag == User.User2 &&
            ball.transform.position.x > -5 && serveMoveCnt >= 300) 
        {
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

            if (xyz.x != 0 && xyz.z != 0)
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
            CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.005f;
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

            //�Ώۂ���������
            player.rotation = Quaternion.Slerp(this.transform.rotation, rotation, speed);
        }

        //�U�郂�[�V�����łȂ����̓t���O���I���ɂ��Ă���
        if (animator.GetBool("is_RightShake") == false)
        {
            onceFlg = true;
        }
    }

    void Swing()
    {
        //�U���Ԏ��Ȃ�50�J�E���g��ɑҋ@��Ԃɖ߂�
        if (animator.GetBool("is_RightShake") == true)
        {
            motionCnt++;

            if (motionCnt > 40)
            {
                motionCnt = 0;

                //�v���C���[��Ԃ�ҋ@�ɕύX
                CharaStatus.NowState = 0;

                //�v���C���[��ҋ@���[�V�����ɂ���
                animator.SetBool("is_RightShake", false);

                //�v���C���[�̃X�^�~�i�����炷
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.005f;
            }
        }

        //�X�C���O��ԂŃ{�[���Ɠ���������A�㋗������a���̂Ȃ��͈͂�
        if (animator.GetBool("is_RightShake") == true && hitFlg == true && onceFlg == true &&
            dis <= 20 && this.transform.position.x > ball.transform.position.x               )
        {

            CharaStatus.Rad = (float)Shot.GetRadian;          //���W�A���l
            CharaStatus.Distance = (float)Shot.GetDistance;   //����

            //Debug.Log(dis);
            //Debug.Log(CharaStatus.Rad);
            //Debug.Log(CharaStatus.Distance);

            //�U��
            Base.Swing(CharaStatus.CharaPower, Shot.GetPower, Shot.GetTapTime, User.User1);

            //���̏�����x�������s�����邽�߂Ƀt���O�����܂���
            onceFlg = false;

            //���P�b�g�Ƃ�Hit�t���O�������瑤�ŃI�t(�����瑤�����Ŋ����������炱�����̃t���O���ƈႢ��������������)
            hitFlg = false;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        // ���̂��g���K�[�ɐڐG���Ƃ��A�P�x�����Ă΂��

        //�v���C���[���̃��P�b�g�Ɠ���������
        if (collision.name == "Ball")
        {
            hitFlg = true;
        }
    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.name == "Ball")
        {
            //Debug.Log("�������Ă���");
        }
    }
}