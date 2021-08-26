using TapStateManager;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Touch;
public class CharacterMove : MonoBehaviour
{
    //外部のやつの読み込み用
    [SerializeField] Base Base;
    [SerializeField] Animator animator;
    [SerializeField] CharaStatus CharaStatus;
    [SerializeField] Transform player;
    [SerializeField] Ball ball;
    [SerializeField] GameObject net;
    [SerializeField] GameObject pointB;
    [SerializeField] Score score;
    [SerializeField] public Shot Shot;
    [SerializeField] Judge judge;

    //前の座標と今の座標を比べるために使う変数
    Vector3 nowPosition;

    //理解はしてないけど3d空間上でのClick座標を取得するのに使う
    RaycastHit hit;

    //

    int   cnt    = 0;        //モーション管理用のカウント変数
    int   serveMoveCnt = 0;        //サーブした直後に自動移動をさせないためのカウント
    float dis          = 0;        //距離を測る為の変数
    bool swingFlg = false;
    bool  hitFlg       = false;    //ラケットに当たったとするフラグ
    bool  onceFlg      = false;     //スイング処理を一回だけ行うためのフラグ
    bool  serveMoveFlg = true;     //サーブした直後に自動移動をさせないためのフラグ
    bool autoFlg = false;
    bool aniHit = false;
    bool stockFlg = false;
    bool tapFlg = false;


    float screenAspect; //画面のアスペクト比
    float targetAspect = (float)1080; //目的のアスペクト比
    float magRate;
    float a;
    //

    void Start()
    {

        screenAspect = (float)Screen.width;
        magRate = (float)screenAspect/ (float)targetAspect; //目的アスペクト比にするための倍率

        Debug.Log(magRate);
        //a = magRate - 1; //目的アスペクト比にするための倍率

        //読み込み
        Base        = GameObject.Find("Player").GetComponent<Base>();
        animator    = GameObject.Find("Player").GetComponent<Animator>();
        CharaStatus = GameObject.Find("Player").GetComponent<CharaStatus>();
        player      = GameObject.Find("Player").GetComponent<Transform>();
        ball        = GameObject.Find("Ball").GetComponent<Ball>();
        net         = GameObject.Find("Net");
        score       = GameObject.Find("Score").GetComponent<Score>();
        Shot        = GameObject.Find("Shot").GetComponent<Shot>();
        judge       = GameObject.Find("PlayerRacket").GetComponent<Judge>();
        //こっちサーブの時
        if (GameManager.instance.serveUser == User.User1) 
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
    }

    public void Init()
    {
        //現状の移動指定地を削除
        GetComponent<NavMeshAgent>().ResetPath();

        //読み込み
        Base        = GameObject.Find("Player").GetComponent<Base>();
        animator    = GameObject.Find("Player").GetComponent<Animator>();
        CharaStatus = GameObject.Find("Player").GetComponent<CharaStatus>();
        player      = GameObject.Find("Player").GetComponent<Transform>();
        ball        = GameObject.Find("Ball").GetComponent<Ball>();
        net         = GameObject.Find("Net");
        score       = GameObject.Find("Score").GetComponent<Score>();
        Shot        = GameObject.Find("Shot").GetComponent<Shot>();
        judge = GameObject.Find("PlayerRacket").GetComponent<Judge>();

        //数値のリセット
        this.CharaStatus.Rad = 0;
        this.CharaStatus.Distance = 0;
        player.transform.position = new Vector3(125, 0, 0);
        dis = 0;
        cnt = 0;
        serveMoveCnt = 0;
        onceFlg = false;
        hitFlg = false;
        serveMoveFlg = true;
        swingFlg = false;
        autoFlg = false;
        aniHit = false;
        stockFlg = false;
        tapFlg = false;
        //プレイヤーのスタミナを回復
        this.CharaStatus.CharaStamina = CharaStatus.CharaStamina + 0.5f;


        //こっちサーブの時
        if (GameManager.instance.serveUser == User.User1)
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
        //サーブじゃない時なら
        else
        {
            if (score.user2Score % 2 == 0)
            {
                //偶数
                //対角線上に配置する予定
                player.transform.position = new Vector3(125, 0, -25);
            }
            else
            {
                //奇数
                //対角線上に配置する予定
                player.transform.position = new Vector3(125, 0, 25);
            }
        }

        //Debug.Log("PlayerのInit処理の実行");
    }

    void Update()
    {
        if(pointB == null && pointB.activeSelf == true)
        {
            pointB = GameObject.Find("PointB");
        }

        //キャラとボールの距離を測る
        dis = Vector3.Distance(this.transform.position, ball.transform.position);

        //Debug.Log(GameManager.instance.serveUser);
        //Debug.Log(ball.nowUserTag);

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

        if (GameManager.instance.gameState == GameState.GameSet)
        {
            //スタミナを回復
            this.CharaStatus.CharaStamina = 10;
        }
    }


    void TapMove()
    {

        //こちらがサーブする側ではないとき
        if (GameManager.instance.isServe != true) 
        {
            serveMoveFlg = false;
            serveMoveCnt = 300;

            //クリック
            if (Base.touch_state._touch_flag == true && Base.touch_state._touch_phase == TouchPhase.Ended)
            {
                //Debug.Log("サーブ時ではない行動");

                //現状の移動指定地を削除
                GetComponent<NavMeshAgent>().ResetPath();

                //クリック時間によって処理を分ける
                if (Shot.GetDistance < 50) 
                {
                    //移動の処理
                    Vector3 xyz = Base.Move(Input.mousePosition, hit);

                    //ネット越えないように
                    if (xyz.x >= 20)
                    {
                        GetComponent<NavMeshAgent>().destination = Base.Move(Input.mousePosition, hit);
                    }
                }
                //長押し(スイングを行ったとする)
                else
                {
                    tapFlg = true;
                    if (autoFlg == true)
                    {
                        stockFlg = true;
                    }
                    else
                    {
                        //Serveアニメーションの実行
                        animator.Play("Swing", 0, 0f);
                        swingFlg = true;
                        //円の大きさを測る
                        CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                        //プレイヤー状態を振るに変更
                        CharaStatus.NowState = 2;
                    }
                }
            }
        }
        //横移動のみ
        //こっちサーブの時
        else
        if (GameManager.instance.serveUser == User.User1)
        {
            //クリック
            if (Base.touch_state._touch_flag == true && Base.touch_state._touch_phase == TouchPhase.Ended)
            {
                //クリックの長さによって処理を分ける
                if (Shot.GetDistance < 50)
                {
                    //Debug.Log("サーブ時の横移動");

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
                if (GameManager.instance.serveUser == User.User1)
                {
                    tapFlg = true;
                    swingFlg = true;
                    //円の大きさを測る
                    CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                    //プレイヤー状態を振るに変更
                    CharaStatus.NowState = 2;

                    serveMoveFlg = false;

                    animator.Play("Serve", 0, 0f);
                }
            }
        }

        if (autoFlg == false && tapFlg == true && stockFlg == true &&  player.transform.position.x - ball.transform.position.x  < 15) 
        {
            //Serveアニメーションの実行
            animator.Play("Swing", 0, 0f);
            swingFlg = true;
            //円の大きさを測る
            CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

            //プレイヤー状態を振るに変更
            CharaStatus.NowState = 2;

            stockFlg = false;
            tapFlg = true;
        }

    }

    void AutoMove()
    {
        if (hitFlg == true)
        {
            //現状の移動指定地を削除
            GetComponent<NavMeshAgent>().ResetPath();
            autoFlg = false;
        }

        //Debug.Log(serveMoveCnt);

        if (serveMoveFlg == false)
        {
            serveMoveCnt++;
        }

        

        //オート移動処理
        if (GameManager.instance.isServe != true && ball.nowUserTag == User.User2 && serveMoveCnt >= 0 && pointB.activeSelf == true && autoFlg == false)
        {
            autoFlg = true;
            //x7～x119がコートの内側
            //z55～z-55がコートの内側

            Vector3 xyz = new Vector3(pointB.transform.position.x+15, 0, pointB.transform.position.z - 10);

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
            CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.00005f;
        }
        else
        {
            //プレイヤーを待機モーションにする
            animator.SetBool("is_Run", false);

            //プレイヤー状態を待機に変更
            CharaStatus.NowState = 0;

            autoFlg = false;
        }

        if (animator.GetBool("is_Run") == false)
        {
            //スピードを決める
            float speed = 0.1f;

            //自分と相手とのベクトル差を取得
            Vector3 relativePos = net.transform.position - player.transform.position;

            //方向を回転情報にする
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            //対象を向かせる
            player.rotation = Quaternion.Slerp(this.transform.rotation, rotation, speed);
        }
    }

    void Swing()
    {

        if (judge.Hit == true)
        {
            hitFlg = true;
        }
        else
        {
            hitFlg = false;
            cnt = 0;
        }

        //スイング状態でボールと当たったら
        if (swingFlg == true && hitFlg == true && onceFlg == false)
        {
            a = magRate - 1;
            if (cnt == 0)
            {

                if (a > 0) 
                {
                    CharaStatus.Rad = (float)Shot.GetRadian;          //ラジアン値
                    CharaStatus.Distance = (float)Shot.GetDistance / (1+a);   //距離
                }
                else
                {
                    a=(1+Mathf.Abs(a));
                    CharaStatus.Rad = (float)Shot.GetRadian;          //ラジアン値
                    CharaStatus.Distance = (float)Shot.GetDistance * (a);   //距離
                }

                CharaStatus.CharaPower = CharaStatus.CharaPower + Shot.GetDistance / 700;
                //Debug.Log(Shot.GetDistance / 1250);
               // Debug.Log(CharaStatus.CharaPower);
                //振る
                Base.Swing(CharaStatus.CharaPower, Shot.GetPower, Shot.GetTapTime, User.User1);
            }

            cnt++;

            //この処理一度だけ実行させるためにフラグをかませる
            onceFlg = true;

            //ラケットとのHitフラグをこちら側でオフ(あちら側だけで完結させたらこっちのフラグ情報と違いが発生したため)
            hitFlg = false;

            tapFlg = false;
        }
    }

    //モーション再生中、特定のタイミングで呼ぶ関数

    void SwingStart()
    {
        swingFlg = true;
        cnt = 1;
    }

    void SwingEnd()
    {
        onceFlg = false;
        swingFlg = false;
        cnt = 0;
    }

    void Call()
    {
        //Serveアニメーションの実行
        animator.speed =1.7f;
    }

    void A()
    {
        animator.speed = 0.5f;
        Invoke("Call", 0.4f);
    }
}