using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildsButton : UnitButton
{
    public void SpawnBuilding()
    {
        CameraController.SpawnBuild(prefab);
    }


    public override void SpawnUnit()
    {
       // base.SpawnUnit();
    }

}
