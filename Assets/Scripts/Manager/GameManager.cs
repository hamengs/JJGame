using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    public CinemachineCamera cinemachineCamera;
    public GameObject gameOverUI; // GameOver 界面预制体或场景中的 UI 元素

    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        PlayerManager.Instance.OnPlayerDeath += OnPlayerDead;
    }

    private void OnDestroy()
    {
        if(PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnPlayerDeath -= OnPlayerDead;
        }
    }

    private void OnPlayerDead()
    {

        StopTrackingPlayer();
        Debug.Log("Dead");
        UIManager.Instance.GameOver();
    }

    public void StopTrackingPlayer()
    {
        if (cinemachineCamera.Follow != null)
        {
            cinemachineCamera.Follow = null;
        }

    }




}
