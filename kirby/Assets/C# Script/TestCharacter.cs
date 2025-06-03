// ----------------------------------------------------
// ファイル名 : TestCharacter.cs
// 用途　　　 : 仮の操作キャラ。
// 更新日　　 : 2025/05/01 プログラムの作成。
// 制作者　　 : 澤田蒼空
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
    // 関数名 : Move
    // 用途　 : 移動処理。
    // 引数　 : 無し。
    // 戻り値 : 無し。
    // 更新日 : 2025/05/09 メソッドの作成。
    // 制作者 : 澤田蒼空
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
    // 関数名 : Shot
    // 用途　 : 移動処理。
    // 引数　 : 無し。
    // 戻り値 : 無し。
    // 更新日 : 2025/05/13 メソッドの作成。
    // 制作者 : 澤田蒼空
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

    // 弾丸の生成時の方向。
    private Direction _moveDirection;
}



