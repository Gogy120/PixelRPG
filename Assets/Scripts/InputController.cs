using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class InputController : MonoBehaviour
{
    private Player player;
    private Dictionary<string, KeyCode> keyBinds = new Dictionary<string, KeyCode>()
    {
        {"ability1",KeyCode.Q},
        {"ability2",KeyCode.E},
        {"ability3",KeyCode.R},
        {"swapCharacter1",KeyCode.Alpha1},
        {"swapCharacter2",KeyCode.Alpha2},
        {"swapCharacter3",KeyCode.Alpha3},
        {"interact",KeyCode.F},
        {"pause",KeyCode.Escape}
    };


    void Start()
    {
        player = this.GetComponent<Player>();
        keyBinds = GameSaveManager.LoadKeyBinds(keyBinds);
    }

    void Update()
    {
        //Interact
        if (Input.GetKeyDown(keyBinds["interact"]))
        {
            player.Interact();
        }
        //Pause
        if (Input.GetKeyDown(keyBinds["pause"])) 
        {
            player.TogglePause(true);
        }
        //Attack
        if (Input.GetMouseButton(0))
        {
            player.Attack();
        }
        //Abilities
        if (Input.GetKeyDown(keyBinds["ability1"]))
        {
            player.characterManager.CastAbility(1);
        }
        else if (Input.GetKeyDown(keyBinds["ability2"]))
        {
            player.characterManager.CastAbility(2);
        }
        else if (Input.GetKeyDown(keyBinds["ability3"]))
        {
            player.characterManager.CastAbility(3);
        }
        //Swapping
        if (Input.GetKeyDown(keyBinds["swapCharacter1"]))
        {
            player.characterManager.SwapCharacter(0);
        }
        if (Input.GetKeyDown(keyBinds["swapCharacter2"]))
        {
            player.characterManager.SwapCharacter(1);
        }
        if (Input.GetKeyDown(keyBinds["swapCharacter3"]))
        {
            player.characterManager.SwapCharacter(2);
        }
        //Scrolling
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            player.cameraManager.ZoomIn();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            player.cameraManager.ZoomOut();
        }
        //Debug
        if (Input.GetKeyDown(KeyCode.F11)) { StartCoroutine(Toolkit.GoToMap("Brawl1")); }
        if (Input.GetKeyDown(KeyCode.F12)) { PlayerPrefs.DeleteAll(); Debug.Log("Deleted PlayerPrefs"); }
    }
}
