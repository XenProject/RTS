using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Interactable
{
    private NavMeshAgent agent;
    [SerializeField]
    private Transform target;

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

    public void Start()
    {
        Player.Instance = Camera.main.GetComponentInParent<Player>();
        agent = GetComponent<NavMeshAgent>();
    }

    public override void OnMouseDown()
    {
        Player.Instance.AddSelectedUnit(gameObject);
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
        agent.stoppingDistance = 0;
    }
}
