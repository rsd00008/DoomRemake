using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour, IAction
{
    public EntryRotation entryRotation;
    public ShipMovement shipMovement;

    public void Activate()
    {
        shipMovement.Activate(); // Inicia cerrando FirstDoor
    }

    public void ShipHasReachedTargetPosition()
    {
        entryRotation.Activate(); // Abre SecondDoor una vez que el ascensor llega
    }
}
