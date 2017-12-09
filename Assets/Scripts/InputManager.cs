using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

    public RectTransform SelectableZone;

    //Для SelectableZone
    private Vector3 startPos;
    private Vector3 endPos;
    private Canvas mainCanvas;
    private float scaleFactor;
    //

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
            if (GameManager.MyPlayer.Selected.Count > 0 && GameManager.MyPlayer.Selected[0].GetComponent<Interactable>().Owner == GameManager.MyPlayer)
            {
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                switch (LayerMask.LayerToName(hit.transform.gameObject.layer))
                {
                    case "Clickable":
                        foreach (GameObject unit in GameManager.MyPlayer.Selected)
                        {
                            if (unit.GetComponent<Unit>() != null)
                            {
                                unit.GetComponent<Unit>().SetFocus(hit.transform);
                            }
                            else break;
                        }
                        break;
                    case "Default":
                        foreach (GameObject unit in GameManager.MyPlayer.Selected)
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
}
