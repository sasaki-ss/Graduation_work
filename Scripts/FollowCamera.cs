using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //追尾するキャラ
    [SerializeField] Transform player;

    void Update()
    {
        // 座標を取得して移動させる
        Vector3 pos = player.position;
        transform.position = player.position;

        //俯瞰視点にするため少しずらす
        pos.z = transform.position.z - 10;
        pos.y = transform.position.y + 10;
        transform.position = pos;

        //Cameraの角度を傾ける
        transform.rotation = Quaternion.Euler(45,0, 0);
    }
}
