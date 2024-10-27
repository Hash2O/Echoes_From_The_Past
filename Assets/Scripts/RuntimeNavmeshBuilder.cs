using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Meta.XR.MRUtilityKit;
public class RuntimeNavmeshBuilder : MonoBehaviour
{
    private NavMeshSurface _navMeshSurface;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshSurface = GetComponent<NavMeshSurface>();

        //Wait for scene creation before initializing navmesh
        MRUK.Instance.RegisterSceneLoadedCallback(BuildNavmesh);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildNavmesh()
    {
        StartCoroutine("BuildNavmeshRoutine");
    }

    //Making a coroutine to call the navmesh AFTER spawning anchors (obstacles here)
    public IEnumerator BuildNavmeshRoutine()
    {
        yield return new WaitForEndOfFrame();
        _navMeshSurface.BuildNavMesh();
    }

}
