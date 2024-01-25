using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsFree { get; private set; } = true;

    public bool IsTaken { get; private set; } = false;

    public void Reserv()
    {
        IsFree = false;
    }

    public void Take()
    {
        IsTaken = true;
    }

    public void ResetState()
    {
        IsFree = true;
        IsTaken = false;
    }
}