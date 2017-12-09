using UnityEngine;

/*public interface IInteractable{
    float GetRadius();
    int GetOwner();
    void OnMouseDown();
}*/

public abstract class Interactable : MonoBehaviour
{
    public float Radius;
    public Player Owner;

    public abstract void OnMouseDown();
}
