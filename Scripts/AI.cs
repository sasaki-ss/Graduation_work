using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
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

    int   motionCnt = 0;        //���[�V�����Ǘ��p�̃J�E���g�ϐ�
    int   serveCnt  = 0;        //�T�[�u���̂�����Ƃ����ҋ@���ԕϐ�
    int   miss      = 0;        //����̐��l�Ȃ�ړ������Ȃ��ϐ�
    float dis       = 0;        //�����𑪂�ׂ̕ϐ�
    bool  hitFlg    = false;    //���P�b�g�ɓ��������Ƃ���t���O
    bool  onceFlg   = true;     //�X�C���O��������񂾂��s�����߂̃t���O

    void Start()
    {
        //�ǂݍ���
        Base        = GameObject.Find("Player2").GetComponent<Base>();
        animator    = GameObject.Find("Player2").GetComponent<Animator>();
        CharaStatus = GameObject.Find("Player2").GetComponent<CharaStatus>();
        player      = GameObject.Find("Player2").GetComponent<Transform>();
        ball        = GameObject.Find("Ball").GetComponent<Ball>();
        net         = GameObject.Find("Net");
        pointB      = GameObject.Find("pointB");
        score       = GameObject.Find("Score").GetComponent<Score>();
        Shot        = GameObject.Find("Shot").GetComponent<Shot>();

        //�������T�[�u�̎�
        if (ball.nowUserTag == User.User2)
        {
            if (score.user2Score % 2 == 0)
            {
                //�
                //����ƑΊp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //����
                //����ƑΊp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }
        else
        {
            //if���ł��������T�[�u�Ȃ̂����肵�Ă���
            if (score.user1Score % 2 == 0)
            {
                //�
                //����ƑΊp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //����
                //����ƑΊp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }
    }

    public void Init()
    {
        //����̈ړ��w��n���폜
        GetComponent<NavMeshAgent>().ResetPath();

        //�ǂݍ���
        Base        = GameObject.Find("Player2").GetComponent<Base>();
        animator    = GameObject.Find("Player2").GetComponent<Animator>();
        CharaStatus = GameObject.Find("Player2").GetComponent<CharaStatus>();
        player      = GameObject.Find("Player2").GetComponent<Transform>();
        ball        = GameObject.Find("Ball").GetComponent<Ball>();
        net         = GameObject.Find("Net");
        pointB      = GameObject.Find("pointB");
        score       = GameObject.Find("Score").GetComponent<Score>();
        Shot        = GameObject.Find("Shot").GetComponent<Shot>();

        //���l�̃��Z�b�g
        this.CharaStatus.Rad      = 0;
        this.CharaStatus.Distance = 0;
        motionCnt = 0;
        serveCnt  = 0;
        miss      = 0;
        dis       = 0;
        hitFlg    = false;
        onceFlg   = true;

        //�U�郂�[�V������false��
        this.animator.SetBool("is_RightShake", false);

        //�������T�[�u�̎�
        if (GameManager.instance.serveUser == User.User2) 
        {
            if (score.user2Score % 2 == 0)
            {
                //�
                //����ƑΊp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //����
                //����ƑΊp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }
        //�T�[�u����Ȃ����Ȃ�
        else
        {
            if (score.user1Score % 2 == 0)
            {
                //�
                //����ƑΊp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //����
                //����ƑΊp����ɔz�u����\��
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }

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

        //�U�郂�[�V�����łȂ����̓t���O���I���ɂ��Ă���
        if (animator.GetBool("is_RightShake") == false)
        {
            onceFlg = true;
        }
    }

    void AutoMove()
    {
        //�I�[�g�ړ�����

        //x-7�`x119��AI�R�[�g�̓���
        //z55�`z-55��AI�R�[�g�̓���

        int patternX = 0;
        int patternZ = 0;

        //X�̏ꍇ
        if (pointB.transform.position.x < -7 && pointB.transform.position.x >= -30)
        {
            //Debug.Log("-7�`-30");
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

        //�����̐ݒ�(�ݒ肵�����l���o���AI�͓����Ȃ�)
        miss = Random.Range(1, 20);

        //�ŏ��̂�
        if (ball.boundCount != 0) 
        {
            if (pointB.transform.position.x >= -62 && pointB.transform.position.x <= 0)
            {
                if (miss != 13 && ball.transform.position.x <= 60 && patternX != 0 && patternZ != 0)
                {
                    //�ړ�������
                    GetComponent<NavMeshAgent>().destination = xyz;
                }
            }
        }
        //�ʏ�
        else
        {
            if (ball.boundCount != 0 && GameManager.instance.gameState != GameState.Serve && 
                miss != 13 && ball.transform.position.x <= 60 && patternX != 0 && patternZ != 0)
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
        //�����𑪂�(pointB�Ƃ�)
        dis = Vector3.Distance(this.transform.position, pointB.transform.position);

        //�����炪�T�[�u���鑤�ł͂Ȃ��Ƃ�
        if (GameManager.instance.isServe != true)
        {
            //��a���̂Ȃ��͈͂ɂ�����
            if (ball.boundCount != 0 && dis <= 50)
            {
                //�X�C���OAnimation�ɂ���\��
                //�X�C���O�t���O(���[�V�����t���O�����˂Ă�t���O���I��)
                animator.SetBool("is_RightShake", true);

                //�~�̑傫���𑪂�
                CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                //�v���C���[��Ԃ�U��ɕύX
                CharaStatus.NowState = 2;

                //�v���C���[�̃X�^�~�i�����炷
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.005f;
            }
        }
        //�������T�[�u�̎�
        else
        if (GameManager.instance.serveUser == User.User2)
        {
            //�Ƃ肠�����Ԋu��������
            serveCnt++;

            //Debug.Log(cnt);
            if (serveCnt > 200)
            {
                //Debug.Log("AI�T�[�u");

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

                    //�K���ɐݒ�(�p�x�Ƌ���)
                    this.CharaStatus.Rad = 1.8f;        //���W�A���l
                    this.CharaStatus.Distance = 600;    //����
                }
                //�E��
                else
                {
                    Debug.Log("�E������");

                    //�K���ɐݒ�(�p�x�Ƌ���)
                    this.CharaStatus.Rad = 0.9f;        //���W�A���l
                    this.CharaStatus.Distance = 600;    //����
                }
            }
        }

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

        //�X�C���O��ԂŃ{�[���Ɠ���������
        if (animator.GetBool("is_RightShake") == true && hitFlg == true && onceFlg == true)
        {
            Vector2 parameter;

            //�K���ɐݒ�(��ԋ����Ƒ؋󎞊�)
            float power = 1.5f, taptime = 50;

            //�T�[�u���Ȃ�
            if (GameManager.instance.serveUser == User.User2 && GameManager.instance.isServe == true)
            {
                //�p�x�Ƌ����͏������ւ�Őݒ肳��Ă�
                //�U��
                Base.Swing(CharaStatus.CharaPower, power, taptime, User.User2);
            }
            else
            {
                //�K���ɐݒ�(�p�x�Ƌ���)
                parameter = TargetPoint();
                CharaStatus.Rad = parameter.x;
                CharaStatus.Distance = parameter.y;

                //�U��
                Base.Swing(CharaStatus.CharaPower, power, taptime, User.User2);
            }

            //���̏�����x�������s�����邽�߂Ƀt���O�����܂���
            onceFlg = false;

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

    private void OnTriggerExit(Collider collision)
    {
        // ���̂��g���K�[�ɗ��ꂽ�Ƃ��A�P�x�����Ă΂��

        //�v���C���[���̃��P�b�g�Ɨ��ꂽ�Ƃ�
        if (collision.name == "Ball")
        {
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
