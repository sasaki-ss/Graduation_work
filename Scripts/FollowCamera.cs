using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform player;


    void Update()
    {
        // ç¿ïWÇéÊìæ
        Vector3 pos = player.position;
        transform.position = player.position;
        pos.z = transform.position.z - 10;
        pos.y = transform.position.y + 10;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(45,0, 0);
    }
}
