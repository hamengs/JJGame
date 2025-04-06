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
    private bool canTeleport;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&canTeleport&& transitionTo != TransitionTo.Spawn)
        {
            SceneController.Instance.Transition(SceneController.Instance.currentPortal);
        }
    }


    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneController.Instance.inTransitionArea = true;
            SceneController.Instance.currentPortal = this;
            canTeleport = true;
            if (!hasSaved)
            {
                SaveManager.Instance.SaveGame();
                hasSaved = true;
            }
            if (transitionTo != TransitionTo.Spawn)
            {
                UIManager.Instance.ShowEButton(transform.position + new Vector3(0, 2f, 0));
            }
           


        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneController.Instance.inTransitionArea = false;
            hasSaved = false;
            canTeleport = false;
            UIManager.Instance.HideEButton();
            StartCoroutine(DelayedReset());
        }
    }

    private IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(0.5f);
        SceneController.Instance.hasTransitioned = false;
    }
}
