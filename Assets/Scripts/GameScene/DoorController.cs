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
        // 1. �̹��� �������� ���� ��ġ �缳��
        transform.localPosition = openedDoorPosition;
        // 2. �̹��� ����
        spriteRenderer.sprite = doorOpen;
        // 3. �� ���� ������ �ִ� ���� Shadow ��Ȱ��ȭ
        shadowController.DisableShadow();
        // 4. NavMesh �������� ���ϴ� �������� ���� 
        navMeshModifier.area = 0;
        // 5. NavMesh rebake
        NavMeshManager.Instance.ReBake();
    }

    public void OnDoorClosed()
    {
        if (!isDoorOpen) return;
        isDoorOpen = false;
        // 1. �̹��� �������� ���� ��ġ �缳��
        transform.localPosition = closedDoorPosition; 
        // 2. �̹��� ����
        spriteRenderer.sprite = doorClose; 
        // 3. �� ���� ������ �ִ� ���� Shadow Ȱ��ȭ
        shadowController.EnableShadow();
        // 4. NavMesh�� �������� ���ϴ� �������� �߰�
        navMeshModifier.area = 1;
        // 5. NavMesh rebake
        NavMeshManager.Instance.ReBake();
    }
}
