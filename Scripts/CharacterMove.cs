using UnityEngine;
using UnityEngine.AI;

public class CharacterMove : MonoBehaviour
{
    //キャラのNavMeshAgent
    public NavMeshAgent agent;

    //ステータス取得用
    public CharaStatus CharaStatus;

    //理解はしてないけど3d空間上でのClick座標を取得するのに使う
    RaycastHit hit;

    //キャラ
    [SerializeField] Transform player;

    private void Start()
    {
        //移動速度設定
        agent = GetComponent<NavMeshAgent>();
        agent.speed = (float)CharaStatus.CharaSpeed;
    }

    void Update()
    {
        //左クリックしたら
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }
        /*スタミナが減る処理
         * まだ途中なので完成してない
        if(hit.point.x >player.position.x)
        {

        }


        Debug.Log("kyら:" + player.position);
        Debug.Log("hit:" + hit.point);
        */

    }

    private void MoveToCursor()
    {
        //カメラの場所とマウスのClick座標からZ座標を求める
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hasHit = Physics.Raycast(ray, out hit);

        //移動速度減少
        agent.speed -= 0.5f;
        CharaStatus.CharaSpeed = agent.speed;

        //Clickした場所まで移動
        if (hasHit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }
}