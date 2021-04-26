using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Touch;
public class CharacterMove : MonoBehaviour
{
    //�X�e�[�^�X�擾�p
    public CharaStatus CharaStatus;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

    //�L����
    [SerializeField] Animator animator;
    [SerializeField] Transform player;

    Vector3 a;
    Vector2 touch;
    Vector3 touchPosition;

    void Start()
    {

    }

    void Update()
    {
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
            animator.SetBool("is_Volley", true);
        }
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
            MoveToCursor();
        }

        //�����W�ƑO���W�ɈႤ�ꍇ
        if (player.position != a)
        {
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
            animator.SetBool("is_Run", false);
        }
        a = player.position;
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
}