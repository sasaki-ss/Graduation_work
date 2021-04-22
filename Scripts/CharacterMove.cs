using UnityEngine;
using UnityEngine.AI;

public class CharacterMove : MonoBehaviour
{
    //�L������NavMeshAgent
    public NavMeshAgent agent;

    //�X�e�[�^�X�擾�p
    public CharaStatus CharaStatus;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

    //�L����
    [SerializeField] Transform player;

    private void Start()
    {
        //�ړ����x�ݒ�
        agent = GetComponent<NavMeshAgent>();
        agent.speed = (float)CharaStatus.CharaSpeed;
    }

    void Update()
    {
        //���N���b�N������
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }
        /*�X�^�~�i�����鏈��
         * �܂��r���Ȃ̂Ŋ������ĂȂ�
        if(hit.point.x >player.position.x)
        {

        }


        Debug.Log("ky��:" + player.position);
        Debug.Log("hit:" + hit.point);
        */

    }

    private void MoveToCursor()
    {
        //�J�����̏ꏊ�ƃ}�E�X��Click���W����Z���W�����߂�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hasHit = Physics.Raycast(ray, out hit);

        //�ړ����x����
        agent.speed -= 0.5f;
        CharaStatus.CharaSpeed = agent.speed;

        //Click�����ꏊ�܂ňړ�
        if (hasHit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }
}