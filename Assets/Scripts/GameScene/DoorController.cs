using NavMeshPlus.Components;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public DoorNames doorName;
    public Sprite doorOpen;
    public Vector3 openedDoorPosition;
    public bool isDoorOpen;
    public ShadowController shadowController;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        isDoorOpen = false;
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    public void Open()
    {
        if (isDoorOpen) return;
        isDoorOpen = true;
        // 1. 이미지 변경으로 인한 위치 재설정
        transform.localPosition = openedDoorPosition;
        // 2. 이미지 변경
        spriteRenderer.sprite = doorOpen;
        // 3. 이 문이 가리고 있던 지역 Shadow 비활성화
        shadowController.DisableShadow();
        // 4. NavMesh rebake
        GetComponent<NavMeshModifier>().area = 0;
        NavMeshManager.Instance.ReBake();
        // 5. DoorManager에게 문 열림 보고
        DoorManager.Instance.OnDoorOpened(doorName);
    }
}
