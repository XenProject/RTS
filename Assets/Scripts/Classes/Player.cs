﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (clear) ClearSelectedUnits();
        Selected.Add(selectedObj);
        GameManager.Instance.Portrait.sprite = selected[0].Icon;
        selectedObj.SelectedIcon = GameObject.Instantiate( Resources.Load<GameObject>("UI/SelectedIcon"), GameManager.Instance.SelectedPanel.transform );
        selectedObj.SelectedIcon.GetComponent<Image>().sprite = selectedObj.Icon;
        selectedObj.GetComponentInChildren<Projector>().enabled = true;
    }

    public void AddUnitToAllUnits(Unit newUnit)
    {
        AllUnits.Add(newUnit);
    }

    public Resource[] GetAllResources()
    {
        return resources;
    }

    public void ClearSelectedUnits()
    {
        foreach(Interactable obj in selected)
        {
            obj.GetComponentInChildren<Projector>().enabled = false;
            GameObject.DestroyImmediate(obj.SelectedIcon);
            obj.SelectedIcon = null;
        }
        selected.Clear();
        GameManager.Instance.Portrait.sprite = null;
    }
}
