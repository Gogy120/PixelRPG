using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Toolkit : MonoBehaviour
{
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
        GameObject soundObj = Resources.Load<GameObject>("Prefabs/Sound");
        GameObject sound = Instantiate(soundObj,Vector2.zero,Quaternion.identity);
        AudioSource audioSource = sound.GetComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("Sounds/" + name);
        audioSource.pitch = Random.Range(pitchMin,pitchMax);
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(sound,audioSource.clip.length);
    }

    public static void PlaySound(string name)
    {
        GameObject soundObj = Resources.Load<GameObject>("Prefabs/Sound");
        GameObject sound = Instantiate(soundObj, Vector2.zero, Quaternion.identity);
        AudioSource audioSource = sound.GetComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("Sounds/" + name);
        audioSource.Play();
        Destroy(sound, audioSource.clip.length);
    }

    public static void SpawnParticle(string name, Vector2 position)
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Particles/" + name),position,Quaternion.identity);
    }

    public static void SpawnEffect(string name, Vector2 position)
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Effects/" + name), position, Quaternion.identity);
    }

    public static void GoToScene(string scene)
    {
        PlayerPrefs.SetString("scene_to_load", scene);
        GameObject transitionClose = Instantiate(Resources.Load<GameObject>("Prefabs/Transition_Close"),Vector2.zero,Quaternion.identity);
        transitionClose.transform.parent = GameObject.Find("Canvas").transform;
        RectTransform rectTransform = transitionClose.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(-1920,0);
        rectTransform.offsetMax = new Vector2(1920,0);
    }

}
