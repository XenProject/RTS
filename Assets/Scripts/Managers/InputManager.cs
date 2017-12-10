using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

    public RectTransform SelectableZone;
    public GameObject PausePanel;

    public List<Position> pos;

    //Для SelectableZone
    private Vector3 startPos;
    private Vector3 endPos;
    private Canvas mainCanvas;
    private float scaleFactor;
    //
    //Для выделения юнитов в SelectableZone
    private Vector2 mousePos1;
    private Vector2 mousePos2;
    //
    private bool isPaused = false;

    // Use this for initialization
    void Start () {
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        scaleFactor = mainCanvas.scaleFactor;
    }

    // Update is called once per frame
    void Update()
    {
        //Левая кнопка мыши
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                startPos = hit.point;
            }
            SelectableZone.gameObject.SetActive(true);
            mousePos1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            SelectableZone.gameObject.SetActive(false);
            mousePos2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if(mousePos1 != mousePos2)
            {
                SelectObjects();
            }
        }
        if (Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            Vector3 squareStart = Camera.main.WorldToScreenPoint(startPos);
            squareStart.z = 0f;
            Vector3 center = (squareStart + endPos) / 2f;
            SelectableZone.position = center;

            float sizeX = Mathf.Abs(squareStart.x - endPos.x);
            float sizeY = Mathf.Abs(squareStart.y - endPos.y);

            SelectableZone.sizeDelta = new Vector2(sizeX, sizeY);
            SelectableZone.sizeDelta /= scaleFactor;//Отменяем Scale Factor
        }
        //Правая кнопка мыши
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())//Вторая проверка: нажали ли мы на объект интерфейса?
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (GameManager.MyPlayer.Selected.Count > 0 && GameManager.MyPlayer.Selected[0].Owner == GameManager.MyPlayer)
            {
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                switch (LayerMask.LayerToName(hit.transform.gameObject.layer))
                {
                    case "Clickable":
                        foreach (Interactable unit in GameManager.MyPlayer.Selected)
                        {
                            if (unit.GetComponent<Unit>() != null)
                            {
                                unit.GetComponent<Unit>().SetFocus(hit.transform);
                            }
                            else break;
                        }
                        break;
                    case "Default":
                        int i = 0;
                        foreach (Interactable unit in GameManager.MyPlayer.Selected)
                        {
                            if (unit.GetComponent<Unit>() != null)
                            {
                                Vector3 targetPos = new Vector3( hit.point.x + pos[i].x, hit.point.y, hit.point.z + pos[i].z);
                                unit.GetComponent<Unit>().MoveToPoint(targetPos);
                                i++;
                            }
                            else break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        //Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameManager.MyPlayer.Selected.Count > 0)
            {
                GameManager.MyPlayer.ClearSelectedUnits();
            }
            else
            {
                MenuButton();
            }
        }
    }

    void SelectObjects()
    {
        bool hasOne = false;
        Rect selectRect = new Rect(mousePos1.x, mousePos1.y, mousePos2.x - mousePos1.x, mousePos2.y - mousePos1.y);
        foreach (Unit unit in GameManager.MyPlayer.AllUnits)
        {
            if(unit != null)
            {
                if(selectRect.Contains(Camera.main.WorldToViewportPoint( unit.transform.position), true))
                {
                    if (!hasOne)
                    {
                        GameManager.MyPlayer.ClearSelectedUnits();
                        hasOne = true;
                    }
                    if(GameManager.MyPlayer.Selected.Count <= 12)
                    {
                        GameManager.MyPlayer.AddSelectedUnit(unit, false);
                    }
                }
            }
        }
    }

    public void MenuButton()
    {
        if (!isPaused)
        {
            Time.timeScale = 0.0f;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            isPaused = false;
        }
        PausePanel.SetActive(isPaused);
    }
}

[Serializable]
public class Position
{
    public float x;
    public float z;
}
