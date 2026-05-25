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


        ContainerColor? color = null;

        foreach (ShipSlot slot in slots)
        {
            if (slot.currentContainer == null)
            {
                return false;
            }

            if (color == null)
            {
                color = slot.currentContainer.containerColor;
            }
            else if (slot.currentContainer.containerColor != color)
            {
                return false;
            }
        }

        return true;
    }

    public void CompleteShip()
    {
        isCompleted = true;

        gameObject.SetActive(false);
    }

    public List<Container> GetTopSameColorGroup(Container clickedContainer)
    {
        List<Container> result = new List<Container>();

        if (clickedContainer == null)
            return result;

        int clickedIndex = -1;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentContainer == clickedContainer)
            {
                clickedIndex = i;
                break;
            }
        }

        if (clickedIndex == -1)
            return result;

        for (int i = clickedIndex + 1; i < slots.Length; i++)
        {
            if (slots[i].currentContainer != null)
                return result;
        }

        ContainerColor color = clickedContainer.containerColor;

        for (int i = clickedIndex; i >= 0; i--)
        {
            if (slots[i].currentContainer == null)
                break;

            if (slots[i].currentContainer.containerColor != color)
                break;

            result.Add(slots[i].currentContainer);
        }

        return result;
    }

    public bool CanAcceptColor(ContainerColor color)
    {
        for (int i = slots.Length - 1; i >= 0; i--)
        {
            if (slots[i].currentContainer != null)
            {
                return slots[i].currentContainer.containerColor == color;
            }
        }

        return true;
    }
}