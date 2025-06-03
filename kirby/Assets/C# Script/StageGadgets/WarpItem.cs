// ----------------------------------------------------
// ファイル名 : WarpItem.cs
// 用途　　　 : 別のステージ間の移動を行うワープアイテムの処理。
// 更新日　　 : 2025/05/01 プログラムの作成。
// 制作者　　 : 澤田蒼空
// ----------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpItem : MonoBehaviour
{
	[System.Serializable]
    public struct WarpItemData
    {
		[Header("移動先のステージ（シーン）"), SerializeField]
		public string NextStage;
		[Header("判定を行うゲームオブジェクトのタグ")]
		public string CheckTarget;
	}

    void Update()
    {
       // 通常時のアニメーション。 
    }

	public void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("NextStage");
		if (collider.gameObject.tag == _data.CheckTarget)
		{
			// アニメーションや効果音を追加。
			SceneManager.LoadScene(_data.NextStage);
		}
	}

	// 以下privateのプロパティ。
	[Header("ワープアイテムのデータ"), SerializeField]
	private WarpItemData _data;
}
