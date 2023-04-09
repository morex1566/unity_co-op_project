using System;
using UnityEngine;

public class MapEditorCamera : MonoBehaviour
{
    public float scrollSpeed = 0.05f;
    public Vector3 cameraStartPosition;
    public float cameraHeightMin = 9f;
    public float cameraHeightMax = 21f;
    public float cameraMoveSpeed = 2.5f;

    private Vector3 lastMousePosition;

    private void Awake()
    {
        cameraStartPosition = new Vector3(0, 15, 0);
        transform.position = cameraStartPosition;
    }

    void Update()
    {
        // ACTION : 스크롤 다운으로 카메라 확대
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && transform.position.y > cameraHeightMin)
        {
            // 카메라 y값 감소
            transform.position += Vector3.down * scrollSpeed;
        }

        // ACTION :스크롤 업으로 카메라 축소
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && transform.position.y < cameraHeightMax)
        {
            // 카메라 y값 증가
            transform.position += Vector3.up * scrollSpeed;
        }
        
        // ACTION : 휠 클릭하고 있으면 마우스를 움직여 카메라 X-Z 이동
        if (Input.GetMouseButton(2))
        {
            Vector3 currentMousePosition = Input.mousePosition;

            Vector3 deltaMousePosition = currentMousePosition - lastMousePosition;
            
            Vector3 pan = new Vector3(-deltaMousePosition.x, 0, -deltaMousePosition.y) * cameraMoveSpeed * Time.deltaTime;

            transform.Translate(pan, Space.World);

            lastMousePosition = currentMousePosition;
        }
        else
        {
            lastMousePosition = Input.mousePosition;
        }
    }
}
