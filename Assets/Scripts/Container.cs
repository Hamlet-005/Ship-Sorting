using UnityEngine;

public enum ContainerColor
{
    Yellow,
    Blue,
    Green,
    Red
}

public class Container : MonoBehaviour
{
    public ContainerColor containerColor;

    public Ship currentShip;
    public ShipSlot currentSlot;

    public Renderer containerRenderer;

    public Material yellowMaterial;
    public Material blueMaterial;
    public Material greenMaterial;
    public Material redMaterial;

    public void SetColor(ContainerColor newColor)
    {
        containerColor = newColor;

        if (containerRenderer == null)
            containerRenderer = GetComponent<Renderer>();

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
        }
    }
}