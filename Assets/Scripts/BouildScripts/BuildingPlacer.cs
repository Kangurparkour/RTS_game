using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] Color enableColor = Color.blue;
    [SerializeField] Color disableColor = Color.red;
    [SerializeField] float maxNavMeshDistance = 1f;

    Vector3 startPos;
    [SerializeField] Vector3 posOffset;
    [SerializeField] Vector3 boxOffset;
    [SerializeField] Vector3 boxSize;
    [SerializeField] LayerMask boxLayerMask=-1;

    new Renderer renderer;

    RaycastHit rayHit;
    NavMeshHit navHit;
    NavMeshPath path;

    private void Awake()
    {
        startPos = transform.position;
        renderer = GetComponentInChildren<Renderer>(true);
        path = new NavMeshPath();

    }

    private void Update()
    {
        renderer.sharedMaterial.color = CanBuildHere() ? enableColor : disableColor;
    }


    public bool CanBuildHere()
    {
        if (!Physics.CheckBox(transform.position + boxOffset/2f, boxSize,transform.rotation,boxLayerMask))
        {
            if (NavMesh.SamplePosition(transform.position, out navHit, maxNavMeshDistance, NavMesh.AllAreas))
            {
                if (NavMesh.CalculatePath(startPos, transform.position, NavMesh.AllAreas, path))
                {
                    for (int i = 0; i < path.corners.Length-1; i++)
                    {
                        Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.cyan);
                    }

                    return path.status == NavMeshPathStatus.PathComplete;
                }
            }
        }

        return false;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position + posOffset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + boxOffset, boxSize);
        

    }

}
