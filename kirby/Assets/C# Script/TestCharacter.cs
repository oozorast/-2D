// ----------------------------------------------------
// �t�@�C���� : TestCharacter.cs
// �p�r�@�@�@ : ���̑���L�����B
// �X�V���@�@ : 2025/05/01 �v���O�����̍쐬�B
// ����ҁ@�@ : �V�c����
// ----------------------------------------------------
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right
    }

    void Update()
    {
        Move();
        Shot();
	}

    // ----------------------------------------------------
    // �֐��� : Move
    // �p�r�@ : �ړ������B
    // �����@ : �����B
    // �߂�l : �����B
    // �X�V�� : 2025/05/09 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public void Move()
	{
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _moveDirection = Direction.Left;
            transform.Translate(-5.0f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _moveDirection = Direction.Right;
            transform.Translate(5.0f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0.0f, 5.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0.0f, -5.0f, 0.0f);
        }
    }

    // ----------------------------------------------------
    // �֐��� : Shot
    // �p�r�@ : �ړ������B
    // �����@ : �����B
    // �߂�l : �����B
    // �X�V�� : 2025/05/13 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public void Shot()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            switch (_moveDirection)
            {
                case Direction.Left:
                    {
                        Instantiate(Bullet, transform.position, Quaternion.identity);
                        break;
                    }
                case Direction.Right:
                    {
                        Instantiate(Bullet, transform.position, Quaternion.identity);
                        break;
                    }
            }
        }
    }

    public GameObject Bullet;

    // �e�ۂ̐������̕����B
    private Direction _moveDirection;
}



