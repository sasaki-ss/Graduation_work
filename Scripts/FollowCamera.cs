using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //�ǔ�����L����
    [SerializeField] Transform player;

    void Update()
    {
        // ���W���擾���Ĉړ�������
        Vector3 pos = player.position;
        transform.position = player.position;

        //���Վ��_�ɂ��邽�ߏ������炷
        pos.z = transform.position.z - 10;
        pos.y = transform.position.y + 10;
        transform.position = pos;

        //Camera�̊p�x���X����
        transform.rotation = Quaternion.Euler(45,0, 0);
    }
}
