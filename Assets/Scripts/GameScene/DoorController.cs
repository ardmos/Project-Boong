using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Sprite doorOpen;
    public Vector3 openedDoorPosition;
    public bool isDoorOpen;

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
        // 4. NavMesh rebake
        GetComponent<NavMeshModifier>().area = 0;
        NavMeshManager.Instance.ReBake();
    }
}
