using UnityEngine;

public class ShipFloat : MonoBehaviour
{
    public float floatAmount = 0.04f;
    public float floatSpeed = 1.5f;

    public float rotateAmount = 2f;
    public float rotateSpeed = 1.2f;

    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * floatSpeed) * floatAmount;

        transform.position = startPos + new Vector3(0, y, 0);

        float zRot = Mathf.Sin(Time.time * rotateSpeed) * rotateAmount;

        transform.rotation =
            startRot * Quaternion.Euler(0, 0, zRot);
    }
}