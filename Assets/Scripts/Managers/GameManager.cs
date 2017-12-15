using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text SizeText;
    public Text VisualText;

    public Image Portrait;
    public Text UnitName;
    public GameObject InfoPanel;
    public Text HealthText;
    public Text DamageText;
    public Text ArmorText;

    public GameObject SelectedPanel;
    public GameObject ResourcePanel;
    public GameObject BuildingButton;
    public Text GameTime;

    public Texture CircleEnemy;
    public Texture CircleFriendly;

    public Color[] TeamColors = new Color[2];
    public Player[] AllPlayers = new Player[2];//Таблица всех игроков
    public static Player MyPlayer;//Это мы

    public int NumIntersection = 0;

    public Unit unitForInfo;

    #region Singleton
    public static GameManager Instance;
    public void Awake()
    {
        Instance = this;
    }
    #endregion
	// Use this for initialization
	void Start () {
        //Инициализация всех игроков
        for (int i = 0; i < AllPlayers.Length; i++)
        {
            AllPlayers[i] = new Player();
            AllPlayers[i].TeamNumber = i;
            AllPlayers[i].GetResourceByName("Gold").CurrentValue = 100;
        }
        AllPlayers[AllPlayers.Length - 1].isBot = true;//Делаем последнего игрока Ботом(пока так***)
        MyPlayer = AllPlayers[0];
        //Перенос на панель ресурсов
        for(int i = 0; i < MyPlayer.GetAllResources().Length; i++)
        {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Resource"), GameManager.Instance.ResourcePanel.transform);
            go.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/Resources/" + (ResourceType)i);
            go.GetComponentInChildren<Text>().text = MyPlayer.GetAllResources()[i].CurrentValue.ToString();
        }
        //
        //Присвоение юнитов с тегом "Player0" нулевому игроку
        GameObject[] units = GameObject.FindGameObjectsWithTag("Player0");
        foreach(GameObject unit in units)
        {
            unit.GetComponent<Unit>().Owner = AllPlayers[0];
            AllPlayers[0].AddUnitToAllUnits(unit.GetComponent<Unit>());
            unit.transform.Find("MinimapIcon").GetComponent<MeshRenderer>().material.color = TeamColors[0];
        }
        //*****************Debugging******************
        units[units.Length - 1].GetComponent<Unit>().Name = "Other";
        units[units.Length - 1].GetComponent<Unit>().Priority = 1;
        units[units.Length - 1].GetComponent<Unit>().IsBuilder = true;
        //
        //Присвоение юнитов с тегом "Player1" первому игроку
        units = GameObject.FindGameObjectsWithTag("Player1");
        foreach (GameObject unit in units)
        {
            unit.GetComponent<Unit>().Owner = AllPlayers[1];
            AllPlayers[1].AddUnitToAllUnits(unit.GetComponent<Unit>());
            unit.transform.Find("MinimapIcon").GetComponent<MeshRenderer>().material.color = TeamColors[1];
        }
        //
        //Debug.Log( units[0].GetComponent<Unit>().Equals(units[1].GetComponent<Unit>() ) );
        //
    }
	
	// Update is called once per frame
	void Update () {
        if (SizeText.gameObject.activeSelf)
        {
            VisualText.text = String.Format("{0}\nHealth: {1}/{2}", unitForInfo.Name, unitForInfo.GetHealth(), unitForInfo.GetMaxHealth());
            SizeText.text = VisualText.text;
            SizeText.transform.position = Input.mousePosition + (new Vector3(32, 32, 0) * GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor);
        }
        if(MyPlayer.Selected.Count>0 && MyPlayer.Selected[0] as Building)
        {
            HealthText.text = MyPlayer.Selected[0].GetHealth().ToString() + "/" + MyPlayer.Selected[0].GetMaxHealth().ToString();
        }
    }

    public void ShowUnitInfo(Unit unit)
    {
        unitForInfo = unit;
        SizeText.gameObject.SetActive(true);
    }
}
