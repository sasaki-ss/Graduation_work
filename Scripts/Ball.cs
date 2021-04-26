using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�{�[���N���X
public class Ball : MonoBehaviour
{
    [SerializeField]
    private float altitudeReach;    //���B���x
    [SerializeField]
    private float ReachDistance;    //���B����
    [SerializeField]
    private float launchAltitude;   //�ŏo���x
    [SerializeField]
    private float gravity;          //�d�͉����x

    [SerializeField]
    private float flightTime;       //�؋󎞊�
    [SerializeField]
    private float initialVelocity;  //�����x

    private void Start()
    {
        launchAltitude = this.transform.position.y;

        gravity = Physics.gravity.y;

        flightTime = (Mathf.Sqrt(2 * -gravity * (altitudeReach - launchAltitude)) +
            Mathf.Sqrt(2 * -gravity * altitudeReach)) / -gravity;

        initialVelocity = Mathf.Sqrt(((ReachDistance / flightTime) * (ReachDistance / flightTime)) +
            2 * -gravity * (altitudeReach - launchAltitude));

        Debug.Log("�؋󎞊� = " + flightTime);
    }

    private void Update()
    {
        launchAltitude = this.transform.position.y;
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("�����܂���");
        }
    }
}
