using System;
using System.Collections.Generic;
using UnityEngine;

public enum DoorName
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

    private Dictionary<DoorName, DoorController> doorControllers = new Dictionary<DoorName, DoorController>();
    private Dictionary<DoorName, EventHandler<DoorEventArgs>> doorEvents = new Dictionary<DoorName, EventHandler<DoorEventArgs>>();

    private void Awake()
    {
        Instance = this;

        // �� ���� ���� DoorController ã�Ƽ� �߰�
        foreach (DoorName doorName in Enum.GetValues(typeof(DoorName)))
        {
            DoorController doorController = GameObject.Find(doorName.ToString()).GetComponent<DoorController>();
            if (doorController != null)
            {
                doorControllers.Add(doorName, doorController);
                doorEvents.Add(doorName, (sender, e) => { });
            }
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    // ���� ���� ���� ���� �̺�Ʈ �߻�
    public void OpenDoor(DoorName doorName)
    {
        doorControllers[doorName].OnDoorOpened();
        doorEvents[doorName].Invoke(this, new DoorEventArgs(DoorEvent.Opened, doorName));
    }

    // ���� �ݰ� ���� ���� �̺�Ʈ �߻�
    public void CloseAllDoors()
    {
        foreach (DoorName doorName in Enum.GetValues(typeof(DoorName)))
        {
            doorControllers[doorName].OnDoorClosed();
            doorEvents[doorName].Invoke(this, new DoorEventArgs(DoorEvent.Closed, doorName));
        }
    }

    // PuppyAI Ŭ�������� �� ���� ���� �̺�Ʈ�� ������ �� �ִ� �޼���
    public void SubscribeToDoorEvent(DoorName doorName, EventHandler<DoorEventArgs> handler)
    {
        doorEvents[doorName] += handler;
    }

    // PuppyAI Ŭ�������� �̺�Ʈ ������ ����� �� �ִ� �޼���
    public void UnsubscribeFromDoorEvent(DoorName doorName, EventHandler<DoorEventArgs> handler)
    {
        doorEvents[doorName] -= handler;
    }
}
