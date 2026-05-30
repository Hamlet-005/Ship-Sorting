using UnityEngine;
using DG.Tweening;

public class NeutralShip : MonoBehaviour
{
    private Ship ship;

    public Container outputContainerPrefab;
    public ContainerColor outputColor = ContainerColor.Yellow;
    public Ship outputShip;
    public int outputCount = 1;
    public ContainerColor acceptedColor = ContainerColor.Yellow;
    public bool useAcceptedColor = true;


    void Awake()
    {
        ship = GetComponent<Ship>();
    }

    public bool CanAcceptColor(ContainerColor color)
    {
        if (useAcceptedColor)
            return color == acceptedColor;

        return color != outputColor;
    }

    public void TryExchange()
    {
        if (!HasFourSameColor())
            return;

        Sequence sequence = DOTween.Sequence();

        foreach (ShipSlot slot in ship.slots)
        {
            if (slot.currentContainer != null)
            {
                Container container = slot.currentContainer;

                sequence.Join(
                    container.transform
                        .DOScale(Vector3.zero, 0.25f)
                        .SetEase(Ease.InBack)
                );
            }
        }

        sequence.OnComplete(() =>
        {
            foreach (ShipSlot slot in ship.slots)
            {
                if (slot.currentContainer != null)
                {
                    Destroy(slot.currentContainer.gameObject);
                    slot.currentContainer = null;
                }
            }

            SpawnOutputContainer();
        });
    }

    bool HasFourSameColor()
    {
        if (ship.slots.Length != 4)
            return false;

        ContainerColor? color = null;

        foreach (ShipSlot slot in ship.slots)
        {
            if (slot.currentContainer == null)
                return false;

            if (color == null)
                color = slot.currentContainer.containerColor;
            else if (slot.currentContainer.containerColor != color)
                return false;
        }

        return true;
    }

    void SpawnOutputContainer()
    {
        if (outputContainerPrefab == null || outputShip == null)
            return;

        var emptySlots = outputShip.GetEmptySlots();

        if (emptySlots.Count == 0)
            return;

        int spawnCount = Mathf.Min(outputCount, emptySlots.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            ShipSlot targetSlot = emptySlots[i];

            Container newContainer = Instantiate(outputContainerPrefab);
            newContainer.SetColor(outputColor);
            newContainer.currentShip = outputShip;
            newContainer.currentSlot = targetSlot;
            targetSlot.currentContainer = newContainer;

            Vector3 startPos = transform.position;
            startPos.y += 0.8f;
            Vector3 finalWorldPos = targetSlot.transform.position;

            newContainer.transform.SetParent(null);
            newContainer.transform.position = startPos;
            newContainer.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            newContainer.transform.localScale = Vector3.one;

            float delay = i * 0.15f;

            newContainer.transform
                .DOJump(finalWorldPos, 0.8f, 1, 0.45f)
                .SetDelay(delay)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    newContainer.transform.SetParent(targetSlot.transform, false);
                    newContainer.transform.localPosition = new Vector3(0f, 0f, 0.11f);
                    newContainer.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                    newContainer.transform.localScale = Vector3.one;
                });
        }
    }
}