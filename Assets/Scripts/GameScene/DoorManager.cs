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

        // 각 문에 대한 DoorController 찾아서 추가
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

    // 문을 열고 상태 변경 이벤트 발생
    public void OpenDoor(DoorNames doorName)
    {
        doorControllers[doorName].OnDoorOpened();
        doorEvents[doorName](this, new DoorEventArgs(DoorEvent.Opened, doorName));
    }

    // 문을 닫고 상태 변경 이벤트 발생
    public void CloseAllDoors()
    {
        foreach (DoorNames doorName in Enum.GetValues(typeof(DoorNames)))
        {
            doorControllers[doorName].OnDoorClosed();
            doorEvents[doorName](this, new DoorEventArgs(DoorEvent.Closed, doorName));
        }
    }

    // PuppyAI 클래스에서 각 문에 대한 이벤트를 구독할 수 있는 메서드
    public void SubscribeToDoorEvent(DoorNames doorName, EventHandler<DoorEventArgs> handler)
    {
        doorEvents[doorName] += handler;
    }

    // PuppyAI 클래스에서 이벤트 구독을 취소할 수 있는 메서드
    public void UnsubscribeFromDoorEvent(DoorNames doorName, EventHandler<DoorEventArgs> handler)
    {
        doorEvents[doorName] -= handler;
    }
}
