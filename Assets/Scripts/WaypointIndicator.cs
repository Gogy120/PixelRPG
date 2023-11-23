using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointIndicator : MonoBehaviour
{
    public Transform target;
    public float distanceFromCenter;

    void Update()
    {
        if (target != null)
        {
            UpdateWaypointPosition();
        }
    }

    void UpdateWaypointPosition()
    {
        distanceFromCenter = Screen.height * 0.45f;
        Debug.LogWarning(distanceFromCenter);
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(new Vector3(target.position.x,target.position.y + 0.5f,target.position.z));
        Vector3 indicatorDir = targetScreenPos - new Vector3(Screen.width / 2, Screen.height / 2, 0);
        float angle = Mathf.Atan2(indicatorDir.y, indicatorDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * distanceFromCenter + Screen.width / 2;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * distanceFromCenter + Screen.height / 2;
        transform.position = new Vector3(x, y, 0);
    }
}
