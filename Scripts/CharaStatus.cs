using UnityEngine;
using UnityEngine.AI;

public class CharaStatus : MonoBehaviour
{
    //キャラクターの各ステータス(現在は三個)

    //パワー
    private double charaPower = 10;
    //スタミナ
    private double charaStamina = 10;
    //スピード
    private double charaSpeed = 50;
    //ラケットを振ったフラグ
    private bool racketFlg = false; 

    private double maxStamina =10;

    //キャラのNavMeshAgent
    [SerializeField] NavMeshAgent agent;

    //パワーのプロパティー
    public double CharaPower
    {
        get { return this.charaPower; }
        set { this.charaPower = value; }
    }

    //スタミナのプロパティー
    public double CharaStamina
    {
        get { return this.charaStamina; }
        set { this.charaStamina = value; }
    }

    //スピードのプロパティー
    public double CharaSpeed
    {
        get { return this.charaSpeed; }
        set { this.charaSpeed = value; }
    }

    //ラケットフラグのプロパティー
    public bool RacketFlg
    {
        get { return this.racketFlg; }
        set { this.racketFlg = value; }
    }

    void Start()
    {
        //移動速度設定
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //Debug.Log("キャラステータス：　パワー："+charaPower+"　スタミナ:"+ charaStamina + "　スピード:" + charaSpeed);

        //スタミナが無い
        if (charaStamina <= maxStamina / 10 * 0)
        {
            charaSpeed = 0;
            //Debug.Log("すたみな0割：");
        }
        //スタミナが1割の時の移動速度
        else if (charaStamina >= maxStamina / 10 * 1 && charaStamina < maxStamina / 10 * 3)
        {
            charaSpeed = 10;
            //Debug.Log("すたみな1割：");
        }
        //スタミナが3割の時の移動速度
        else if(charaStamina >= maxStamina / 10 * 3 && charaStamina < maxStamina / 10 * 5)
        {
            charaSpeed = 25;
            //Debug.Log("すたみな3割：");
        }
        //スタミナが5割の時の移動速度
        else if(charaStamina >= maxStamina / 10 * 5 && charaStamina < maxStamina / 10 * 8)
        {
            //Debug.Log("すたみな5割：");
            charaSpeed = 40;
        }
        //スタミナが8割の時の移動速度
        else if(charaStamina >= maxStamina / 10 * 8 && charaStamina < maxStamina / 10 * 10)
        {
            //Debug.Log("すたみな8割：");
            charaSpeed = 50;
        }

        if (charaStamina < 0) charaStamina = 0;

        agent.speed = (float)charaSpeed;
    }
}
