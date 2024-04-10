using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Toolkit : MonoBehaviour
{
    public static string physicalDamageColor = "#CD683D";
    public static string magicDamageColor = "#4D9BE6";
    public static string hpColor = "#1EBC73";
    public static string attackSpeedColor = "#F9C22B";
    public static void SpawnText(string text, Vector2 position, Color32 color)
    {
        GameObject damageNumberObj = Resources.Load<GameObject>("Prefabs/DamageNumber");
        GameObject damageNumber = Instantiate(damageNumberObj,position,Quaternion.identity);
        TextMeshPro damageNumberText = damageNumber.GetComponentInChildren<TextMeshPro>();
        damageNumberText.text = text;
        damageNumberText.color = color;
        Move move = damageNumber.GetComponentInChildren<Move>();
        move.dir = new Vector2(Random.Range(-1f,1f), 1.25f);
        Destroy(damageNumber,0.75f);
    }

    public static void PlaySound(string name, float volume, float pitchMin, float pitchMax)
    {
        if (name != null)
        {
            GameObject soundObj = Resources.Load<GameObject>("Prefabs/Sound");
            GameObject sound = Instantiate(soundObj, Vector2.zero, Quaternion.identity);
            AudioSource audioSource = sound.GetComponent<AudioSource>();
            audioSource.clip = Resources.Load<AudioClip>("Sounds/" + name);
            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(sound, audioSource.clip.length);
        }
    }

    public static void PlaySound(string name)
    {
        if (name != null)
        {
            GameObject soundObj = Resources.Load<GameObject>("Prefabs/Sound");
            GameObject sound = Instantiate(soundObj, Vector2.zero, Quaternion.identity);
            AudioSource audioSource = sound.GetComponent<AudioSource>();
            audioSource.clip = Resources.Load<AudioClip>("Sounds/" + name);
            audioSource.Play();
            Destroy(sound, audioSource.clip.length);
        }
    }

    public static void SpawnParticle(string name, Vector2 position)
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Particles/" + name),position,Quaternion.identity);
    }

    public static void SpawnEffect(string name, Vector2 position)
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Effects/" + name), position, Quaternion.identity);
    }

    public static IEnumerator GoToMap(string scene) //Loads MainScene and a map
    {
        PlayerPrefs.SetString("current_map", scene);
        GameObject transitionClose = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Transition_Close"),Vector2.zero,Quaternion.identity);
        transitionClose.transform.parent = GameObject.Find("Canvas").transform;
        RectTransform rectTransform = transitionClose.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(-1920,0);
        rectTransform.offsetMax = new Vector2(1920,0);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainScene");
    }


    public static IEnumerator GoToScene(string scene) //Loads just a scene
    {

        Debug.Log("Going to scene: " + scene);
        GameObject transitionClose = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Transition_Close"), Vector2.zero, Quaternion.identity);
        transitionClose.transform.parent = GameObject.Find("Canvas").transform;
        RectTransform rectTransform = transitionClose.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(-1920, 0);
        rectTransform.offsetMax = new Vector2(1920, 0);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);

    }

    public static float GetEnemyScalingMult()
    {
        return PlayerPrefs.GetInt("tier", 1) * PlayerPrefs.GetInt("selectedDifficulty", 1);
    }

    public static string IntToRoman(int number)
    {
        string roman = "";
        switch (number)
        {
            case 1: roman = "I"; break;
            case 2: roman = "II"; break;
            case 3: roman = "III"; break;
        }
        return roman;
    }
}
