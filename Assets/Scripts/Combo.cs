using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI comboText;
    public Animator comboTextAnimator;
    public TextMeshProUGUI comboEndText;
    public Animator comboEndTextAnimator;

    private int comboCount = 0;
    private float lastAttackTime;
    private float comboResetTime = 1.25f;
    private bool isComboActive = false;

    void Update()
    {
        if (Time.time - lastAttackTime > comboResetTime && isComboActive)
        {
            EndCombo();
        }
    }

    public void AddCombo()
    {
        isComboActive = true;
        comboCount++;
        comboTextAnimator.SetTrigger("combo" + Random.Range(1, 3));
        comboText.text = "x" + comboCount;
        lastAttackTime = Time.time;
    }

    private void EndCombo()
    {
        isComboActive = false;
        comboText.text = "";
        comboEndTextAnimator.SetTrigger("end");
        string comboMessage = "";
        if (comboCount >= 300) { comboMessage = "Massacre!"; }
        else if (comboCount < 300 && comboCount >= 200) { comboMessage = "Madness!"; }
        else if (comboCount < 200 && comboCount >= 100) { comboMessage = "Hog wild!"; }
        else if (comboCount < 100 && comboCount >= 50) { comboMessage = "Nice!"; }
        comboEndText.text = comboMessage;
        comboCount = 0;
    }
}
