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
    [SerializeField]
    protected int armor;

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

    public int GetArmor()
    {
        return armor;
    }

    public void TakeDamage(int damage)
    {
        damage -= armor;
        this.curHealth -= damage;
        if (curHealth <= 0) Die();
        if (curHealth > maxHealth) curHealth = maxHealth;
    }

    public void Die()
    {
        Player player = GameManager.MyPlayer;
        if(player.GetNowSelectedType() == this)
        {
            if (this as Unit)
            {
                player.FindOnDie(this);
                Owner.AllUnits.Remove(this as Unit);
            }
            else
            {
                player.SetupNowSelected(-1);
            }
        }
    }

    public abstract void OnMouseDown();

    public void OnMouseEnter()
    {
        if(GameManager.MyPlayer != Owner && GameManager.MyPlayer.Selected.Count > 0 && GameManager.MyPlayer.Selected[0].Owner != Owner)
        {
            GameManager.Instance.GetComponent<InputManager>().SetCursorByName("Attack");
        }
        else
        {
            GameManager.Instance.GetComponent<InputManager>().SetCursorByName();
        }
    }

    public void OnMouseExit()
    {
        if (GameManager.MyPlayer != Owner)
        {
            GameManager.Instance.GetComponent<InputManager>().SetCursorByName();
        }
    }

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
