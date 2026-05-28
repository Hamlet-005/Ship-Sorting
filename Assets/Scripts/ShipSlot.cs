using UnityEngine;

public class ShipSlot : MonoBehaviour
{
    public Container currentContainer;

    public bool IsEmpty()
    {
        return currentContainer == null;
    }
}