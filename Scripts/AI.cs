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
    [SerializeField] Judgement judgement;
    [SerializeField] Ball ball;
    [SerializeField] GameObject net;
    [SerializeField] public Shot Shot;

    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

      int motionCnt = 0;
     bool swingFlg  = false;
    float dis       = 0;
    void Start()
    {
        //���P�b�g�̎擾
        judgement = GameObject.Find("AIRacket").GetComponent<Judgement>();
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    public void Init()
    {
        player.transform.position = new Vector3(-105,0,0);
        motionCnt = 0;
        swingFlg = false;
        dis = 0;
        //if���ł��������T�[�u�Ȃ̂����肵�Ă���
        /*
        if()
        {
           //�Ίp����ɔz�u����\��
           player.transform.position = new Vector3(-105,0,0);
        }
        */

        //���P�b�g�̎擾
        judgement = GameObject.Find("AIRacket").GetComponent<Judgement>();
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
        //nowUserTag
        if (ball.nowUserTag == "Player" && dis >= 50 && ball.transform.position.x <= -20)
        {
            //�v���C���[�𑖂郂�[�V�����ɂ���
            this.animator.SetBool("is_Run", true);

            //�ړ�������
            this.GetComponent<NavMeshAgent>().destination = ball.transform.position;
        }
        else
        {
            //����̈ړ��w��n���폜
            this.GetComponent<NavMeshAgent>().ResetPath();

            //�~�̑傫���𑪂�
            this.CharaStatus.CharaCircle = Base.CircleScale(Shot.GetDistance);

            //�v���C���[��Ԃ�U��ɕύX
            this.CharaStatus.NowState = 2;
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
        if (dis <= 15)
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
        //if�������������ǌ���͂��̂܂܂�
        if (ball.nowUserTag == "Player" && swingFlg == true)
        {
            //�U��
            //�p�����[�^���傱���ƒ��ڂ������Ă�
            Base.Swing(CharaStatus.CharaPower * 1.5f, Shot.GetPower + 10);

            judgement.HitFlg2 = false;
        }
    }
}
