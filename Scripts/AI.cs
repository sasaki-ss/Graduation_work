using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    //���ʂ̂��
    [SerializeField] Base Base;

    //�e�X�̂��
    [SerializeField] Animator animator;
    [SerializeField] CharaStatus CharaStatus;
    [SerializeField] Transform player;
    [SerializeField] Ball ball;
    [SerializeField] GameObject net;
    [SerializeField] public Shot Shot;

    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

      int motionCnt = 0;
     bool swingFlg  = false;
     bool hitFlg    = false;
     bool autoFlg   = true;
    float dis       = 0;
      int miss      = 0;  //�ړ������Ȃ�
    void Start()
    {
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    public void Init()
    {
        player.transform.position = new Vector3(-105,0,0);
        motionCnt = 0;
        autoFlg = true;
        swingFlg = false;
        hitFlg = false;
        dis = 0;
        miss = 0;
        //if���ł��������T�[�u�Ȃ̂����肵�Ă���
        /*
        if()
        {
           //�Ίp����ɔz�u����\��
           player.transform.position = new Vector3(-105,0,0);
        }
        */

        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    void Update()
    {
        //�L�����ƃ{�[���̋����𑪂�
        dis = Vector3.Distance(this.GetComponent<NavMeshAgent>().transform.position, ball.transform.position);

        //�����ړ����̏���
        AutoMove();

        //�ړ��������肷�鏈��
        JudgeMove();

        //���P�b�g��U�������̏���
        Swing();

        //���݂̍��W���擾
        nowPosition = player.position;
    }

    void AutoMove()
    {
        //�I�[�g�ړ�����
        //�A�E�g���ǂ����ňړ����Ȃ��̂����ꂽ��
        if (miss != 7 && ball.nowUserTag == "Player" && ball.transform.position.x <= -7) 
        {
            //������x�܂ŋ߂Â�����
            if (dis >= 10 && autoFlg == true)
            {
                Vector3 xyz = new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.z);

                //�ړ�������
                GetComponent<NavMeshAgent>().destination = xyz;

                //�����ړ��͈��̂�
                autoFlg = false;
            }
        }
    }

    void JudgeMove()
    {
        //�ړ������ǂ���
        if (Base.PositionJudge(player.position, nowPosition))
        {
            //�v���C���[�𑖂郂�[�V�����ɂ���
            this.animator.SetBool("is_Run", true);

            //�v���C���[��Ԃ��ړ��ɕύX
            this.CharaStatus.NowState = 1;

            //�v���C���[�̃X�^�~�i�����炷
            this.CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
        }
        else
        {
            //�v���C���[��ҋ@���[�V�����ɂ���
            this.animator.SetBool("is_Run", false);

            //�v���C���[��Ԃ�ҋ@�ɕύX
            this.CharaStatus.NowState = 0;
        }

        if (animator.GetBool("is_Run") == false)
        {
            //�X�s�[�h�����߂�
            float speed = 0.1f;

            //�����Ƒ���Ƃ̃x�N�g�������擾
            Vector3 relativePos = net.transform.position - player.transform.position;

            //��������]���ɂ���
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            //�ΏۂɌ�������
            player.rotation = Quaternion.Slerp(this.transform.rotation, rotation, speed);
        }
    }

    void Swing()
    {
        if (dis <=50)
        {
            this.animator.SetBool("is_RightShake", true);
        }

        //�U���Ԏ��Ȃ�50�J�E���g��ɑҋ@��Ԃɖ߂�
        if (this.animator.GetBool("is_RightShake") == true)
        {
            motionCnt++;

            if (motionCnt > 0)
            {
                swingFlg = true;
            }

            if (motionCnt > 140)
            {
                motionCnt = 0;

                //�v���C���[��Ԃ�ҋ@�ɕύX
                this.CharaStatus.NowState = 0;

                //�v���C���[��ҋ@���[�V�����ɂ���
                this.animator.SetBool("is_RightShake", false);
                swingFlg = false;
            }
        }

        //�U�������P�b�g������������
        if (ball.nowUserTag == "Player" && hitFlg == true && swingFlg == true)
        {
            //�U��
            //�p�x�ɂ���Ĕ���
            //�X���C�v�̒����ɂ���Ĕ���
            //�v���C���[���猩���Ƃ�
            /*
             * 0.5 :����M��
             * 2.5 :�E��M��
             */

            Vector2 parameter;

            parameter = TargetPoint(Shot.GetPower, CharaStatus.Distance, CharaStatus.Rad);
            CharaStatus.Rad = parameter.x;
            CharaStatus.Distance = parameter.y;
            //�p�����[�^���傱���ƒ��ڂ������Ă�
            //Debug.Log(parameter.x + ":::"+ parameter.y);
            Base.Swing(CharaStatus.CharaPower * 1.5f, Shot.GetPower + 10);

            hitFlg = false;

            autoFlg = true;

            miss = Random.Range(0, 20);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        // ���̂��g���K�[�ɐڐG���Ƃ��A�P�x�����Ă΂��

        //�v���C���[���̃��P�b�g�Ɠ���������
        if (collision.name == "Ball")
        {
            //Debug.Log("aaa");

            hitFlg = true;
        }
    }

    private Vector2 TargetPoint(double _shotPower, double _distance, double _rad)
    {
        float targetPointX = 0;
        float targetPointY = 0;

        int mode�@= Random.Range(1, 4);
        int pattern = Random.Range(1, 3);

        if (mode == 2)
        {
            //�l�ɂ���đI�΂�₷���̕ύX������
            if (_rad > 1.0 && _rad < 1.2)
            {
                pattern = Random.Range(1, 1);
            }
        }

        if (mode == 3)
        {
            //�l�ɂ���đI�΂�₷���̕ύX������
            if (_shotPower > 31 && _shotPower < 34)
            {
                pattern = Random.Range(3, 3);
            }
            else
            if (_shotPower > 113 && _shotPower < 115)
            {
                pattern = Random.Range(2, 2);
            }

        }

        if (pattern == 1)
        {
            //�؋󎞊Ԃɂ���ĕϓ�
            if (_shotPower > 0 && _shotPower < 30)
            {
                targetPointX = 1f;
                targetPointY = 270 * 3f;
            }
            else
            if (_shotPower > 30 && _shotPower < 60)
            {
                targetPointX = 2f;
                targetPointY = 270 * 4.5f;
            }
            else
            if (_shotPower > 60 && _shotPower < 90)
            {
                targetPointX = 1.8f;
                targetPointY = 270 * 1.3f;
            }
            else
            if (_shotPower > 90 && _shotPower < 120)
            {
                targetPointX = 1.5f;
                targetPointY = 270 * 2.5f;
            }
            else
            {
                targetPointX = 1.8f;
                targetPointY = 270 * 2.6f;
            }
        }



        if (pattern == 2)
        {
            //�����ɂ���ĕϓ�
            if (_distance > 0 && _distance < 30)
            {
                targetPointX = 0.5f;
                targetPointY = 270 * 2.5f;
            }
            else
            if (_distance > 30 && _distance < 60)
            {
                targetPointX = 2.5f;
                targetPointY = 270 * 1.5f;
            }
            else
            if (_distance > 60 && _distance < 90)
            {
                targetPointX = 0.75f;
                targetPointY = 270 * 3.5f;
            }
            else
            if (_distance > 90 && _distance < 120)
            {
                targetPointX = 0.9f;
                targetPointY = 270 * 4f;
            }
            else
            if (_distance > 120 && _distance < 150)
            {
                targetPointX = 1.5f;
                targetPointY = 270 * 2f;
            }
            else
            if (_distance > 150 && _distance < 180)
            {
                targetPointX = 1.8f;
                targetPointY = 270 * 1.8f;
            }
            else
            if (_distance > 180 && _distance < 210)
            {
                targetPointX = 1.8f;
                targetPointY = 270 * 3f;
            }
            else
            if (_distance > 210 && _distance < 240)
            {
                targetPointX = 1.8f;
                targetPointY = 270 * 1.4f;
            }
            else
            if (_distance > 240 && _distance < 270)
            {
                targetPointX = 2f;
                targetPointY = 270 * 2.5f;
            }
            else
            if (_distance > 270 && _distance < 300)
            {
                targetPointX = 2.2f;
                targetPointY = 270 * 1.5f;
            }
            else
            if (_distance > 300 && _distance < 330)
            {
                targetPointX = 1.6f;
                targetPointY = 270 * 1.9f;
            }
            else
            {
                targetPointX = 1.5f;
                targetPointY = 270 * 2f;
            }
        }



        if (pattern == 3)
        {
            //���W�A���l�ɂ���ĕϓ�
            if (_rad > 0 && _rad < 0.5)
            {
                targetPointX = 3f;
                targetPointY = 270 * 4f;
            }
            else
            if (_rad > 0.5 && _rad < 1)
            {
                targetPointX = 0f;
                targetPointY = 270 * 3f;
            }
            else
            if (_rad > 1 && _rad < 1.5)
            {
                targetPointX = 1.5f;
                targetPointY = 270 * 5f;
            }
            else
            if (_rad > 1.5 && _rad < 1.75)
            {
                targetPointX = 1.9f;
                targetPointY = 270 * 4.3f;
            }
            else
            if (_rad > 1.75 && _rad < 2)
            {
                targetPointX = 0.6f;
                targetPointY = 270 * 3f;
            }
            else
            {
                targetPointX = 2.1f;
                targetPointY = 270 * 4f;
            }
        }

        if (mode == 4)
        {

            targetPointX = Random.Range(1, 25) / 10;
            targetPointY = 270 * Random.Range(10, 35) / 10;
        }

        Vector2 targetPoint = new Vector2(targetPointX, targetPointY);

        Debug.Log("mode"+mode);
        Debug.Log("pattern"+pattern);
        Debug.Log("rad" + targetPointX);
        Debug.Log("dis" + targetPointY);

        return targetPoint;
    }
}
