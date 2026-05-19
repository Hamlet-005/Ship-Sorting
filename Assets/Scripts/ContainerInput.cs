using System.Collections.Generic;
using UnityEngine;

public class ContainerInput : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;

    private float dragHeight = 0.7f;

    private Container selectedContainer;
    private List<Container> selectedGroup = new List<Container>();
    private List<Vector3> startPositions = new List<Vector3>();

    private Vector3 dragStartWorldPoint;

    private List<ShipSlot> startSlots = new List<ShipSlot>();
    private List<Ship> startShips = new List<Ship>();

    void Start()
    {
        mainCamera = Camera.main;
        selectedContainer = GetComponent<Container>();
    }

    void OnMouseDown()
    {
        isDragging = true;

        selectedGroup.Clear();
        startPositions.Clear();

        selectedGroup = selectedContainer.currentShip.GetContainersByColor(selectedContainer.containerColor);
        Debug.Log("Selected group count: " + selectedGroup.Count);

        startSlots.Clear();
        startShips.Clear();

        foreach (Container c in selectedGroup)
        {
            startPositions.Add(c.transform.position);
            startSlots.Add(c.currentSlot);
            startShips.Add(c.currentShip);
        }

        dragStartWorldPoint = GetMouseWorldPoint();
    }

    void OnMouseUp()
    {
        isDragging = false;

        Ship targetShip = FindClosestShipWithEmptySlot();

        if (targetShip == null)
        {
            ReturnGroupBack();
            return;
        }

        List<ShipSlot> emptySlots = targetShip.GetEmptySlots();
        int moveCount = Mathf.Min(emptySlots.Count, selectedGroup.Count);

        if (moveCount <= 0)
        {
            ReturnGroupBack();
            return;
        }

        for (int i = 0; i < selectedGroup.Count; i++)
        {
            if (i < moveCount)
            {
                MoveContainerToSlot(selectedGroup[i], emptySlots[i]);
            }
            else
            {
                Container c = selectedGroup[i];

                c.transform.position = startPositions[i];

                c.currentSlot = startSlots[i];
                c.currentShip = startShips[i];

                startSlots[i].currentContainer = c;

                c.transform.SetParent(startSlots[i].transform);
            }
        }

        GameManager.Instance.UseMove();
        CheckCompletedShips();
        GameManager.Instance.CheckWin();
    }

    void Update()
    {
        if (!isDragging)
            return;

        Vector3 currentWorldPoint = GetMouseWorldPoint();
        Vector3 offset = currentWorldPoint - dragStartWorldPoint;
        offset.y = 0;

        for (int i = 0; i < selectedGroup.Count; i++)
        {
            Vector3 pos = startPositions[i] + offset;
            pos.y = dragHeight;
            selectedGroup[i].transform.position = pos;
        }
    }

    Vector3 GetMouseWorldPoint()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    Ship FindClosestShipWithEmptySlot()
    {
        Ship[] ships = FindObjectsByType<Ship>(FindObjectsSortMode.None);

        Ship closestShip = null;
        float closestDistance = 1.2f;

        Vector3 groupCenter = selectedGroup[0].transform.position;
        groupCenter.y = 0;

        foreach (Ship ship in ships)
        {
            if (ship.isCompleted) continue;
            if (ship == selectedContainer.currentShip) continue;
            if (ship.EmptySlotCount() <= 0) continue;

            Vector3 shipPos = ship.transform.position;
            shipPos.y = 0;

            float distance = Vector3.Distance(groupCenter, shipPos);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestShip = ship;
            }
        }

        return closestShip;
    }

    void ReturnGroupBack()
    {
        for (int i = 0; i < selectedGroup.Count; i++)
        {
            Container c = selectedGroup[i];

            c.transform.position = startPositions[i];

            c.currentSlot = startSlots[i];
            c.currentShip = startShips[i];

            startSlots[i].currentContainer = c;

            c.transform.SetParent(startSlots[i].transform);
        }
    }

    void MoveContainerToSlot(Container c, ShipSlot targetSlot)
    {
        if (c.currentSlot != null)
            c.currentSlot.currentContainer = null;

        Ship targetShip = targetSlot.GetComponentInParent<Ship>();

        targetSlot.currentContainer = c;
        c.currentSlot = targetSlot;
        c.currentShip = targetShip;

        c.transform.SetParent(targetSlot.transform);

        Vector3 pos = targetSlot.transform.position;
        pos.y += 0.32f;
        c.transform.position = pos;
    }

    void CheckCompletedShips()
    {
        Ship[] ships = FindObjectsByType<Ship>(FindObjectsSortMode.None);

        foreach (Ship ship in ships)
        {
            if (ship.IsCompleted())
            {
                ship.CompleteShip();
            }
        }
    }
}