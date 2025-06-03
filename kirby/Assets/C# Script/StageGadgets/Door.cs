// ----------------------------------------------------
// �t�@�C���� : Door.cs
// �p�r�@�@�@ : �����X�e�[�W�����ړ�����h�A�̎d�|���̏����B
// �X�V���@�@ : 2025/05/02 �v���O�����̍쐬�B
// �@�@�@�@�@   2025/05/02 �L�[���͂ňړ��o����悤�ɂ����B
// ����ҁ@�@ : �V�c����
// ----------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using static WarpItem;

public class Door : MonoBehaviour
{
	[System.Serializable]
	public struct DoorData
	{
		[Header("�ړ���̈ʒu"), SerializeField]
		public Vector2 MovePosition;
		[Header("������s���Q�[���I�u�W�F�N�g�̃^�O")]
		public string CheckTarget;
	}

    void Update()
    {
		// �ʏ펞�̃A�j���[�V�����B
	}

	// ----------------------------------------------------
	// �֐��� : MoveCharacter
	// �p�r�@ : �L�����N�^�[�����O�ɃZ�b�g�������W�ֈړ�������B
	// �����@ : �����B
	// �߂�l : �����B
	// �X�V�� : 2025/05/02 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public void MoveCharacter()
	{
		// �擾�����Q�[���I�u�W�F�N�g�̈ʒu��ύX�B
		_gameObjectData.transform.position = new Vector3(_data.MovePosition.x, _data.MovePosition.y, 0.0f);
	}

	// ----------------------------------------------------
	// �֐��� : CollisionProcess
	// �p�r�@ : �Փˎ��̏����B
	// �����@ : �����B
	// �߂�l : �����B
	// �X�V�� : 2025/05/09 ���\�b�h�̍쐬�B
	// ����� : �V�c����
	// ----------------------------------------------------
	public void CollisionProcess(Collider2D collider)
	{
		// ���O�Ɏw�肵���^�O�̃Q�[���I�u�W�F�N�g�ł������ꍇ�̂�MoveCharacter���\�b�h���Ăяo����B
		if (collider.gameObject.tag == _data.CheckTarget)
		{
			_gameObjectData = GameObject.Find(collider.gameObject.name);
			// �A�j���[�V��������ʉ���ǉ��B
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

	// �ȉ�private�̃v���p�e�B�B
	[Header("�h�A�̎d�|���̃f�[�^"), SerializeField]
	private DoorData _data;
	// �ړ�������Q�[���I�u�W�F�N�g�̏��
	private GameObject _gameObjectData;
}
