// ----------------------------------------------------
// ファイル名 : EnemyFly.cs
// 用途　　　 : 上下に移動して飛ぶ敵の処理。
// 更新日　　 : 2025/05/29 プログラムの作成。
// 制作者　　 : 澤田蒼空
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
        // ダメージを受けた後は、すぐに更新処理を行わず数秒間待機する。
        if (IsDamageWaitEnd())
        {
            EnemyMoveProcess();
            ChangeVertical();
            // 以下、更新処理。
            if (DamageChack())
            {
                ChangeDirection();
            }
        }
        
    }

    // ----------------------------------------------------
    // 関数名 : ChangeVertical
    // 用途　 : 高さの変更。
    // 引数　 : 無し。
    // 戻り値 : 無し。
    // 更新日 : 2025/05/29 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public void ChangeVertical()
    {
        Vector3 newPosition;
        newPosition.y = Mathf.Sin(Time.time * _verticalMoveSpeed) * _verticalValue;
        transform.position = new Vector3(transform.position.x, newPosition.y, transform.position.z);
    }

    // ----------------------------------------------------
    // 関数名 : ChangeDirection
    // 用途　 : 移動方向を操作キャラクターの方向にする。
    // 引数　 : 無し。
    // 戻り値 : 無し。
    // 更新日 : 2025/05/29 メソッドの作成。
    // 制作者 : 澤田蒼空
    // ----------------------------------------------------
    public void ChangeDirection()
    {
        SetDirection(SearchMainCharacter());
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

    [SerializeField, Header("高さの移動の範囲の値")]
    private float _verticalValue;
    [SerializeField, Header("高さの移動の速度")]
    private float _verticalMoveSpeed;
    private Animator _animator;
}
