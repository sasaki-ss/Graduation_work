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

    //��������Q�[���I�u�W�F�N�g
    public Ball ball;

    //��������Q�[���I�u�W�F�N�g
    public GameObject racket;


    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

    int motionCnt = 0;
    bool autoFlg = false;

    bool swingFlg = false;

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
        //���P�b�g�̎擾
        judgement = GameObject.Find("PlayerRacket").GetComponent<Judgement>();
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    void Update()
    {
        //����r�� �E�U��A���U��A�{���[(�R�����g�A�E�g)
        {
            /*
            //�E�U��
            if (Input.GetKey("right"))
            {
                animator.SetBool("is_RightShake", true);
            }

            //100�J�E���g��ɑҋ@���[�V������
            if (animator.GetBool("is_RightShake") == true)
            {
                motionCnt++;

                if (motionCnt > 100)
                {
                    motionCnt = 0;
                    //�v���C���[��ҋ@���[�V�����ɂ���
                    animator.SetBool("is_RightShake", false);


                }
            }

            //���U��
            if (Input.GetKey("left"))
            {
                animator.SetBool("is_LeftShake", true);
            }

            //100�J�E���g��ɑҋ@���[�V������
            if (animator.GetBool("is_LeftShake") == true)
            {
                motionCnt++;

                if (motionCnt > 100)
                {
                    motionCnt = 0;
                    //�v���C���[��ҋ@���[�V�����ɂ���
                    animator.SetBool("is_LeftShake", false);
                }
            }

            //�{���[
            if (Input.GetKey("down"))
            {
                animator.SetBool("is_Volley", true);
            }

            //100�J�E���g��ɑҋ@���[�V������
            if (animator.GetBool("is_Volley") == true)
            {
                motionCnt++;

                if (motionCnt > 100)
                {
                    motionCnt = 0;
                    //�v���C���[��ҋ@���[�V�����ɂ���
                    animator.SetBool("is_Volley", false);
                }
            }
            */
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

        //�N���b�N
        if (Base.touch_state._touch_flag == true && Base.touch_state._touch_phase == TouchPhase.Ended)
        {
            //����̈ړ��w��n���폜
            GetComponent<NavMeshAgent>().ResetPath();

            //�N���b�N���Ԃɂ���ď����𕪂���
            if (Shot.GetTapTime <= 20) 
            {
                //�ړ��̏���
                Vector3 xyz = Base.Move(Input.mousePosition, hit);

                //�l�b�g�z���Ȃ��悤��
                if(xyz.x>=20)
                {
                    GetComponent<NavMeshAgent>().destination = Base.Move(Input.mousePosition, hit);
                }
            }
            //������
            else
            {
                //racket.transform.position = new Vector3(player.position.x - 5, player.position.y +1, player.position.z);

                //�X�C���OAnimation�ɂ���\��
                animator.SetBool("is_RightShake", true);

                //�~�̑傫���𑪂�
                CharaStatus.CharaCircle = Base.CircleScale(Shot.GetDistance);

                //�v���C���[��Ԃ�U��ɕύX
                CharaStatus.RacketSwing = 2;
            }
        }

        //�I�[�g�ړ�����
        if (ball.nowUserTag == "Player2" && ball.transform.position.x >= 7) 
        {
            //��_�Ԃ̋����𑪂�
            float dis = Vector3.Distance(GetComponent<NavMeshAgent>().transform.position, ball.transform.position);

            //������x�܂ŋ߂Â�����
            if (dis >= 50 && autoFlg == true) 
            {
                Vector3 xyz = new Vector3(ball.transform.position.x+60, ball.transform.position.y, ball.transform.position.z);

                //�ړ�������
                GetComponent<NavMeshAgent>().destination = xyz;

                //�����ړ��͈��̂�
                autoFlg = false;

            }
        }

        //�ړ������ǂ���
        if (Base.PositionJudge(player.position, nowPosition))
        {
            //�v���C���[�𑖂郂�[�V�����ɂ���
            animator.SetBool("is_Run", true);

            //�v���C���[��Ԃ��ړ��ɕύX
            CharaStatus.RacketSwing = 1;

            //�v���C���[�̃X�^�~�i�����炷
            CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
        }
        else
        {
            //�v���C���[��ҋ@���[�V�����ɂ���
            animator.SetBool("is_Run", false);

            //�v���C���[��Ԃ�ҋ@�ɕύX
            CharaStatus.RacketSwing = 0;
        }

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

                //racket.transform.position = new Vector3(0, -100, 0);

                //�v���C���[��Ԃ�ҋ@�ɕύX
                CharaStatus.RacketSwing = 0;

                //�v���C���[��ҋ@���[�V�����ɂ���
                animator.SetBool("is_RightShake", false);
            }
        }

        if (animator.GetBool("is_RightShake") == false)
        {
            swingFlg = false;
        }

            //�U�������P�b�g������������
            if (ball.nowUserTag == "Player2" && judgement.HitFlg == true && swingFlg == true) 
        {
           // Debug.Log("ssssssssssss");
            //�U��
            Base.Swing(CharaStatus.CharaPower, Shot.GetPower);

            //�����ړ��t���O������
            autoFlg = true;
            
            //���P�b�g�Ƃ�Hit�t���O�������瑤�ŃI�t(�����瑤�����Ŋ����������炱�����̃t���O���ƈႢ��������������)
            judgement.HitFlg = false;
        }

        //���݂̍��W���擾
        nowPosition = player.position;
    }
}