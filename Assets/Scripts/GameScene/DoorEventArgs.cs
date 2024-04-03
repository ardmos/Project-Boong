using System;

public class DoorEventArgs : EventArgs
{
    public DoorEvent DoorEvent { get; }
    public DoorName DoorName { get; }

    public DoorEventArgs(DoorEvent doorEvent, DoorName doorName)
    {
        DoorEvent = doorEvent;
        DoorName = doorName;
    }
}