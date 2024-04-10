using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEvents : MonoBehaviour
{
    public void DestroyItself()
    {
        Destroy(this.gameObject);
    }

    public void DisableItself()
    {
        this.gameObject.SetActive(false);
    }

    public void GotoScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
