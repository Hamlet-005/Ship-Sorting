using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ship : MonoBehaviour
{
    public ShipSlot[] slots;

    public bool isCompleted = false;

    void Awake()
    {
        slots = GetComponentsInChildren<ShipSlot>(true);
    }

    public List<ShipSlot> GetEmptySlots()
    {
        List<ShipSlot> empty = new List<ShipSlot>();

        foreach (ShipSlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                empty.Add(slot);
            }
        }

        return empty;
    }

    public int EmptySlotCount()
    {
        int count = 0;

        foreach (ShipSlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                count++;
            }
        }

        return count;
    }

    public Container GetTopContainer()
    {
        for (int i = slots.Length - 1; i >= 0; i--)
        {
            if (slots[i].currentContainer != null)
            {
                return slots[i].currentContainer;
            }
        }

        return null;
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

        ContainerColor color = clickedContainer.containerColor;

        for (int i = clickedIndex + 1; i < slots.Length; i++)
        {
            if (slots[i].currentContainer == null)
                continue;

            if (slots[i].currentContainer.containerColor != color)
                return result;
        }

        for (int i = slots.Length - 1; i >= 0; i--)
        {
            if (slots[i].currentContainer == null)
                continue;

            if (slots[i].currentContainer.containerColor == color)
            {
                result.Add(slots[i].currentContainer);
            }
            else
            {
                break;
            }
        }

        return result;
    }

    public bool IsCompletelyFull()
    {
        foreach (ShipSlot slot in slots)
        {
            if (slot.currentContainer == null)
                return false;
        }

        return true;
    }

    public bool IsCompleted()
    {
        if (slots == null || slots.Length == 0)
            return false;

        if (!IsCompletelyFull())
            return false;

        if (slots[0].currentContainer == null)
            return false;

        ContainerColor color = slots[0].currentContainer.containerColor;

        foreach (ShipSlot slot in slots)
        {
            if (slot.currentContainer == null)
                return false;

            if (slot.currentContainer.containerColor != color)
                return false;
        }

        return true;
    }

    public void CompleteShip()
    {
        if (isCompleted)
            return;

        isCompleted = true;

        Vector3 startPos = transform.position;
        Vector3 forwardExit = startPos + new Vector3(0f, 0f, 9f);

        bool shipInFront = Physics.Raycast(
            startPos + Vector3.up * 0.5f,
            Vector3.forward,
            4f
        );

        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(1f);

        if (!shipInFront)
        {
            sequence.Append(
                transform.DOMove(forwardExit, 2.5f)
                    .SetEase(Ease.InOutSine)
            );
        }
        else
        {
            float sideDirection = startPos.x < 0 ? -1f : 1f;

            Vector3 sideStep =
                startPos + new Vector3(2.2f * sideDirection, 0f, 1.5f);

            Vector3 exitPos =
                startPos + new Vector3(4.5f * sideDirection, 0f, 9f);

            sequence.Append(
                transform.DOMove(sideStep, 0.8f)
                    .SetEase(Ease.OutQuad)
            );

            sequence.Join(
                transform.DORotate(
                    transform.eulerAngles +
                    new Vector3(0f, 10f * sideDirection, 0f),
                    1.1f
                )
            );

            sequence.Append(
                transform.DOMove(exitPos, 2.2f)
                    .SetEase(Ease.InOutSine)
            );
        }

        sequence.OnComplete(() =>
        {
            gameObject.SetActive(false);

            GameManager.Instance.CheckWin();
        });
    }
}