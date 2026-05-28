using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    private Ship selectedShip;

    private List<Container> selectedGroup =
        new List<Container>();

    private Vector3 liftOffset =
        new Vector3(0, 0.5f, 0);

    void Awake()
    {
        Instance = this;
    }

    public void OnShipClicked(Ship clickedShip)
    {
        if (clickedShip == null)
            return;

        if (clickedShip.isCompleted)
            return;

        if (selectedShip == null)
        {
            SelectShip(clickedShip);
            return;
        }

        if (selectedShip == clickedShip)
        {
            ReturnContainers();
            return;
        }

        TryMoveTo(clickedShip);
    }

    void SelectShip(Ship ship)
    {
        Container topContainer =
            ship.GetTopContainer();

        if (topContainer == null)
            return;

        List<Container> movable =
            ship.GetTopSameColorGroup(topContainer);

        if (movable.Count == 0)
            return;

        selectedShip = ship;
        selectedGroup = movable;

        foreach (Container c in selectedGroup)
        {
            Vector3 targetPos =
                c.transform.position + liftOffset;

            c.transform.DOMove(
                targetPos,
                0.18f
            );
        }
    }

    void TryMoveTo(Ship targetShip)
    {
        if (targetShip == null)
        {
            ReturnContainers();
            return;
        }

        NeutralShip neutral = targetShip.GetComponent<NeutralShip>();

        if (neutral != null)
        {
            if (selectedGroup[0].isHidden ||
                !neutral.CanAcceptColor(selectedGroup[0].containerColor))
            {
                ReturnContainers();
                return;
            }
        }

        List<ShipSlot> emptySlots = targetShip.GetEmptySlots();

        int moveCount = Mathf.Min(
            emptySlots.Count,
            selectedGroup.Count
        );

        if (moveCount <= 0)
        {
            ReturnContainers();
            return;
        }

        for (int i = 0; i < selectedGroup.Count; i++)
        {
            Container c = selectedGroup[i];

            if (i < moveCount)
            {
                ShipSlot targetSlot = emptySlots[i];

                if (c.currentSlot != null)
                {
                    c.currentSlot.currentContainer = null;
                }

                targetSlot.currentContainer = c;

                c.currentSlot = targetSlot;
                c.currentShip = targetShip;

                c.transform.SetParent(targetSlot.transform, false);
                c.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);

                c.transform.DOLocalMove(
                    new Vector3(0f, 0f, 0.11f),
                    0.25f
                )
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    c.transform.DOPunchScale(
                        new Vector3(0.08f, -0.12f, 0.08f),
                        0.18f,
                        1,
                        0.5f
                    );
                });
            }
            else
            {
                c.transform.SetParent(c.currentSlot.transform, false);
                c.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);

                c.transform.DOLocalMove(
                    new Vector3(0f, 0f, 0.11f),
                    0.2f
                )
                .SetEase(Ease.OutQuad);
            }
        }

        GameManager.Instance.UseMove();

        selectedShip.RevealTopHiddenContainer();
        targetShip.RevealTopHiddenContainer();

        CheckCompletedShips();

        if (neutral != null)
        {
            neutral.TryExchange();
        }

        selectedShip = null;
        selectedGroup.Clear();
    }

    void ReturnContainers()
    {
        foreach (Container c in selectedGroup)
        {
            if (c.currentSlot == null)
                continue;

            c.transform.SetParent(c.currentSlot.transform, false);

            c.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);

            c.transform.DOLocalMove(
                new Vector3(0f, 0f, 0.11f),
                0.15f
            );
        }

        selectedShip = null;
        selectedGroup.Clear();
    }

    void CheckCompletedShips()
    {
        Ship[] ships = FindObjectsByType<Ship>(FindObjectsSortMode.None);

        foreach (Ship ship in ships)
        {
            if (ship.GetComponent<NeutralShip>() != null)
                continue;

            if (ship.isCompleted)
                continue;

            if (ship.IsCompleted())
            {
                ship.CompleteShip();
            }
        }
    }
}