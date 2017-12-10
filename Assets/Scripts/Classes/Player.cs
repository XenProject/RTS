using System;
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
        if (clear) ClearSelectedUnits();//Очищаем список выделенных юнитов
        Selected.Add(selectedObj);//Добаляем нового юнита в этот список

        GameManager.Instance.Portrait.sprite = selected[0].Icon;//Изменяем портрет на первого в списке
        GameManager.Instance.Portrait.gameObject.SetActive(true);//Включаем портрет
        //Создаем иконку на панели выделенных юнитов
        selectedObj.SelectedIcon = GameObject.Instantiate( Resources.Load<GameObject>("Prefabs/UI/SelectedIcon"), GameManager.Instance.SelectedPanel.transform );
        selectedObj.SelectedIcon.GetComponent<Image>().sprite = selectedObj.Icon;
        //
        Projector proj = selectedObj.GetComponentInChildren<Projector>();//Рисуем круг под выделенным юнитом в зависимости от команды
        if (GameManager.MyPlayer == selectedObj.Owner)
        {
            proj.material.SetTexture("_ShadowTex", GameManager.Instance.CircleFriendly);
        }
        else
        {
            proj.material.SetTexture("_ShadowTex", GameManager.Instance.CircleEnemy);
        }
        proj.enabled = true;//Включаем отображение круга
    }

    public void AddUnitToAllUnits(Unit newUnit)
    {
        AllUnits.Add(newUnit);
    }

    public Resource[] GetAllResources()
    {
        return resources;
    }

    public Resource GetResourceByName(string name)
    {
        foreach(Resource res in resources)
        {
            if (name == res.GetResourceName())
                return res;
        }
        return null;
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
        GameManager.Instance.Portrait.gameObject.SetActive(false);
    }
}
