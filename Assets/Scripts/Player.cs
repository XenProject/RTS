using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class Player : MonoBehaviour {

    public RectTransform SelectableZone;

    //Singleton в будущем придется убрать
    #region Singleton
    public static Player Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    [SerializeField]
    private int teamNumber;
    [SerializeField]
    private List<GameObject> selected;//Выбранные юниты
    [SerializeField]
    private Resource[] resources = new Resource[Enum.GetNames(typeof(ResourceType)).Length];

    private Vector3 startPos;
    private Vector3 endPos;
    private Canvas mainCanvas;
    private float scaleFactor;

    // Use this for initialization
    void Start () {
        //Инициализация всех типов ресурсов
		for(int i = 0; i< resources.Length; i++)
        {
            resources[i] = new Resource((ResourceType)i);
        }
        //Инициализация номера команды
        teamNumber = 0;
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        scaleFactor = mainCanvas.scaleFactor;
    }
	
	// Update is called once per frame
	void Update () {
        //Левая кнопка мыши
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                startPos = hit.point;
            }
            SelectableZone.gameObject.SetActive(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            SelectableZone.gameObject.SetActive(false);
        }
        if (Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            Vector3 squareStart = Camera.main.WorldToScreenPoint(startPos);
            squareStart.z = 0f;
            Vector3 center = (squareStart+endPos)/2f;
            SelectableZone.position = center;

            float sizeX = Mathf.Abs(squareStart.x - endPos.x);
            float sizeY = Mathf.Abs(squareStart.y - endPos.y);

            SelectableZone.sizeDelta = new Vector2(sizeX, sizeY);
            SelectableZone.sizeDelta /= scaleFactor;//Отменяем Scale Factor
        }
        //Правая кнопка мыши
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() )//Вторая проверка: нажали ли мы на объект интерфейса?
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (selected.Count > 0 && selected[0].GetComponent<Interactable>().Owner == teamNumber)
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
                    case "Default":
                        foreach (GameObject unit in selected)
                        {
                            if (unit.GetComponent<Unit>() != null)
                            {
                                unit.GetComponent<Unit>().MoveToPoint(hit.point);
                            }
                            else break;
                        }
                        break;
                    default:
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
