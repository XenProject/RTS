using UnityEngine;

/*public interface IInteractable{
    float GetRadius();
    int GetOwner();
    void OnMouseDown();
}*/

public abstract class Interactable : MonoBehaviour
{
    public float Radius;
    public int Owner;

    public abstract void OnMouseDown();
}
