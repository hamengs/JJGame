using System.Collections;
using UnityEngine;

public enum TransitionTo
{
    A, B, C, D, Spawn
}

public enum TransitionDestination
{
    A, B, C, D, Spawn
}

public class TransitionPoint : MonoBehaviour
{
    public TransitionTo transitionTo;
    public TransitionDestination transitionDestination;
    public bool hasSaved;


    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneController.Instance.inTransitionArea = true;
            SceneController.Instance.currentPortal = this;
            if (!hasSaved)
            {
                SaveManager.Instance.SaveGame();
                hasSaved = true;
            }

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneController.Instance.inTransitionArea = false;
            hasSaved = false;
            StartCoroutine(DelayedReset());
        }
    }

    private IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(0.5f);
        SceneController.Instance.hasTransitioned = false;
    }
}
