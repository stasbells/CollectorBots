using UnityEngine;

public class Flag : MonoBehaviour
{
    public bool IsDestroy { get; private set; } = false;

    public void Destroy()
    {
        if (!IsDestroy)
        {
            Destroy(gameObject);
            IsDestroy = true;
        }
    }
}