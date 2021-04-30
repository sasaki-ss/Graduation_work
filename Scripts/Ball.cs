using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�{�[���N���X
public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;           //Rigidbody
    [SerializeField]
    private Vector3 endPoint;       //�I�_�n�_
    [SerializeField]
    private float flightTime;       //�؋󎞊�
    [SerializeField]
    private float speedRate;        //�؋󎞊Ԃ���Ƃ����ړ����x�{��

    [SerializeField]
    private float e;  //�����W��

    [SerializeField]
    private Vector3 diff;       //����

    private bool isNet;
    [SerializeField]
    private bool isShot;
    [SerializeField]
    private bool isBound;       //�o�E���h�t���O
    [SerializeField]
    private bool isProjection;  //���˃t���O   

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        SphereCollider sc = this.GetComponent<SphereCollider>();
        PhysicMaterial bound = sc.material;

        MeshCollider mc = GameObject.Find("Court_Base").GetComponent<MeshCollider>();
        PhysicMaterial field = mc.material;

        e = (bound.bounciness + field.bounciness) / 2f;

        isNet = false;
        isShot = false;
        isProjection = false;

    }

    //�������Z���s����ۂ̏���
    private void FixedUpdate()
    {
        if(isBound && flightTime < 0.005f)
        {
            isBound = false;
        }

        if (Input.GetMouseButtonDown(0) && !isShot)
        {
            isBound = false;
            isShot = true;

            endPoint = GameObject.Find("pointB").transform.position;

            StartCoroutine(ProjectileMotion(endPoint, flightTime,
                speedRate, Physics.gravity.y));
        }

        if (isBound && !isProjection)
        {
            endPoint += diff * e;
            flightTime *= e;
            speedRate *= e;
            StartCoroutine(ProjectileMotion(endPoint, flightTime,
                speedRate, Physics.gravity.y));
        }
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

        for(float t = 0f; t < _flightTime; t += (Time.deltaTime * _speedRate))
        {
            if (isNet) yield break;

            Vector3 p = Vector3.Lerp(startPoint, _endPoint,
                t / _flightTime);                                    //���������̍��W�����߂�(x,z���W)
            p.y = startPoint.y + vn * t + 0.5f * _gravity * t * t;  //���������̍��W y

            rb.MovePosition(p);
            //transform.position = p;

            yield return null;
        }
        isShot = false;
        isBound = true;
        isProjection = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Net"))
        {
            Debug.Log("����������");
            isNet = true;
        }

        if(other.gameObject.CompareTag("Field"))
        {
            Debug.Log("�n�ʒ��n");
        }
    }
}
