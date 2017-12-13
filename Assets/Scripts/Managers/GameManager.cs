using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Image Portrait;
    public Text UnitName;
    public GameObject SelectedPanel;
    public GameObject ResourcePanel;
    public GameObject BuildingButton;
    public Text GameTime;

    public Texture CircleEnemy;
    public Texture CircleFriendly;

    public Player[] allPlayers = new Player[2];//Таблица всех игроков
    public static Player MyPlayer;//Это мы

    public int NumIntersection = 0;

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
        for (int i = 0; i < allPlayers.Length; i++)
        {
            allPlayers[i] = new Player();
            allPlayers[i].TeamNumber = i;
            allPlayers[i].GetResourceByName("Gold").CurrentValue = 100;
        }
        allPlayers[allPlayers.Length - 1].isBot = true;//Делаем последнего игрока Ботом(пока так***)
        MyPlayer = allPlayers[0];
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
            unit.GetComponent<Unit>().Owner = allPlayers[0];
            allPlayers[0].AddUnitToAllUnits(unit.GetComponent<Unit>());
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
            unit.GetComponent<Unit>().Owner = allPlayers[1];
            allPlayers[1].AddUnitToAllUnits(unit.GetComponent<Unit>());
        }
        //
        //Debug.Log( units[0].GetComponent<Unit>().Equals(units[1].GetComponent<Unit>() ) );
        //
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
