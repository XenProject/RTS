using UnityEngine;

public interface IInteractable{
    float GetRadius();
    int GetOwner();
    void OnMouseDown();
}
