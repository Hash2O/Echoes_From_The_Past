using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class OrbsSpawner : MonoBehaviour
{
    public int numberOfOrbsToSpawn = 5;
    public GameObject orbPrefab;
    public float height;

    public List<GameObject> spawnedOrbs;

    public int maxNumberOfTry = 100;
    private int currentNumberOfTry = 0;

    //Singleton : ici, donner accès à ce script plus facilement aux autres scripts, comme Ghost
    // qui doit trouver l'orbe la plus proche de chaque GO instancié
    public static OrbsSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        MRUK.Instance.RegisterSceneLoadedCallback(SpawnOrbs);
    }

    public void EatOrb(GameObject orb)
    {
        spawnedOrbs.Remove(orb);
        Destroy(orb);

        //Easy finish
        if (spawnedOrbs.Count == 0)
        {
            //Restart si les ghost ont mangé tous les orbes
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public void SpawnOrbs()
    {
        for (int i = 0; i < numberOfOrbsToSpawn; i++)
        {
            Vector3 randomPosition = Vector3.zero;
            
            MRUKRoom room = MRUK.Instance.GetCurrentRoom();

            while (currentNumberOfTry < maxNumberOfTry)
            {
                bool hasFound = room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.FACING_UP,
                                    1,
                                    LabelFilter.Included(MRUKAnchor.SceneLabels.FLOOR),
                                    out randomPosition,
                                    out Vector3 n);
                
                if (hasFound) break;

                currentNumberOfTry++;
            }

            randomPosition.y = height;

            GameObject spawned = Instantiate(orbPrefab, randomPosition, Quaternion.identity);

            spawnedOrbs.Add(spawned);
        }
    }
}
