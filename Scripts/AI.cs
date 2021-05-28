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
    bool autoFlg = true;
    float dis       = 0;
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
        if (ball.nowUserTag == "Player" && ball.transform.position.x <= -7)
        {
            //������x�܂ŋ߂Â�����
            if (dis >= 40 && autoFlg == true)
            {
                Vector3 xyz = new Vector3(ball.transform.position.x - 60, ball.transform.position.y, ball.transform.position.z);

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
        if (dis <= 20)
        {
            this.animator.SetBool("is_RightShake", true);
        }

        //�U���Ԏ��Ȃ�50�J�E���g��ɑҋ@��Ԃɖ߂�
        if (this.animator.GetBool("is_RightShake") == true)
        {
            motionCnt++;

            if (motionCnt > 60)
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
            }
        }

        if (animator.GetBool("is_RightShake") == false)
        {
            swingFlg = false;
        }

        //�U�������P�b�g������������
        if (ball.nowUserTag == "Player" && hitFlg == true && swingFlg == true)
        {
            //�U��
            //�p�x�ɂ���Ĕ���
            //�X���C�v�̒����ɂ���Ĕ���


            //�p�����[�^���傱���ƒ��ڂ������Ă�
            Base.Swing(CharaStatus.CharaPower * 1.5f, Shot.GetPower + 10);

            hitFlg = false;

            autoFlg = true;
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
}
