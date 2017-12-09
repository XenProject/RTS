using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class Player{
    public bool isBot = false;

    [SerializeField]
    private int teamNumber;
    [SerializeField]
    private List<Interactable> selected;//Выбранные юниты
    [SerializeField]
    private Resource[] resources = new Resource[Enum.GetNames(typeof(ResourceType)).Length];
    [SerializeField]
    public List<Unit> AllUnits;

    public int TeamNumber
    {
        get
        {
            return teamNumber;
        }

        set
        {
            teamNumber = value;
        }
    }

    public List<Interactable> Selected
    {
        get
        {
            return selected;
        }

        set
        {
            selected = value;
        }
    }

    public Player()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            resources[i] = new Resource((ResourceType)i);
        }
        selected = new List<Interactable>();
        AllUnits = new List<Unit>();
    }

    public void AddSelectedUnit(Interactable selectedObj, bool clear = true)
    {
        if (clear) Selected.Clear();
        Selected.Add(selectedObj);
    }

    public void AddUnitToAllUnits(Unit newUnit)
    {
        AllUnits.Add(newUnit);
    }

    public Resource[] GetAllResources()
    {
        return resources;
    }
}
