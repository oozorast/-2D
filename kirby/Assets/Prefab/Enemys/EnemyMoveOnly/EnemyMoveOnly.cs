// ----------------------------------------------------
// �t�@�C���� : EnemyMoveOnly.cs
// �p�r�@�@�@ : �ړ��݂̂��s���G�L�����N�^�[�̓���B
// �X�V���@�@ : 2025/04/25 �v���O�����̍쐬�B
// ����ҁ@�@ : �V�c����
// ----------------------------------------------------
using UnityEngine;

public class EnemyMoveOnly : Enemy
{
    void Start()
    {
        EnemyStart();
		_animator = GetComponent<Animator>();
	}

    void Update()
    {
		// �_���[�W���󂯂���́A�����ɍX�V�������s�킸���b�ԑҋ@����B
        if (IsDamageWaitEnd())
        {
            // �ȉ��A�X�V�����B
            EnemyMoveProcess();
            DamageChack();
        }
	}

    // ----------------------------------------------------
    // �֐��� : SetAnimationInteger
    // �p�r�@ : Integer�^�̃A�j���[�V������ݒ肷��B
    // �����@ : �����B
    // �߂�l : �����B
    // �X�V�� : 2025/05/09 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public override void SetAnimationInteger(string animationName, int setIntger)
	{
		_animator.SetInteger(animationName, setIntger);
	}

	// ----------------------------------------------------
	// �֐��� : SetAnimationBool
	// �p�r�@ : Bool�^�̃A�j���[�V������ݒ肷��B
	// �����@ : �����B
	// �߂�l : �����B
	// �X�V�� : 2025/04/25 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public override void SetAnimationBool(string animationName, bool setBool)
	{
		_animator.SetBool(animationName, setBool);
	}

    // ----------------------------------------------------
    // �֐��� : AttackAnimationStop
    // �p�r�@ : �U���A�j���[�V�������~�߂�B
    // �����@ : �����B
    // �߂�l : �����B
    // �X�V�� : 2025/05/23 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public override void AttackAnimationStop()
    {
        //SetAnimationBool("IsAttacking", false);
    }

    // �ȉ�private�̃v���p�e�B�B
    private Animator _animator;
}

