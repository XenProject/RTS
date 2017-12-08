using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : MonoBehaviour {

    [SerializeField]
    private int teamNumber;
    [SerializeField]
    private List<GameObject> selected;//Выбранные юниты
    [SerializeField]
    private Resource[] resources = new Resource[Enum.GetNames(typeof(ResourceType)).Length];

    // Use this for initialization
    void Start () {
        //Инициализация всех типов ресурсов
		for(int i = 0; i< resources.Length; i++)
        {
            resources[i] = new Resource((ResourceType)i);
        }
        //Инициализация номера команды
        teamNumber = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (selected.Count > 0 && selected[0].GetComponent<IInteractable>().GetOwner() == teamNumber)
            {
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                switch ( LayerMask.LayerToName(hit.transform.gameObject.layer))
                {
                    case "Clickable":
                        foreach (GameObject unit in selected)
                        {
                            if (unit.GetComponent<Unit>() != null)
                            {
                                unit.GetComponent<Unit>().SetFocus(hit.transform);
                            }
                            else break;
                        }
                        break;
                    default:
                        foreach (GameObject unit in selected)
                        {
                            if (unit.GetComponent<Unit>() != null)
                            {
                                unit.GetComponent<Unit>().MoveToPoint(hit.point);
                            }
                            else break;
                        }
                        break;
                }
            }
        }
	}

    public void AddSelectedUnit(GameObject selectedObj, bool clear = true)
    {
        if (clear) selected.Clear();
        selected.Add(selectedObj);
    }
}
