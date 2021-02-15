using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archer : Unit, ISelectable
{
    [Header("Archer")]
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

    public void SetSelected(bool isSelected)
    {
        healthBar.gameObject.SetActive(isSelected);
    }

    private void GetCommand(Vector3 destination)
    {
        agent.SetDestination(destination);
        task = Task.move;

    }
    private void GetCommand(Enemy enemy)
    {
        if (enemy)
        {
            target = enemy.transform;
            task = Task.chase;
        }
    }
    public override void DealDamage()
    {
        if (shoot())
        {
            base.DealDamage();
        }
    }
    private bool shoot()
    {
        Vector3 startPos = shootPlace.transform.position + Vector3.up;
        Vector3 direction = transform.forward;


        RaycastHit hit;
        if (Physics.Raycast(startPos, direction, out hit, attackDistance, shootingLayer))
        {
            StartShooting(startPos, hit.point);
            Unit unit = hit.collider.gameObject.GetComponent<Unit>();
            return unit;
        }

        return false;
    }

    private void StartShooting(Vector3 linestart, Vector3 lineEnd)
    {

        lineRenderer.SetPositions(new Vector3[] { linestart, lineEnd });
        lineRenderer.enabled = true;
        Invoke("EndShooting", shootDuration);
    }

    private void EndShooting()
    {
        lineRenderer.enabled = false;
    }
}
