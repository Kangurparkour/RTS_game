using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [Header("Enemy")]
    [SerializeField] float walkingSpeed = 1f;
    [SerializeField] float chasingSpeed = 5f;
    [SerializeField] float patrolRadious = 5f;
    [SerializeField] float idleCooldown = 2f;
    [SerializeField] Transform[] wayPoints;


    Vector3 SpawnPoint;
    float idleTimer;
    int nextWaypoint=0;

    List<Unit> seenSoldiers = new List<Unit>();
    Unit ClosestSoldier
    {
        get
        {
            if (seenSoldiers == null || seenSoldiers.Count <= 0) return null;

            float minDistance = float.MaxValue;
            Unit closestAlly = null;
            foreach (Unit ally in seenSoldiers)
            {
                if (!ally && !ally.IsAlive) continue;
                float distance = Vector3.Distance(ally.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestAlly = ally;
                }
            }
            return closestAlly;
        }
    }

    protected override void Start()
    {
        base.Start();
        walkingSpeed = agent.speed;
        SpawnPoint = transform.position;
    }

    protected override void Update()
    {
        base.Update();

    }

    protected override void OnTriggerEnter(Collider obj)
    {
        base.OnTriggerEnter(obj);
        var AllyUnit = obj.GetComponent<Unit>();
        if (AllyUnit && !seenSoldiers.Contains(AllyUnit) && AllyUnit is ISelectable)
            seenSoldiers.Add(AllyUnit);


    }

    protected override void OnTriggerExit(Collider obj)
    {
        base.OnTriggerExit(obj);
    }
    protected override void Idling()
    {
        base.Idling();
        UpdatePatrol();
        if ((idleTimer -= Time.deltaTime) < 0)
        {
            idleTimer = idleCooldown;
            task = Task.move;
        }

    }

    protected override void Moveing()
    {
        base.Moveing();
        agent.speed = walkingSpeed;
        UpdatePatrol();
    }
    protected override void Chasing()
    {
        base.Chasing();
        agent.speed = chasingSpeed;
    }

    private void UpdateSight()
    {
        Unit unit = ClosestSoldier;
        if (unit && unit.IsAlive)
        {
            target = unit.transform;
            task = Task.chase;
        }
    }

    public override void ReciveDamage(float damage)
    {
        base.ReciveDamage(damage);
       
    }

    private void UpdatePatrol()
    {
        agent.speed = walkingSpeed;
        agent.destination = wayPoints[nextWaypoint].position;
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            nextWaypoint = (nextWaypoint+1) % wayPoints.Length;
        }
        UpdateSight();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.blue;
        SpawnPoint = transform.position;
        Gizmos.DrawWireSphere(SpawnPoint, patrolRadious);
    }

}
