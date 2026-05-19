using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public ShipSlot[] slots;
    public bool isCompleted = false;

    void Awake()
    {
        slots = GetComponentsInChildren<ShipSlot>();
    }

    public List<ShipSlot> GetEmptySlots()
    {
        List<ShipSlot> emptySlots = new List<ShipSlot>();

        foreach (ShipSlot slot in slots)
        {
            if (slot.IsEmpty())
                emptySlots.Add(slot);
        }

        return emptySlots;
    }

    public int EmptySlotCount()
    {
        return GetEmptySlots().Count;
    }

    public List<Container> GetContainersByColor(ContainerColor color)
    {
        List<Container> result = new List<Container>();

        foreach (ShipSlot slot in slots)
        {
            if (slot.currentContainer != null &&
                slot.currentContainer.containerColor == color)
            {
                result.Add(slot.currentContainer);
            }
        }

        return result;
    }

    public bool IsCompleted()
    {
        if (isCompleted || slots.Length == 0)
            return false;

        Debug.Log("Checking " + gameObject.name + " slots count: " + slots.Length);

        ContainerColor? color = null;

        foreach (ShipSlot slot in slots)
        {
            if (slot.currentContainer == null)
            {
                Debug.Log(gameObject.name + " NOT completed because " + slot.name + " is empty");
                return false;
            }

            Debug.Log(
                gameObject.name + " / " +
                slot.name + " has " +
                slot.currentContainer.name + " color: " +
                slot.currentContainer.containerColor
            );

            if (color == null)
            {
                color = slot.currentContainer.containerColor;
            }
            else if (slot.currentContainer.containerColor != color)
            {
                Debug.Log(gameObject.name + " NOT completed because colors are different");
                return false;
            }
        }

        Debug.Log(gameObject.name + " IS COMPLETED");
        return true;
    }

    public void CompleteShip()
    {
        isCompleted = true;
        Debug.Log(gameObject.name + " COMPLETED");

        gameObject.SetActive(false);
    }
}