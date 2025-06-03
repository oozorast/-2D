// ----------------------------------------------------
// ファイル名 : Door.cs
// 用途　　　 : 同じステージ内を移動するドアの仕掛けの処理。
// 更新日　　 : 2025/05/02 プログラムの作成。
// 　　　　　   2025/05/02 キー入力で移動出来るようにした。
// 制作者　　 : 澤田蒼空
// ----------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using static WarpItem;

public class Door : MonoBehaviour
{
	[System.Serializable]
	public struct DoorData
	{
		[Header("移動先の位置"), SerializeField]
		public Vector2 MovePosition;
		[Header("判定を行うゲームオブジェクトのタグ")]
		public string CheckTarget;
	}

    void Update()
    {
		// 通常時のアニメーション。
	}

	// ----------------------------------------------------
	// 関数名 : MoveCharacter
	// 用途　 : キャラクターを事前にセットした座標へ移動させる。
	// 引数　 : 無し。
	// 戻り値 : 無し。
	// 更新日 : 2025/05/02 メソッドの作成。
	// 制作者 : 澤田蒼空
	// ----------------------------------------------------
	public void MoveCharacter()
	{
		// 取得したゲームオブジェクトの位置を変更。
		_gameObjectData.transform.position = new Vector3(_data.MovePosition.x, _data.MovePosition.y, 0.0f);
	}

	// ----------------------------------------------------
	// 関数名 : CollisionProcess
	// 用途　 : 衝突時の処理。
	// 引数　 : 無し。
	// 戻り値 : 無し。
	// 更新日 : 2025/05/09 メソッドの作成。
	// 制作者 : 澤田蒼空
	// ----------------------------------------------------
	public void CollisionProcess(Collider2D collider)
	{
		// 事前に指定したタグのゲームオブジェクトであった場合のみMoveCharacterメソッドを呼び出せる。
		if (collider.gameObject.tag == _data.CheckTarget)
		{
			_gameObjectData = GameObject.Find(collider.gameObject.name);
			// アニメーションや効果音を追加。
			MoveCharacter();
		}
	}

	public void OnTriggerEnter2D(Collider2D collider)
	{
        Debug.Log("A");
  //      if (Input.GetKeyDown(KeyCode.A))
		//{
  //          Debug.Log("B");
  //          CollisionProcess(collider);
		//}
	}

	// 以下privateのプロパティ。
	[Header("ドアの仕掛けのデータ"), SerializeField]
	private DoorData _data;
	// 移動させるゲームオブジェクトの情報
	private GameObject _gameObjectData;
}
