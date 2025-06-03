// ----------------------------------------------------
// �t�@�C���� : TestWeapon.cs
// �p�r�@�@�@ : ���̕���B
// �X�V���@�@ : 2025/05/09 �v���O�����̍쐬�B
// ����ҁ@�@ : �V�c����
// ----------------------------------------------------
using UnityEngine;

public class TestWeapon : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponData
    {
        public int Damage;
        public int Impact;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(15.0f, 0.0f, 0.0f);
    }

    [SerializeField]
    private WeaponData _weaponDamage;
    private int _direction;
}
