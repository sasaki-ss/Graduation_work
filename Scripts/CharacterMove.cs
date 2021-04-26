using UnityEngine;
using UnityEngine.AI;

public class CharacterMove : MonoBehaviour
{
    //�X�e�[�^�X�擾�p
    public CharaStatus CharaStatus;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

    //�L����
    [SerializeField] Transform player;
    Vector3 a;
    void Update()
    {
        //���N���b�N������
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }

        //�����W�ƑO���W�ɈႤ�ꍇ
        if (player.position != a)
        {
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

        a = player.position;
    }

    private void MoveToCursor()
    {
        //�J�����̏ꏊ�ƃ}�E�X��Click���W����Z���W�����߂�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hasHit = Physics.Raycast(ray, out hit);

        //Click�����ꏊ�܂ňړ�
        if (hasHit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }
}