// ----------------------------------------------------
// ファイル名 : EnemyMoveBeam.cs
// 用途　　　 : ジャンプして、ビームを出す敵キャラクターの動作。
// 更新日　　 : 2025/05/16 プログラムの作成。
// 制作者　　 : 澤田蒼空
// ----------------------------------------------------
using UnityEngine;

public class EnemyBeam : Enemy
{
    public struct BeamRotate
    {
        public float Rotate;
        public float Max;
        public float Minimum;
    }


    void Start()
    {
        EnemyStart();
        _beamRotate.Rotate = -45.0f;
        _animator = GetComponent<Animator>();
	}

    void Update()
    {
        // ダメージを受けた後は、すぐに更新処理を行わず数秒間待機する。
        if (IsDamageWaitEnd())
        {
            // 攻撃しなかった場合のみ移動を行う。
            if (!EnemyBeamAttack())
            {
                // 4回移動を行った後にジャンプする。
                JumpProcess();
                EnemyMoveProcess();
            }
            // 以下、更新処理。
            DamageChack();
        }
    }

    // ----------------------------------------------------
    // 関数名 : SetAnimationInteger
    // 用途　 : Integer型のアニメーションを設定する。
    // 引数　 : 無し。
    // 戻り値 : 無し。
    // 更新日 : 2025/05/09 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public override void SetAnimationInteger(string animationName, int setIntger)
	{
		_animator.SetInteger(animationName, setIntger);
	}

    // ----------------------------------------------------
    // 関数名 : SetAnimationBool
    // 用途　 : Bool型のアニメーションを設定する。
    // 引数　 : 無し。
    // 戻り値 : 無し。
    // 更新日 : 2025/04/25 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public override void SetAnimationBool(string animationName, bool setBool)
	{
		_animator.SetBool(animationName, setBool);
	}

    // ----------------------------------------------------
    // 関数名 : JumpProcess
    // 用途　 : 現在の_moveCounterの値を確認し、４以上になったらジャンプの処理を行う。
    // 引数　 : 無し。
    // 戻り値 : 無し。
    // 更新日 : 2025/05/16 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public void JumpProcess()
    {
        // 指定した歩数以上になったらジャンプ処理を行う。
        if (CheckMoveCount(_jumpTiming))
        {
            Jump();
        }
    }

    // ----------------------------------------------------
    // 関数名 : EnemyBeamAttack
    // 用途　 : 操作キャラとの距離が設定した距離以下だった場合にビーム攻撃を行う。
    // 引数　 : 無し。
    // 戻り値 : 攻撃中ならtrueを返す。
    // 更新日 : 2025/05/23 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public bool EnemyBeamAttack()
    {
        bool attacking = false;
        Vector3 characterDistance = GetMainCharacterPosition() - transform.position;
        if (Mathf.Abs(characterDistance.x) <= _attackDistance && Mathf.Abs(characterDistance.y) <= _attackDistance)
        {
            //SetRenderer(false);
            SetAnimationBool("IsAttacking", true);
            if (!_beamUsing)
            {
                if (AttackTiming())
                {
                    _beamUsing = true;
                    // ビームの最初の角度を設定。（現在の移動方向に合わせる。）
                    _beamRotate.Rotate = (GetDirection() == Direction.Left)
                    ? -90.0f
                    : 280.0f;
                }
            }
            if (_beamUsing)
            {
                // ビームを出す方向を確認し、変更した角度をRotateに代入。
                _beamRotate.Rotate = (GetDirection() == Direction.Left) 
                    ? (_beamRotate.Rotate += _rotateChangeSpeed) 
                    : (_beamRotate.Rotate -= _rotateChangeSpeed);
                // 最大角度以下の場合にはビームを生成。（左右で角度の条件を変更する。）
                if ((GetDirection() == Direction.Left && _beamRotate.Rotate <= 80.0f)
                    || (GetDirection() == Direction.Right && _beamRotate.Rotate >= 180.0f))
                {
                    if (BeamTiming())
                    {
                        Instantiate(
                    GetWeaponData(),
                    new Vector3(GetWeaponStartPosition(), transform.position.y, transform.position.z),
                    Quaternion.Euler(0.0f, 0.0f, _beamRotate.Rotate)
                    );
                        _beamUsing = true;
                    }
                }
                else
                {
                    // 角度のリセット。
                    _beamRotate.Rotate = -90.0f;
                    _beamUsing = false;
                }
            }
            attacking = true;
        }
        else
        {
            SetAnimationBool("IsAttacking", false);
        }
        return attacking;
    }
    
    // ----------------------------------------------------
    // 関数名 : AttackTiming
    // 用途　 : ビームの間隔を調整。
    // 引数　 : 無し。
    // 戻り値 : 設定した時間を過ぎたらtrueを返す。
    // 更新日 : 2025/05/23 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public bool BeamTiming()
    {
        bool beamTiming = false;
        if (_biamTimer >= _beamTiming)
        {
            beamTiming = true;
            _biamTimer = 0.0f;
        }
        else
        {
            _biamTimer += Time.deltaTime;
        }
        return beamTiming;
    }

    // ----------------------------------------------------
    // 関数名 : AttackAnimationStop
    // 用途　 : 攻撃アニメーションを止める。
    // 引数　 : 無し。
    // 戻り値 : 無し。
    // 更新日 : 2025/05/23 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public override void AttackAnimationStop()
    {
        SetAnimationBool("IsAttacking", false);
    }

    // 以下privateのプロパティ。
    private Animator _animator;
    [SerializeField]
    private int _jumpTiming;
    [SerializeField]
    private int _attackDistance;
    [SerializeField]
    private BeamRotate _beamRotate;
    private bool _beamUsing;
    private float _biamTimer;
    [SerializeField]
    private float _beamTiming;
    [SerializeField, Header("ビームの角度変更の速度")]
    private float _rotateChangeSpeed;
}

