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

    public abstract void OnMouseDown();
}
