using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Interactable
{
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

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        maxHealth = 100;
        curHealth = maxHealth;
        IsBuilder = false;
        Name = "TestUnit";
        Priority = 0;
        damage = 20;
        attackSpeed = 1f;
        attackDelay = 0.3f;
    }

    public override void OnMouseDown()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
            GameManager.MyPlayer.AddSelectedObject(this);
    }

    void Update()
    {
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
        if (target != null)
        {
            agent.SetDestination(target.position);
            Interactable targetObj = target.GetComponent<Interactable>();
            if ( targetObj.Owner != this.Owner && attackCooldown <= 0)
            {
                float distance = Vector3.Distance(target.position, transform.position);
                if(distance <= agent.stoppingDistance)
                {
                    StartCoroutine(DoDamage(targetObj, attackDelay));
                    attackCooldown = 1f / attackSpeed;
                    GetComponent<Animator>().SetTrigger("attack");
                }
            }
        }
        if (SelectedIcon != null)
        {
            SelectedIcon.transform.GetChild(0).GetComponent<Image>().fillAmount = GetHealthPercentage();
        }
        if(currentBuilding != null)
        {
            float distance = Vector3.Distance(currentBuilding.transform.position, transform.position);
            if(distance <= agent.stoppingDistance)
            {
                currentBuilding.GetComponent<MeshRenderer>().material.color = Color.white;
                currentBuilding.GetComponent<NavMeshObstacle>().enabled = true;
                currentBuilding.Planed = false;
                currentBuilding = null;
                RemoveFocus();
                agent.ResetPath();
            }
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
        DeleteCurrentBuilding();
        RemoveFocus();
        agent.SetDestination(targetPoint);
    }

    public void SetFocus(Transform target)
    {
        DeleteCurrentBuilding();

        agent.stoppingDistance = target.GetComponent<Interactable>().Radius;
        this.target = target;
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
            SetFocus(building.transform);
            currentBuilding = building.GetComponent<Building>();
        }
    }

    IEnumerator DoDamage(Interactable target, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(target != null)
            target.TakeDamage(damage);
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
