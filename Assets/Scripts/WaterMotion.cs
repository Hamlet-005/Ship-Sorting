using UnityEngine;

public class WaterMotion : MonoBehaviour
{
    public float moveSpeed = 0.03f;
    public float waveAmount = 0.03f;
    public float waveSpeed = 1.2f;

    private Vector3 startPosition;
    private Renderer rend;

    void Start()
    {
        startPosition = transform.position;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * waveSpeed) * waveAmount;
        transform.position = startPosition + new Vector3(0, y, 0);

        if (rend != null)
        {
            rend.material.mainTextureOffset += new Vector2(moveSpeed, moveSpeed) * Time.deltaTime;
        }
    }
}