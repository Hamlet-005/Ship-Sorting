using UnityEngine;

[RequireComponent(typeof(Ship))]
[RequireComponent(typeof(Collider))]
public class ShipInput : MonoBehaviour
{
    private Ship ship;

    void Awake()
    {
        ship = GetComponent<Ship>();
    }

    void OnMouseDown()
    {
        if (ship == null)
            return;

        if (ship.isCompleted)
            return;

        if (SelectionManager.Instance == null)
            return;

        SelectionManager.Instance.OnShipClicked(ship);
    }
}