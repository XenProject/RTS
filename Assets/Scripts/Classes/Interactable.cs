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

    public abstract void OnMouseDown();
}
