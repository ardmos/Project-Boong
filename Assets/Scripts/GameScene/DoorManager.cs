using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorNames
{
    Kitchen,
    BedRoom,
    BathRoom,
    WorkoutRoom,
    Garage
}

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance { get; private set; }

    public event EventHandler OnKitchenDoorOpen;
    public event EventHandler OnBedRoomDoorOpen;
    public event EventHandler OnBathRoomDoorOpen;
    public event EventHandler OnWorkoutRoomDoorOpen;
    public event EventHandler OnGarageDoorOpen;

    private void Awake()
    {
        Instance = this;
    }

    public void OnDoorOpened(DoorNames doorName)
    {
        switch (doorName)
        {
            case DoorNames.Kitchen:
                OnKitchenDoorOpen.Invoke(this, EventArgs.Empty);
                break;
            case DoorNames.BedRoom:
                OnBedRoomDoorOpen.Invoke(this, EventArgs.Empty);
                break;
            case DoorNames.BathRoom:
                OnBathRoomDoorOpen.Invoke(this, EventArgs.Empty);
                break;
            case DoorNames.WorkoutRoom:
                OnWorkoutRoomDoorOpen.Invoke(this, EventArgs.Empty);
                break;
            case DoorNames.Garage:
                OnGarageDoorOpen.Invoke(this, EventArgs.Empty);
                break;
        }
    }
}
