using UnityEngine;

public class MiniMapBorder : MonoBehaviour
{
    public Vector2 MiniMapLastPos = Vector2.zero;
    public float MiniMapAbsoluteWidth = 248.0f; //in pixels
    public float MiniMapAbsoluteHeight = 124.0f;
    public int MiniMapOffsetX = 20;
    public int MiniMapOffsetY = 64;

    public Vector3 ScreenCorner1;
    public Vector3 ScreenCorner2;

    public Vector3 MiniMapCorner1;
    public Vector3 MiniMapCorner2;

    public float MiniMapScreenOffsetX1;
    public float MiniMapScreenOffsetY1;

    public float MiniMapScreenOffsetX2;
    public float MiniMapScreenOffsetY2;

    Camera MiniMapCamera;
    void Start()
    {
        MiniMapCamera = gameObject.GetComponent<Camera>();
    }

    public Texture2D BorderTexture; //just a small transparent image with a white border

    LayerMask MapBackgroundMask;

    void OnGUI()
    {
        //MiniMap Stuff
        Vector2 MiniMapCurrentPos = new Vector2(Screen.width, Screen.height);
        if ((MiniMapLastPos - MiniMapCurrentPos).magnitude != 2)
        {
            MiniMapLastPos = MiniMapCurrentPos;
            MiniMapCamera.pixelRect = new Rect(MiniMapOffsetX, Screen.height - MiniMapAbsoluteHeight - MiniMapOffsetY, MiniMapAbsoluteWidth, MiniMapAbsoluteHeight);
        }

        Ray ray1 = Camera.main.ViewportPointToRay(new Vector3(0, 1, 0));
        RaycastHit hit1;
        if (Physics.Raycast(ray1, out hit1, Mathf.Infinity, MapBackgroundMask))
        {
            ScreenCorner1 = new Vector3(hit1.point.x, hit1.point.y, hit1.point.z);
            Debug.Log(ScreenCorner1);
        }
        Ray ray2 = Camera.main.ViewportPointToRay(new Vector3(1, 0, 0));
        RaycastHit hit2;
        if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, MapBackgroundMask))
        {
            ScreenCorner2 = new Vector3(hit2.point.x, hit2.point.y, hit2.point.z);
            Debug.Log(ScreenCorner2);
        }

        MiniMapCorner1 = MiniMapCamera.WorldToViewportPoint(ScreenCorner1);
        MiniMapCorner2 = MiniMapCamera.WorldToViewportPoint(ScreenCorner2);

        MiniMapScreenOffsetX1 = MiniMapAbsoluteWidth * MiniMapCorner1.x;
        MiniMapScreenOffsetY1 = MiniMapAbsoluteHeight * MiniMapCorner1.y;

        MiniMapScreenOffsetX2 = MiniMapAbsoluteWidth * MiniMapCorner2.x;
        MiniMapScreenOffsetY2 = MiniMapAbsoluteHeight * MiniMapCorner2.y;


        Rect MiniMapWindow = new Rect(MiniMapScreenOffsetX1 + MiniMapScreenOffsetX1, MiniMapScreenOffsetY1 - MiniMapScreenOffsetY1, MiniMapScreenOffsetX2 - MiniMapScreenOffsetX1, MiniMapScreenOffsetY1 - MiniMapScreenOffsetY2);
        GUI.Box(MiniMapWindow, BorderTexture);
    }
}