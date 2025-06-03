using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Enemy Enemy;
    
    public void Start()
    {
        Application.targetFrameRate = 60;
    }
}
