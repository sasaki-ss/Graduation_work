using UnityEngine;

public class CharaStatus : MonoBehaviour
{
    //�L�����N�^�[�̊e�X�e�[�^�X(���݂͎O��)

    //�p���[
    private double charaPower = 10;
    //�X�^�~�i
    private double charaStamina = 10;
    //�X�s�[�h
    private double charaSpeed = 10;

    //�p���[�̃v���p�e�B�[
    public double CharaPower
    {
        get { return this.charaPower; }
        set { this.charaPower = value; }
    }

    //�X�^�~�i�̃v���p�e�B�[
    public double CharaStamina
    {
        get { return this.charaStamina; }
        set { this.charaStamina = value; }
    }

    //�X�s�[�h�̃v���p�e�B�[
    public double CharaSpeed
    {
        get { return this.charaSpeed; }
        set { this.charaSpeed = value; }
    }
    void Update()
    {
        Debug.Log("�L�����X�e�[�^�X�F�@�p���[�F"+charaPower+"�@�X�^�~�i:"+ charaStamina + "�@�X�s�[�h:" + charaSpeed);
    }
}
