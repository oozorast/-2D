// ----------------------------------------------------
// ファイル名 : Enemy.cs
// 用途　　　 : 全ての敵キャラクターで共通するメソッドをまとめたクラス。
// 更新日　　 : 2025/04/17 プログラムの作成。
// 　　　                  移動用のメソッドのMoveEnemyを追加。
// 　　　　　   2025/04/18 壁に到達した際に左右逆に移動する処理を追加。
// 制作者　　 : 澤田蒼空
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
	// 敵の行動の種類用。（Normal: 通常状態　Attack: キャラ発見時）
	public enum EnemyActionType
	{
		Normal,
		Attack
	}

	// 敵キャラの移動やRayを飛ばす際に使用する方向。
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

	// 壁や足場等の判定用に使用するRayを縦と横をどこまで飛ばすかを確認する用。
	[System.Serializable]
	public struct Scale
	{
		public float Vartical;
		public float Horizontal;
	}
	// 敵キャラクターに共通するステータス。
	[System.Serializable]
	public struct EnemyStatus
	{
		[Tooltip("敵キャラクターの体力（何発攻撃されたら倒されるか）")]
		public int Hp;
		[Tooltip("敵キャラクターのジャンプ力（AddForce）")]
		public float JumpPower;
		[Tooltip("敵キャラクターがダメージを受けた際の吹き飛ぶ距離（AddForce）")]
		public float DamageImpact;
		[Tooltip("縦と横のRayの距離")]
		public Scale RayScale;
		[Tooltip("倒した際に行う演出")]
		public GameObject WinEffect;
        [Tooltip("吹き飛んだ後に移動せず待機する時間")]
        public float WaitTime;
        [Tooltip("１歩の移動にかかる時間")]
        public float MoveTime;
		public float AttackInterval;
    }

	[System.Serializable]
	public struct CheckData
	{
		[Tooltip("方向転換の際に使用するステージの物体")]
		public LayerMask ObjectLayer;
        [Tooltip("操作キャラクターのタグ（敵キャラクターのダメージ判定用）")]
        public string MainCharacterTag;
		[Tooltip("操作キャラクターの武器として判定するオブジェクトのタグ名（敵キャラクターのダメージ判定用）")]
		public string WeaponTag;
        [Tooltip("敵キャラクターの歩数")]
        public int MoveCount;
		public bool IsCountStart;
        [Tooltip("ダメージ判定を行うレイヤー")]
		public LayerMask DamageObject;
		public GameObject Weapon;
        public float AttackTimer;
		[Tooltip("trueの場合は移動する際に壁の判定を行う")]
		public bool IsWallCheck;
		[Tooltip("武器の開始位置")]
		public float WeaponStartPosition;
    }

    public void EnemyStart()
    {
		_actionType = EnemyActionType.Normal;
        _rigidBody2D = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
        // 操作キャラの位置を取得。
        //_character = 

        // 初期の移動方向を設定。
        _moveDirection = SearchMainCharacter();
		CounterReset();
		Debug.Log("StartEnd");
	}

    // ----------------------------------------------------
    // 関数名 : EnemyMoveProcess
    // 用途　 : 壁の確認を行いつつ敵を移動させる。
    // 引数　 : 無し。
    // 戻り値 : 無し。
    // 更新日 : 2025/04/17 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public void EnemyMoveProcess()
	{
        // 壁の確認をしてから敵を移動させる。
        if (_checkData.IsWallCheck)
		{
            DirectionChange();
        }
        Move();
    }

    // ----------------------------------------------------
    // 関数名 : MoveCount
    // 用途　 : アニメーションの終了時、歩数に１追加する。
    // 引数　 : 無し。
    // 戻り値 : 無し。
    // 更新日 : 2025/05/22 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public void MoveCount()
	{
		++_moveCounter;
    }

    // ----------------------------------------------------
    // 関数名 : CheckMoveCount
    // 用途　 : 現在の移動歩数を確認し、checkValue以上だったらtrueをしカウンターをリセット。
    // 引数　 : checkValue : 確認に使用する値。（この歩数以上ならtrueを返す。）
    // 戻り値 : checkValue以上だったらtrueを返す。
    // 更新日 : 2025/05/22 メソッドの作成。
    // 制作者 : 澤田蒼空
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
	// 関数名 : CheckMoveType
	// 用途　 : _enemyStatusを確認し、敵の状態に合わせた移動処理関数を呼び出す。
	// 引数　 : 無し。
	// 戻り値 : 無し。
	// 更新日 : 2025/04/17 メソッドの作成。
	// 制作者 : 澤田蒼空
	// ----------------------------------------------------
	//public void CheckMoveType()
 //   {
	//	switch (_actionType)
	//	{
	//		// Normal: 通常時の移動の処理。(左右移動　壁に衝突するか足場の端に到達したら逆方向に移動。)
	//		case EnemyActionType.Normal:
	//			{
	//				// 壁の確認をしてから敵を移動させる。
	//				DirectionChange();
	//				Move();
	//				break;
	//			}
	//		// Attack:キャラ発見時の移動の処理。(操作キャラに接近して攻撃。)
	//		case EnemyActionType.Attack:
	//			{
	//				break;
	//			}
	//	}
 //   }

	// ----------------------------------------------------
	// 関数名 : DirectionChange
	// 用途　 : 移動方向を反転させるかの確認と反転処理。
	// 引数　 : 無し
	// 戻り値 : 無し。
	// 更新日 : 2025/04/17 メソッドの作成。
	// 制作者 : 澤田蒼空
	// ----------------------------------------------------
	public void DirectionChange()
	{
		// 壁に衝突した場合、敵の移動方向を左右反転させる。
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
	// 関数名 : IsWall
	// 用途　 : 壁に衝突したかどうかを確認。
	// 引数　 : 無し
	// 戻り値 : 壁に衝突した場合trueを返す。
	// 更新日 : 2025/04/17 メソッドの作成。
	// 　　　   2025/04/18 Raycastの判定を行うメソッドを追加。
	// 制作者 : 澤田蒼空
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
	// 関数名 : IsGroundEnd
	// 用途　 : 足場の端に到達したかどうかを確認。
	// 引数　 : 無し
	// 戻り値 : 足場の端に到達した場合trueを返す。
	// 更新日 : 2025/04/17 メソッドの作成。
	// 制作者 : 澤田蒼空
	// ----------------------------------------------------
	//public bool IsGroundEnd()
	//{
	//	bool isGroundEnd = false;
	//	// 足場に接地中且つ、進行方向の少し先に足場が無ければ移動方向を左右反転させる。
	//	// ２つ目の呼び出しの編集。
	//	if (Raycast(_objectLayer, Direction.Down) && !Raycast(_objectLayer, Direction.Down))
	//	{

	//	}
	//	return isGroundEnd;
	//}


	// ----------------------------------------------------
	// 関数名 : Move
	// 用途　 : 現在の移動方向を確認し、その方向へ移動させる。
	// 引数　 : 無し。
	// 戻り値 : 無し。
	// 更新日 : 2025/04/17 メソッドの作成。
	// 制作者 : 澤田蒼空
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
	// 関数名 : Jump
	// 用途　 : 足場に接地している際にステータスで設定された値だけ上方向へ移動させる。
	// 引数　 : 無し。
	// 戻り値 : 無し。
	// 更新日 : 2025/04/18 メソッドの作成。
	// 制作者 : 澤田蒼空
	// ----------------------------------------------------
	public void Jump()
	{
		if (!Raycast(Direction.Down, _enemyStatus.RayScale.Vartical))
		{
			_rigidBody2D.AddForce(Vector2.up * (_enemyStatus.JumpPower));
		}
    }

	// ----------------------------------------------------
	// 関数名 : Impact
	// 用途　 : 敵キャラクターを進行方向と逆に吹き飛ばす。
	// 引数　 : 無し。
	// 戻り値 : 無し。
	// 更新日 : 2025/04/22 メソッドの作成。
	// 制作者 : 澤田蒼空
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
    // 関数名 : DamageChack
    // 用途　 : ダメージの確認。
    // 引数　 : 無し。
    // 戻り値 : ダメージがあった場合はtrueを返す。
    // 更新日 : 2025/05/13 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public bool DamageChack()
	{
		bool isDamaged = false;
		// 操作キャラの攻撃に当たった場合はダメージ処理を行う。
		// 左右の両方を確認する。（当たった方向ごとに吹き飛ぶ方向を変える為。）
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
	// 関数名 : Damaged
	// 用途　 : ダメージを受け取った際に呼び出される処理。
	// 引数　 : 無し。
	// 戻り値 : 無し。
	// 更新日 : 2025/04/24 メソッドの作成。
	// 制作者 : 澤田蒼空
	// ----------------------------------------------------
	public void Damaged(Direction collisionDirection)
	{
        // 効果音。

        // 敵キャラクターを吹き飛ばしてから一定時間待機。
        SetAnimationBool("IsDamaged", true);
        Impact(collisionDirection);
		// 敵にダメージを与え、体力が０以下だった場合は敵を倒す。
		_enemyStatus.Hp -= 1;
        if (_enemyStatus.Hp <= 0 )
		{
			// 敵を倒した際の演出を行う。
			Instantiate(_enemyStatus.WinEffect, transform.position, Quaternion.identity);
			Destroy(this.gameObject);
		}
		_isWait = true;
	}

	// ----------------------------------------------------
	// 関数名 : WaitCounter
	// 用途　 : _isWaitがtrueの間、カウントを数え指定した秒数を超えたらfalseにする。
	// 引数　 : 無し。
	// 戻り値 : 待機を続ける場合はtrueを返す。
	// 更新日 : 2025/04/25 メソッドの作成。
	// 制作者 : 澤田蒼空
	// ----------------------------------------------------
	public bool WaitCounter()
	{
		// まだ待機をする場合はカウントを続ける。（_isWaitがtrueの場合。）
		if (_isWait)
		{
			// カウンターの処理。
			// 3 : 待ち時間。　60 : フレームの数。
			if (_waitCounter > (_enemyStatus.WaitTime * 60))
			{
				// 待機の終了とカウンターのリセット。
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
	// 関数名 : CounterReset
	// 用途　 : _waitCounterをリセット。
	// 引数　 : 無し。
	// 戻り値 : 無し。
	// 更新日 : 2025/04/25 メソッドの作成。
	// 制作者 : 澤田蒼空
	// ----------------------------------------------------
	public void CounterReset()
	{
		_waitCounter = 0;
	}

    // ----------------------------------------------------
    // 関数名 : Raycast
    // 用途　 : 引数で渡された方向とレイヤーを使用し、確認を行う。
    // 引数　 : rayDirection     : Rayを飛ばす方向。 
    // 　　　   rayScale         : Rayの長さ。
    // 戻り値 : 確認したいレイヤーと同じレイヤーを発見出来たらtrueを返す。
    // 更新日 : 2025/04/18 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public bool Raycast(Direction rayDirection, float rayScale)
	{
		// 同じレイヤーを発見したらtrueにして戻り値として返す。
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
	// 関数名 : IsDamageWaitEnd
	// 用途　 : ダメージ後の待機が終了したらダメージ中のアニメーションをfalseに変更し、trueを返す。
	// 引数　 : 無し。
	// 戻り値 : ダメージ後の待機が終了したらtrueを返す。
	// 更新日 : 2025/05/01 メソッドの作成。
	// 制作者 : 澤田蒼空
	// ----------------------------------------------------
	public bool IsDamageWaitEnd()
	{
        bool isWaitEnd = false;
        // WaitCounterメソッドで待機時間を確認し、待機中だった場合はfalseを返す。
        if (!WaitCounter())
		{
			// 一定時間待機した後、アニメーションを戻す。
			SetAnimationBool("IsDamaged", false);
            isWaitEnd = true;
        }
        return isWaitEnd;
	}

	// ----------------------------------------------------
	// 関数名 : GetMainCharacterPosition
	// 用途　 : 操作キャラの座標を取得。
	// 引数　 : 無し。
	// 戻り値 : 操作キャラの座標。
	// 更新日 : 2025/05/23 メソッドの作成。
	// 制作者 : 澤田蒼空
	// ----------------------------------------------------
	public Vector3 GetMainCharacterPosition()
	{
		return _character.transform.position;
	}

    // ----------------------------------------------------
    // 関数名 : GetWeaponData
    // 用途　 : 武器の情報を取得。
    // 引数　 : 無し。
    // 戻り値 : 設定されている武器の情報。
    // 更新日 : 2025/05/23 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public GameObject GetWeaponData()
	{
		return _checkData.Weapon;
	}

    // ----------------------------------------------------
    // 関数名 : AttackTiming
    // 用途　 : 攻撃の間隔を調整。
    // 引数　 : 無し。
    // 戻り値 : 設定した時間を過ぎたらtrueを返す。
    // 更新日 : 2025/05/23 メソッドの作成。
    // 制作者 : 澤田蒼空
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
    // 関数名 : GetDirection
    // 用途　 : 現在の移動方向を返す。
    // 引数　 : 無し。
    // 戻り値 : 現在の移動方向。
    // 更新日 : 2025/05/29 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public Direction GetDirection()
	{
		return _moveDirection;
    }

    // ----------------------------------------------------
    // 関数名 : SetDirection
    // 用途　 : 移動方向を設定。
    // 引数　 : 無し。
    // 戻り値 : 新しく設定する移動方向。
    // 更新日 : 2025/05/29 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public void SetDirection(Direction newDirection)
	{
		_moveDirection = newDirection;
    }

    // ----------------------------------------------------
    // 関数名 : SetRenderer
    // 用途　 : 描画の設定。
    // 引数　 : set : falseなら描画を反転する。
    // 戻り値 : 無し。
    // 更新日 : 2025/05/30 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public void SetRenderer(bool set)
    {
        _spriteRenderer.flipX = set;
    }

    // ----------------------------------------------------
    // 関数名 : SearchMainCharacter
    // 用途　 : 操作キャラクターを探してその方向を返す。
    // 引数　 : 無し。
    // 戻り値 : 操作キャラクターの方向。
    // 更新日 : 2025/05/30 メソッドの作成。
    // 制作者 : 澤田蒼空
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
    // 関数名 : GetWeaponStartPosition
    // 用途　 : キャラクターから少し離れた位置（weaponStartPositionの値。）を開始位置として返す。
    // 引数　 : 無し。
    // 戻り値 : 武器の開始位置。
    // 更新日 : 2025/05/30 メソッドの作成。
    // 制作者 : 澤田蒼空
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
    // 以下、virtualメソッド。
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



    // 以下privateのプロパティ。
    [Header("敵キャラクターのステータス"), SerializeField]
	private EnemyStatus _enemyStatus;
	private EnemyActionType _actionType;
    [SerializeField]
	private Direction _moveDirection;
	[Header("敵キャラクターの移動速度"), SerializeField]
	private float _enemyMoveSpeed;

	[Header("確認に使用するレイヤーとタグ"), SerializeField]
	private CheckData _checkData;

	// 待機する場合はtrueにする。
	private bool _isWait;
	[SerializeField]
	private int _waitCounter;

	// コンポーネント取得用。
	private Rigidbody2D _rigidBody2D;
	private SpriteRenderer _spriteRenderer;

	// 操作キャラクターの捜索に使用。
	[Header("追尾対象のゲームオブジェクト"), SerializeField]
	private GameObject _character;

    // 経過時間確認に使用する。
    [SerializeField]
    private int _moveCounter;
}
