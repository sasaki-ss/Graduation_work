using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Touch;
public class CharacterMove : MonoBehaviour
{
    //�X�e�[�^�X�擾�p
    public CharaStatus CharaStatus;

    //Shot�p
    public Shot Shot;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;
    int a = 0;
    //�L����
    [SerializeField] Animator animator;
    [SerializeField] Transform player;
    [SerializeField] GameObject ball;

    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�^�b�`���Ɏg������
    Vector2 touch;
    Vector3 touchPosition;

    void Start()
    {

    }

    void Update()
    {
        /*����r�� �E�U��A���U��A�{���[
        if (Input.GetKey("right"))
        {
            animator.SetBool("is_RightShake", true);
        }
        if (Input.GetKey("left"))
        {
            animator.SetBool("is_LeftShake", true);
        }
        if (Input.GetKey("down"))
        {
           // AutoMove();
            animator.SetBool("is_Volley", true);
        }

        if(animator.GetBool("is_Volley")==true)
        {
            Debug.Log(a);
            a++;

            if(a>100)
            {
                a = 0;
                //�v���C���[�𗧂��~�܂胂�[�V�����ɂ���
                animator.SetBool("is_Volley", false);
            }
        }
        */

        /*
        //�^�b�`���̏����@�m�F�͂��ĂȂ�
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touch = Input.GetTouch(0).deltaPosition;

            touchPosition = new Vector3(touch.x, touch.y,0);
        }
        */

        //���N���b�N������
        if (Input.GetMouseButtonDown(0))
        {

            //����̈ړ��w��n���폜
            GetComponent<NavMeshAgent>().ResetPath();

            //�N���b�N���̏���
            MoveToCursor();
        }


        //���N���b�N������
        if (Input.GetMouseButtonDown(1))
        {
            //�X���C�h�̒����ɂ���ĉ~�̑傫�����ς��
            if(Shot.GetDistance  >= 0  && Shot.GetDistance < 5 )
            {
                CharaStatus.CharaCircle = 50;
            }
            else
            if (Shot.GetDistance >= 5  && Shot.GetDistance < 10)
            {
                CharaStatus.CharaCircle = 42;
            }
            else
            if (Shot.GetDistance >= 10 && Shot.GetDistance < 15)
            {
                CharaStatus.CharaCircle = 34;
            }
            else
            if (Shot.GetDistance >= 15 && Shot.GetDistance < 20)
            {
                CharaStatus.CharaCircle = 26;
            }
            else
            if (Shot.GetDistance >= 20 && Shot.GetDistance < 25)
            {
                CharaStatus.CharaCircle = 18;
            }
            else
            if (Shot.GetDistance >= 25 && Shot.GetDistance < 30)
            {
                CharaStatus.CharaCircle = 10;
            }
            else
            {
                CharaStatus.CharaCircle = 2;
            }
        }

        //�I�[�g�ړ�����
        /*if(�v���C���[�ɑ΂��Č������Ă��Ă����Ԃ̒e�Ȃ�)
        {
            AutoMove();
        }
        */

        //���W����
        PositionJudge();
       
    }

    private void MoveToCursor()
    {
        //�J�����̏ꏊ�ƃ}�E�X��Click���W����Z���W�����߂�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //�^�b�`���̏����@�m�F�͂��ĂȂ�
        //Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        bool hasHit = Physics.Raycast(ray, out hit);

        //Click�����ꏊ�܂ňړ�
        if (hasHit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }

    private void PositionJudge()
    {
        //�����W�ƑO���W�ɈႤ�ꍇ
        if (player.position != nowPosition)
        {
            //�v���C���[�𑖂郂�[�V�����ɂ���
            animator.SetBool("is_Run", true);

            //�ړ����Ȃ�X�^�~�i����(��������W�ƖړI�n�̍��W�̃Y����1�ȏ�̏ꍇ)
            if (hit.point.x - player.position.x < -1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }

            if (hit.point.x - player.position.x > 1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }

            if (hit.point.y - player.position.y < -1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }

            if (hit.point.y - player.position.y > 1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }

            if (hit.point.z - player.position.z < -1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }

            if (hit.point.z - player.position.z > 1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }
        }
        else
        {
            //�v���C���[�𗧂��~�܂胂�[�V�����ɂ���
            animator.SetBool("is_Run", false);
        }

        //���݂̍��W���擾
        nowPosition = player.position;
    }

    private void AutoMove()
    {
        //�{�[���̃I�u�W�F�N�g�̏����擾
        ball = GameObject.Find("ball");

        //�ړ�������
        GetComponent<NavMeshAgent>().destination = ball.transform.position;      
        //Debug.Log(ball.transform.position);
    }
}