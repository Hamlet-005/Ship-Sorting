using UnityEngine;
using DG.Tweening;

public enum ContainerColor
{
    Yellow,
    Blue,
    Green,
    Red,
    Orange
}

public class Container : MonoBehaviour
{
    public ContainerColor containerColor;

    public Ship currentShip;
    public ShipSlot currentSlot;

    [Header("Visual")]
    public Renderer containerRenderer;

    public Material yellowMaterial;
    public Material blueMaterial;
    public Material greenMaterial;
    public Material redMaterial;
    public Material orangeMaterial;

    [Header("Question Container")]
    public bool isHidden = false;
    public ContainerColor realColor;
    public Material hiddenMaterial;

    public void SetColor(ContainerColor newColor)
    {
        containerColor = newColor;

        if (containerRenderer == null)
            containerRenderer = GetComponentInChildren<Renderer>();

        if (containerRenderer == null)
            return;

        switch (newColor)
        {
            case ContainerColor.Yellow:
                containerRenderer.material = yellowMaterial;
                break;

            case ContainerColor.Blue:
                containerRenderer.material = blueMaterial;
                break;

            case ContainerColor.Green:
                containerRenderer.material = greenMaterial;
                break;

            case ContainerColor.Red:
                containerRenderer.material = redMaterial;
                break;

            case ContainerColor.Orange:
                containerRenderer.material = orangeMaterial;
                break;
        }
    }

    public void SetHidden(ContainerColor hiddenRealColor)
    {
        isHidden = true;
        realColor = hiddenRealColor;

        if (containerRenderer == null)
            containerRenderer = GetComponentInChildren<Renderer>();

        if (containerRenderer != null && hiddenMaterial != null)
            containerRenderer.material = hiddenMaterial;
    }

    public void Reveal()
    {
        if (!isHidden)
            return;

        isHidden = false;

        transform.DOPunchScale(
            new Vector3(0.25f, 0.25f, 0.25f),
            0.25f,
            1,
            0.5f
        );

        Sequence seq = DOTween.Sequence();

        seq.Append(
            transform.DOScale(
                Vector3.zero,
                0.12f
            ).SetEase(Ease.InBack)
        );

        seq.AppendCallback(() =>
        {
            SetColor(realColor);
        });

        seq.Append(
            transform.DOScale(
                Vector3.one,
                0.18f
            ).SetEase(Ease.OutBack)
        );
    }

    public bool IsRevealed()
    {
        return !isHidden;
    }
}