using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public float speed = 0.1f;

    public float eatDistance = 0.2f;

    public GameObject GetClosestOrb()
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        List<GameObject> orbs = OrbsSpawner.instance.spawnedOrbs;

        foreach (var item in orbs)
        {
            //Init des valeurs pour calcul de la distance
            Vector3 ghostPosition = transform.position;
            ghostPosition.y = 0;
            Vector3 orbPosition = item.transform.position;
            orbPosition.y = 0;

            float d = Vector3.Distance(ghostPosition, orbPosition);

            if (d < minDistance)
            {
                minDistance = d;
                closest = item;
            }
        }

        if (minDistance < eatDistance)
        {
            OrbsSpawner.instance.EatOrb(closest);
        }

        return closest;
    }

    // Update is called once per frame
    void Update()
    {
        if(!agent.enabled) return;

        GameObject closest = GetClosestOrb();

        if (closest)
        {
            Vector3 targetPosition = closest.transform.position;

            agent.SetDestination(targetPosition);
            agent.speed = speed;
        }
        else
        {
            Vector3 targetPosition = Camera.main.transform.position;

            agent.SetDestination(targetPosition);
            agent.speed = speed;
        }


    }

    public void DispellGhost()
    {
        //Ghost immobilisé
        agent.enabled = false;

        //Animation Disparition Ghost
        animator.SetTrigger("Death");

    }

    public void RemoveGhost()
    {
        Destroy(gameObject);    
    }
}
