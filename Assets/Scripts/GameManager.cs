using UnityEngine;

public class GameManager : MonoBehaviour {

    public Player[] allPlayers = new Player[2];//Таблица всех игроков
    public static Player MyPlayer;//Это мы

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
        for(int i = 0; i < allPlayers.Length; i++)
        {
            allPlayers[i] = new Player();
            allPlayers[i].TeamNumber = i;
        }
        allPlayers[allPlayers.Length - 1].isBot = true;//Делаем последнего игрока Ботом(пока так***)
        MyPlayer = allPlayers[0];
        GameObject.Find("Unit1").GetComponent<Unit>().Owner = MyPlayer;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
