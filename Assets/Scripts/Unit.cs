using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [Header("Unit")]
    [SerializeField] GameObject hpSliderPrefab;
    [SerializeField] float MaxHp = 80;
    [SerializeField] float actualHp;
    [SerializeField] protected float attackDistance = 1;
    [SerializeField] float attackDamage = 1;
    [SerializeField] float attackCooldown;
    [SerializeField] float timeToDestroyDeadUnit = 10f;
    protected enum Task { idle, move, chase, attack }
    public static List<Unit> SelectableUnits { get { return selectableUnits; } }
    static List<Unit> selectableUnits = new List<Unit>();

    protected Animator animator;
    protected NavMeshAgent agent;
    protected Task task = Task.idle;
    protected Transform target;

    private float attackTimer;
    private float timer;
    protected HealthBar healthBar;
    public float Hp_Percent
    {
        get { return actualHp / MaxHp; }
    }
    public bool IsAlive { get { return actualHp > 0; } }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        actualHp = MaxHp;
        healthBar = Instantiate(hpSliderPrefab, transform).GetComponent<HealthBar>();

    }

    protected virtual void Start()
    {
        timer = timeToDestroyDeadUnit;

        if (this is ISelectable)
        {
            selectableUnits.Add(this);
            (this as ISelectable).SetSelected(false);
        }

    }

    protected virtual void Update()
    {
        if (IsAlive)
        {
            switch (task)
            {
                case Task.idle: Idling(); break;
                case Task.move: Moveing(); break;
                case Task.chase: Chasing(); break;
                case Task.attack: Attacking(); break;
            }
        }
        else
        {
            if ((timer -= Time.deltaTime) <= 5)
            {
                agent.velocity = Vector3.zero;
                Destroy(this.gameObject);
            }

        }

        Animate();
    }

    protected virtual void OnTriggerEnter(Collider obj)
    {

    }
    protected virtual void OnTriggerExit(Collider obj)
    {

    }

    private void OnDestroy()
    {
        if (this is ISelectable) selectableUnits.Remove(this);
    }

    protected virtual void Idling()
    {
        agent.velocity = Vector3.zero;
    }
    protected virtual void Attacking()
    {
        if (target)
        {
            agent.velocity = Vector3.zero;
            transform.LookAt(target);
            var distance = Vector3.Distance(target.position, transform.position);
            if (distance <= attackDistance)
            {
                if ((attackTimer -= Time.deltaTime) <= 0)
                    Attack();

            }
            else
            {
                task = Task.chase;
            }
        }
        else
            task = Task.idle;
    }
    protected virtual void Moveing()
    {
        var distanceToDestination = Vector3.Distance(agent.destination, transform.position);
        if (distanceToDestination <= agent.stoppingDistance)
        {
            task = Task.idle;
        }
    }
    protected virtual void Chasing()
    {
        if (target)
        {
            agent.SetDestination(target.position);
            var distance = Vector3.Distance(agent.destination, transform.position);
            if (distance <= attackDistance)
            {
                task = Task.attack;
            }
        }
        else
        {
            task = Task.idle;
        }
    }
    protected virtual void Animate()
    {
        var speedVecor = agent.velocity;
        speedVecor.y = 0;
        float speed = speedVecor.magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsAlive", IsAlive);
    }

    public virtual void Attack()
    {
        if (target)
        {
            Unit unit = target.GetComponent<Unit>();
            if (unit && unit.IsAlive)
            {
                animator.SetTrigger("Attack");
                attackTimer = attackCooldown;
            }
            else
                target = null;
        }

    }

    public virtual void DealDamage()
    {
        if (target)
        {
            Unit unit = target.GetComponent<Unit>();
            if (unit)
            {
                unit.ReciveDamage(attackDamage);
            }
        }
    }

    public virtual void ReciveDamage(float damage)
    {
        if (IsAlive)
            actualHp -= damage;

        if (!IsAlive)
        {
            agent.enabled = false;
            foreach (var collider in GetComponents<Collider>())
            {
                collider.enabled = false;
            }
        }

    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

}
