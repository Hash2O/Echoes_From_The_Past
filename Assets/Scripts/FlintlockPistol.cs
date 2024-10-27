using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlintlockPistol : MonoBehaviour
{
    public OVRInput.RawButton shootingButton;
    public LineRenderer linePrefab;
    public GameObject shootImpactPrefab;
    public Transform shootingPoint;
    public float maxLineDistance = 5;
    public float lineShowTimer = 0.3f;
    public LayerMask layerMask;
    public AudioSource audioSource;
    public AudioClip shootingAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(shootingButton))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        audioSource.PlayOneShot(shootingAudioClip);

        Ray ray = new Ray(shootingPoint.position, shootingPoint.forward);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit, maxLineDistance, layerMask);

        Vector3 endPoint = Vector3.zero;

        if(hasHit)
        {
            //stop the ray
            endPoint = hit.point;

            Ghost ghost = hit.transform.parent.GetComponent<Ghost>();

            if(ghost)
            {
                hit.collider.enabled = false;
                ghost.DispellGhost();
            }
            else
            {
                //adjusting impact prefab with hit point normal
                Quaternion shootImpactRotation = Quaternion.LookRotation(-hit.normal);

                //Instatiating impact at hit position
                GameObject shootImpact = Instantiate(shootImpactPrefab, hit.point, shootImpactRotation);

                //Remove impact after a sec
                Destroy(shootImpact, 1f);
            }
        }
        else
        {
            endPoint = shootingPoint.position + shootingPoint.forward * maxLineDistance;
        }


        LineRenderer line = Instantiate(linePrefab);
        line.positionCount = 2;
        line.SetPosition(0, shootingPoint.position);

        line.SetPosition(1, endPoint);

        Destroy(line.gameObject, lineShowTimer);
    }

}
