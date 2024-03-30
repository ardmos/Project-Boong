using System;
using System.Collections.Generic;
using UnityEngine;

public enum DoorNames
{
    Door_Kitchen,
    Door_BedRoom,
    Door_BathRoom,
    Door_WorkoutRoom,
    Door_Garage
}

public enum DoorEvent
{
    Opened,
    Closed
}

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance { get; private set; }

    private Dictionary<DoorNames, DoorController> doorControllers = new Dictionary<DoorNames, DoorController>();
    private Dictionary<DoorNames, EventHandler<DoorEventArgs>> doorEvents = new Dictionary<DoorNames, EventHandler<DoorEventArgs>>();

    private void Awake()
    {
        Instance = this;

        // �� ���� ���� DoorController ã�Ƽ� �߰�
        foreach (DoorNames doorName in Enum.GetValues(typeof(DoorNames)))
        {
            DoorController doorController = GameObject.Find(doorName.ToString()).GetComponent<DoorController>();
            if (doorController != null)
            {
                doorControllers.Add(doorName, doorController);
                doorEvents.Add(doorName, (sender, e) => { });
            }
        }
    }

    // ���� ���� ���� ���� �̺�Ʈ �߻�
    public void OpenDoor(DoorNames doorName)
    {
        doorControllers[doorName].OnDoorOpened();
        doorEvents[doorName](this, new DoorEventArgs(DoorEvent.Opened, doorName));
    }

    // ���� �ݰ� ���� ���� �̺�Ʈ �߻�
    public void CloseAllDoors()
    {
        foreach (DoorNames doorName in Enum.GetValues(typeof(DoorNames)))
        {
            doorControllers[doorName].OnDoorClosed();
            doorEvents[doorName](this, new DoorEventArgs(DoorEvent.Closed, doorName));
        }
    }

    // PuppyAI Ŭ�������� �� ���� ���� �̺�Ʈ�� ������ �� �ִ� �޼���
    public void SubscribeToDoorEvent(DoorNames doorName, EventHandler<DoorEventArgs> handler)
    {
        doorEvents[doorName] += handler;
    }

    // PuppyAI Ŭ�������� �̺�Ʈ ������ ����� �� �ִ� �޼���
    public void UnsubscribeFromDoorEvent(DoorNames doorName, EventHandler<DoorEventArgs> handler)
    {
        doorEvents[doorName] -= handler;
    }
}
