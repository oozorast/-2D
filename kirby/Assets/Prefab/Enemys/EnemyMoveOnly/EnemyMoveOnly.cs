// ----------------------------------------------------
// ファイル名 : EnemyMoveOnly.cs
// 用途　　　 : 移動のみを行う敵キャラクターの動作。
// 更新日　　 : 2025/04/25 プログラムの作成。
// 制作者　　 : 澤田蒼空
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
		// ダメージを受けた後は、すぐに更新処理を行わず数秒間待機する。
        if (IsDamageWaitEnd())
        {
            // 以下、更新処理。
            EnemyMoveProcess();
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
    // 関数名 : AttackAnimationStop
    // 用途　 : 攻撃アニメーションを止める。
    // 引数　 : 無し。
    // 戻り値 : 無し。
    // 更新日 : 2025/05/23 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public override void AttackAnimationStop()
    {
        //SetAnimationBool("IsAttacking", false);
    }

    // 以下privateのプロパティ。
    private Animator _animator;
}

