using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�R�A�N���X
public class Score : MonoBehaviour
{
    [SerializeField]
    private int matchPScore;    //�}�b�`�|�C���g�̓��_

    public int user1Score { get; private set; }     //user1�̃X�R�A
    public int user2Score { get; private set; }     //user2�̃X�R�A
    public bool isUser1MatchP { get; private set; } //user1�̃}�b�`�|�C���g�t���O
    public bool isUser2MatchP { get; private set; } //user2�̃}�b�`�|�C���g�t���O

    //����������
    private void Start()
    {
        Init();
    }

    //����������
    private void Init()
    {
        user1Score = 0;
        user2Score = 0;
        isUser1MatchP = false;
        isUser2MatchP = false;
    }

    //�}�b�`�|�C���g�����Z�b�g���鏈��(�f���[�X�̍ۂɎg��)
    public void MatchPReset()
    {
        isUser1MatchP = false;
        isUser2MatchP = false;
    }

    //�X�R�A�����Z����
    public void AddScore(string _playerTag)
    {
        //�f���[�X�̍�
        if (GameManager.instance.isDeuce)
        {
            matchPScore++;
            GameManager.instance.isDeuce = false;
        }

        //�^�O��"Player"�̏ꍇ
        if (_playerTag == "Player")
        {
            user1Score++;
            if (user1Score == matchPScore) isUser1MatchP = true;
        }
        //�^�O��"Player2"�̏ꍇ
        else
        {
            user2Score++;
            if (user2Score == matchPScore) isUser2MatchP = true;
        }

        //�X�R�A�ǉ��t���O���I���ɂ���
        GameManager.instance.isAddScore = true;

        Debug.Log("User1 : " + user1Score + " " + "User2 : " + user2Score);
    }
}
