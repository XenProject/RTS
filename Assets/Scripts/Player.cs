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
    private LayerMask movementMask;
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
            if (Physics.Raycast(ray,out hit))
            {

            }
        }
	}

    public void AddSelectedUnit(GameObject selectedObj, bool clear = true)
    {
        if (clear) selected.Clear();
        selected.Add(selectedObj);
    }
}
