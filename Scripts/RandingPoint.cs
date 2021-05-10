using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandingPoint : MonoBehaviour
{
    [SerializeField]
    private float alpha;        //���l
    [SerializeField]
    private float red;          //��
    [SerializeField]
    private float green;        //��
    [SerializeField]
    private float blue;         //��
    [SerializeField]
    private float maxTime;      //�ő厞��
    [SerializeField]
    private float fedeOutSpeed; //�t�F�C�h�A�E�g���鑬�x

    private void Start()
    {
        //�e�l���擾����
        red = GetComponent<SpriteRenderer>().color.r;
        green = GetComponent<SpriteRenderer>().color.g;
        blue = GetComponent<SpriteRenderer>().color.b;
        alpha = GetComponent<SpriteRenderer>().color.a;

        //�t�F�C�h�A�E�g���鑬�x
        fedeOutSpeed = maxTime / 255f;
    }

    private void Update()
    {
        //���l���t�F�C�h�A�E�g���鑬�x�����炷
        alpha -= fedeOutSpeed;

        //���l��0�ȉ��̏ꍇ���̃I�u�W�F�N�g���폜
        if(alpha <= 0)
        {
            Destroy(this.gameObject);
        }

        //�F��K�p����
        GetComponent<SpriteRenderer>().color = new Color(red, green, blue, alpha);
    }
}
