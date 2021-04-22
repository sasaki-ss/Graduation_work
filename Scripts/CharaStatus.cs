using UnityEngine;

public class CharaStatus : MonoBehaviour
{
    //キャラクターの各ステータス(現在は三個)

    //パワー
    private double charaPower = 10;
    //スタミナ
    private double charaStamina = 10;
    //スピード
    private double charaSpeed = 10;

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
    void Update()
    {
        Debug.Log("キャラステータス：　パワー："+charaPower+"　スタミナ:"+ charaStamina + "　スピード:" + charaSpeed);
    }
}
