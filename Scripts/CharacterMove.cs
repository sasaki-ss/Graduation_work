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


    //��������Q�[���I�u�W�F�N�g
    public GameObject Ball;

    //��������Q�[���I�u�W�F�N�g
    public GameObject racket;


    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

    int a = 0;

    void Start()
    {
        //���P�b�g�̎擾
        judgement = GameObject.Find("Cube").GetComponent<Judgement>();
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
                a++;

                if (a > 100)
                {
                    a = 0;
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
                a++;

                if (a > 100)
                {
                    a = 0;
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
                a++;

                if (a > 100)
                {
                    a = 0;
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
            if (Base.Shot.GetTapTime <= 10) 
            {
                //�ړ��̏���
                GetComponent<NavMeshAgent>().destination = Base.Move(Input.mousePosition, hit);
            }
            //������
            else
            {
                racket.transform.position =new Vector3(player.position.x-5,player.position.y, player.position.z);

                //�X�C���OAnimation�ɂ���\��
                animator.SetBool("is_RightShake", true);

                //�~�̑傫���𑪂�
                CharaStatus.CharaCircle = Base.CircleScale();

                //�v���C���[��Ԃ�U��ɕύX
                CharaStatus.RacketSwing = 2;
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

        //�I�[�g�ړ�����(�R�����g�A�E�g)
        {
            /*
            if (�v���C���[�ɑ΂��Č������Ă��Ă����Ԃ̒e�Ȃ�)
            {
                AutoMove();
            }
            */
        }

        //�U���Ԏ��Ȃ�50�J�E���g��ɑҋ@��Ԃɖ߂�
        if (animator.GetBool("is_RightShake") == true)
        {
            a++;

            if (a > 50)
            {
                a = 0;

                racket.transform.position = new Vector3(0, -100, 0);

                //�v���C���[��Ԃ�ҋ@�ɕύX
                CharaStatus.RacketSwing = 0;

                //�v���C���[��ҋ@���[�V�����ɂ���
                animator.SetBool("is_RightShake", false);
            }
        }

        //�U�������P�b�g������������
        if (judgement.HitFlg == true && CharaStatus.RacketSwing == 0) 
        {
            //�U��
            Base.Swing(CharaStatus.CharaPower);
        }

        //���݂̍��W���擾
        nowPosition = player.position;
    }
}