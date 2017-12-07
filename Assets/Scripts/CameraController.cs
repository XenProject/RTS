using UnityEngine;

public class CameraController : MonoBehaviour {

    public float camSpeed = 20f;
    public float scrollSpeed = 20f;

    private float borderThickness = 10f;
    private float minY = 20f;
    private float maxY = 120f;

    // Update is called once per frame
    void Update () {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - borderThickness)
        {
            pos.z += camSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= borderThickness)
        {
            pos.z -= camSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= borderThickness)
        {
            pos.x -= camSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - borderThickness)
        {
            pos.x += camSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -20, 120);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -20, 120);
        transform.position = pos;
	}
}
