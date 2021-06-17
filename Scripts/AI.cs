using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    //���ʂ̂��
    [SerializeField] Base Base;

    //�e�X�̂��
    [SerializeField] Animator animator;
    [SerializeField] CharaStatus CharaStatus;
    [SerializeField] Transform player;
    [SerializeField] Ball ball;
    [SerializeField] GameObject net;
    [SerializeField] public Shot Shot;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject pointB;
    [SerializeField] Score score;

    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

    int motionCnt = 0;
    public bool swingFlg = false;
    bool hitFlg = false;
    bool resetFlg = false;
    bool start_SwingFlg = true;
    float dis = 0;
    bool boundFlg = true;
    int miss = 0;  //�ړ������Ȃ�
    int cnt = 0;
    void Start()
    {
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
        //�������T�[�u�̎�
        if (ball.nowUserTag == User.User1)
        {
            //if���ł��������T�[�u�Ȃ̂����肵�Ă���
            if (score.user2Score % 2 == 0)
            {
                //�
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //����
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }
        else
        {
            //if���ł��������T�[�u�Ȃ̂����肵�Ă���
            if (score.user1Score % 2 == 0)
            {
                //�
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //����
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }
    }

    public void Init()
    {
        //����̈ړ��w��n���폜
        GetComponent<NavMeshAgent>().ResetPath();

        this.CharaStatus.Rad = 0;
        this.CharaStatus.Distance = 0;
        player.transform.position = new Vector3(-125, 0, 0);
        motionCnt = 0;
        boundFlg = true;
        swingFlg = true;
        start_SwingFlg = false;
        resetFlg = false;
        hitFlg = false;
        dis = 0;
        miss = 0;
        cnt = 0;
        this.animator.SetBool("is_RightShake", false);

        //�������T�[�u�̎�
        if (ball.nowUserTag == User.User1)
        {
            //if���ł��������T�[�u�Ȃ̂����肵�Ă���
            if (score.user2Score % 2 == 0)
            {
                //�
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //����
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }
        else
        {
            //if���ł��������T�[�u�Ȃ̂����肵�Ă���
            if (score.user1Score % 2 == 0)
            {
                //�
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //����
                //�Ίp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }

        Shot = GameObject.Find("Shot").GetComponent<Shot>();

        Debug.Log("AI��Init�����̎��s");
    }

    void Update()
    {

        //�L�����ƃ{�[���̋����𑪂�
        dis = Vector3.Distance(this.GetComponent<NavMeshAgent>().transform.position, ball.transform.position);

        //�����ړ����̏���
        AutoMove();

        //�ړ��������肷�鏈��
        JudgeMove();

        //���P�b�g��U�������̏���
        Swing();

        //���݂̍��W���擾
        nowPosition = player.position;

        //�A�b�v�f�[�g�Œ��ڂ���Animation�̏��������̏��}�V�ȓ����ɂȂ��Ă����
        if (resetFlg == true && dis <= 150)
        {
            //�U��
            //�p�x�ɂ���Ĕ���
            //�X���C�v�̒����ɂ���Ĕ���
            //�v���C���[���猩���Ƃ�
            // 0.5 :����M��
            // 2.5 :�E��M��

            Vector2 parameter;

            parameter = TargetPoint();
            CharaStatus.Rad = parameter.x;
            CharaStatus.Distance = parameter.y;

            //�p�����[�^���傱���ƒ��ڂ������Ă�
            Debug.Log(parameter.x + ":::"+ parameter.y);
            Base.Swing(CharaStatus.CharaPower * 1.5f, Shot.GetPower + 10,0);

            start_SwingFlg = false;

            this.animator.SetBool("is_RightShake", false);
            motionCnt = 0;
            swingFlg = false;
            hitFlg = false;
            resetFlg = false;
        }

        //�~�X���Ă��J�E���g��͌��ɖ߂��Ă���
        if (motionCnt > 300)
        {
            this.animator.SetBool("is_RightShake", false);
            motionCnt = 0;
            swingFlg = false;
            hitFlg = false;
            resetFlg = false;
        }
    }

    void AutoMove()
    {
        //�I�[�g�ړ�����

        // Debug.Log("x:"+pointB.transform.position.x+ "y:" + pointB.transform.position.y+ "z:" + pointB.transform.position.z );

        //x-7�`x119��AI�R�[�g�̓���
        //z55�`z-55��AI�R�[�g�̓���

        int patternX = 0;
        int patternZ = 0;

        //X�̏ꍇ
        if (pointB.transform.position.x < -7 && pointB.transform.position.x >= -30)
        {
            // Debug.Log("-7�`-30");
            patternX = 1;
        }
        else
        if (pointB.transform.position.x < -30 && pointB.transform.position.x >= -60)
        {
            //Debug.Log("-30�`-60");
            patternX = 2;
        }
        else
        if (pointB.transform.position.x < -60 && pointB.transform.position.x >= -90)
        {
            //Debug.Log("-60�`-90");
            patternX = 3;
        }
        else
        if (pointB.transform.position.x < -90 && pointB.transform.position.x >= -119)
        {
            //Debug.Log("-90�`-119");
            patternX = 4;
        }
        else
        {
            //Debug.Log("-119�`or-7<x");
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
                xyz.x = -45;
                break;
            case 2:
                xyz.x = -75;
                break;
            case 3:
                xyz.x = -105;
                break;
            case 4:
                xyz.x = -135;
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

        //Debug.Log(ball.boundCount);

        //�����̐ݒ�(�ݒ肵�����l���o���AI�͓����Ȃ�)
        int miss = Random.Range(1, 20);

        //�ŏ��̂�
        if (ball.boundCount != 0 && start_SwingFlg == true)
        {
            if (pointB.transform.position.x >= -62 && pointB.transform.position.x <= 0)
            {
                if (miss != 13 && ball.transform.position.x <= 60 && patternX != 0 && patternZ != 0)
                {
                    //�ړ�������
                    GetComponent<NavMeshAgent>().destination = xyz;
                    swingFlg = true;
                }
            }
        }
        //�ʏ�
        else
        {
            if (ball.boundCount !=0 && gameManager.gameState != GameState.Serve && miss != 13 && ball.transform.position.x <= 60 && patternX != 0 && patternZ != 0)
            {
                //�ړ�������
                GetComponent<NavMeshAgent>().destination = xyz;
                swingFlg = true;
            }
        }

        //Debug.Log(swingFlg);
    }

    void JudgeMove()
    {
        //�ړ������ǂ���
        if (Base.PositionJudge(player.position, nowPosition))
        {
            //�v���C���[�𑖂郂�[�V�����ɂ���
            this.animator.SetBool("is_Run", true);

            //�v���C���[��Ԃ��ړ��ɕύX
            this.CharaStatus.NowState = 1;

            //�v���C���[�̃X�^�~�i�����炷
            this.CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.005f;
        }
        else
        {
            //�v���C���[��ҋ@���[�V�����ɂ���
            this.animator.SetBool("is_Run", false);

            //�v���C���[��Ԃ�ҋ@�ɕύX
            this.CharaStatus.NowState = 0;
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
        float dis = Vector3.Distance(this.transform.position, pointB.transform.position);

        //�T�[�u�t���O��true�Ȃ炵�Ȃ�
        if (GameManager.instance.isServe != true)
        {
            //��a���̂Ȃ��͈͂ɂ�����
            if (ball.boundCount!=0 && dis <= 50)
            {
                // Debug.Log("���݂̋���: " + dis);

                if (swingFlg == true && animator.GetBool("is_Run") == false)
                {
                    //�v���C���[���X�C���O���[�V�����ɂ���
                    this.animator.SetBool("is_RightShake", true);
                    //Debug.Log("�ӂ�����");
                    motionCnt++;
                    //�v���C���[�̃X�^�~�i�����炷
                    CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.005f;
                }

                if (this.animator.GetBool("is_RightShake") == true)
                {
                    if (hitFlg == true)
                    {
                        resetFlg = true;
                    }
                }
            }
        }
        else
        if (ball.nowUserTag == User.User1)
        {
            //�Ƃ肠�����Ԋu��������
            cnt++;

            if (cnt > 300)
            {
                //Debug.Log("AI�̃T�[�u");
                //�X�C���OAnimation�ɂ���\��
                animator.SetBool("is_RightShake", true);

                //�~�̑傫���𑪂�
                CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                //�v���C���[��Ԃ�U��ɕύX
                CharaStatus.NowState = 2;

                //�T�[�u�������Ȃ�
                if (this.transform.position.z <= 0)
                {
                    Debug.Log("��������");
                    this.CharaStatus.Rad = 1.3f;          //���W�A���l
                    this.CharaStatus.Distance = -200;   //����
                }
                //�E��
                else
                {
                    Debug.Log("�E������");
                    this.CharaStatus.Rad = 1.3f;          //���W�A���l
                    this.CharaStatus.Distance = -200;   //����
                }

                //Debug.Log(CharaStatus.Rad);
                //Debug.Log(CharaStatus.Distance);

                //�U��
                float a = 6;
                float b = (float)CharaStatus.CharaPower / 6;

                //Debug.Log(a);
                //Debug.Log(b);

                //�T�[�u�t���O�I�t
                GameManager.instance.isServe = false;

                //�T�[�u�֐��Ă�

                //Debug.Log(a+"aaa"+b);
                ball.Serve(a, b);
                cnt = 0;
            } 
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // ���̂��g���K�[�ɐڐG���Ƃ��A�P�x�����Ă΂��

        //�v���C���[���̃��P�b�g�Ɠ���������
        if (collision.name == "Ball")
        {
            //Debug.Log("hu");

            hitFlg = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        // ���̂��g���K�[�ɗ��ꂽ�Ƃ��A�P�x�����Ă΂��

        //�v���C���[���̃��P�b�g�Ɨ��ꂽ�Ƃ�
        if (collision.name == "Ball")
        {
            //Debug.Log("hu");

            hitFlg = false;
        }
    }

    private Vector2 TargetPoint()
    {
        float targetPointX = 1.5f;
        float targetPointY = 400;

        int mode = Random.Range(1, 4);

        //�v���C���[�̃R�[�g�̍��O��
        if (pointB.transform.position.z < -40 && pointB.transform.position.z > -0)
        {
            targetPointX = 0.9f;
        }

        //�v���C���[�̃R�[�g�̉E�O�� 
        if (pointB.transform.position.z > -9 && pointB.transform.position.z < 9)
        {
            targetPointX = 1.5f;
        }

        //�v���C���[�̃R�[�g�̉E�O�� 
        if (pointB.transform.position.z > 10 && pointB.transform.position.z < 40)
        {
            targetPointX = 2.1f;
        }


        // Debug.Log("pointB:"+ pointB.transform.position.x +":::"+ pointB.transform.position.z);
        if (mode == 2)
        {

            //�v���C���[�̃R�[�g�̑O����
            if (pointB.transform.position.x < 0 && pointB.transform.position.x > -40)
            {
                int a = Random.Range(1, 3);
                if (a != 2)
                {
                    targetPointX = 1.3f;
                }
                targetPointY = 400 * 2.5f;
            }

            //�v���C���[�̃R�[�g�̒���
            if (pointB.transform.position.x < -41 && pointB.transform.position.x > -80)
            {
                int a = Random.Range(1, 3);
                if (a != 2)
                {
                    targetPointX = 1.8f;
                }
                targetPointY = 400 * 3f;
            }

            //�v���C���[�̃R�[�g�̌㒆��
            if (pointB.transform.position.x < -81 && pointB.transform.position.x > -120)
            {
                int a = Random.Range(1, 3);
                if (a != 3)
                {
                    targetPointX = 2.2f;
                }
                targetPointY = 400 * 2.5f;
            }
        }

        if (mode == 4)
        {

            targetPointX = Random.Range(1, 25) / 10;
            targetPointY = 400 * Random.Range(10, 35) / 10;
        }
        Vector2 targetPoint = new Vector2(targetPointX, targetPointY);

        //Debug.Log("rad" + targetPointX);
        //Debug.Log("dis" + targetPointY);

        return targetPoint;
    }
}
