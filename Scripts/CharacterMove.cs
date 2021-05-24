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
    [SerializeField] public Shot Shot;

    //生成するゲームオブジェクト
    public Ball ball;

    //生成するゲームオブジェクト
    public GameObject racket;


    //前の座標と今の座標を比べるために使う変数
    Vector3 nowPosition;

    //理解はしてないけど3d空間上でのClick座標を取得するのに使う
    RaycastHit hit;

    int motionCnt = 0;
    bool autoFlg = false;

    bool swingFlg = false;

    void Start()
    {
        //ラケットの取得
        judgement = GameObject.Find("PlayerRacket").GetComponent<Judgement>();
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    public void Init()
    {
        player.transform.position = new Vector3(105, 0, 0);
        motionCnt = 0;
        autoFlg = false;
        swingFlg = false;
        //ラケットの取得
        judgement = GameObject.Find("PlayerRacket").GetComponent<Judgement>();
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
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
                motionCnt++;

                if (motionCnt > 100)
                {
                    motionCnt = 0;
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
                motionCnt++;

                if (motionCnt > 100)
                {
                    motionCnt = 0;
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
                motionCnt++;

                if (motionCnt > 100)
                {
                    motionCnt = 0;
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
            if (Shot.GetTapTime <= 20) 
            {
                //移動の処理
                Vector3 xyz = Base.Move(Input.mousePosition, hit);

                //ネット越えないように
                if(xyz.x>=20)
                {
                    GetComponent<NavMeshAgent>().destination = Base.Move(Input.mousePosition, hit);
                }
            }
            //長押し
            else
            {
                //racket.transform.position = new Vector3(player.position.x - 5, player.position.y +1, player.position.z);

                //スイングAnimationにする予定
                animator.SetBool("is_RightShake", true);

                //円の大きさを測る
                CharaStatus.CharaCircle = Base.CircleScale(Shot.GetDistance);

                //プレイヤー状態を振るに変更
                CharaStatus.RacketSwing = 2;
            }
        }

        //オート移動処理
        if (ball.nowUserTag == "Player2" && ball.transform.position.x >= 7) 
        {
            //二点間の距離を測る
            float dis = Vector3.Distance(GetComponent<NavMeshAgent>().transform.position, ball.transform.position);

            //ある程度まで近づいたら
            if (dis >= 50 && autoFlg == true) 
            {
                Vector3 xyz = new Vector3(ball.transform.position.x+60, ball.transform.position.y, ball.transform.position.z);

                //移動させる
                GetComponent<NavMeshAgent>().destination = xyz;

                //自動移動は一回のみ
                autoFlg = false;

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

        //振る状態時なら50カウント後に待機状態に戻す
        if (animator.GetBool("is_RightShake") == true)
        {
            motionCnt++;

            if (motionCnt > 50)
            {
                swingFlg = true;
            }

            if (motionCnt > 60) 
            { 
                motionCnt = 0;

                //racket.transform.position = new Vector3(0, -100, 0);

                //プレイヤー状態を待機に変更
                CharaStatus.RacketSwing = 0;

                //プレイヤーを待機モーションにする
                animator.SetBool("is_RightShake", false);
            }
        }

        if (animator.GetBool("is_RightShake") == false)
        {
            swingFlg = false;
        }

            //振ったラケットが当たったら
            if (ball.nowUserTag == "Player2" && judgement.HitFlg == true && swingFlg == true) 
        {
           // Debug.Log("ssssssssssss");
            //振る
            Base.Swing(CharaStatus.CharaPower, Shot.GetPower);

            //自動移動フラグがたつ
            autoFlg = true;
            
            //ラケットとのHitフラグをこちら側でオフ(あちら側だけで完結させたらこっちのフラグ情報と違いが発生したため)
            judgement.HitFlg = false;
        }

        //現在の座標を取得
        nowPosition = player.position;
    }
}