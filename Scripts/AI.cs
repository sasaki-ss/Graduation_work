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
    [SerializeField] public Shot Shot;
    //��������Q�[���I�u�W�F�N�g
    public GameObject racket;


    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

    int motionCnt = 0;

    void Start()
    {
        //���P�b�g�̎擾
        judgement = GameObject.Find("AIRacket").GetComponent<Judgement>();
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    void Update()
    {
        float dis = Vector3.Distance(this.GetComponent<NavMeshAgent>().transform.position, ball.transform.position);
        
        //nowUserTag
        if (ball.nowUserTag == "Player" && dis >= 10 && ball.transform.position.x<=-20)
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

            racket.transform.position = new Vector3(player.position.x, player.position.y + 1, player.position.z);

            //�~�̑傫���𑪂�
            this.CharaStatus.CharaCircle = Base.CircleScale(Shot.GetDistance);

            //�v���C���[��Ԃ�U��ɕύX
            this.CharaStatus.RacketSwing = 2;
        }

        //�ړ������ǂ���
        if (Base.PositionJudge(player.position, nowPosition))
        {
            //�v���C���[�𑖂郂�[�V�����ɂ���
            this.animator.SetBool("is_Run", true);

            //�v���C���[��Ԃ��ړ��ɕύX
            this.CharaStatus.RacketSwing = 1;

            //�v���C���[�̃X�^�~�i�����炷
            this.CharaStatus.CharaStamina = CharaStatus.CharaStamina - 0.001f;
        }
        else
        {
            //�v���C���[��ҋ@���[�V�����ɂ���
            this.animator.SetBool("is_Run", false);
            //�v���C���[��Ԃ�ҋ@�ɕύX
            this.CharaStatus.RacketSwing = 0;
        }

        if(dis <=50)
        {
            this.animator.SetBool("is_RightShake", true);
        }

        //�U���Ԏ��Ȃ�50�J�E���g��ɑҋ@��Ԃɖ߂�
        if (this.animator.GetBool("is_RightShake") == true)
        {
            motionCnt++;

            if (motionCnt > 50)
            {
                motionCnt = 0;

                //�v���C���[��Ԃ�ҋ@�ɕύX
                this.CharaStatus.RacketSwing = 0;

                //�v���C���[��ҋ@���[�V�����ɂ���
                this.animator.SetBool("is_RightShake", false);
            }
        }
        //nowUserTag
        //�U�������P�b�g������������
        //if�������������ǌ���͂��̂܂܂�
        if (ball.nowUserTag == "Player" && dis <= 10)
        {
            Debug.Log(Shot.GetPower);
            //�U��
            //�p�����[�^���傱���ƒ��ڂ������Ă�
            Base.Swing(CharaStatus.CharaPower * 1.5f, Shot.GetPower * 5);

            racket.transform.position = new Vector3(0, -100, 0);
            judgement.HitFlg2 = false;
        }

        //Debug.Log(judgement.HitFlg2);

        //���݂̍��W���擾
        nowPosition = player.position;
    }
}
