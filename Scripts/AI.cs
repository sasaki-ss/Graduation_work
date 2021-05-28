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

    //前の座標と今の座標を比べるために使う変数
    Vector3 nowPosition;

    //理解はしてないけど3d空間上でのClick座標を取得するのに使う
    RaycastHit hit;

      int motionCnt = 0;
     bool swingFlg  = false;
     bool hitFlg    = false;
    bool autoFlg = true;
    float dis       = 0;
    void Start()
    {
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    public void Init()
    {
        player.transform.position = new Vector3(-105,0,0);
        motionCnt = 0;
        autoFlg = true;
        swingFlg = false;
        hitFlg = false;
        dis = 0;
        //if文でこっちがサーブなのか判定してから
        /*
        if()
        {
           //対角線上に配置する予定
           player.transform.position = new Vector3(-105,0,0);
        }
        */

        Shot = GameObject.Find("Shot").GetComponent<Shot>();
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
    }

    void AutoMove()
    {
        //オート移動処理
        if (ball.nowUserTag == "Player" && ball.transform.position.x <= -7)
        {
            //ある程度まで近づいたら
            if (dis >= 40 && autoFlg == true)
            {
                Vector3 xyz = new Vector3(ball.transform.position.x - 60, ball.transform.position.y, ball.transform.position.z);

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
        if (dis <= 20)
        {
            this.animator.SetBool("is_RightShake", true);
        }

        //振る状態時なら50カウント後に待機状態に戻す
        if (this.animator.GetBool("is_RightShake") == true)
        {
            motionCnt++;

            if (motionCnt > 60)
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
            }
        }

        if (animator.GetBool("is_RightShake") == false)
        {
            swingFlg = false;
        }

        //振ったラケットが当たったら
        if (ball.nowUserTag == "Player" && hitFlg == true && swingFlg == true)
        {
            //振る
            //角度によって判定
            //スワイプの長さによって判定


            //パラメータちょこっと直接いじってる
            Base.Swing(CharaStatus.CharaPower * 1.5f, Shot.GetPower + 10);

            hitFlg = false;

            autoFlg = true;
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
}
