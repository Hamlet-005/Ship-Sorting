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

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                {
                    if (ship == null) return;
                    if (ship.isCompleted) return;
                    if (SelectionManager.Instance == null) return;

                    SelectionManager.Instance.OnShipClicked(ship);
                }
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                {
                    if (ship == null) return;
                    if (ship.isCompleted) return;
                    if (SelectionManager.Instance == null) return;

                    SelectionManager.Instance.OnShipClicked(ship);
                }
            }
        }
#endif
    }
}