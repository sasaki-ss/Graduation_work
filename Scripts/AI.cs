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
     bool autoFlg   = true;
    float dis       = 0;
      int miss      = 0;  //移動をしない
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
        miss = 0;
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
        //アウトかどうかで移動しないのも入れたい
        if (miss != 7 && ball.nowUserTag == "Player" && ball.transform.position.x <= -7) 
        {
            //ある程度まで近づいたら
            if (dis >= 10 && autoFlg == true)
            {
                Vector3 xyz = new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.z);

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
        if (dis <=50)
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

            parameter = TargetPoint(Shot.GetPower, CharaStatus.Distance, CharaStatus.Rad);
            CharaStatus.Rad = parameter.x;
            CharaStatus.Distance = parameter.y;
            //パラメータちょこっと直接いじってる
            //Debug.Log(parameter.x + ":::"+ parameter.y);
            Base.Swing(CharaStatus.CharaPower * 1.5f, Shot.GetPower + 10);

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

    private Vector2 TargetPoint(double _shotPower, double _distance, double _rad)
    {
        float targetPointX = 0;
        float targetPointY = 0;

        int mode　= Random.Range(1, 4);
        int pattern = Random.Range(1, 3);

        if (mode == 2)
        {
            //値によって選ばれやすさの変更をする
            if (_rad > 1.0 && _rad < 1.2)
            {
                pattern = Random.Range(1, 1);
            }
        }

        if (mode == 3)
        {
            //値によって選ばれやすさの変更をする
            if (_shotPower > 31 && _shotPower < 34)
            {
                pattern = Random.Range(3, 3);
            }
            else
            if (_shotPower > 113 && _shotPower < 115)
            {
                pattern = Random.Range(2, 2);
            }

        }

        if (pattern == 1)
        {
            //滞空時間によって変動
            if (_shotPower > 0 && _shotPower < 30)
            {
                targetPointX = 1f;
                targetPointY = 270 * 3f;
            }
            else
            if (_shotPower > 30 && _shotPower < 60)
            {
                targetPointX = 2f;
                targetPointY = 270 * 4.5f;
            }
            else
            if (_shotPower > 60 && _shotPower < 90)
            {
                targetPointX = 1.8f;
                targetPointY = 270 * 1.3f;
            }
            else
            if (_shotPower > 90 && _shotPower < 120)
            {
                targetPointX = 1.5f;
                targetPointY = 270 * 2.5f;
            }
            else
            {
                targetPointX = 1.8f;
                targetPointY = 270 * 2.6f;
            }
        }



        if (pattern == 2)
        {
            //距離によって変動
            if (_distance > 0 && _distance < 30)
            {
                targetPointX = 0.5f;
                targetPointY = 270 * 2.5f;
            }
            else
            if (_distance > 30 && _distance < 60)
            {
                targetPointX = 2.5f;
                targetPointY = 270 * 1.5f;
            }
            else
            if (_distance > 60 && _distance < 90)
            {
                targetPointX = 0.75f;
                targetPointY = 270 * 3.5f;
            }
            else
            if (_distance > 90 && _distance < 120)
            {
                targetPointX = 0.9f;
                targetPointY = 270 * 4f;
            }
            else
            if (_distance > 120 && _distance < 150)
            {
                targetPointX = 1.5f;
                targetPointY = 270 * 2f;
            }
            else
            if (_distance > 150 && _distance < 180)
            {
                targetPointX = 1.8f;
                targetPointY = 270 * 1.8f;
            }
            else
            if (_distance > 180 && _distance < 210)
            {
                targetPointX = 1.8f;
                targetPointY = 270 * 3f;
            }
            else
            if (_distance > 210 && _distance < 240)
            {
                targetPointX = 1.8f;
                targetPointY = 270 * 1.4f;
            }
            else
            if (_distance > 240 && _distance < 270)
            {
                targetPointX = 2f;
                targetPointY = 270 * 2.5f;
            }
            else
            if (_distance > 270 && _distance < 300)
            {
                targetPointX = 2.2f;
                targetPointY = 270 * 1.5f;
            }
            else
            if (_distance > 300 && _distance < 330)
            {
                targetPointX = 1.6f;
                targetPointY = 270 * 1.9f;
            }
            else
            {
                targetPointX = 1.5f;
                targetPointY = 270 * 2f;
            }
        }



        if (pattern == 3)
        {
            //ラジアン値によって変動
            if (_rad > 0 && _rad < 0.5)
            {
                targetPointX = 3f;
                targetPointY = 270 * 4f;
            }
            else
            if (_rad > 0.5 && _rad < 1)
            {
                targetPointX = 0f;
                targetPointY = 270 * 3f;
            }
            else
            if (_rad > 1 && _rad < 1.5)
            {
                targetPointX = 1.5f;
                targetPointY = 270 * 5f;
            }
            else
            if (_rad > 1.5 && _rad < 1.75)
            {
                targetPointX = 1.9f;
                targetPointY = 270 * 4.3f;
            }
            else
            if (_rad > 1.75 && _rad < 2)
            {
                targetPointX = 0.6f;
                targetPointY = 270 * 3f;
            }
            else
            {
                targetPointX = 2.1f;
                targetPointY = 270 * 4f;
            }
        }

        if (mode == 4)
        {

            targetPointX = Random.Range(1, 25) / 10;
            targetPointY = 270 * Random.Range(10, 35) / 10;
        }

        Vector2 targetPoint = new Vector2(targetPointX, targetPointY);

        Debug.Log("mode"+mode);
        Debug.Log("pattern"+pattern);
        Debug.Log("rad" + targetPointX);
        Debug.Log("dis" + targetPointY);

        return targetPoint;
    }
}
