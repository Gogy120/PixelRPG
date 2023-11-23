using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occlusion : MonoBehaviour
{
    public GameObject child;

    private Player player;
    private WaitForSeconds delay = new WaitForSeconds(0.1f);
    private float distance = 12;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine(OcclusionLoop());
    }

    private IEnumerator OcclusionLoop()
    {
        while (true)
        {
            yield return delay;
            if (Vector2.Distance(this.transform.position, player.transform.position) >= player.cameraManager.GetOcclusionDistance())
            {
                child.SetActive(false);
            }
            else { child.SetActive(true); }
        }
    }
}
