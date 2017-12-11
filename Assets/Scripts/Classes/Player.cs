using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[Serializable]
public class Player{
    public bool isBot = false;

    [SerializeField]
    private int teamNumber;
    [SerializeField]
    private List<Interactable> selected;//Выбранные юниты
    [SerializeField]
    private Unit nowSelectedType;
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

    public void AddSelectedObject(Interactable selectedObj, bool clear = true)
    {
        if (clear) ClearSelectedUnits();//Очищаем список выделенных юнитов
        Selected.Add(selectedObj);//Добаляем нового юнита в этот список

        //Рисуем круг под выделенным юнитом в зависимости от команды
        Projector proj = selectedObj.GetComponentInChildren<Projector>();
        if (GameManager.MyPlayer == selectedObj.Owner)
        {
            proj.material.SetTexture("_ShadowTex", GameManager.Instance.CircleFriendly);//_ShadowTex из названия шейдера Projector/Multiply
        }
        else
        {
            proj.material.SetTexture("_ShadowTex", GameManager.Instance.CircleEnemy);
        }
        proj.enabled = true;//Включаем отображение круга

        if (selectedObj is Unit)
        {
            //Создаем иконку на панели выделенных юнитов
            selectedObj.SelectedIcon = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/SelectedIcon"), GameManager.Instance.SelectedPanel.transform);
            selectedObj.SelectedIcon.GetComponent<Image>().sprite = selectedObj.Icon;
            //
            //Сортировка****Надо будет оптимизировать, чтобы не вызывалась при добавлении каждого юнита
            SortSelectedList();
        }
        else
        {
            ActivatePortrait(selected[0]);
        }    
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
        SetupNowSelected(-1);
    }

    public void SortSelectedList()
    {
        if (selected.Count > 1)
        {
            selected = (selected.OrderByDescending(u => (u as Unit).Priority)).ToList<Interactable>();
            for (int i = 0; i < selected.Count; i++)
            {
                (selected[i] as Unit).SelectedIcon.transform.SetSiblingIndex(i);
            }
        }
        SetupNowSelected(0);//Первый
    }

    private void ActivateOneTypeUnits()//Зеленая рамка на юнитов одного типа
    {
        if (selected.Count == 1)
        {
            nowSelectedType.SelectedIcon.transform.GetChild(1).GetComponent<Image>().enabled = true;//Зеленая рамка
        }
        else
        {
            foreach (Unit unit in selected)
            {
                if (unit.CompareWith(nowSelectedType)) unit.SelectedIcon.transform.GetChild(1).GetComponent<Image>().enabled = true;
                else unit.SelectedIcon.transform.GetChild(1).GetComponent<Image>().enabled = false;
            }
        }
    }

    private void SetupNowSelected(int index)
    {
        if (index == -1)//Если юнитов нет
        {
            nowSelectedType = null;
            GameManager.Instance.BuildingButton.SetActive(false);            
            GameManager.Instance.Portrait.gameObject.SetActive(false);
            GameManager.Instance.UnitName.gameObject.SetActive(false);
            return;
        }

        nowSelectedType = selected[index] as Unit;

        if (nowSelectedType.IsBuilder)//Если юнит строитель
            GameManager.Instance.BuildingButton.SetActive(true);
        else
            GameManager.Instance.BuildingButton.SetActive(false);

        ActivatePortrait(nowSelectedType);

        ActivateOneTypeUnits();
    }

    public void ChangeNowSelected()
    {
        if (selected.Count <= 1) return;
        if ((selected[0] as Unit).CompareWith((selected[selected.Count - 1] as Unit))) return;//Если все юниты одного типа
        
        if( !FindNextSelected(selected.IndexOf(nowSelectedType)))//Если не найдем, идем сначала массива
            FindNextSelected(0);
    }

    private bool FindNextSelected(int beginIndex)
    {
        for (int i = beginIndex; i < selected.Count - 1; i++)
        {
            if ((selected[i] as Unit).CompareWith(selected[i + 1] as Unit)) continue;
            else
            {
                SetupNowSelected(i + 1);
                return true;
            }
        }
        if (!(selected[selected.Count-1] as Unit).CompareWith(selected[0] as Unit))
        {
            SetupNowSelected(0);
            return true;
        }
        return false;
    }

    private void ActivatePortrait(Interactable obj)
    {
        GameManager.Instance.Portrait.sprite = obj.Icon;//Изменяем портрет на выбранный тип
        GameManager.Instance.Portrait.gameObject.SetActive(true);//Включаем портрет
        GameManager.Instance.UnitName.text = obj.Name;//Пишем имя
        GameManager.Instance.UnitName.gameObject.SetActive(true);
    }
}
