using UnityEngine;

public class ShipSlot : MonoBehaviour
{
    public Container currentContainer;

    public bool IsEmpty()
    {
        if (currentContainer == null)
        {
            return true;
        }

        return false;
    }
}