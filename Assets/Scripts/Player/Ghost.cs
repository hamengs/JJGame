using UnityEngine;

public class Ghost : MonoBehaviour
{
    public void DestroyGhost()
    {
        Destroy(this.gameObject);
    }
}
