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
}