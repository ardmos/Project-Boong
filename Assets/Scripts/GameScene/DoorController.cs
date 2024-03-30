using NavMeshPlus.Components;
using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private NavMeshModifier navMeshModifier;

    public Sprite doorOpen;
    public Sprite doorClose;
    public Vector3 openedDoorPosition;
    public Vector3 closedDoorPosition;
    public bool isDoorOpen;
    public ShadowController shadowController;

    private void Awake()
    {
        isDoorOpen = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        navMeshModifier = GetComponent<NavMeshModifier>();
    }

    public void OnDoorOpened()
    {
        if (isDoorOpen) return;
        isDoorOpen = true;
        // 1. 이미지 변경으로 인한 위치 재설정
        transform.localPosition = openedDoorPosition;
        // 2. 이미지 변경
        spriteRenderer.sprite = doorOpen;
        // 3. 이 문이 가리고 있던 지역 Shadow 비활성화
        shadowController.DisableShadow();
        // 4. NavMesh 지나가지 못하는 영역에서 제거 
        navMeshModifier.area = 0;
        // 5. NavMesh rebake
        NavMeshManager.Instance.ReBake();
    }

    public void OnDoorClosed()
    {
        if (!isDoorOpen) return;
        isDoorOpen = false;
        // 1. 이미지 변경으로 인한 위치 재설정
        transform.localPosition = closedDoorPosition; 
        // 2. 이미지 변경
        spriteRenderer.sprite = doorClose; 
        // 3. 이 문이 가리고 있던 지역 Shadow 활성화
        shadowController.EnableShadow();
        // 4. NavMesh에 지나가지 못하는 영역으로 추가
        navMeshModifier.area = 1;
        // 5. NavMesh rebake
        NavMeshManager.Instance.ReBake();
    }
}
