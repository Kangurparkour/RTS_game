using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{

    protected enum Task { idle, move, chase, attack }
    public static List<ISelectable> SelectableUnits { get { return selectableUnits; } }
    static List<ISelectable> selectableUnits = new List<ISelectable>();

    protected Animator animator;
    protected NavMeshAgent agent;
    protected Task task = Task.idle;

    [SerializeField] float MaxHp = 80;
    [SerializeField] float actualHp;
    [SerializeField] GameObject hpSliderPrefab;

    protected HealthBar healthBar;
    public float Hp_Percent
    {
        get { return actualHp / MaxHp; }
    }
    public bool IsAlive { get { return actualHp > 0; } }

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
        if (IsAlive)
        {
            switch (task)
            {
                case Task.idle: Idling(); break;
                case Task.move: Moveing(); break;
                case Task.chase: chasing();  break;
                case Task.attack: Attacking();  break;
            }
        }

        Animate();
    }

    protected virtual void Idling() 
    {
        agent.velocity = Vector3.zero;
    }
    protected virtual void Attacking() 
    {
        agent.velocity = Vector3.zero;
    }
    protected virtual void Moveing() 
    {
        var distanceToDestination = Vector3.Distance(agent.destination,transform.position);
        if(distanceToDestination <= agent.stoppingDistance)
        {
            task = Task.idle;
        }
    }
    protected virtual void chasing() 
    {

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
