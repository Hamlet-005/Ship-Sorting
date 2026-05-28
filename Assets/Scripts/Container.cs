using UnityEngine;

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
}