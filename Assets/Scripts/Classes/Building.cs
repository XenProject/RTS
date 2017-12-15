using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider))]
public class Building : Interactable{

    public bool Planed;
    //private float buildingDelay;

    void Start()
    {
        maxHealth = 1000;
        curHealth = maxHealth;
        armor = 0;
        Planed = true;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<SphereCollider>().radius = AgroRadius / 2;
    }

    void Update()
    {
        //if (buildingDelay > 0) buildingDelay -= Time.deltaTime;
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
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (/*buildingDelay <= 0 && */!Planed && this.tag != "CurBuild")
            {
                GameManager.MyPlayer.AddSelectedObject(this);
            }
        }
    }
}
