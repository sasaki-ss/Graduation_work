using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
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

    //前の座標と今の座標を比べるために使う変数
    Vector3 nowPosition;

    int   motionCnt = 0;        //モーション管理用のカウント変数
    int   serveCnt  = 0;        //サーブ時のちょっとした待機時間変数
    int   miss      = 0;        //特定の数値なら移動をしない変数
    float dis       = 0;        //距離を測る為の変数
    bool  hitFlg    = false;    //ラケットに当たったとするフラグ
    bool  onceFlg   = true;     //スイング処理を一回だけ行うためのフラグ

    void Start()
    {
        //読み込み
        Base        = GameObject.Find("Player2").GetComponent<Base>();
        animator    = GameObject.Find("Player2").GetComponent<Animator>();
        CharaStatus = GameObject.Find("Player2").GetComponent<CharaStatus>();
        player      = GameObject.Find("Player2").GetComponent<Transform>();
        ball        = GameObject.Find("Ball").GetComponent<Ball>();
        net         = GameObject.Find("Net");
        pointB      = GameObject.Find("pointB");
        score       = GameObject.Find("Score").GetComponent<Score>();
        Shot        = GameObject.Find("Shot").GetComponent<Shot>();

        //こっちサーブの時
        if (ball.nowUserTag == User.User2)
        {
            if (score.user2Score % 2 == 0)
            {
                //奇数
                //相手と対角線上に配置する予定
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //偶数
                //相手と対角線上に配置する予定
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }
        else
        {
            //if文でこっちがサーブなのか判定してから
            if (score.user1Score % 2 == 0)
            {
                //奇数
                //相手と対角線上に配置する予定
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //偶数
                //相手と対角線上に配置する予定
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }
    }

    public void Init()
    {
        //現状の移動指定地を削除
        GetComponent<NavMeshAgent>().ResetPath();

        //読み込み
        Base        = GameObject.Find("Player2").GetComponent<Base>();
        animator    = GameObject.Find("Player2").GetComponent<Animator>();
        CharaStatus = GameObject.Find("Player2").GetComponent<CharaStatus>();
        player      = GameObject.Find("Player2").GetComponent<Transform>();
        ball        = GameObject.Find("Ball").GetComponent<Ball>();
        net         = GameObject.Find("Net");
        pointB      = GameObject.Find("pointB");
        score       = GameObject.Find("Score").GetComponent<Score>();
        Shot        = GameObject.Find("Shot").GetComponent<Shot>();

        //数値のリセット
        this.CharaStatus.Rad      = 0;
        this.CharaStatus.Distance = 0;
        motionCnt = 0;
        serveCnt  = 0;
        miss      = 0;
        dis       = 0;
        hitFlg    = false;
        onceFlg   = true;

        //振るモーションをfalseに
        this.animator.SetBool("is_RightShake", false);

        //こっちサーブの時
        if (GameManager.instance.serveUser == User.User2) 
        {
            if (score.user2Score % 2 == 0)
            {
                //奇数
                //相手と対角線上に配置する予定
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //偶数
                //相手と対角線上に配置する予定
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }
        //サーブじゃない時なら
        else
        {
            if (score.user1Score % 2 == 0)
            {
                //奇数
                //相手と対角線上に配置する予定
                player.transform.position = new Vector3(-125, 0, 25);
            }
            else
            {
                //偶数
                //相手と対角線上に配置する予定
                player.transform.position = new Vector3(-125, 0, -25);
            }
        }

        Debug.Log("AIのInit処理の実行");
    }

    void Update()
    {

        //キャラとボールの距離を測る
        dis = Vector3.Distance(this.GetComponent<NavMeshAgent>().transform.position, ball.transform.position);

        //自動移動時の処理
        AutoMove();

        //移動中か判定する処理
        JudgeMove();

        //ラケットを振った時の処理
        Swing();

        //現在の座標を取得
        nowPosition = player.position;

        //振るモーションでない時はフラグをオンにしておく
        if (animator.GetBool("is_RightShake") == false)
        {
            onceFlg = true;
        }
    }

    void AutoMove()
    {
        //オート移動処理

        //x-7～x119がAIコートの内側
        //z55～z-55がAIコートの内側

        int patternX = 0;
        int patternZ = 0;

        //Xの場合
        if (pointB.transform.position.x < -7 && pointB.transform.position.x >= -30)
        {
            //Debug.Log("-7～-30");
            patternX = 1;
        }
        else
        if (pointB.transform.position.x < -30 && pointB.transform.position.x >= -60)
        {
            //Debug.Log("-30～-60");
            patternX = 2;
        }
        else
        if (pointB.transform.position.x < -60 && pointB.transform.position.x >= -90)
        {
            //Debug.Log("-60～-90");
            patternX = 3;
        }
        else
        if (pointB.transform.position.x < -90 && pointB.transform.position.x >= -119)
        {
            //Debug.Log("-90～-119");
            patternX = 4;
        }
        else
        {
            //Debug.Log("-119～or-7<x");
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

        Vector3 xyz = new Vector3(0, 0, 0);

        //場所に応じて移動(X座標)
        switch (patternX)
        {
            case 1:
                xyz.x = -45;
                break;
            case 2:
                xyz.x = -75;
                break;
            case 3:
                xyz.x = -105;
                break;
            case 4:
                xyz.x = -135;
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

        //乱数の設定(設定した数値が出ればAIは動かない)
        miss = Random.Range(1, 20);

        //最初のみ
        if (ball.boundCount != 0) 
        {
            if (pointB.transform.position.x >= -62 && pointB.transform.position.x <= 0)
            {
                if (miss != 13 && ball.transform.position.x <= 60 && patternX != 0 && patternZ != 0)
                {
                    //移動させる
                    GetComponent<NavMeshAgent>().destination = xyz;
                }
            }
        }
        //通常
        else
        {
            if (ball.boundCount != 0 && GameManager.instance.gameState != GameState.Serve && 
                miss != 13 && ball.transform.position.x <= 60 && patternX != 0 && patternZ != 0)
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
            this.animator.SetBool("is_Run", true);

            //プレイヤー状態を移動に変更
            this.CharaStatus.NowState = 1;

            //プレイヤーのスタミナを減らす
            this.CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.005f;
        }
        else
        {
            //プレイヤーを待機モーションにする
            this.animator.SetBool("is_Run", false);

            //プレイヤー状態を待機に変更
            this.CharaStatus.NowState = 0;
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
        //距離を測る(pointBとの)
        dis = Vector3.Distance(this.transform.position, pointB.transform.position);

        //こちらがサーブする側ではないとき
        if (GameManager.instance.isServe != true)
        {
            //違和感のない範囲にいたら
            if (ball.boundCount != 0 && dis <= 50)
            {
                //スイングAnimationにする予定
                //スイングフラグ(モーションフラグも兼ねてるフラグをオン)
                animator.SetBool("is_RightShake", true);

                //円の大きさを測る
                CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                //プレイヤー状態を振るに変更
                CharaStatus.NowState = 2;

                //プレイヤーのスタミナを減らす
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.005f;
            }
        }
        //こっちサーブの時
        else
        if (GameManager.instance.serveUser == User.User2)
        {
            //とりあえず間隔をあける
            serveCnt++;

            //Debug.Log(cnt);
            if (serveCnt > 200)
            {
                //Debug.Log("AIサーブ");

                //スイングAnimationにする予定
                animator.SetBool("is_RightShake", true);

                //円の大きさを測る
                CharaStatus.CharaCircle = Base.CircleScale(Shot.GetTapTime);

                //プレイヤー状態を振るに変更
                CharaStatus.NowState = 2;

                //サーブ時左側なら
                if (this.transform.position.z <= 0)
                {
                    Debug.Log("左側から");

                    //適当に設定(角度と距離)
                    this.CharaStatus.Rad = 1.8f;        //ラジアン値
                    this.CharaStatus.Distance = 600;    //距離
                }
                //右側
                else
                {
                    Debug.Log("右側から");

                    //適当に設定(角度と距離)
                    this.CharaStatus.Rad = 0.9f;        //ラジアン値
                    this.CharaStatus.Distance = 600;    //距離
                }
            }
        }

        //振る状態時なら50カウント後に待機状態に戻す
        if (animator.GetBool("is_RightShake") == true)
        {
            motionCnt++;

            if (motionCnt > 40)
            {
                motionCnt = 0;

                //プレイヤー状態を待機に変更
                CharaStatus.NowState = 0;

                //プレイヤーを待機モーションにする
                animator.SetBool("is_RightShake", false);

                //プレイヤーのスタミナを減らす
                CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.005f;
            }
        }

        //スイング状態でボールと当たったら
        if (animator.GetBool("is_RightShake") == true && hitFlg == true && onceFlg == true)
        {
            Vector2 parameter;

            //適当に設定(飛ぶ強さと滞空時間)
            float power = 1.5f, taptime = 50;

            //サーブ時なら
            if (GameManager.instance.serveUser == User.User2 && GameManager.instance.isServe == true)
            {
                //角度と距離は少し上らへんで設定されてる
                //振る
                Base.Swing(CharaStatus.CharaPower, power, taptime, User.User2);
            }
            else
            {
                //適当に設定(角度と距離)
                parameter = TargetPoint();
                CharaStatus.Rad = parameter.x;
                CharaStatus.Distance = parameter.y;

                //振る
                Base.Swing(CharaStatus.CharaPower, power, taptime, User.User2);
            }

            //この処理一度だけ実行させるためにフラグをかませる
            onceFlg = false;

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

    private void OnTriggerExit(Collider collision)
    {
        // 物体がトリガーに離れたとき、１度だけ呼ばれる

        //プレイヤー側のラケットと離れたとき
        if (collision.name == "Ball")
        {
            hitFlg = false;
        }
    }

    private Vector2 TargetPoint()
    {
        float targetPointX = 1.5f;
        float targetPointY = 400;

        int mode = Random.Range(1, 4);

        //プレイヤーのコートの左前側
        if (pointB.transform.position.z < -40 && pointB.transform.position.z > -0)
        {
            targetPointX = 0.9f;
        }

        //プレイヤーのコートの右前側 
        if (pointB.transform.position.z > -9 && pointB.transform.position.z < 9)
        {
            targetPointX = 1.5f;
        }

        //プレイヤーのコートの右前側 
        if (pointB.transform.position.z > 10 && pointB.transform.position.z < 40)
        {
            targetPointX = 2.1f;
        }

        if (mode == 2)
        {

            //プレイヤーのコートの前中央
            if (pointB.transform.position.x < 0 && pointB.transform.position.x > -40)
            {
                int a = Random.Range(1, 3);

                if (a != 2)
                {
                    targetPointX = 1.3f;
                }

                targetPointY = 400 * 2.5f;
            }

            //プレイヤーのコートの中央
            if (pointB.transform.position.x < -41 && pointB.transform.position.x > -80)
            {
                int a = Random.Range(1, 3);

                if (a != 2)
                {
                    targetPointX = 1.8f;
                }

                targetPointY = 400 * 3f;
            }

            //プレイヤーのコートの後中央
            if (pointB.transform.position.x < -81 && pointB.transform.position.x > -120)
            {
                int a = Random.Range(1, 3);

                if (a != 3)
                {
                    targetPointX = 2.2f;
                }

                targetPointY = 400 * 2.5f;
            }
        }

        if (mode == 4)
        {
            targetPointX = Random.Range(1, 25) / 10;
            targetPointY = 400 * Random.Range(10, 35) / 10;
        }

        Vector2 targetPoint = new Vector2(targetPointX, targetPointY);

        //Debug.Log("rad" + targetPointX);
        //Debug.Log("dis" + targetPointY);

        return targetPoint;
    }
}
