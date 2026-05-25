using UnityEngine;

public class NeutralShip : MonoBehaviour
{
    private Ship ship;

    public Container outputContainerPrefab;
    public ContainerColor outputColor = ContainerColor.Red;
    public Ship outputShip;

    void Awake()
    {
        ship = GetComponent<Ship>();
    }

    public void TryExchange()
    {
        if (!HasFourSameColor())
            return;

        foreach (ShipSlot slot in ship.slots)
        {
            if (slot.currentContainer != null)
            {
                Destroy(slot.currentContainer.gameObject);
                slot.currentContainer = null;
            }
        }

        SpawnOutputContainer();

        Debug.Log("Neutral exchange completed");
    }

    bool HasFourSameColor()
    {
        if (ship.slots.Length != 4)
            return false;

        ContainerColor? color = null;

        foreach (ShipSlot slot in ship.slots)
        {
            if (slot.currentContainer == null)
                return false;

            if (color == null)
                color = slot.currentContainer.containerColor;
            else if (slot.currentContainer.containerColor != color)
                return false;
        }

        return true;
    }

    void SpawnOutputContainer()
    {
        if (outputContainerPrefab == null || outputShip == null)
            return;

        ShipSlot targetSlot = outputShip.GetEmptySlots()[0];

        Container newContainer = Instantiate(outputContainerPrefab);

        newContainer.transform.SetParent(targetSlot.transform, true);

        newContainer.transform.localScale = outputContainerPrefab.transform.localScale;
        newContainer.transform.rotation = outputContainerPrefab.transform.rotation;

        newContainer.SetColor(outputColor);

        newContainer.currentShip = outputShip;
        newContainer.currentSlot = targetSlot;

        targetSlot.currentContainer = newContainer;

        Vector3 pos = targetSlot.transform.position;
        pos.y += 0.32f;
        newContainer.transform.position = pos;
    }

    public bool CanAcceptColor(ContainerColor color)
    {
        return color != outputColor;
    }
}