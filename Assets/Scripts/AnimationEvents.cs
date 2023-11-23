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

    public void GotoScene()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("scene_to_load","Menu"));
    }
}
