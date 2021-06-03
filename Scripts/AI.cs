using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    //共通のやつ
    [SerializeField] Base Base;

    //各々のやつ
    [SerializeField] Animator animator;
    [SerializeField] CharaStatus CharaStatus;
    [SerializeField] Transform player;
    [SerializeField] Ball ball;
    [SerializeField] GameObject net;
    [SerializeField] public Shot Shot;
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject pointB;

    //前の座標と今の座標を比べるために使う変数
    Vector3 nowPosition;

    //理解はしてないけど3d空間上でのClick座標を取得するのに使う
    RaycastHit hit;

      int motionCnt = 0;
     bool swingFlg  = false;
     bool hitFlg    = false;
     bool autoFlg   = true;
    float dis = 0;
    
    //現状動かない
    bool boundFlg = true;

    int miss      = 0;  //移動をしない
    void Start()
    {
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    public void Init()
    {
        //現状の移動指定地を削除
        GetComponent<NavMeshAgent>().ResetPath();


        player.transform.position = new Vector3(-150,0,0);
        motionCnt = 0;
        autoFlg = true;
        boundFlg = true;
        swingFlg = false;
        hitFlg = false;
        dis = 0;
        miss = 0;

        //if文でこっちがサーブなのか判定してから
        if (ball.nowUserTag == "Player")
        {
            //対角線上に配置する予定
            player.transform.position = new Vector3(-105, 0, -25);
        }
        else
        {
            //対角線上に配置する予定
            player.transform.position = new Vector3(-105, 0, 25);
        }

        Shot = GameObject.Find("Shot").GetComponent<Shot>();

        Base.InitCnt += 1;
        Debug.Log("AI");
    }

    void Update()
    {
        if (Base.InitCnt == 1) 
        {
            Init();
        }

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
    }

    void AutoMove()
    {
        //Debug.Log(ball.boundCount);

        if (ball.boundCount != 0)
        {
            boundFlg = false;
        }

        //オート移動処理
        if (ball.boundCount != 0 && pointB.transform.position.x > -200 && pointB.transform.position.x < 7)
        {


            if (autoFlg == true)
            {

                Vector3 xyz = new Vector3(-110, 0, 0);

                //プレイヤーのコートの左側
                if (pointB.transform.position.z < -40 && pointB.transform.position.z > -0)
                {
                    xyz = new Vector3(-105, 0, -25);
                }

                //プレイヤーのコートの右側 
                if (pointB.transform.position.z > -9 && pointB.transform.position.z < 9)
                {
                    xyz = new Vector3(-105, 0, -2);

                }

                //プレイヤーのコートの中央
                if (pointB.transform.position.z > 10 && pointB.transform.position.z < 40)
                {
                    xyz = new Vector3(-105, 0, 25);

                }

                //プレイヤーのコートの前中央
                if (pointB.transform.position.x < 0 && pointB.transform.position.x > -40)
                {
                    xyz = new Vector3(-40, 0, 25);
                }

                //プレイヤーのコートの中央
                if (pointB.transform.position.x < 41 && pointB.transform.position.x > -80)
                {
                    xyz = new Vector3(-80, 0, 25);

                }

                //プレイヤーのコートの後中央
                if (pointB.transform.position.x < -81 && pointB.transform.position.x > -120)
                {
                    xyz = new Vector3(-110, 0, 25);
                }
                //xyz = new Vector3(pointB.transform.position.x, 0, pointB.transform.position.z);


                //移動させる
                GetComponent<NavMeshAgent>().destination = xyz;

                //自動移動は一回のみ
                autoFlg = false;

                Debug.Log(xyz);
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
            this.CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
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
        if (dis <=30)
        {
            this.animator.SetBool("is_RightShake", true);
        }

        //振る状態時なら50カウント後に待機状態に戻す
        if (this.animator.GetBool("is_RightShake") == true)
        {
            motionCnt++;

            if (motionCnt > 0)
            {
                swingFlg = true;
            }

            if (motionCnt > 140)
            {
                motionCnt = 0;

                //プレイヤー状態を待機に変更
                this.CharaStatus.NowState = 0;

                //プレイヤーを待機モーションにする
                this.animator.SetBool("is_RightShake", false);
                swingFlg = false;
            }
        }

        //振ったラケットが当たったら
        if (ball.nowUserTag == "Player" && hitFlg == true && swingFlg == true)
        {
            //振る
            //角度によって判定
            //スワイプの長さによって判定
            //プレイヤーから見たとき
            /*
             * 0.5 :左上ギリ
             * 2.5 :右上ギリ
             */

            Vector2 parameter;

            parameter = TargetPoint();
            CharaStatus.Rad = parameter.x;
            CharaStatus.Distance = parameter.y;

            //パラメータちょこっと直接いじってる
            //Debug.Log(parameter.x + ":::"+ parameter.y);
            Base.Swing(CharaStatus.CharaPower * 1.5f, Shot.GetPower + 20);

            hitFlg = false;

            autoFlg = true;

            miss = Random.Range(0, 20);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        // 物体がトリガーに接触しとき、１度だけ呼ばれる

        //プレイヤー側のラケットと当たったら
        if (collision.name == "Ball")
        {
            //Debug.Log("aaa");

            hitFlg = true;
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


        Debug.Log("pointB:"+ pointB.transform.position.x +":::"+ pointB.transform.position.z);
        if (mode ==2)
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

        if(mode == 4)
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
