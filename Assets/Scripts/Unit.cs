using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour, IInteractable
{
    private int owner;
    private Player player;
    private NavMeshAgent agent;

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
        player = Camera.main.GetComponentInParent<Player>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void OnMouseDown()
    {
        player.AddSelectedUnit(gameObject);
    }

    public void MoveToPoint(Vector3 targetPoint)
    {
        agent.SetDestination(targetPoint);
    }
}
