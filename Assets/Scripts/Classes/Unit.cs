using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
        agent = GetComponent<NavMeshAgent>();
        maxHealth = 100;
        curHealth = maxHealth;
    }

    public override void OnMouseDown()
    {
        GameManager.MyPlayer.AddSelectedUnit(this);
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
}
