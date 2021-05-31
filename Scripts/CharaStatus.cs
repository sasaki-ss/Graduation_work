using UnityEngine;
using UnityEngine.AI;

public class CharaStatus : MonoBehaviour
{
    //�L�����N�^�[�̊e�X�e�[�^�X(���݂͎O��)

    //�p���[
    private double charaPower = 10;
    //�X�^�~�i
    private double charaStamina = 10;
    //�X�s�[�h
    private double charaSpeed = 50;
    //�~�̑傫��
    private int charaCircle = 0;

    //�v���C���[�̏��(���[�V�����ł�)
    private int nowState = 0;

    private double rad = 0;
    private double distance = 0;
    public double Rad
    {
        get { return this.rad; }
        set { this.rad = value; }
    }

    public double Distance
    {
        get { return this.distance; }
        set { this.distance = value; }
    }

    private double maxStamina =10;

    //�L������NavMeshAgent
    [SerializeField] NavMeshAgent agent;

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
    //�~�̑傫���̃v���p�e�B�[
    public int CharaCircle
    {
        get { return this.charaCircle; }
        set { this.charaCircle = value; }
    }

    //�v���C���[�̏�Ԃ̃v���p�e�B�[  0:�ʏ� 1:�ړ� 2:�U��
    public int NowState
    {
        get { return this.nowState; }
        set { this.nowState = value; }
    }

    void Start()
    {
        //�ړ����x�ݒ�
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //Debug.Log("�L�����X�e�[�^�X�F�@�p���[�F"+charaPower+"�@�X�^�~�i:"+ charaStamina + "�@�X�s�[�h:" + charaSpeed);

        //�X�^�~�i������
        if (charaStamina <= maxStamina / 10 * 0)
        {
            charaSpeed = 0;
            //Debug.Log("�����݂�0���F");
        }
        //�X�^�~�i��1���̎��̈ړ����x
        else if (charaStamina >= maxStamina / 10 * 1 && charaStamina < maxStamina / 10 * 3)
        {
            charaSpeed = 10;
            //Debug.Log("�����݂�1���F");
        }
        //�X�^�~�i��3���̎��̈ړ����x
        else if(charaStamina >= maxStamina / 10 * 3 && charaStamina < maxStamina / 10 * 5)
        {
            charaSpeed = 25;
            //Debug.Log("�����݂�3���F");
        }
        //�X�^�~�i��5���̎��̈ړ����x
        else if(charaStamina >= maxStamina / 10 * 5 && charaStamina < maxStamina / 10 * 8)
        {
            //Debug.Log("�����݂�5���F");
            charaSpeed = 40;
        }
        //�X�^�~�i��8���̎��̈ړ����x
        else if(charaStamina >= maxStamina / 10 * 8 && charaStamina < maxStamina / 10 * 10)
        {
            //Debug.Log("�����݂�8���F");
            charaSpeed = 50;
        }

        //0�Ŏ~�߂�
        if (charaStamina < 0) charaStamina = 0;

        //�X�s�[�h��ݒ�
        agent.speed = (float)charaSpeed;
    }
}
