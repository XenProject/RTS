using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour, IInteractable
{
    public float Radius = 3.0f;
    public int owner;

    private Player player;
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

    public float GetRadius()
    {
        return Radius;
    }

    public int GetOwner()
    {
        return owner;
    }

    public void Start()
    {
        player = Camera.main.GetComponentInParent<Player>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void OnMouseDown()
    {
        player.AddSelectedUnit(gameObject);
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
        agent.stoppingDistance = target.GetComponent<IInteractable>().GetRadius() * 0.8f;
        this.target = target;
    }

    public void RemoveFocus()
    {
        target = null;
        agent.stoppingDistance = 0;
    }
}
