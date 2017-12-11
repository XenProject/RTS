using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Building : Interactable{

    [SerializeField]
    private bool isCurrentBuild = false;

    public static void BuildByName(string buildingName)
    {
        Debug.Log("Build: " + buildingName);
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Buildings/" + buildingName), new Vector3(50, 1, 50), Quaternion.identity);
        go.GetComponent<Building>().Name = buildingName;
        go.GetComponent<Building>().Owner = GameManager.MyPlayer;
        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<Building>().isCurrentBuild = true;
    }

    void Start()
    {
        maxHealth = 1000;
        curHealth = maxHealth;
    }

    void Update()
    {
        if (isCurrentBuild)
        {
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                this.transform.position = hit.point;
                Debug.Log(GameManager.Instance.NumIntersection);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.NumIntersection++;
    }

    public void OnTriggerExit(Collider other)
    {
        GameManager.Instance.NumIntersection--;
    }

    public override void OnMouseDown()
    {
        GameManager.MyPlayer.AddSelectedObject(this);
    }
}
