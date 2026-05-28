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
        if (SelectionManager.Instance == null)
            return;

        SelectionManager.Instance.OnShipClicked(ship);
    }
}