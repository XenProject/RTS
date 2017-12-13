using UnityEngine;
using UnityEngine.UI;

/*public interface IInteractable{
    float GetRadius();
    int GetOwner();
    void OnMouseDown();
}*/

public abstract class Interactable : MonoBehaviour
{
    public Sprite Icon;
    public GameObject SelectedIcon;
    public float Radius;
    public Player Owner;

    [SerializeField]
    public string Name;
    [SerializeField]
    protected int curHealth;
    [SerializeField]
    protected int maxHealth;

    public int GetHealth()
    {
        return curHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercentage()
    {
        return (float)curHealth / maxHealth;
    }

    public void TakeDamage(int damage)
    {
        this.curHealth -= damage;
        if (curHealth <= 0) Die();
        if (curHealth > maxHealth) curHealth = maxHealth;
    }

    public void Die()
    {
        GameObject.DestroyImmediate(this.gameObject);
    }

    public abstract void OnMouseDown();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    /*public static bool operator ==(Interactable left, Interactable right)
    {
        if (left.Equals(null)) return false;
        if (left.Name == right.Name)
            return true;
        return false;
    }

    public static bool operator !=(Interactable left, Interactable right)
    {
        if (!left.Equals(null)) return true;
        if (left.Name == right.Name)
            return false;
        return true;
    }
    
    public override bool Equals(object other)
    {
        return base.Equals(other);
    }
    
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }*/
}
