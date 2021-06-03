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
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject pointB;

    //�O�̍��W�ƍ��̍��W���ׂ邽�߂Ɏg���ϐ�
    Vector3 nowPosition;

    //�����͂��ĂȂ�����3d��ԏ�ł�Click���W���擾����̂Ɏg��
    RaycastHit hit;

      int motionCnt = 0;
     bool swingFlg  = false;
     bool hitFlg    = false;
     bool autoFlg   = true;
    float dis = 0;
    
    //���󓮂��Ȃ�
    bool boundFlg = true;

    int miss      = 0;  //�ړ������Ȃ�
    void Start()
    {
        Shot = GameObject.Find("Shot").GetComponent<Shot>();
    }

    public void Init()
    {
        //����̈ړ��w��n���폜
        GetComponent<NavMeshAgent>().ResetPath();


        player.transform.position = new Vector3(-150,0,0);
        motionCnt = 0;
        autoFlg = true;
        boundFlg = true;
        swingFlg = false;
        hitFlg = false;
        dis = 0;
        miss = 0;

        //if���ł��������T�[�u�Ȃ̂����肵�Ă���
        if (ball.nowUserTag == "Player")
        {
            //�Ίp����ɔz�u����\��
            player.transform.position = new Vector3(-105, 0, -25);
        }
        else
        {
            //�Ίp����ɔz�u����\��
            player.transform.position = new Vector3(-105, 0, 25);
        }

        Shot = GameObject.Find("Shot").GetComponent<Shot>();

        Base.InitCnt += 1;
        Debug.Log("AI");
    }

    void Update()
    {
        if (Base.InitCnt == 1) 
        {
            Init();
        }

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
        //Debug.Log(ball.boundCount);

        if (ball.boundCount != 0)
        {
            boundFlg = false;
        }

        //�I�[�g�ړ�����
        if (ball.boundCount != 0 && pointB.transform.position.x > -200 && pointB.transform.position.x < 7)
        {


            if (autoFlg == true)
            {

                Vector3 xyz = new Vector3(-110, 0, 0);

                //�v���C���[�̃R�[�g�̍���
                if (pointB.transform.position.z < -40 && pointB.transform.position.z > -0)
                {
                    xyz = new Vector3(-105, 0, -25);
                }

                //�v���C���[�̃R�[�g�̉E�� 
                if (pointB.transform.position.z > -9 && pointB.transform.position.z < 9)
                {
                    xyz = new Vector3(-105, 0, -2);

                }

                //�v���C���[�̃R�[�g�̒���
                if (pointB.transform.position.z > 10 && pointB.transform.position.z < 40)
                {
                    xyz = new Vector3(-105, 0, 25);

                }

                //�v���C���[�̃R�[�g�̑O����
                if (pointB.transform.position.x < 0 && pointB.transform.position.x > -40)
                {
                    xyz = new Vector3(-40, 0, 25);
                }

                //�v���C���[�̃R�[�g�̒���
                if (pointB.transform.position.x < 41 && pointB.transform.position.x > -80)
                {
                    xyz = new Vector3(-80, 0, 25);

                }

                //�v���C���[�̃R�[�g�̌㒆��
                if (pointB.transform.position.x < -81 && pointB.transform.position.x > -120)
                {
                    xyz = new Vector3(-110, 0, 25);
                }
                //xyz = new Vector3(pointB.transform.position.x, 0, pointB.transform.position.z);


                //�ړ�������
                GetComponent<NavMeshAgent>().destination = xyz;

                //�����ړ��͈��̂�
                autoFlg = false;

                Debug.Log(xyz);
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
        if (dis <=30)
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

            parameter = TargetPoint();
            CharaStatus.Rad = parameter.x;
            CharaStatus.Distance = parameter.y;

            //�p�����[�^���傱���ƒ��ڂ������Ă�
            //Debug.Log(parameter.x + ":::"+ parameter.y);
            Base.Swing(CharaStatus.CharaPower * 1.5f, Shot.GetPower + 20);

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

    private Vector2 TargetPoint()
    {
        float targetPointX = 1.5f;
        float targetPointY = 400;

        int mode = Random.Range(1, 4);

        //�v���C���[�̃R�[�g�̍��O��
        if (pointB.transform.position.z < -40 && pointB.transform.position.z > -0)
        {
            targetPointX = 0.9f;
        }

        //�v���C���[�̃R�[�g�̉E�O�� 
        if (pointB.transform.position.z > -9 && pointB.transform.position.z < 9)
        {
            targetPointX = 1.5f;
        }

        //�v���C���[�̃R�[�g�̉E�O�� 
        if (pointB.transform.position.z > 10 && pointB.transform.position.z < 40)
        {
            targetPointX = 2.1f;
        }


        Debug.Log("pointB:"+ pointB.transform.position.x +":::"+ pointB.transform.position.z);
        if (mode ==2)
        { 

            //�v���C���[�̃R�[�g�̑O����
            if (pointB.transform.position.x < 0 && pointB.transform.position.x > -40)
            {
                int a = Random.Range(1, 3);
                if (a != 2)
                {
                    targetPointX = 1.3f;
                }
                targetPointY = 400 * 2.5f;
            }

            //�v���C���[�̃R�[�g�̒���
            if (pointB.transform.position.x < -41 && pointB.transform.position.x > -80)
            {
                int a = Random.Range(1, 3);
                if (a != 2)
                {
                    targetPointX = 1.8f;
                }
                targetPointY = 400 * 3f;
            }

            //�v���C���[�̃R�[�g�̌㒆��
            if (pointB.transform.position.x < -81 && pointB.transform.position.x > -120)
            {
                int a = Random.Range(1, 3);
                if (a != 3)
                {
                    targetPointX = 2.2f;
                }
                targetPointY = 400 * 2.5f;
            }
        }

        if(mode == 4)
        {

            targetPointX = Random.Range(1, 25) / 10;
            targetPointY = 400 * Random.Range(10, 35) / 10;
        }
        Vector2 targetPoint = new Vector2(targetPointX, targetPointY);

        //Debug.Log("rad" + targetPointX);
        //Debug.Log("dis" + targetPointY);

        return targetPoint;
    }
}
