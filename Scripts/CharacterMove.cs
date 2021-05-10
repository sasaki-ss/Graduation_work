using TapStateManager;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Touch;
public class CharacterMove : MonoBehaviour
{
    //ステータス取得用
    public CharaStatus CharaStatus;

    //Shot用
    public Shot Shot;

    //理解はしてないけど3d空間上でのClick座標を取得するのに使う
    RaycastHit hit;
    int a = 0;
    //キャラ
    [SerializeField] Animator animator;
    [SerializeField] Transform player;
    [SerializeField] GameObject ball;

    //前の座標と今の座標を比べるために使う変数
    Vector3 nowPosition;

    //タッチ時に使う処理
    Vector2 touch;
    Vector3 touchPosition;

    TouchManager tMger;      //TapStateManager内のTouchManager

    void Start()
    {
        tMger = new TouchManager(); //初期化
    }

    void Update()
    {
        //製作途中 右振り、左振り、ボレー

        //右振り
        if (Input.GetKey("right"))
        {
            animator.SetBool("is_RightShake", true);
        }

        //100カウント後に待機モーションに
        if (animator.GetBool("is_RightShake") == true)
        {
            a++;

            if (a > 100)
            {
                a = 0;
                //プレイヤーを待機モーションにする
                animator.SetBool("is_RightShake", false);
            }
        }

        //左振り
        if (Input.GetKey("left"))
        {
            animator.SetBool("is_LeftShake", true);
        }

        //100カウント後に待機モーションに
        if (animator.GetBool("is_LeftShake") == true)
        {
            a++;

            if (a > 100)
            {
                a = 0;
                //プレイヤーを待機モーションにする
                animator.SetBool("is_LeftShake", false);
            }
        }

        //ボレー
        if (Input.GetKey("down"))
        {
            animator.SetBool("is_Volley", true);
        }

        //100カウント後に待機モーションに
        if (animator.GetBool("is_Volley")==true)
        {
            a++;

            if(a>100)
            {
                a = 0;
                //プレイヤーを待機モーションにする
                animator.SetBool("is_Volley", false);
            }
        }

        /*
        if (Input.GetKey("up"))
        {
             AutoMove();
        }
        */

        /*
        //タッチ時の処理　確認はしてない
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touch = Input.GetTouch(0).deltaPosition;

            touchPosition = new Vector3(touch.x, touch.y,0);
        }
        */

        tMger.update(); //更新

        TouchManager touch_state = tMger.getTouch();    //タッチ取得


        Debug.Log(touch_state._touch_phase == TouchPhase.Ended);
        Debug.Log(touch_state._touch_phase == TouchPhase.Moved);
        Debug.Log(touch_state._touch_phase);

        //クリック
        if (touch_state._touch_flag == true && touch_state._touch_phase == TouchPhase.Ended)
        {

            //現状の移動指定地を削除
            GetComponent<NavMeshAgent>().ResetPath();

            CharaStatus.RacketSwing = 1;

            //クリック時の処理
            MoveToCursor();
        }


        //長押し中
        if (touch_state._touch_flag == true && touch_state._touch_phase == TouchPhase.Moved)
        {
            CharaStatus.RacketSwing = 2;

            //スライドの長さによって円の大きさが変わる
            if (Shot.GetDistance  >= 0  && Shot.GetDistance < 5 )
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

        //オート移動処理
        /*if(プレイヤーに対して向かってきている状態の弾なら)
        {
            AutoMove();
        }
        */

        //座標判定
        PositionJudge();
       
    }

    private void MoveToCursor()
    {
        //プレイヤーを走るモーションにする
        animator.SetBool("is_Run", true);

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

    private void PositionJudge()
    {
        //現座標と前座標に違う場合
        if (player.position != nowPosition)
        {
            //移動中ならスタミナ減少(今いる座標と目的地の座標のズレが1以上の場合)
            if (hit.point.x - player.position.x < -1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }
            else
            if (hit.point.x - player.position.x > 1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }
            else
            if (hit.point.y - player.position.y < -1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }
            else
            if (hit.point.y - player.position.y > 1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }
            else
            if (hit.point.z - player.position.z < -1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }
            else
            if (hit.point.z - player.position.z > 1)
            {
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
            }
            else
            {
                //プレイヤーを待機モーションにする
                animator.SetBool("is_Run", false);
                CharaStatus.RacketSwing = 0;
            }
        }

        //現在の座標を取得
        nowPosition = player.position;
    }

    private void AutoMove()
    {
        //ボールのオブジェクトの情報を取得
        ball = GameObject.Find("ball");

        //移動させる
        GetComponent<NavMeshAgent>().destination = ball.transform.position;      
        //Debug.Log(ball.transform.position);
    }
}