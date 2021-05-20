using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //追尾するキャラ
    //[SerializeField] Transform player;

    //posision 60 70 0
    //rotation 32 -90 0

    private GameObject player;
    private Vector3 playerPos;

    private GameObject camera;
    private Vector3 cameraPos;

    private void Start()
    {
        player = GameObject.Find("Player");     //プレイヤー格納

        camera = GameObject.Find("Main Camera");//
        camera.transform.position = new Vector3(player.transform.position.x + 60, 70, 0);
        camera.transform.rotation = Quaternion.Euler(32, -90, 0);
    }

    void Update()
    {
        camera.transform.position = new Vector3(player.transform.position.x + 60, 70, player.transform.position.z/1.2f);
        if(player.transform.position.z < 0)
        {
            camera.transform.rotation = Quaternion.Euler(32, -90 - player.transform.position.z/4, 0);
        }
        else if (player.transform.position.z > 0)
        {
            camera.transform.rotation = Quaternion.Euler(32, -90 - player.transform.position.z / 4, 0);
        }
    }
}
