using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Touch;
public class CharacterMove : MonoBehaviour
{
    //ステータス取得用
    public CharaStatus CharaStatus;

    //理解はしてないけど3d空間上でのClick座標を取得するのに使う
    RaycastHit hit;

    //キャラ
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
        //タッチ時の処理　確認はしてない
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touch = Input.GetTouch(0).deltaPosition;

            touchPosition = new Vector3(touch.x, touch.y,0);
        }
        */
        //左クリックしたら
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }

        //現座標と前座標に違う場合
        if (player.position != a)
        {
            animator.SetBool("is_Run", true);
            //移動中ならスタミナ減少(今いる座標と目的地の座標のズレが1以上の場合)
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
        //カメラの場所とマウスのClick座標からZ座標を求める
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //タッチ時の処理　確認はしてない
        //Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        bool hasHit = Physics.Raycast(ray, out hit);

        //Clickした場所まで移動
        if (hasHit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }
}