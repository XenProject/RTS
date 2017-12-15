using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Interactable
{
    public Image HealthBarFill;

    public float attackSpeed;

    private float attackCooldown = 0f;
    private float attackDelay;
    private int damage;

    private NavMeshAgent agent;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool isBuilder;

    private Building currentBuilding;
    private int priority;
    [SerializeField]
    private State state;

    [SerializeField]
    private Order lastOrder = new Order();

    private enum State
    {
        Idle,
        Hold,
        Move,
        Attack,
        Build
    }

    private class Order
    {
        public Transform lastTarget { get; private set; }
        public Vector3 lastTargetPos { get; private set; }

        public Order()
        {
            lastTarget = null;
            lastTargetPos = Vector3.zero;
        }

        public void SetOrder(Transform lastTarget)
        {
            this.lastTargetPos = Vector3.zero;
            this.lastTarget = lastTarget;
        }

        public void SetOrder(Vector3 lastTargetPos)
        {
            this.lastTargetPos = lastTargetPos;
            this.lastTarget = null;
        }

        public void Clear()
        {
            this.lastTarget = null;
            this.lastTargetPos = Vector3.zero;
        }
    }

    public NavMeshAgent Agent
    {
        get
        {
            return agent;
        }

        set
        {
            agent = value;
        }
    }

    public bool IsBuilder
    {
        get
        {
            return isBuilder;
        }

        set
        {
            isBuilder = value;
        }
    }

    public int Priority
    {
        get
        {
            return priority;
        }

        set
        {
            priority = value;
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        maxHealth = 100;
        curHealth = maxHealth;
        IsBuilder = false;
        Name = "TestUnit";
        Priority = 0;
        damage = 20;
        armor = 0;
        attackSpeed = 1f;
        attackDelay = 0.3f;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<SphereCollider>().radius = AgroRadius / 2;
        state = State.Idle;
    }

    private void OnTriggerStay(Collider other)
    {
        if (target == null && state == State.Idle)
        {
            if (other.GetComponent<Unit>())
            {
                Unit enemyUnit = other.GetComponent<Unit>();
                if (enemyUnit.Owner != Owner)
                {
                    this.SetFocus(enemyUnit.transform);
                }
            }
            else
            {
                Building enemyBuilding = other.GetComponent<Building>();
                if(enemyBuilding != null && !enemyBuilding.Planed && enemyBuilding.Owner != Owner)
                {
                    this.SetFocus(enemyBuilding.transform);
                }
            }
        
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AgroRadius);

        //Vector3 vec = (transform.position - target.position).normalized * target.GetComponent<Building>().Radius;
        //Gizmos.DrawLine(transform.position, target.position + vec);
    }

    public override void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.MyPlayer.AddSelectedObject(this);
            OnMouseEnter();
        }    
    }

    void Update()
    {
        float lifePrt = GetHealthPercentage();
        HealthBarFill.fillAmount = lifePrt;
        HealthBarFill.color = Color.Lerp(Color.red, Color.green, lifePrt);

        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
        if (target != null)
        { 
            if (target.GetComponent<Building>() && currentBuilding == null)//т.к здание, вычисляем ближайшую доступную точку
            {
                Vector3 vec = (transform.position - target.position).normalized * (target.GetComponent<Building>().Radius - 0.2f) + target.position;
                agent.stoppingDistance = 0f;
                agent.SetDestination(vec);
            }
            else
            {
                agent.SetDestination(target.position);
            }
            Interactable targetObj = target.GetComponent<Interactable>();
            if ( targetObj.Owner != this.Owner && attackCooldown <= 0)
            {
                float distance = Vector3.Distance(target.position, transform.position);
                if(distance <= target.GetComponent<Interactable>().Radius)
                {
                    StartCoroutine(DoDamage(targetObj, attackDelay));
                    attackCooldown = 1f / attackSpeed;
                    GetComponent<Animator>().SetTrigger("attack");
                    state = State.Attack;
                    agent.isStopped = true;
                }
            }
        }
        if (SelectedIcon != null)
        {
            SelectedIcon.transform.GetChild(0).GetComponent<Image>().fillAmount = lifePrt;
            SelectedIcon.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, lifePrt);
        }
        if(currentBuilding != null)
        {
            float distance = Vector3.Distance(currentBuilding.transform.position, transform.position);
            if(distance <= agent.stoppingDistance)
            {
                currentBuilding.GetComponent<MeshRenderer>().material.color = Color.white;
                currentBuilding.GetComponent<NavMeshObstacle>().enabled = true;
                currentBuilding.Planed = false;
                currentBuilding.transform.Find("MinimapIcon").GetComponent<MeshRenderer>().material.color = GameManager.Instance.TeamColors[Owner.TeamNumber];
                currentBuilding.transform.Find("MinimapIcon").GetComponent<MeshRenderer>().enabled = true;
                currentBuilding = null;
                RemoveFocus();
                agent.ResetPath();
            }
        }
        //States
        if (!agent.hasPath && state == State.Move && target == null)
        {
            state = State.Idle;
        }
        if(state == State.Attack)
        {
            FaceTarget();
        }
    }

    private void DeleteCurrentBuilding()
    {
        if (currentBuilding != null)
        {
            GameObject.DestroyImmediate(currentBuilding.gameObject);
            currentBuilding = null;
        }
    }

    public void MoveToPoint(Vector3 targetPoint)
    {
        if (state != State.Attack)
        {
            DeleteCurrentBuilding();
            RemoveFocus();
            agent.SetDestination(targetPoint);
            state = State.Move;
        }
        else
            lastOrder.SetOrder(targetPoint);
    }

    public void SetFocus(Transform target)
    {
        if (state != State.Attack)
        {
            DeleteCurrentBuilding();
            agent.stoppingDistance = target.GetComponent<Interactable>().Radius;
            this.target = target;
            state = State.Move;
        }
        else
            lastOrder.SetOrder(target);
    }

    public void RemoveFocus()
    {
        target = null;
        agent.stoppingDistance = 0f;
    }

    public bool CompareWith(Unit unit)
    {
        if( this.Name == unit.Name && ( this.isBuilder == unit.isBuilder ))
        {
            return true;
        }
        return false;
    }

    public void Build(GameObject building)
    {
        if (isBuilder)
        {
            RemoveFocus();
            SetFocus(building.transform);
            currentBuilding = building.GetComponent<Building>();
        }
    }

    public void FaceTarget()
    {
        if (target == null) return;
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
    }

    IEnumerator DoDamage(Interactable target, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(target != null)
            target.TakeDamage(damage);
    }

    public void AttackEnd()
    {
        agent.isStopped = false;
        state = State.Move;
        if (target == null)
        {
            agent.ResetPath();
            state = State.Idle;
        }
        CheckNextOrder();
    }

    private void CheckNextOrder()
    {
        if (lastOrder.lastTarget != null)
        {
            SetFocus(lastOrder.lastTarget);
            lastOrder.Clear();
        }
        if(lastOrder.lastTargetPos != Vector3.zero)
        {
            MoveToPoint(lastOrder.lastTargetPos);
            lastOrder.Clear();
        }
    }

    public void SetupHoldState()
    {
        if (state != State.Attack)
        {
            RemoveFocus();
            agent.ResetPath();
            DeleteCurrentBuilding();
            state = State.Hold;
        }
    }

    public void Stop()
    {
        if(state != State.Attack)
        {
            RemoveFocus();
            agent.ResetPath();
            DeleteCurrentBuilding();
            state = State.Idle;
        }
    }
    /*
    public static bool operator <(Unit left, Unit right)
    {
        return left.priority < right.priority;
    }

    public static bool operator >(Unit left, Unit right)
    {
        return left.priority > right.priority;
    }*/
}
