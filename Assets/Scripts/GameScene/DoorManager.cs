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

        // 각 문에 대한 DoorController 찾아서 추가
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

    // 문을 열고 상태 변경 이벤트 발생
    public void OpenDoor(DoorName doorName)
    {
        doorControllers[doorName].OnDoorOpened();
        doorEvents[doorName].Invoke(this, new DoorEventArgs(DoorEvent.Opened, doorName));
    }

    // 문을 닫고 상태 변경 이벤트 발생
    public void CloseAllDoors()
    {
        foreach (DoorName doorName in Enum.GetValues(typeof(DoorName)))
        {
            doorControllers[doorName].OnDoorClosed();
            doorEvents[doorName].Invoke(this, new DoorEventArgs(DoorEvent.Closed, doorName));
        }
    }

    // PuppyAI 클래스에서 각 문에 대한 이벤트를 구독할 수 있는 메서드
    public void SubscribeToDoorEvent(DoorName doorName, EventHandler<DoorEventArgs> handler)
    {
        doorEvents[doorName] += handler;
    }

    // PuppyAI 클래스에서 이벤트 구독을 취소할 수 있는 메서드
    public void UnsubscribeFromDoorEvent(DoorName doorName, EventHandler<DoorEventArgs> handler)
    {
        doorEvents[doorName] -= handler;
    }
}
