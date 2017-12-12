using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Interactable
{
    private NavMeshAgent agent;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool isBuilder;

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
    }

    public override void OnMouseDown()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
            GameManager.MyPlayer.AddSelectedObject(this);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
        if (SelectedIcon != null)
        {
            SelectedIcon.transform.GetChild(0).GetComponent<Image>().fillAmount = GetHealthPercentage();
        }
    }

    public void MoveToPoint(Vector3 targetPoint)
    {
        RemoveFocus();
        agent.SetDestination(targetPoint);
    }

    public void SetFocus(Transform target)
    {
        agent.stoppingDistance = target.GetComponent<Interactable>().Radius * 0.8f;
        this.target = target;
    }

    public void RemoveFocus()
    {
        target = null;
        agent.stoppingDistance = 0.5f;
    }

    public bool CompareWith(Unit unit)
    {
        if( this.Name == unit.Name && ( this.isBuilder == unit.isBuilder ))
        {
            return true;
        }
        return false;
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
