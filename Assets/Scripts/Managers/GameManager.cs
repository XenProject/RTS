using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Texture2D CustomCursor;

    public Image Portrait;
    public GameObject SelectedPanel;
    public GameObject ResourcePanel;
    public Text GameTime;

    public Texture CircleEnemy;
    public Texture CircleFriendly;

    public Player[] allPlayers = new Player[2];//Таблица всех игроков
    public static Player MyPlayer;//Это мы

    private CursorMode cursorMode = CursorMode.ForceSoftware;

    #region Singleton
    public static GameManager Instance;
    public void Awake()
    {
        Instance = this;
    }
    #endregion
	// Use this for initialization
	void Start () {
        //Инициализация курсора
        Cursor.SetCursor(CustomCursor, Vector2.zero, cursorMode );
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
            go.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/Resources/" + (ResourceType)i);
            go.GetComponentInChildren<Text>().text = MyPlayer.GetAllResources()[i].CurrentValue.ToString();
        }
        //
        //Присвоение юнитов с тегом "Player0" нулевому игроку
        GameObject[] units = GameObject.FindGameObjectsWithTag("Player0");
        foreach(GameObject unit in units)
        {
            unit.GetComponent<Unit>().Owner = MyPlayer;
            MyPlayer.AddUnitToAllUnits(unit.GetComponent<Unit>());
        }
        //
    }
	
	// Update is called once per frame
	void Update () { 
    }
}
