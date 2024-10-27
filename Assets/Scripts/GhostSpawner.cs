using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class GhostSpawner : MonoBehaviour
{
    public float spawnTimer = 1;
    public GameObject ghostToSpawn;

    public float minEdgeDistance = 0.5f;
    public MRUKAnchor.SceneLabels spawnLabels;
    public float normalOffset;

    public int spawnTry = 100;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //To prevent issue if no room is existing or loaded
        if (!MRUK.Instance && !MRUK.Instance.IsInitialized) return;

        timer += Time.deltaTime;

        if (timer > spawnTimer)
        {
            SpawnGhost();

            //reset timer
            timer -= spawnTimer;
        }
    }

    public void SpawnGhost()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();

        int currentTry = 0;

        while (currentTry < spawnTry) {

            bool hasFoundPosition = room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);

            if (hasFoundPosition)
            {

                //random positioning ghosts
                Vector3 randomPositionNormalOffset = pos + norm * normalOffset;

                //ghost on the ground
                randomPositionNormalOffset.y = 0;

                Instantiate(ghostToSpawn, randomPositionNormalOffset, Quaternion.identity);

                return;
            }
            else
            {
                currentTry++;
            }

        }
        
    }
}
