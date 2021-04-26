using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボールクラス
public class Ball : MonoBehaviour
{
    [SerializeField]
    private float altitudeReach;    //到達高度
    [SerializeField]
    private float ReachDistance;    //到達距離
    [SerializeField]
    private float launchAltitude;   //打出高度
    [SerializeField]
    private float gravity;          //重力加速度

    [SerializeField]
    private float flightTime;       //滞空時間
    [SerializeField]
    private float initialVelocity;  //初速度

    private void Start()
    {
        launchAltitude = this.transform.position.y;

        gravity = Physics.gravity.y;

        flightTime = (Mathf.Sqrt(2 * -gravity * (altitudeReach - launchAltitude)) +
            Mathf.Sqrt(2 * -gravity * altitudeReach)) / -gravity;

        initialVelocity = Mathf.Sqrt(((ReachDistance / flightTime) * (ReachDistance / flightTime)) +
            2 * -gravity * (altitudeReach - launchAltitude));

        Debug.Log("滞空時間 = " + flightTime);
    }

    private void Update()
    {
        launchAltitude = this.transform.position.y;
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("撃ちました");
        }
    }
}
