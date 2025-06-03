// ----------------------------------------------------
// �t�@�C���� : WarpItem.cs
// �p�r�@�@�@ : �ʂ̃X�e�[�W�Ԃ̈ړ����s�����[�v�A�C�e���̏����B
// �X�V���@�@ : 2025/05/01 �v���O�����̍쐬�B
// ����ҁ@�@ : �V�c����
// ----------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpItem : MonoBehaviour
{
	[System.Serializable]
    public struct WarpItemData
    {
		[Header("�ړ���̃X�e�[�W�i�V�[���j"), SerializeField]
		public string NextStage;
		[Header("������s���Q�[���I�u�W�F�N�g�̃^�O")]
		public string CheckTarget;
	}

    void Update()
    {
       // �ʏ펞�̃A�j���[�V�����B 
    }

	public void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("NextStage");
		if (collider.gameObject.tag == _data.CheckTarget)
		{
			// �A�j���[�V��������ʉ���ǉ��B
			SceneManager.LoadScene(_data.NextStage);
		}
	}

	// �ȉ�private�̃v���p�e�B�B
	[Header("���[�v�A�C�e���̃f�[�^"), SerializeField]
	private WarpItemData _data;
}
