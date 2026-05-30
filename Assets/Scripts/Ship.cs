using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ship : MonoBehaviour
{
    public ShipSlot[] slots;

    private AudioManager audioManager;

    public bool isCompleted = false;

    void Awake()
    {
        slots = GetComponentsInChildren<ShipSlot>(true);
    }

    private void Start()
    {
        if (FindAnyObjectByType<AudioManager>() != null) audioManager = FindAnyObjectByType<AudioManager>();
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

        if (clickedContainer.isHidden)
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
            Container container = slots[i].currentContainer;

            if (container == null)
                continue;

            if (container.isHidden || container.containerColor != color)
                return result;
        }

        for (int i = slots.Length - 1; i >= 0; i--)
        {
            Container container = slots[i].currentContainer;

            if (container == null)
                continue;

            if (!container.isHidden && container.containerColor == color)
            {
                result.Add(container);
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
        if (isCompleted) return;
        isCompleted = true;

        ShipFloat shipFloat = GetComponent<ShipFloat>();
        if (shipFloat != null) shipFloat.enabled = false;

        Vector3 startPos = transform.position;
        Vector3 forwardExit = startPos + new Vector3(0f, 0f, 25f);

        bool shipInFront = false;
        Ship[] allShips = FindObjectsByType<Ship>(FindObjectsSortMode.None);

        foreach (Ship other in allShips)
        {
            if (other == this) continue;
            if (other.isCompleted) continue;

            Vector3 diff = other.transform.position - startPos;
            float forwardDist = Vector3.Dot(diff, Vector3.forward);
            float sideDist = Mathf.Abs(diff.x);

            if (forwardDist > 0.5f && forwardDist < 6f && sideDist < 2f)
            {
                shipInFront = true;
                break;
            }
        }

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        audioManager.PlayShipSound();

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
            Vector3 backAndTurn = startPos + new Vector3(0f, 0f, -1.2f);
            float turnAngle = 18f * sideDirection;
            Vector3 finalRotation = transform.eulerAngles + new Vector3(0f, turnAngle, 0f);
            Vector3 exitDirection = Quaternion.Euler(0f, turnAngle, 0f) * Vector3.forward;
            Vector3 exitPos = backAndTurn + exitDirection * 18f;

            sequence.Append(
                transform.DOMove(backAndTurn, 1.0f)
                    .SetEase(Ease.InOutSine)
            );
            sequence.Join(
                transform.DORotate(finalRotation, 1.0f)
                    .SetEase(Ease.InOutSine)
            );
            sequence.Append(
                transform.DOMove(exitPos, 3.5f)
                    .SetEase(Ease.InOutSine)
            );
        }

        sequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
            GameManager.Instance.CheckWin();
        });
    }

    public void RevealTopHiddenContainer()
    {
        Container top = GetTopContainer();

        if (top != null && top.isHidden)
        {
            top.Reveal();
        }
    }
}