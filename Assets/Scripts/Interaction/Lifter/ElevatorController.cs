using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour, IAction
{
    public DoorRotation FirstDoor; // Puerta delantera que se cierra inicialmente
    public DoorRotation SecondDoor; // Puerta trasera que se abre después de que el ascensor sube
    public LifterMovement elevator;
    
    void OnEnable()
    {
        LifterMovement.OnElevatorReachedTarget += ElevatorHasReached;
        DoorRotation.OnDoorMovementComplete += DoorHasMoved;
    }

    void OnDisable()
    {
        LifterMovement.OnElevatorReachedTarget -= ElevatorHasReached;
        DoorRotation.OnDoorMovementComplete -= DoorHasMoved;
    }

    public void Activate()
    {
        FirstDoor.Activate(true); // Inicia cerrando FirstDoor
    }

    void ElevatorHasReached()
    {
        SecondDoor.Activate(false); // Abre SecondDoor una vez que el ascensor llega
    }

    void DoorHasMoved(bool isFirstDoor)
    {
        if (isFirstDoor && !elevator.IsActivated())
        {
            elevator.Activate(); // Activa el ascensor
        }
    }
}