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
    [SerializeField] Judge judge;

    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

    //

    int   cnt    = 0;        //���[�V�����Ǘ��p�̃J�E���g�ϐ�
    int   serveMoveCnt = 0;        //�T�[�u��������Ɏ����ړ��������Ȃ����߂̃J�E���g
    float dis          = 0;        //�����𑪂�ׂ̕ϐ�
    bool swingFlg = false;
    bool  hitFlg       = false;    //���P�b�g�ɓ��������Ƃ���t���O
    bool  onceFlg      = false;     //�X�C���O��������񂾂��s�����߂̃t���O
    bool  serveMoveFlg = true;     //�T�[�u��������Ɏ����ړ��������Ȃ����߂̃t���O
    bool autoFlg = false;
    bool aniHit = false;
    bool stockFlg = false;
    bool tapFlg = false;


    float screenAspect; //��ʂ̃A�X�y�N�g��
    float targetAspect = (float)1080; //�ړI�̃A�X�y�N�g��
    float magRate;
    float a;
    //

    void Start()
    {

        screenAspect = (float)Screen.width;
        magRate = (float)screenAspect/ (float)targetAspect; //�ړI�A�X�y�N�g��ɂ��邽�߂̔{��

        Debug.Log(magRate);
        //a = magRate - 1; //�ړI�A�X�y�N�g��ɂ��邽�߂̔{��

        //�ǂݍ���
        Base        = GameObject.Find("Player").GetComponent<Base>();
        animator    = GameObject.Find("Player").GetComponent<Animator>();
        CharaStatus = GameObject.Find("Player").GetComponent<CharaStatus>();
        player      = GameObject.Find("Player").GetComponent<Transform>();
        ball        = GameObject.Find("Ball").GetComponent<Ball>();
        net         = GameObject.Find("Net");
        score       = GameObject.Find("Score").GetComponent<Score>();
        Shot        = GameObject.Find("Shot").GetComponent<Shot>();
        judge       = GameObject.Find("PlayerRacket").GetComponent<Judge>();
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
        score       = GameObject.Find("Score").GetComponent<Score>();
        Shot        = GameObject.Find("Shot").GetComponent<Shot>();
        judge = GameObject.Find("PlayerRacket").GetComponent<Judge>();

        //���l�̃��Z�b�g
        this.CharaStatus.Rad = 0;
        this.CharaStatus.Distance = 0;
        player.transform.position = new Vector3(125, 0, 0);
        dis = 0;
        cnt = 0;
        serveMoveCnt = 0;
        onceFlg = false;
        hitFlg = false;
        serveMoveFlg = true;
        swingFlg = false;
        autoFlg = false;
        aniHit = false;
        stockFlg = false;
        tapFlg = false;
        //�v���C���[�̃X�^�~�i����
        this.CharaStatus.CharaStamina = CharaStatus.CharaStamina + 0.5f;


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
        if(pointB == null && pointB.activeSelf == true)
        {
            pointB = GameObject.Find("PointB");
        }

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

        if (GameManager.instance.gameState == GameState.GameSet)
        {
            //�X�^�~�i����
            this.CharaStatus.CharaStamina = 10;
        }
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
                if (Shot.GetDistance < 50) 
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
                    tapFlg = true;
                    if (autoFlg == true)
                    {
                        stockFlg = true;
                    }
                    else
                    {
                        //Serve�A�j���[�V�����̎��s
                        animator.Play("Swing", 0, 0f);
                        swingFlg = true;
                        //�~�̑傫���𑪂�
                        CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                        //�v���C���[��Ԃ�U��ɕύX
                        CharaStatus.NowState = 2;
                    }
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
                //�N���b�N�̒����ɂ���ď����𕪂���
                if (Shot.GetDistance < 50)
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
                    tapFlg = true;
                    swingFlg = true;
                    //�~�̑傫���𑪂�
                    CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                    //�v���C���[��Ԃ�U��ɕύX
                    CharaStatus.NowState = 2;

                    serveMoveFlg = false;

                    animator.Play("Serve", 0, 0f);
                }
            }
        }

        if (autoFlg == false && tapFlg == true && stockFlg == true &&  player.transform.position.x - ball.transform.position.x  < 15) 
        {
            //Serve�A�j���[�V�����̎��s
            animator.Play("Swing", 0, 0f);
            swingFlg = true;
            //�~�̑傫���𑪂�
            CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

            //�v���C���[��Ԃ�U��ɕύX
            CharaStatus.NowState = 2;

            stockFlg = false;
            tapFlg = true;
        }

    }

    void AutoMove()
    {
        if (hitFlg == true)
        {
            //����̈ړ��w��n���폜
            GetComponent<NavMeshAgent>().ResetPath();
            autoFlg = false;
        }

        //Debug.Log(serveMoveCnt);

        if (serveMoveFlg == false)
        {
            serveMoveCnt++;
        }

        

        //�I�[�g�ړ�����
        if (GameManager.instance.isServe != true && ball.nowUserTag == User.User2 && serveMoveCnt >= 0 && pointB.activeSelf == true && autoFlg == false)
        {
            autoFlg = true;
            //x7�`x119���R�[�g�̓���
            //z55�`z-55���R�[�g�̓���

            Vector3 xyz = new Vector3(pointB.transform.position.x+15, 0, pointB.transform.position.z - 10);

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
            CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.00005f;
        }
        else
        {
            //�v���C���[��ҋ@���[�V�����ɂ���
            animator.SetBool("is_Run", false);

            //�v���C���[��Ԃ�ҋ@�ɕύX
            CharaStatus.NowState = 0;

            autoFlg = false;
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
    }

    void Swing()
    {

        if (judge.Hit == true)
        {
            hitFlg = true;
        }
        else
        {
            hitFlg = false;
            cnt = 0;
        }

        //�X�C���O��ԂŃ{�[���Ɠ���������
        if (swingFlg == true && hitFlg == true && onceFlg == false)
        {
            a = magRate - 1;
            if (cnt == 0)
            {

                if (a > 0) 
                {
                    CharaStatus.Rad = (float)Shot.GetRadian;          //���W�A���l
                    CharaStatus.Distance = (float)Shot.GetDistance / (1+a);   //����
                }
                else
                {
                    a=(1+Mathf.Abs(a));
                    CharaStatus.Rad = (float)Shot.GetRadian;          //���W�A���l
                    CharaStatus.Distance = (float)Shot.GetDistance * (a);   //����
                }

                CharaStatus.CharaPower = CharaStatus.CharaPower + Shot.GetDistance / 700;
                //Debug.Log(Shot.GetDistance / 1250);
               // Debug.Log(CharaStatus.CharaPower);
                //�U��
                Base.Swing(CharaStatus.CharaPower, Shot.GetPower, Shot.GetTapTime, User.User1);
            }

            cnt++;

            //���̏�����x�������s�����邽�߂Ƀt���O�����܂���
            onceFlg = true;

            //���P�b�g�Ƃ�Hit�t���O�������瑤�ŃI�t(�����瑤�����Ŋ����������炱�����̃t���O���ƈႢ��������������)
            hitFlg = false;

            tapFlg = false;
        }
    }

    //���[�V�����Đ����A����̃^�C�~���O�ŌĂԊ֐�

    void SwingStart()
    {
        swingFlg = true;
        cnt = 1;
    }

    void SwingEnd()
    {
        onceFlg = false;
        swingFlg = false;
        cnt = 0;
    }

    void Call()
    {
        //Serve�A�j���[�V�����̎��s
        animator.speed =1.7f;
    }

    void A()
    {
        animator.speed = 0.5f;
        Invoke("Call", 0.4f);
    }
}