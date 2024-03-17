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
        // 1. �̹��� �������� ���� ��ġ �缳��
        transform.localPosition = openedDoorPosition;
        // 2. �̹��� ����
        spriteRenderer.sprite = doorOpen;
        // 3. �� ���� ������ �ִ� ���� Shadow ��Ȱ��ȭ
        shadowController.DisableShadow();
        // 4. NavMesh rebake
        GetComponent<NavMeshModifier>().area = 0;
        NavMeshManager.Instance.ReBake();
        // 5. DoorManager���� �� ���� ����
        DoorManager.Instance.OnDoorOpened(doorName);
    }
}
