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
    [SerializeField] public Shot Shot;
    [SerializeField] GameObject net;
    [SerializeField] Ball ball;
    [SerializeField] GameObject pointB;
    //前の座標と今の座標を比べるために使う変数
    Vector3 nowPosition;

    //理解はしてないけど3d空間上でのClick座標を取得するのに使う
    RaycastHit hit;

     int motionCnt = 0;
    bool autoFlg   = false;
    bool swingFlg  = false;
    bool hitFlg = false;

    void Start()
    {
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    void Init()
    {
        player.transform.position = new Vector3(110, 0, 0);
        motionCnt = 0;
        autoFlg = false;
        swingFlg = false;
        hitFlg = false;

        //現状の移動指定地を削除
        GetComponent<NavMeshAgent>().ResetPath();

        //if文でこっちがサーブなのか判定してから

        if(ball.nowUserTag =="Player2")
        {
           //対角線上に配置する予定
           player.transform.position = new Vector3(105,0,-25);
        }
        else
        {
            //対角線上に配置する予定
            player.transform.position = new Vector3(105, 0, 25);
        }
   
        Shot = GameObject.Find("Shot").GetComponent<Shot>();

        Base.InitCnt += 1;
        Debug.Log("Player");
    }

    void Update()
    {
        if (Base.InitCnt == 2) 
        {
            Init();
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

        //タッチ時の処理
        TapMove();

        //自動移動時の処理
        AutoMove();

        //移動中か判定する処理
        JudgeMove();

        //ラケットを振った時の処理
        Swing();

        //現在の座標を取得
        nowPosition = player.position;
    }






    void TapMove()
    {
        //クリック
        if (Base.touch_state._touch_flag == true && Base.touch_state._touch_phase == TouchPhase.Ended)
        {
            //現状の移動指定地を削除
            GetComponent<NavMeshAgent>().ResetPath();

            //クリック時間によって処理を分ける
            if (Shot.GetTapTime <= 10)
            {
                //移動の処理
                Vector3 xyz = Base.Move(Input.mousePosition, hit);

                //ネット越えないように
                if (xyz.x >= 20)
                {
                    GetComponent<NavMeshAgent>().destination = Base.Move(Input.mousePosition, hit);
                }
            }
            //長押し
            else
            {
                //スイングAnimationにする予定
                animator.SetBool("is_RightShake", true);

                //円の大きさを測る
                CharaStatus.CharaCircle = Base.CircleScale(Shot.GetDistance);

                //プレイヤー状態を振るに変更
                CharaStatus.NowState = 2;
            }
        }
    }

    void AutoMove()
    {
        //オート移動処理
        if (ball.nowUserTag == "Player2" && ball.transform.position.x >= 7)
        {
            if (autoFlg == true)
            {

                Vector3 xyz = new Vector3(0, 0, 0);

                //プレイヤーのコートの左側
                if (pointB.transform.position.z < -40 && pointB.transform.position.z > -0)
                {
                    xyz = new Vector3(110, 0, -25);
                }

                //プレイヤーのコートの右側 
                if (pointB.transform.position.z > -9 && pointB.transform.position.z < 9)
                {
                    xyz = new Vector3(110, 0, -2);

                }

                //プレイヤーのコートの中央
                if (pointB.transform.position.z > 10 && pointB.transform.position.z < 40)
                {
                    xyz = new Vector3(110, 0, 25);

                }

                //プレイヤーのコートの前中央
                if (pointB.transform.position.x < 0 && pointB.transform.position.x > 40)
                {
                    xyz = new Vector3(40, 0, 25);
                }

                //プレイヤーのコートの中央
                if (pointB.transform.position.x < 41 && pointB.transform.position.x > 80)
                {
                    xyz = new Vector3(80, 0, 25);

                }

                //プレイヤーのコートの後中央
                if (pointB.transform.position.x < 81 && pointB.transform.position.x > 120)
                {
                    xyz = new Vector3(110, 0, 25);
                }

                //移動させる
                GetComponent<NavMeshAgent>().destination = xyz;

                //自動移動は一回のみ
                autoFlg = false;
            }
        }
    }

    void JudgeMove()
    {
        //移動中かどうか
        if (Base.PositionJudge(player.position, nowPosition))
        {
            //プレイヤーを走るモーションにする
            animator.SetBool("is_Run", true);

            //プレイヤー状態を移動に変更
            CharaStatus.NowState = 1;

            //プレイヤーのスタミナを減らす
            CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
        }
        else
        {
            //プレイヤーを待機モーションにする
            animator.SetBool("is_Run", false);

            //プレイヤー状態を待機に変更
            CharaStatus.NowState = 0;
        }

        if (animator.GetBool("is_Run") == false)
        {
            //スピードを決める
            float speed = 0.1f;

            //自分と相手とのベクトル差を取得
            Vector3 relativePos = net.transform.position - player.transform.position;

            //方向を回転情報にする
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            //対象に向かせる
            player.rotation = Quaternion.Slerp(this.transform.rotation, rotation, speed);
        }
    }

    void Swing()
    {
        //振る状態時なら50カウント後に待機状態に戻す
        if (animator.GetBool("is_RightShake") == true)
        {
            motionCnt++;

            if (motionCnt > 30)
            {
                swingFlg = true;
            }

            if (motionCnt > 40)
            {
                motionCnt = 0;

                //プレイヤー状態を待機に変更
                CharaStatus.NowState = 0;

                //プレイヤーを待機モーションにする
                animator.SetBool("is_RightShake", false);

                swingFlg = false;
            }
        }

        //振ったラケットが当たったら
        if (ball.nowUserTag == "Player2" && hitFlg == true && swingFlg == true)
        {
            CharaStatus.Rad = (float)Shot.GetRadian;          //ラジアン値
            CharaStatus.Distance = (float)Shot.GetDistance;   //距離

            CharaStatus.Distance *=2;

            //振る
            Base.Swing(CharaStatus.CharaPower, Shot.GetPower);

            //自動移動フラグがたつ
            autoFlg = true;

            //ラケットとのHitフラグをこちら側でオフ(あちら側だけで完結させたらこっちのフラグ情報と違いが発生したため)
            hitFlg = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // 物体がトリガーに接触しとき、１度だけ呼ばれる

        //プレイヤー側のラケットと当たったら
        if (collision.name == "Ball")
        {
            hitFlg = true;
        }
    }
}