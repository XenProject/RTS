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
    }

    public override void OnMouseDown()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
            GameManager.MyPlayer.AddSelectedObject(this);
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
        if(currentBuilding != null)
        {
            if(agent.remainingDistance <= currentBuilding.Radius)
            {
                currentBuilding.GetComponent<MeshRenderer>().material.color = Color.white;
                currentBuilding.Planed = false;
                currentBuilding = null;
                agent.isStopped = true;
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

    public void Build(GameObject building)
    {
        DeleteCurrentBuilding();
        if (isBuilder)
        {
            agent.SetDestination(building.transform.position);
            currentBuilding = building.GetComponent<Building>();
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
