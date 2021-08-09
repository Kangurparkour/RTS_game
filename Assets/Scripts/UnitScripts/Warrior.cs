using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Unit, ISelectable
{
    [Header("Ally")]
    List<Enemy> seenEnemies = new List<Enemy>();
    public bool isAutoAttackMode = false;

    Enemy ClosestEnemy
    {
        get
        {

            if (seenEnemies == null || seenEnemies.Count <= 0) return null;
            float minDistance = float.MaxValue;
            Enemy closestEnemy = null;

            foreach (Enemy enemy in seenEnemies)
            {
                if (!enemy && !enemy.IsAlive) continue;
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }


        set
        {
            ClosestEnemy = value;
        }
    }
    protected override void OnTriggerEnter(Collider obj)
    {
        base.OnTriggerEnter(obj);
        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy && !seenEnemies.Contains(enemy))
            seenEnemies.Add(enemy);
    }

    private void UpdateSight()
    {
        if (ClosestEnemy && ClosestEnemy.IsAlive && isAutoAttackMode)
        {
            GetCommand(ClosestEnemy.GetComponent<Enemy>());
        }
    }

    protected override void Idling()
    {
        base.Idling();
        UpdateSight();
    }
    protected override void Moveing()
    {
        base.Moveing();
        UpdateSight();
    }


    public void SetSelected(bool isSelected)
    {
        healthBar.gameObject.SetActive(isSelected);
    }

    protected void GetCommand(Vector3 destination)
    {
        agent.SetDestination(destination);
        task = Task.move;
    }
    protected void GetCommand(Enemy enemy)
    {
        if (enemy)
        {
            target = enemy.transform;
            task = Task.chase;
        }
    }
    protected void GetCommand(Warrior warrior)
    {
        if (warrior)
        {
            target = warrior.transform;
            task = Task.chase;
        }
    }
}
