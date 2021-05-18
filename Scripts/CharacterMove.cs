using TapStateManager;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Touch;
public class CharacterMove : MonoBehaviour
{
    //共通のやつ
    [SerializeField] Base Base;

    //各々のやつ
    [SerializeField] Animator animator;
    [SerializeField] CharaStatus CharaStatus;
    [SerializeField] Transform player;
    [SerializeField] Judgement judgement;


    //生成するゲームオブジェクト
    public GameObject Ball;

    //生成するゲームオブジェクト
    public GameObject racket;


    //前の座標と今の座標を比べるために使う変数
    Vector3 nowPosition;

    //理解はしてないけど3d空間上でのClick座標を取得するのに使う
    RaycastHit hit;

    int a = 0;

    void Start()
    {
        //ラケットの取得
        judgement = GameObject.Find("Cube").GetComponent<Judgement>();
    }

    void Update()
    {
        //製作途中 右振り、左振り、ボレー(コメントアウト)
        {
            /*
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
            if (animator.GetBool("is_Volley") == true)
            {
                a++;

                if (a > 100)
                {
                    a = 0;
                    //プレイヤーを待機モーションにする
                    animator.SetBool("is_Volley", false);
                }
            }
            */
        }

        //コメントアウト
        {
            /*
            //タッチ時の処理　確認はしてない
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                touch = Input.GetTouch(0).deltaPosition;

                touchPosition = new Vector3(touch.x, touch.y,0);
            }
            */
        }

        //クリック
        if (Base.touch_state._touch_flag == true && Base.touch_state._touch_phase == TouchPhase.Ended)
        {
            //現状の移動指定地を削除
            GetComponent<NavMeshAgent>().ResetPath();

            //クリック時間によって処理を分ける
            if (Base.Shot.GetTapTime <= 10) 
            {
                //移動の処理
                GetComponent<NavMeshAgent>().destination = Base.Move(Input.mousePosition, hit);
            }
            //長押し
            else
            {
                racket.transform.position =new Vector3(player.position.x-5,player.position.y, player.position.z);

                //スイングAnimationにする予定
                animator.SetBool("is_RightShake", true);

                //円の大きさを測る
                CharaStatus.CharaCircle = Base.CircleScale();

                //プレイヤー状態を振るに変更
                CharaStatus.RacketSwing = 2;
            }
        }

        //移動中かどうか
        if (Base.PositionJudge(player.position, nowPosition))
        {
            //プレイヤーを走るモーションにする
            animator.SetBool("is_Run", true);

            //プレイヤー状態を移動に変更
            CharaStatus.RacketSwing = 1;

            //プレイヤーのスタミナを減らす
            CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
        }
        else
        {
            //プレイヤーを待機モーションにする
            animator.SetBool("is_Run", false);
            //プレイヤー状態を待機に変更
            CharaStatus.RacketSwing = 0;
        }

        //オート移動処理(コメントアウト)
        {
            /*
            if (プレイヤーに対して向かってきている状態の弾なら)
            {
                AutoMove();
            }
            */
        }

        //振る状態時なら50カウント後に待機状態に戻す
        if (animator.GetBool("is_RightShake") == true)
        {
            a++;

            if (a > 50)
            {
                a = 0;

                racket.transform.position = new Vector3(0, -100, 0);

                //プレイヤー状態を待機に変更
                CharaStatus.RacketSwing = 0;

                //プレイヤーを待機モーションにする
                animator.SetBool("is_RightShake", false);
            }
        }

        //振ったラケットが当たったら
        if (judgement.HitFlg == true && CharaStatus.RacketSwing == 0) 
        {
            //振る
            Base.Swing(CharaStatus.CharaPower);
        }

        //現在の座標を取得
        nowPosition = player.position;
    }
}