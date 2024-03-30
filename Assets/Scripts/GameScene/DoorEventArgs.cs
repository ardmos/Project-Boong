using System;

public class DoorEventArgs : EventArgs
{
    public DoorEvent DoorEvent { get; }
    public DoorNames DoorName { get; }

    public DoorEventArgs(DoorEvent doorEvent, DoorNames doorName)
    {
        DoorEvent = doorEvent;
        DoorName = doorName;
    }
}