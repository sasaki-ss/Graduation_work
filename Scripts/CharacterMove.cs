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
    [SerializeField] Score score;
    //前の座標と今の座標を比べるために使う変数
    Vector3 nowPosition;

    //理解はしてないけど3d空間上でのClick座標を取得するのに使う
    RaycastHit hit;

    int motionCnt = 0;
    bool autoFlg = false;
    bool swingFlg = false;
    bool hitFlg = false;
    
    void Start()
    {
        //if文で判定してから場所決め
        //こっちサーブの時
        if (ball.nowUserTag == User.User2)
        {
            if (score.user1Score % 2 == 0)
            {
                //奇数
                //対角線上に配置する予定
                player.transform.position = new Vector3(125, 0, -25);
            }
            else
            {
                //偶数
                //対角線上に配置する予定
                player.transform.position = new Vector3(125, 0, 25);
            }
        }
        else
        {
            if (score.user2Score % 2 == 0)
            {
                //奇数
                //対角線上に配置する予定
                player.transform.position = new Vector3(125, 0, -25);
            }
            else
            {
                //偶数
                //対角線上に配置する予定
                player.transform.position = new Vector3(125, 0, 25);
            }
        }

        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    public void Init()
    {
        this.CharaStatus.Rad = 0;
        this.CharaStatus.Distance = 0;
        player.transform.position = new Vector3(125, 0, 0);
        motionCnt = 0;
        autoFlg = false;
        swingFlg = false;
        hitFlg = false;

        //現状の移動指定地を削除
        GetComponent<NavMeshAgent>().ResetPath();

        //if文で判定してから場所決め
        //こっちサーブの時
        if (ball.nowUserTag == "Player2")
        {
            if (score.user1Score % 2 == 0)
            {
                //奇数
                //対角線上に配置する予定
                player.transform.position = new Vector3(125, 0, -25);
            }
            else
            {
                //偶数
                //対角線上に配置する予定
                player.transform.position = new Vector3(125, 0, 25);
            }
        }
        else
        {
            if (score.user2Score % 2 == 0)
            {
                //奇数
                //対角線上に配置する予定
                player.transform.position = new Vector3(125, 0, -25);
            }
            else
            {
                //偶数
                //対角線上に配置する予定
                player.transform.position = new Vector3(125, 0, 25);
            }
        }

        Shot = GameObject.Find("Shot").GetComponent<Shot>();

        Debug.Log("PlayerのInit処理の実行");
    }

    void Update()
    {
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
        //サーブフラグがtrueならしない
        if(GameManager.instance.isServe!= true)
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
                    CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                    //プレイヤー状態を振るに変更
                    CharaStatus.NowState = 2;
                }
            }
        }
        //横移動のみ
        //こっちサーブの時
        else
        if (ball.nowUserTag == User.User2)
        {
            //クリック
            if (Base.touch_state._touch_flag == true && Base.touch_state._touch_phase == TouchPhase.Ended)
            {
                //クリック時間によって処理を分ける
                if (Shot.GetTapTime <= 10)
                {
                    //移動の処理
                    Vector3 xyz = Base.Move(Input.mousePosition, hit);

                    xyz.x = 125;

                    //プレイヤー視点からだと自コートの右側
                    if (this.transform.position.z >= 0) 
                    {
                        if (xyz.z <= 0)
                        {
                            xyz.z = 3;
                        }
                        else
                        if (xyz.z > 45)
                        {
                            xyz.z = 42;
                        }
                    }
                    //プレイヤー視点からだと自コートの左側
                    else
                    {
                        if (xyz.z > 0)
                        {
                            xyz.z = -3;
                        }
                        else
                        if (xyz.z < -45)
                        {
                            xyz.z = -42;
                        }
                    }

                    GetComponent<NavMeshAgent>().destination = xyz;          
                }
                else
                if(ball.nowUserTag == User.User2)
                {
                    //スイングAnimationにする予定
                    animator.SetBool("is_RightShake", true);

                    //円の大きさを測る
                    CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                    //プレイヤー状態を振るに変更
                    CharaStatus.NowState = 2;

                    CharaStatus.Rad = (float)Shot.GetRadian;          //ラジアン値
                    CharaStatus.Distance = (float)Shot.GetDistance;   //距離

                    //Debug.Log(CharaStatus.Rad);
                    //Debug.Log(CharaStatus.Distance);

                    //振る
                    float a = (float)Shot.GetPower / 60 + (float)Shot.GetTapTime / 5;
                    float b = (float)CharaStatus.CharaPower / 6;

                    //Debug.Log(a);
                    //Debug.Log(b);

                    //サーブフラグオフ
                    GameManager.instance.isServe = false;

                    //サーブ関数呼ぶ
                    ball.Serve(a, b);
                }
            }
        }
    }

    void AutoMove()
    {
        //オート移動処理
        if (ball.nowUserTag == User.User2)
        {
            // Debug.Log("x:"+pointB.transform.position.x+ "y:" + pointB.transform.position.y+ "z:" + pointB.transform.position.z );

            //x7～x119がコートの内側
            //z55～z-55がコートの内側

            int patternX = 0;
            int patternZ = 0;

            //Xの場合
            if (pointB.transform.position.x > 7 && pointB.transform.position.x <= 30)
            {
                // Debug.Log("7～30");
                patternX = 1;
            }
            else
            if (pointB.transform.position.x > 30 && pointB.transform.position.x <= 60)
            {
                //Debug.Log("30～60");
                patternX = 2;
            }
            else
            if (pointB.transform.position.x > 60 && pointB.transform.position.x <= 90)
            {
                //Debug.Log("60～90");
                patternX = 3;
            }
            else
            if (pointB.transform.position.x > 90 && pointB.transform.position.x <= 119)
            {
                //Debug.Log("90～119");
                patternX = 4;
            }
            else
            {
                //Debug.Log("119～or7>x");
                patternX = 0;
            }

            //Zの場合
            if (pointB.transform.position.z > -55 && pointB.transform.position.z <= -27.5)
            {
                //Debug.Log("-55～-27.5");
                patternZ = 1;
            }
            else
            if (pointB.transform.position.z > -27.5 && pointB.transform.position.z <= 0)
            {
                //Debug.Log("-27.5～0");
                patternZ = 2;
            }
            else
            if (pointB.transform.position.z > 0 && pointB.transform.position.z <= 27.5)
            {
                //Debug.Log("0～27.5");
                patternZ = 3;
            }
            else
            if (pointB.transform.position.z > 27.5 && pointB.transform.position.z <= 55)
            {
                //Debug.Log("27.5～55");
                patternZ = 4;
            }
            else
            {
                //Debug.Log("55～or-55～");
                patternZ = 0;
            }

            //Debug.Log(patternX +":::"+ patternZ);

            Vector3 xyz = new Vector3(0, 0, 0);

            //場所に応じて移動(X座標)
            switch (patternX)
            {
                case 1:
                    xyz.x = 40;
                    break;
                case 2:
                    xyz.x = 70;
                    break;
                case 3:
                    xyz.x = 100;
                    break;
                case 4:
                    xyz.x = 130;
                    break;
                default:
                    break;
            }

            //場所に応じて移動(Z座標)
            switch (patternZ)
            {
                case 1:
                    xyz.z = -40;
                    break;
                case 2:
                    xyz.z = -13;
                    break;
                case 3:
                    xyz.z = 13;
                    break;
                case 4:
                    xyz.z = 40;
                    break;
                default:
                    break;
            }

            if (xyz.x != 0 && xyz.z != 0)
            {
                //移動させる
                GetComponent<NavMeshAgent>().destination = xyz;
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
            CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.005f;
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

                //プレイヤーのスタミナを減らす
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.005f;
            }
        }

        //振ったラケットが当たったら
        if (ball.nowUserTag == User.User2 && hitFlg == true && swingFlg == true)
        {
            CharaStatus.Rad = (float)Shot.GetRadian;          //ラジアン値
            CharaStatus.Distance = (float)Shot.GetDistance;   //距離
                                                              //Debug.Log(Shot.GetTapTime);
                                                              //Debug.Log(Shot.GetPower);

            //振る
            Debug.Log("呼び出し"+this.gameObject.name);
            Base.Swing(CharaStatus.CharaPower, Shot.GetPower, Shot.GetTapTime);
           
            
            
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