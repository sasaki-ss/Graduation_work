using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�{�[���N���X
public class Ball : MonoBehaviour
{
    /*���̃X�N���v�g�ł̂ݎg���ϐ�*/
    private Rigidbody   rb;             //Rigidbody
    private Coroutine   coroutine;      //�R���[�`��
    private Vector3     endPoint;       //�I�_�n�_
    private Vector3     diff;           //����
    private int         nowShotUser;    //���ݑł��Ă��郆�[�U�[
    private int         colCoolTime;    //�����蔻��N�[���^�C��
    private float       flightTime;     //�؋󎞊�
    private float       speedRate;      //�؋󎞊Ԃ���Ƃ����ړ����x�{��
    private float       e;              //�����W��
    private bool        isBound;        //�o�E���h�t���O
    private bool        isProjection;   //���˃t���O
    private bool        isCoolTime;     //�N�[���^�C���t���O
    private bool        isSafetyArea;   //�Z�[�t�e�B�G���A�t���O

    /*�v���p�e�B�֘A*/
    public string       nowUserTag { get; private set; }    //�^�O
    public int          boundCount { get; private set; }    //�o�E���h��
    public bool         isOut { get; private set; }         //�A�E�g�t���O���擾
    public bool         isNet { get; private set; }         //�l�b�g�t���O

    /*�C���X�y�N�^�[�ɕ\�����͐ݒ肷��ϐ�*/
    private GameObject[]    userObj;        //���[�U�[�I�u�W�F�N�g
    [SerializeField]
    private GameObject      randingPoint;   //���n�n�_�I�u�W�F�N�g

    private void Awake()
    {
        //Rigidbody���擾
        rb = this.GetComponent<Rigidbody>();

        //User�I�u�W�F�N�g���擾
        userObj = new GameObject[2];
        userObj[0] = GameObject.Find("Player");
        userObj[1] = GameObject.Find("Player2");

        //PhysiceMaterial���擾
        SphereCollider sc = this.GetComponent<SphereCollider>();
        PhysicMaterial bound = sc.material;

        BoxCollider bc = GameObject.Find("SafetyArea").GetComponent<BoxCollider>();
        PhysicMaterial field = bc.material;

        //�擾����bound��field���甽���W�����Z�o
        e = (bound.bounciness + field.bounciness) / 2f;
    }

    //����������
    private void Start()
    {
        //�ł��Ă��郆�[�U�[�̏�����
        nowShotUser = 0;
        TagChange();

        Init();
    }

    //�������Z���s����ۂ̏���
    private void FixedUpdate()
    {
        if(colCoolTime == 10)
        {
            isCoolTime = false;
            colCoolTime = 0;
        }
        Debug.Log("�o�E���h�� : " + boundCount);

        //�l�b�g�ɓ������Ă�Ƃ�
        if (isNet)
        {
            //�R���[�`�����~����
            StopCoroutine(coroutine);
        }

        //�؋󎞊Ԃ�0.005f�ȉ��̏ꍇ�o�E���h���~������
        if (isBound && flightTime < 1f)
        {
            isBound = false;
        }

        //�o�E���h�̏���
        if (isBound && !isProjection)
        {
            //���B�n�_�A�؋󎞊ԁA���x�{���ɔ����W����������
            //�����@���͑����������Ă�
            endPoint += diff * e;
            flightTime *= e;
            speedRate *= e;
            coroutine = StartCoroutine(ProjectileMotion(endPoint, flightTime,
                speedRate, Physics.gravity.y));
        }

        if (isCoolTime) colCoolTime++;
    }

    private IEnumerator ProjectileMotion(Vector3 _endPoint, float _flightTime,
        float _speedRate, float _gravity)
    {
        Vector3 startPoint = this.transform.position;               //�����ʒu
        float diffY = (_endPoint - startPoint).y;                   //���_�ƏI�_��y�����̍���
        diff.x = (_endPoint - startPoint).x;
        diff.z = (_endPoint - startPoint).z;
        float vn = (diffY - _gravity * 0.5f *
            _flightTime * _flightTime) / _flightTime;               //���������̏����xvn

        isProjection = true;

        for (float t = 0f; t < _flightTime; t += (Time.deltaTime * _speedRate))
        {
            Vector3 p = Vector3.Lerp(startPoint, _endPoint,
                t / _flightTime);                                    //���������̍��W�����߂�(x,z���W)
            p.y = startPoint.y + vn * t + 0.5f * _gravity * t * t;  //���������̍��W y

            //���W���X�V����
            rb.MovePosition(p);

            yield return null;
        }
        isBound = true;
        isProjection = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isCoolTime) return;
        if (GameManager.instance.gameState == GameState.DuringRound)
        {
            //�{�[�����l�b�g�ɓ���������
            if (other.gameObject.CompareTag("Net"))
            {
                isNet = true;

                Debug.Log("����������");
            }

            //�{�[�����t�B�[���h�ɒ��n�����ۂ̏���
            if (other.gameObject.CompareTag("SafetyArea") ||
                other.gameObject.CompareTag("OutArea"))
            {
                //���n�n�_�𐶐�����
                GenerateRandingPoint();
                boundCount++;

                //�Z�[�t�G���A�̏ꍇ
                if (other.gameObject.CompareTag("SafetyArea")) isSafetyArea = true;

                //�A�E�g�G���A�̏ꍇ
                if (!isSafetyArea && boundCount == 1) isOut = true;

                isCoolTime = true;
            }
        }

        //�Q�[���̏�Ԃ��T�[�u�̏ꍇ
        if (GameManager.instance.gameState == GameState.Serve)
        {
            if (other.gameObject.CompareTag("ServeArea"))
            {
                Debug.Log("�T�[�u����");
                GameManager.instance.gameState = GameState.DuringRound;
                GameManager.instance.ChangeField();
                //boundCount++;

                isCoolTime = true;
            }
            else if (!other.gameObject.CompareTag("SwingArea"))
            {
                Debug.Log("�T�[�u���s");
            }
        }
    }

    //�ł���
    public void Strike(float _flightTime, float _speedRate)
    {
        if(GameManager.instance.gameState == GameState.Serve &&
            boundCount == 0 && isProjection && !isBound)
        {
            isOut = true;
        }

        //�^�O��؂�ւ���
        TagChange();

        //���B�n�_���X�V����
        LandingForecast lf = 
            GameObject.Find("RandingPointControl").GetComponent<LandingForecast>();
        lf.PointSetting();

        //�o�E���h�t���O���I�t��
        isBound = false;

        //�Z�[�t�e�B�t���O���I�t��
        isSafetyArea = false;

        //�Ε����˃t���O���I�t��
        if (isProjection)
        {
            isProjection = false;
            //�R���[�`�����~����
            StopCoroutine(coroutine);
        }

        //���B�n�_���擾
        endPoint = GameObject.Find("pointB").transform.position;

        //�؋󎞊Ԃ�Ball�N���X�Ɋi�[
        flightTime = _flightTime;

        //�؋󎞊Ԃ���Ƃ����ړ����x�{����Ball�N���X�Ɋi�[
        speedRate = _speedRate;

        //�Ε����˃R���[�`�����J�n
        coroutine = StartCoroutine(ProjectileMotion(endPoint, flightTime,
            speedRate, Physics.gravity.y));
    }

    public void Init()
    {
        boundCount = 0;
        colCoolTime = 0;
        isBound = false;
        isProjection = false;
        isOut = false;
        isNet = false;
        isSafetyArea = false;
        isCoolTime = false;
    }

    //�^�O�؂�ւ�����
    private void TagChange()
    {
        //���[�U�[��0�Ԗڂ̂Ƃ�
        if(nowShotUser == 0)
        {
            nowShotUser = 1;
        }
        //���[�U�[��1�Ԗڂ̂Ƃ�
        else
        {
            nowShotUser = 0;
        }

        //�^�O���w�肵�����[�U�[�֕ύX����
        nowUserTag = userObj[nowShotUser].name;
        //�o�E���h�񐔂����Z�b�g����
        boundCount = 0;

        Debug.Log("�o�E���h�񐔂����Z�b�g���܂��� " + Time.time);
    }

    //���n�n�_��������
    private void GenerateRandingPoint()
    {
        //���n�n�_�𐶐�
        GameObject instObject = Instantiate(randingPoint);

        //���n�n�_��ݒ�
        instObject.transform.position = new Vector3(this.transform.position.x, 0.02f,
            this.transform.position.z);
    }
}
