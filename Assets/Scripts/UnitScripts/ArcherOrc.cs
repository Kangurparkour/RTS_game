using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherOrc : Enemy
{
    [Header("Orc archer")]
    [Range(0, .3f), SerializeField] float shootDuration = 0;
    [SerializeField] GameObject shootPlace;
    [SerializeField] LayerMask shootingLayer;

    LineRenderer lineRenderer;


    protected override void Start()
    {
        base.Start();
        lineRenderer = shootPlace.GetComponent<LineRenderer>();
        EndShooting();
    }
    public override void DealDamage()
    {
        if (shoot())
        {
            base.DealDamage();
        }


    }
    protected bool shoot()
    {
        Vector3 startPos = shootPlace.transform.position + Vector3.up;
        Vector3 direction = transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(startPos, direction, out hit, attackDistance))
        {
            Debug.Log(hit.point);

            StartShooting(startPos, hit.point);
            if (hit.collider.gameObject.tag == "Soldier");
                Warrior allyUnit = hit.collider.GetComponent<Warrior>();
            return allyUnit;
        }

        return false;
    }

    protected void StartShooting(Vector3 linestart, Vector3 lineEnd)
    {

        lineRenderer.SetPositions(new Vector3[] { linestart, lineEnd });
        lineRenderer.enabled = true;
        Invoke("EndShooting", shootDuration);
    }

    protected void EndShooting()
    {
        lineRenderer.enabled = false;
    }


}
