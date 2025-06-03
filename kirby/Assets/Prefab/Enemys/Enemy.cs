// ----------------------------------------------------
// �t�@�C���� : Enemy.cs
// �p�r�@�@�@ : �S�Ă̓G�L�����N�^�[�ŋ��ʂ��郁�\�b�h���܂Ƃ߂��N���X�B
// �X�V���@�@ : 2025/04/17 �v���O�����̍쐬�B
// �@�@�@                  �ړ��p�̃��\�b�h��MoveEnemy��ǉ��B
// �@�@�@�@�@   2025/04/18 �ǂɓ��B�����ۂɍ��E�t�Ɉړ����鏈����ǉ��B
// ����ҁ@�@ : �V�c����
// ----------------------------------------------------
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

public class Enemy : MonoBehaviour
{
	// �G�̍s���̎�ޗp�B�iNormal: �ʏ��ԁ@Attack: �L�����������j
	public enum EnemyActionType
	{
		Normal,
		Attack
	}

	// �G�L�����̈ړ���Ray���΂��ۂɎg�p��������B
	public enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}

	public enum Animation
	{
		Idle,
		Moving
	}

	// �ǂ⑫�ꓙ�̔���p�Ɏg�p����Ray���c�Ɖ����ǂ��܂Ŕ�΂������m�F����p�B
	[System.Serializable]
	public struct Scale
	{
		public float Vartical;
		public float Horizontal;
	}
	// �G�L�����N�^�[�ɋ��ʂ���X�e�[�^�X�B
	[System.Serializable]
	public struct EnemyStatus
	{
		[Tooltip("�G�L�����N�^�[�̗̑́i�����U�����ꂽ��|����邩�j")]
		public int Hp;
		[Tooltip("�G�L�����N�^�[�̃W�����v�́iAddForce�j")]
		public float JumpPower;
		[Tooltip("�G�L�����N�^�[���_���[�W���󂯂��ۂ̐�����ԋ����iAddForce�j")]
		public float DamageImpact;
		[Tooltip("�c�Ɖ���Ray�̋���")]
		public Scale RayScale;
		[Tooltip("�|�����ۂɍs�����o")]
		public GameObject WinEffect;
        [Tooltip("������񂾌�Ɉړ������ҋ@���鎞��")]
        public float WaitTime;
        [Tooltip("�P���̈ړ��ɂ����鎞��")]
        public float MoveTime;
		public float AttackInterval;
    }

	[System.Serializable]
	public struct CheckData
	{
		[Tooltip("�����]���̍ۂɎg�p����X�e�[�W�̕���")]
		public LayerMask ObjectLayer;
        [Tooltip("����L�����N�^�[�̃^�O�i�G�L�����N�^�[�̃_���[�W����p�j")]
        public string MainCharacterTag;
		[Tooltip("����L�����N�^�[�̕���Ƃ��Ĕ��肷��I�u�W�F�N�g�̃^�O���i�G�L�����N�^�[�̃_���[�W����p�j")]
		public string WeaponTag;
        [Tooltip("�G�L�����N�^�[�̕���")]
        public int MoveCount;
		public bool IsCountStart;
        [Tooltip("�_���[�W������s�����C���[")]
		public LayerMask DamageObject;
		public GameObject Weapon;
        public float AttackTimer;
		[Tooltip("true�̏ꍇ�͈ړ�����ۂɕǂ̔�����s��")]
		public bool IsWallCheck;
		[Tooltip("����̊J�n�ʒu")]
		public float WeaponStartPosition;
    }

    public void EnemyStart()
    {
		_actionType = EnemyActionType.Normal;
        _rigidBody2D = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
        // ����L�����̈ʒu���擾�B
        //_character = 

        // �����̈ړ�������ݒ�B
        _moveDirection = SearchMainCharacter();
		CounterReset();
		Debug.Log("StartEnd");
	}

    // ----------------------------------------------------
    // �֐��� : EnemyMoveProcess
    // �p�r�@ : �ǂ̊m�F���s���G���ړ�������B
    // �����@ : �����B
    // �߂�l : �����B
    // �X�V�� : 2025/04/17 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public void EnemyMoveProcess()
	{
        // �ǂ̊m�F�����Ă���G���ړ�������B
        if (_checkData.IsWallCheck)
		{
            DirectionChange();
        }
        Move();
    }

    // ----------------------------------------------------
    // �֐��� : MoveCount
    // �p�r�@ : �A�j���[�V�����̏I�����A�����ɂP�ǉ�����B
    // �����@ : �����B
    // �߂�l : �����B
    // �X�V�� : 2025/05/22 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public void MoveCount()
	{
		++_moveCounter;
    }

    // ----------------------------------------------------
    // �֐��� : CheckMoveCount
    // �p�r�@ : ���݂̈ړ��������m�F���AcheckValue�ȏゾ������true�����J�E���^�[�����Z�b�g�B
    // �����@ : checkValue : �m�F�Ɏg�p����l�B�i���̕����ȏ�Ȃ�true��Ԃ��B�j
    // �߂�l : checkValue�ȏゾ������true��Ԃ��B
    // �X�V�� : 2025/05/22 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public bool CheckMoveCount(int checkValue)
	{
		bool isCheckValue = false;
        if (_moveCounter >= checkValue)
		{
            isCheckValue = true;
			_moveCounter = 0;
        }
		return isCheckValue;
    }

	// ----------------------------------------------------
	// �֐��� : CheckMoveType
	// �p�r�@ : _enemyStatus���m�F���A�G�̏�Ԃɍ��킹���ړ������֐����Ăяo���B
	// �����@ : �����B
	// �߂�l : �����B
	// �X�V�� : 2025/04/17 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	//public void CheckMoveType()
 //   {
	//	switch (_actionType)
	//	{
	//		// Normal: �ʏ펞�̈ړ��̏����B(���E�ړ��@�ǂɏՓ˂��邩����̒[�ɓ��B������t�����Ɉړ��B)
	//		case EnemyActionType.Normal:
	//			{
	//				// �ǂ̊m�F�����Ă���G���ړ�������B
	//				DirectionChange();
	//				Move();
	//				break;
	//			}
	//		// Attack:�L�����������̈ړ��̏����B(����L�����ɐڋ߂��čU���B)
	//		case EnemyActionType.Attack:
	//			{
	//				break;
	//			}
	//	}
 //   }

	// ----------------------------------------------------
	// �֐��� : DirectionChange
	// �p�r�@ : �ړ������𔽓]�����邩�̊m�F�Ɣ��]�����B
	// �����@ : ����
	// �߂�l : �����B
	// �X�V�� : 2025/04/17 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public void DirectionChange()
	{
		// �ǂɏՓ˂����ꍇ�A�G�̈ړ����������E���]������B
		if (IsWall())
		{
			if (_moveDirection == Direction.Left)
			{
				_moveDirection = Direction.Right;
			}
			else
			{
				_moveDirection = Direction.Left;
			}
		}
	}

	// ----------------------------------------------------
	// �֐��� : IsWall
	// �p�r�@ : �ǂɏՓ˂������ǂ������m�F�B
	// �����@ : ����
	// �߂�l : �ǂɏՓ˂����ꍇtrue��Ԃ��B
	// �X�V�� : 2025/04/17 ���\�b�h�̍쐬�B
	// �@�@�@   2025/04/18 Raycast�̔�����s�����\�b�h��ǉ��B
	// ����� : �V�c����
	// ----------------------------------------------------
	public bool IsWall()
	{
		bool isWall = false;
		if (Raycast(_moveDirection, _enemyStatus.RayScale.Horizontal))
		{
			isWall = true;
		}
		return isWall;
	}

	// ----------------------------------------------------
	// �֐��� : IsGroundEnd
	// �p�r�@ : ����̒[�ɓ��B�������ǂ������m�F�B
	// �����@ : ����
	// �߂�l : ����̒[�ɓ��B�����ꍇtrue��Ԃ��B
	// �X�V�� : 2025/04/17 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	//public bool IsGroundEnd()
	//{
	//	bool isGroundEnd = false;
	//	// ����ɐڒn�����A�i�s�����̏�����ɑ��ꂪ������Έړ����������E���]������B
	//	// �Q�ڂ̌Ăяo���̕ҏW�B
	//	if (Raycast(_objectLayer, Direction.Down) && !Raycast(_objectLayer, Direction.Down))
	//	{

	//	}
	//	return isGroundEnd;
	//}


	// ----------------------------------------------------
	// �֐��� : Move
	// �p�r�@ : ���݂̈ړ��������m�F���A���̕����ֈړ�������B
	// �����@ : �����B
	// �߂�l : �����B
	// �X�V�� : 2025/04/17 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public void Move()
	{
		//SetAnimationInteger("AnimationType", 1);
		switch (_moveDirection)
		{
			case Direction.Left:
				{
					_spriteRenderer.flipX = false;
					transform.Translate(-_enemyMoveSpeed, 0.0f, 0.0f);
					break;
				}
			case Direction.Right:
				{
					_spriteRenderer.flipX = true;
					transform.Translate(_enemyMoveSpeed, 0.0f, 0.0f);
					break;
				}
		}
	}

	// ----------------------------------------------------
	// �֐��� : Jump
	// �p�r�@ : ����ɐڒn���Ă���ۂɃX�e�[�^�X�Őݒ肳�ꂽ�l����������ֈړ�������B
	// �����@ : �����B
	// �߂�l : �����B
	// �X�V�� : 2025/04/18 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public void Jump()
	{
		if (!Raycast(Direction.Down, _enemyStatus.RayScale.Vartical))
		{
			_rigidBody2D.AddForce(Vector2.up * (_enemyStatus.JumpPower));
		}
    }

	// ----------------------------------------------------
	// �֐��� : Impact
	// �p�r�@ : �G�L�����N�^�[��i�s�����Ƌt�ɐ�����΂��B
	// �����@ : �����B
	// �߂�l : �����B
	// �X�V�� : 2025/04/22 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public void Impact(Direction collisionDirection)
	{
		if (collisionDirection == Direction.Left)

        {
			_spriteRenderer.flipX = false;
			_rigidBody2D.AddForce(Vector2.right * (_enemyStatus.DamageImpact));
		}
		else
		{
			_spriteRenderer.flipX = true;
			_rigidBody2D.AddForce(Vector2.left * (_enemyStatus.DamageImpact));
		}
	}

    // ----------------------------------------------------
    // �֐��� : DamageChack
    // �p�r�@ : �_���[�W�̊m�F�B
    // �����@ : �����B
    // �߂�l : �_���[�W���������ꍇ��true��Ԃ��B
    // �X�V�� : 2025/05/13 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public bool DamageChack()
	{
		bool isDamaged = false;
		// ����L�����̍U���ɓ��������ꍇ�̓_���[�W�������s���B
		// ���E�̗������m�F����B�i���������������Ƃɐ�����ԕ�����ς���ׁB�j
		RaycastHit2D hitLeft;
		Vector3 rayStartPosition = transform.position - new Vector3(_enemyStatus.RayScale.Horizontal, _enemyStatus.RayScale.Vartical, 0.0f);
		float rayScale = _enemyStatus.RayScale.Vartical * 2;
		hitLeft = Physics2D.Raycast(rayStartPosition, Vector2.up, rayScale, _checkData.DamageObject);
		if (hitLeft.collider)
		{
			AttackAnimationStop();
            Damaged(Direction.Left);
			isDamaged = true;
		}

		RaycastHit2D hitRight = Physics2D.Raycast(rayStartPosition, Vector2.down, _enemyStatus.RayScale.Vartical * 2, _checkData.DamageObject);
		rayStartPosition = transform.position + new Vector3(_enemyStatus.RayScale.Horizontal, _enemyStatus.RayScale.Vartical, 0.0f);
		hitRight = Physics2D.Raycast(rayStartPosition, Vector2.down, rayScale, _checkData.DamageObject);
		if (hitRight)
		{
            AttackAnimationStop();
            Damaged(Direction.Right);
            isDamaged = true;
        }
		return isDamaged;
    }

	// ----------------------------------------------------
	// �֐��� : Damaged
	// �p�r�@ : �_���[�W���󂯎�����ۂɌĂяo����鏈���B
	// �����@ : �����B
	// �߂�l : �����B
	// �X�V�� : 2025/04/24 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public void Damaged(Direction collisionDirection)
	{
        // ���ʉ��B

        // �G�L�����N�^�[�𐁂���΂��Ă����莞�ԑҋ@�B
        SetAnimationBool("IsDamaged", true);
        Impact(collisionDirection);
		// �G�Ƀ_���[�W��^���A�̗͂��O�ȉ��������ꍇ�͓G��|���B
		_enemyStatus.Hp -= 1;
        if (_enemyStatus.Hp <= 0 )
		{
			// �G��|�����ۂ̉��o���s���B
			Instantiate(_enemyStatus.WinEffect, transform.position, Quaternion.identity);
			Destroy(this.gameObject);
		}
		_isWait = true;
	}

	// ----------------------------------------------------
	// �֐��� : WaitCounter
	// �p�r�@ : _isWait��true�̊ԁA�J�E���g�𐔂��w�肵���b���𒴂�����false�ɂ���B
	// �����@ : �����B
	// �߂�l : �ҋ@�𑱂���ꍇ��true��Ԃ��B
	// �X�V�� : 2025/04/25 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public bool WaitCounter()
	{
		// �܂��ҋ@������ꍇ�̓J�E���g�𑱂���B�i_isWait��true�̏ꍇ�B�j
		if (_isWait)
		{
			// �J�E���^�[�̏����B
			// 3 : �҂����ԁB�@60 : �t���[���̐��B
			if (_waitCounter > (_enemyStatus.WaitTime * 60))
			{
				// �ҋ@�̏I���ƃJ�E���^�[�̃��Z�b�g�B
				_isWait = false;
				CounterReset();
			}
			else
			{
				++_waitCounter;
			}
		}
		return _isWait;
	}

	// ----------------------------------------------------
	// �֐��� : CounterReset
	// �p�r�@ : _waitCounter�����Z�b�g�B
	// �����@ : �����B
	// �߂�l : �����B
	// �X�V�� : 2025/04/25 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public void CounterReset()
	{
		_waitCounter = 0;
	}

    // ----------------------------------------------------
    // �֐��� : Raycast
    // �p�r�@ : �����œn���ꂽ�����ƃ��C���[���g�p���A�m�F���s���B
    // �����@ : rayDirection     : Ray���΂������B 
    // �@�@�@   rayScale         : Ray�̒����B
    // �߂�l : �m�F���������C���[�Ɠ������C���[�𔭌��o������true��Ԃ��B
    // �X�V�� : 2025/04/18 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public bool Raycast(Direction rayDirection, float rayScale)
	{
		// �������C���[�𔭌�������true�ɂ��Ė߂�l�Ƃ��ĕԂ��B
		bool isObject = false;
		RaycastHit2D hit = new RaycastHit2D();
		switch (rayDirection)
		{
			case Direction.Up:
				{
					hit = Physics2D.Raycast(transform.position, Vector2.up, rayScale, _checkData.ObjectLayer);
					break;
				}
			case Direction.Down:
				{
					hit = Physics2D.Raycast(transform.position, Vector2.down, rayScale, _checkData.ObjectLayer);
					break;
				}
			case Direction.Left:
				{
					hit = Physics2D.Raycast(transform.position, Vector2.left, rayScale, _checkData.ObjectLayer);
					break;
				}
			case Direction.Right:
				{
					hit = Physics2D.Raycast(transform.position, Vector2.right, rayScale, _checkData.ObjectLayer);
					break;
				}
		}
		
		if (hit.collider)
		{
			isObject = true;
		}
		return isObject;
	}

	// ----------------------------------------------------
	// �֐��� : IsDamageWaitEnd
	// �p�r�@ : �_���[�W��̑ҋ@���I��������_���[�W���̃A�j���[�V������false�ɕύX���Atrue��Ԃ��B
	// �����@ : �����B
	// �߂�l : �_���[�W��̑ҋ@���I��������true��Ԃ��B
	// �X�V�� : 2025/05/01 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public bool IsDamageWaitEnd()
	{
        bool isWaitEnd = false;
        // WaitCounter���\�b�h�őҋ@���Ԃ��m�F���A�ҋ@���������ꍇ��false��Ԃ��B
        if (!WaitCounter())
		{
			// ��莞�ԑҋ@������A�A�j���[�V������߂��B
			SetAnimationBool("IsDamaged", false);
            isWaitEnd = true;
        }
        return isWaitEnd;
	}

	// ----------------------------------------------------
	// �֐��� : GetMainCharacterPosition
	// �p�r�@ : ����L�����̍��W���擾�B
	// �����@ : �����B
	// �߂�l : ����L�����̍��W�B
	// �X�V�� : 2025/05/23 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public Vector3 GetMainCharacterPosition()
	{
		return _character.transform.position;
	}

    // ----------------------------------------------------
    // �֐��� : GetWeaponData
    // �p�r�@ : ����̏����擾�B
    // �����@ : �����B
    // �߂�l : �ݒ肳��Ă��镐��̏��B
    // �X�V�� : 2025/05/23 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public GameObject GetWeaponData()
	{
		return _checkData.Weapon;
	}

    // ----------------------------------------------------
    // �֐��� : AttackTiming
    // �p�r�@ : �U���̊Ԋu�𒲐��B
    // �����@ : �����B
    // �߂�l : �ݒ肵�����Ԃ��߂�����true��Ԃ��B
    // �X�V�� : 2025/05/23 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public bool AttackTiming()
	{
		bool attackTiming = false;
		if (_checkData.AttackTimer >= _enemyStatus.AttackInterval)
		{
			attackTiming = true;
			_checkData.AttackTimer = 0.0f;
		}
		else
		{
			_checkData.AttackTimer += Time.deltaTime;
        }
		return attackTiming;
	}

    // ----------------------------------------------------
    // �֐��� : GetDirection
    // �p�r�@ : ���݂̈ړ�������Ԃ��B
    // �����@ : �����B
    // �߂�l : ���݂̈ړ������B
    // �X�V�� : 2025/05/29 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public Direction GetDirection()
	{
		return _moveDirection;
    }

    // ----------------------------------------------------
    // �֐��� : SetDirection
    // �p�r�@ : �ړ�������ݒ�B
    // �����@ : �����B
    // �߂�l : �V�����ݒ肷��ړ������B
    // �X�V�� : 2025/05/29 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public void SetDirection(Direction newDirection)
	{
		_moveDirection = newDirection;
    }

    // ----------------------------------------------------
    // �֐��� : SetRenderer
    // �p�r�@ : �`��̐ݒ�B
    // �����@ : set : false�Ȃ�`��𔽓]����B
    // �߂�l : �����B
    // �X�V�� : 2025/05/30 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public void SetRenderer(bool set)
    {
        _spriteRenderer.flipX = set;
    }

    // ----------------------------------------------------
    // �֐��� : SearchMainCharacter
    // �p�r�@ : ����L�����N�^�[��T���Ă��̕�����Ԃ��B
    // �����@ : �����B
    // �߂�l : ����L�����N�^�[�̕����B
    // �X�V�� : 2025/05/30 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------
    public Direction SearchMainCharacter()
	{
		Direction _direciton = Direction.Left;
        if (_character.transform.position.x <= transform.position.x)
        {
            _direciton = Direction.Left;
        }
        else
        {
            _direciton = Direction.Right;
        }
		return _direciton;
    }
    // ----------------------------------------------------
    // �֐��� : GetWeaponStartPosition
    // �p�r�@ : �L�����N�^�[���班�����ꂽ�ʒu�iweaponStartPosition�̒l�B�j���J�n�ʒu�Ƃ��ĕԂ��B
    // �����@ : �����B
    // �߂�l : ����̊J�n�ʒu�B
    // �X�V�� : 2025/05/30 ���\�b�h�̍쐬�B
    // ����� : �V�c����
    // ----------------------------------------------------

    public float GetWeaponStartPosition()
	{
		float startPosition;
        if (_moveDirection == Direction.Left)
		{
			startPosition = transform.position.x - _checkData.WeaponStartPosition;
        }
		else
		{
            startPosition = transform.position.x + _checkData.WeaponStartPosition;
        }
		return startPosition;
	}
    // �ȉ��Avirtual���\�b�h�B
    public virtual void SetAnimationInteger(string animationName, int setIntger)
	{
	}

    public virtual void SetAnimationBool(string animationName, bool setBool)
	{
	}

    public virtual void AttackAnimationStop()
	{
	}


    public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, new Vector3(_enemyStatus.RayScale.Horizontal, 0, 0));
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, new Vector3(0, _enemyStatus.RayScale.Vartical, 0));
	}



    // �ȉ�private�̃v���p�e�B�B
    [Header("�G�L�����N�^�[�̃X�e�[�^�X"), SerializeField]
	private EnemyStatus _enemyStatus;
	private EnemyActionType _actionType;
    [SerializeField]
	private Direction _moveDirection;
	[Header("�G�L�����N�^�[�̈ړ����x"), SerializeField]
	private float _enemyMoveSpeed;

	[Header("�m�F�Ɏg�p���郌�C���[�ƃ^�O"), SerializeField]
	private CheckData _checkData;

	// �ҋ@����ꍇ��true�ɂ���B
	private bool _isWait;
	[SerializeField]
	private int _waitCounter;

	// �R���|�[�l���g�擾�p�B
	private Rigidbody2D _rigidBody2D;
	private SpriteRenderer _spriteRenderer;

	// ����L�����N�^�[�̑{���Ɏg�p�B
	[Header("�ǔ��Ώۂ̃Q�[���I�u�W�F�N�g"), SerializeField]
	private GameObject _character;

    // �o�ߎ��Ԋm�F�Ɏg�p����B
    [SerializeField]
    private int _moveCounter;
}
