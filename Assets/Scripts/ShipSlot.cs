using UnityEngine;

public class ShipSlot : MonoBehaviour
{
    public Container currentContainer;

    public bool IsEmpty()
    {
        if (currentContainer == null)
        {
            Debug.Log(name + " is empty");
            return true;
        }

        Debug.Log(name + " has " + currentContainer.name);
        return false;
    }
}