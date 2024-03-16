using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager Instance { get; private set; }

    private NavMeshSurface Surface2D;

    private void Awake()
    {
        Instance = this;
        Surface2D = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        Surface2D.BuildNavMeshAsync();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void ReBake()
    {
        Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }
}
