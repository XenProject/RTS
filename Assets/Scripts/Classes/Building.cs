using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Building : Interactable{

    public float BuildingDelay;

    void Start()
    {
        maxHealth = 1000;
        curHealth = maxHealth;
    }

    void Update()
    {
        if (BuildingDelay > 0) BuildingDelay -= Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CurBuild")
            GameManager.Instance.NumIntersection++;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "CurBuild")
            GameManager.Instance.NumIntersection--;
    }

    public override void OnMouseDown()
    {
        if (BuildingDelay <= 0 && GameManager.Instance.GetComponent<InputManager>().CurrentBuilding == null)
        {
            GameManager.MyPlayer.AddSelectedObject(this);
        }
    }
}
