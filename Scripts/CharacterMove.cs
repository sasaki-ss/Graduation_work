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
    [SerializeField] Judgement judgement;
    [SerializeField] public Shot Shot;
    [SerializeField] GameObject net;
    [SerializeField] Ball ball;

    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

     int motionCnt = 0;
    bool autoFlg   = false;
    bool swingFlg  = false;

    void Start()
    {
        //���P�b�g�̎擾
        judgement = GameObject.Find("PlayerRacket").GetComponent<Judgement>();
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    public void Init()
    {
        player.transform.position = new Vector3(105, 0, 0);
        motionCnt = 0;
        autoFlg = false;
        swingFlg = false;
        //if���ł��������T�[�u�Ȃ̂����肵�Ă���
        /*
        if()
        {
           //�Ίp����ɔz�u����\��
           player.transform.position = new Vector3(-105,0,0);
        }
        */

        //���P�b�g�̎擾
        judgement = GameObject.Find("PlayerRacket").GetComponent<Judgement>();
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    void Update()
    {
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
            //��_�Ԃ̋����𑪂�
            float dis = Vector3.Distance(GetComponent<NavMeshAgent>().transform.position, ball.transform.position);

            //������x�܂ŋ߂Â�����
            if (dis >= 50 && autoFlg == true)
            {
                Vector3 xyz = new Vector3(ball.transform.position.x + 60, ball.transform.position.y, ball.transform.position.z);

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

            if (motionCnt > 50)
            {
                swingFlg = true;
            }

            if (motionCnt > 60)
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
        if (ball.nowUserTag == "Player2" && judgement.HitFlg == true && swingFlg == true)
        {
            //�U��
            Base.Swing(CharaStatus.CharaPower, Shot.GetPower);

            //�����ړ��t���O������
            autoFlg = true;

            //���P�b�g�Ƃ�Hit�t���O�������瑤�ŃI�t(�����瑤�����Ŋ����������炱�����̃t���O���ƈႢ��������������)
            judgement.HitFlg = false;
        }
    }
}