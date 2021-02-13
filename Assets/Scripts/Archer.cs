using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archer : Unit, ISelectable
{

    public void SetSelected(bool isSelected)
    {
        healthBar.gameObject.SetActive(isSelected);
    }

    
    protected override void Animate()
    {
        base.Animate();

    }


    private void GetCommand(Vector3 destination)
    {
        agent.SetDestination(destination);
        task = Task.move;
      
    }
    private void GetCommand(Enemy enemy)
    {
        //todo
    }

}
