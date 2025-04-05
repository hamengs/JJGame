using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{

    public Dictionary<TransitionTo, TransitionPoint> portalMapping = new Dictionary<TransitionTo, TransitionPoint>();
    public bool inTransitionArea = false;
    public bool hasTransitioned = false;
    public TransitionPoint currentPortal;
    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        base.OnDestroy();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "SceneMenu")
        {
            portalMapping.Clear();
            // 获取当前场景中所有 TransitionPoint 对象
            TransitionPoint[] transitionPoints = FindObjectsByType<TransitionPoint>(FindObjectsSortMode.None);
            foreach (var point in transitionPoints)
            {
                portalMapping[(TransitionTo)point.transitionDestination] = point;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Transition(currentPortal);
        }
    }

    public void Transition(TransitionPoint source)
    {
        if (source == null)
        {
            Debug.LogWarning("Transition source is null");
            return;
        }

        if (portalMapping.TryGetValue((TransitionTo)source.transitionTo, out TransitionPoint target))
        {
            if (inTransitionArea && !hasTransitioned)
            {
                Debug.Log("Teleporting to " + target.name);
                // 使用 target 的 "Destination" 子物体作为传送目的地
                Transform destination = target.transform.Find("Destination");
                if (destination != null)
                {
                    PlayerManager.Instance.playerController.transform.position = destination.position;
                    hasTransitioned = true;
                }
                else
                {
                    Debug.LogWarning("Target " + target.name + "没有找到Destination子物体");
                }
            }
        }
        else
        {
            Debug.LogWarning("No matching portal found for " + source.transitionTo);
        }
    }

}
