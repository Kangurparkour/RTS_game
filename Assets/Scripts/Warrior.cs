using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Unit, ISelectable
{
    
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
        if(enemy)
        {
            target = enemy.transform;
            task = Task.chase;
        }
    }
}
