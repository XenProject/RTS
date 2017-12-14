using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    public bool isLocked = true;
    public float camSpeed = 20f;
    public float scrollSpeed = 20f;
    public Camera MiniMapCamera;
    public RectTransform MiniMapImage;
    public RectTransform MapScreen;

    private float borderThickness = 10f;
    private float minY = 5f;
    private float maxY = 40f;
    private float camStop = 15f;

    // Update is called once per frame

    void Update () {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || (!isLocked && Input.mousePosition.y >= Screen.height - borderThickness) )
        {
            pos.z += camSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || (!isLocked && Input.mousePosition.y <= borderThickness) )
        {
            pos.z -= camSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || (!isLocked && Input.mousePosition.x <= borderThickness) )
        {
            pos.x -= camSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || (!isLocked && Input.mousePosition.x >= Screen.width - borderThickness) )
        {
            pos.x += camSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
        

        pos.x = Mathf.Clamp(pos.x, -20+camStop, 120-camStop);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -20, 100-camStop);
        transform.position = pos;
        CreateBorder();
    }

    private void CreateBorder()
    {
        Ray[] ray = new Ray[3];
        RaycastHit[] hit = new RaycastHit[3];
        Vector3[] screenCorners = new Vector3[3];
        Vector2[] points = new Vector2[3] { new Vector2(0,0),new Vector2(1,1), new Vector2(0,1)};
        Vector3 MiniMapCorner1, MiniMapCorner2, MiniMapCorner3;
        float MiniMapScreenOffsetX2, MiniMapScreenOffsetY1, MiniMapScreenOffsetY2, MiniMapScreenOffsetX3;
        for (int i = 0; i < 3 ; i++)
        {
            ray[i] = Camera.main.ViewportPointToRay(points[i]);
            Physics.Raycast(ray[i], out hit[i], Mathf.Infinity, 1 << LayerMask.NameToLayer("Level"));
            screenCorners[i] = hit[i].point;
            //Instantiate(go, vec[i], Quaternion.identity);
        }
        MiniMapCorner1 = MiniMapCamera.WorldToViewportPoint(screenCorners[0]);
        MiniMapCorner2 = MiniMapCamera.WorldToViewportPoint(screenCorners[1]);
        MiniMapCorner3 = MiniMapCamera.WorldToViewportPoint(screenCorners[2]);

        //MiniMapScreenOffsetX1 = MiniMapImage.rect.width * MiniMapCorner1.x;
        MiniMapScreenOffsetY1 = MiniMapImage.rect.height * MiniMapCorner1.y;

        MiniMapScreenOffsetX2 = MiniMapImage.rect.width * MiniMapCorner2.x;
        MiniMapScreenOffsetY2 = MiniMapImage.rect.height * MiniMapCorner2.y;

        MiniMapScreenOffsetX3 = MiniMapImage.rect.width * MiniMapCorner3.x;
        //MiniMapScreenOffsetY3 = MiniMapImage.rect.height * MiniMapCorner3.y;

        //Rect MiniMapWindow = Rect.MinMaxRect(MiniMapScreenOffsetX3, MiniMapScreenOffsetY1, MiniMapScreenOffsetX2, MiniMapScreenOffsetY2);
        MapScreen.anchoredPosition = new Vector2(MiniMapScreenOffsetX3, MiniMapScreenOffsetY1);
        MapScreen.sizeDelta = new Vector2(MiniMapScreenOffsetX2 - MiniMapScreenOffsetX3, MiniMapScreenOffsetY2 - MiniMapScreenOffsetY1);
        //MapScreen.sizeDelta = new Vector2(MiniMapWindow.width, MiniMapWindow.height);
    }
}
