// ----------------------------------------------------
// �t�@�C���� : Effect.cs
// �p�r�@�@�@ : ���o�̐ݒ�B
// �X�V���@�@ : 2025/05/15 �v���O�����̍쐬�B
// ����ҁ@�@ : �V�c����
// ----------------------------------------------------
using UnityEngine;

public class Effect : MonoBehaviour
{
    [System.Serializable]
    public struct EffectData
    {
        public float Time;
        public Animator Animation;
    }

    void Start()
    {
        _effectData.Animation = GetComponent<Animator>();
    }

    void Update()
    {
        if (_effectData.Animation.GetCurrentAnimatorStateInfo(0).normalizedTime >= _effectData.Time)
        {
            Destroy(this.gameObject);
        }
    }

    [Tooltip("���o�̐ݒ�"), SerializeField]
    private EffectData _effectData;
}
