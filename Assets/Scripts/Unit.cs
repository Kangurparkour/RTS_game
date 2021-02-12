using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public Transform target;

    public static List<ISelectable> SelectableUnits { get { return selectableUnits; } }
    static List<ISelectable> selectableUnits = new List<ISelectable>();

    protected Animator animator;
    [SerializeField] protected NavMeshAgent agent;

    [SerializeField] float MaxHp = 80;
    [SerializeField] float actualHp;
    [SerializeField] GameObject hpSliderPrefab;

    protected HealthBar healthBar;
    public float Hp_Percent
    {
        get { return actualHp / MaxHp; }
    }

    void Start()
    {
        actualHp = MaxHp;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        healthBar = Instantiate(hpSliderPrefab, transform).GetComponent<HealthBar>();
       
        if (this is ISelectable)
        {
            selectableUnits.Add(this as ISelectable);
            (this as ISelectable).SetSelected(false);
        }

    }

    private void OnDestroy()
    {
        if (this is ISelectable) selectableUnits.Remove(this as ISelectable);
    }


    void Update()
    {

        if (target != null)
        {
            agent.SetDestination(target.position);
        }
        else
            Debug.Log("Na razie nie ma celu !");


        Animate();
    }

    protected virtual void Animate()
    {
        var speedVecor = agent.velocity;
        speedVecor.y = 0;
        float speed = speedVecor.magnitude;
        animator.SetFloat("Speed", speed);
    }

    private void TestLIst()
    {
        foreach (Unit unit in SelectableUnits)
        {
            Debug.Log(unit.name);
        }
    }

}
