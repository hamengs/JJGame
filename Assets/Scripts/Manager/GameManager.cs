using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public CinemachineCamera cinemachineCamera;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayerManager.Instance.OnPlayerDeath += OnPlayerDead;

    }

    protected override void OnDestroy()
    {
        if(PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnPlayerDeath -= OnPlayerDead;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
        base.OnDestroy();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "SceneMenu")
        {
            GameObject camObj = GameObject.FindGameObjectWithTag("CinemachineCamera");
            if (camObj != null)
            {
                cinemachineCamera = camObj.GetComponent<CinemachineCamera>();
            }
            else
            {
                Debug.LogWarning("新场景中未找到cinemachinecamera的对象");
            }
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

    public void TrackPlayer()
    {
        if (cinemachineCamera != null)
        {
            if (cinemachineCamera.Follow == null)
            {
                cinemachineCamera.Follow = PlayerManager.Instance.player.transform;
            }
        }

    }

    public void LoadSceneAndStart(string sceneName)
    {
        StartCoroutine(LoadSceneAndStartCoroutine(sceneName));
    }

    public void LoadSceneAndContinue(string sceneName)
    {
        StartCoroutine(LoadSceneAndContinueCoroutine(sceneName));
    }


    private IEnumerator LoadSceneAndStartCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        yield return new WaitUntil(() => asyncLoad.isDone);

        // 在这里执行场景加载后的初始化，比如重置UI、数据等
        UIManager.Instance.HideDeathInformation();
        UIManager.Instance.HideInventory();
        UIManager.Instance.HideChat();
        SaveManager.Instance.savedScene = null;
        InventoryManager.Instance.inventoryData.items.Clear();
        PlayerManager.Instance.playerData.ResetData();
        PlayerManager.Instance.ChangeHealth(0);
        PlayerPrefs.DeleteKey("GameSaveData");
        PlayerPrefs.Save();

    }

    private IEnumerator LoadSceneAndContinueCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        yield return new WaitUntil(() => asyncLoad.isDone);

        SaveManager.Instance.LoadGame();

    }


}
