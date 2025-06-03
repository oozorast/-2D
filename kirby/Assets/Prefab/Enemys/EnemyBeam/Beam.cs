using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-_beamSpeed, 0.0f, 0.0f);
        if (_timer >= _destroyTime)
        {
            Destroy(gameObject);
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    private float _timer;
    [SerializeField]
    private float _destroyTime;
    [SerializeField, Header("ƒr[ƒ€©‘Ì‚ÌˆÚ“®‘¬“x")]
    private float _beamSpeed;
}
