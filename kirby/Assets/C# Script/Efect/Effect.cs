// ----------------------------------------------------
// ファイル名 : Effect.cs
// 用途　　　 : 演出の設定。
// 更新日　　 : 2025/05/15 プログラムの作成。
// 制作者　　 : 澤田蒼空
// ----------------------------------------------------
using UnityEngine;

public class Effect : MonoBehaviour
{
    [System.Serializable]
    public struct EffectData
    {
        public float Time;
        public Animator Animation;
    }

    void Start()
    {
        _effectData.Animation = GetComponent<Animator>();
    }

    void Update()
    {
        if (_effectData.Animation.GetCurrentAnimatorStateInfo(0).normalizedTime >= _effectData.Time)
        {
            Destroy(this.gameObject);
        }
    }

    [Tooltip("演出の設定"), SerializeField]
    private EffectData _effectData;
}
