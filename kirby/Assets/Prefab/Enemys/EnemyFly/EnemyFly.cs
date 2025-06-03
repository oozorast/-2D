// ----------------------------------------------------
// �t�@�C���� : EnemyFly.cs
// �p�r�@�@�@ : �㉺�Ɉړ����Ĕ�ԓG�̏����B
// �X�V���@�@ : 2025/05/29 �v���O�����̍쐬�B
// ����ҁ@�@ : �V�c����
// ----------------------------------------------------
using UnityEngine;
public class EnemyFly : Enemy
{
    void Start()
    {
        EnemyStart();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // �_���[�W���󂯂���́A�����ɍX�V�������s�킸���b�ԑҋ@����B
        if (IsDamageWaitEnd())
        {
            EnemyMoveProcess();
            ChangeVertical();
            // �ȉ��A�X�V�����B
            if (DamageChack())
            {
                ChangeDirection();
            }
        }
        
    }

    // ----------------------------------------------------
    // �֐��� : ChangeVertical
    // �p�r�@ : �����̕ύX�B
    // �����@ : �����B
    // �߂�l : �����B
    // �X�V�� : 2025/05/29 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public void ChangeVertical()
    {
        Vector3 newPosition;
        newPosition.y = Mathf.Sin(Time.time * _verticalMoveSpeed) * _verticalValue;
        transform.position = new Vector3(transform.position.x, newPosition.y, transform.position.z);
    }

    // ----------------------------------------------------
    // �֐��� : ChangeDirection
    // �p�r�@ : �ړ������𑀍�L�����N�^�[�̕����ɂ���B
    // �����@ : �����B
    // �߂�l : �����B
    // �X�V�� : 2025/05/29 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public void ChangeDirection()
    {
        SetDirection(SearchMainCharacter());
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

    [SerializeField, Header("�����̈ړ��͈̔͂̒l")]
    private float _verticalValue;
    [SerializeField, Header("�����̈ړ��̑��x")]
    private float _verticalMoveSpeed;
    private Animator _animator;
}
