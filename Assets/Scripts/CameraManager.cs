using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float dampTime = 0.15f;
    public Transform target;
    public float minZoom = 7f;
    public float maxZoom = 4.5f;

    private Camera cam;
    private float zoomAmount = 0.5f;
    private Vector3 velocity = Vector3.zero;
    private bool isShaking = false;
    private WaitForSeconds shakeDelay = new WaitForSeconds(0.01f);
    void Start()
    {
        cam = this.GetComponent<Camera>();
        cam.orthographicSize = maxZoom;
    }
    void FixedUpdate()
    {
        Vector3 point = cam.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta + Vector3.up * 0.5f;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }

    public void ZoomIn()
    {
        if (cam.orthographicSize > maxZoom && !isShaking)
        {
            cam.orthographicSize -= zoomAmount;
        }
    }
    public void ZoomOut()
    {
        if (cam.orthographicSize < minZoom && !isShaking)
        {
            cam.orthographicSize += zoomAmount;
        }
    }

    public float GetOcclusionDistance()
    {
        return cam.orthographicSize * 3f + 1f;
    }

    public IEnumerator Shake(float magnitude)
    {
        if (!isShaking)
        {
            isShaking = true;
            float defaultZoom = cam.orthographicSize;
            int mult = 1;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    cam.orthographicSize += 0.05f * mult * magnitude;
                    yield return shakeDelay;
                }
                mult *= -1;
            }
            cam.orthographicSize = defaultZoom;
            isShaking = false;
        }

    }
}

