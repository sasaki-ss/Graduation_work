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
    [SerializeField] Judgement judgement;
    [SerializeField] Ball ball;
    [SerializeField] public Shot Shot;
    //生成するゲームオブジェクト
    public GameObject racket;


    //前の座標と今の座標を比べるために使う変数
    Vector3 nowPosition;

    //理解はしてないけど3d空間上でのClick座標を取得するのに使う
    RaycastHit hit;

    int motionCnt = 0;
    bool swingFlg = false;
    void Start()
    {
        //ラケットの取得
        judgement = GameObject.Find("AIRacket").GetComponent<Judgement>();
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    public void Init()
    {
        player.transform.position = new Vector3(-105,0,0);
        motionCnt = 0;
        swingFlg = false;
        //ラケットの取得
        judgement = GameObject.Find("AIRacket").GetComponent<Judgement>();
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    void Update()
    {
        float dis = Vector3.Distance(this.GetComponent<NavMeshAgent>().transform.position, ball.transform.position);
        
        //nowUserTag
        if (ball.nowUserTag == "Player" && dis >= 10 && ball.transform.position.x<=-20)
        {
            //プレイヤーを走るモーションにする
            this.animator.SetBool("is_Run", true);

            //移動させる
            this.GetComponent<NavMeshAgent>().destination = ball.transform.position;
        }
        else
        {
            //現状の移動指定地を削除
            this.GetComponent<NavMeshAgent>().ResetPath();

            //円の大きさを測る
            this.CharaStatus.CharaCircle = Base.CircleScale(Shot.GetDistance);

            //プレイヤー状態を振るに変更
            this.CharaStatus.NowState = 2;
        }

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

        if (dis <=20)
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
        //if文おかしいけど現状はこのままで
        if (ball.nowUserTag == "Player" && swingFlg == true)
        {
            //Debug.Log(Shot.GetPower);
            //振る
            //パラメータちょこっと直接いじってる
            Base.Swing(CharaStatus.CharaPower *2.5f, Shot.GetPower + 15);

            racket.transform.position = new Vector3(0, -100, 0);
            judgement.HitFlg2 = false;
        }

        //Debug.Log(judgement.HitFlg2);

        //現在の座標を取得
        nowPosition = player.position;
    }
}
