using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Components")]
    public Image xpBar;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI xpText;

    private const int baseMaxXp = 220;
    private const int baseXpGain = 2;
    private const float maxXpScaleConstant = 1.13f;
    private const float xpGainScaleConstant = 1.1f;

    private int level = 0;
    private int currentXp = 0;
    void Start()
    {
        level = PlayerPrefs.GetInt("level", 1);
        currentXp = PlayerPrefs.GetInt("current_xp",0);
        UpdateUI();
    }
    public void AddXp()
    {
        int amount = GetXpAmount();
        if (currentXp + amount >= GetMaxXp())
        {
            LevelUp(currentXp + amount - GetMaxXp());
        }
        currentXp += amount;
        PlayerPrefs.SetInt("current_xp", currentXp);
        UpdateUI();
    }

    public int GetXpAmount()
    {
        int xpGain = Mathf.RoundToInt(baseXpGain * Mathf.Pow(xpGainScaleConstant, level - 1));
        return Mathf.RoundToInt(Random.Range(xpGain * 0.85f, xpGain * 1.15f));
    }

    private void LevelUp(int remainingXp)
    {
        level++;
        currentXp = remainingXp;
        PlayerPrefs.SetInt("current_xp",currentXp);
        PlayerPrefs.SetInt("level", level);
        UpdateUI();
    }

    private int GetMaxXp()
    {
        return Mathf.RoundToInt(baseMaxXp * Mathf.Pow(maxXpScaleConstant, level - 1));
    }
    private int GetMaxXpNextLevel()
    {
        return Mathf.RoundToInt(baseMaxXp * Mathf.Pow(maxXpScaleConstant, level));
    }

    private void UpdateUI()
    {
        currentLevelText.text = level.ToString();
        nextLevelText.text = (level + 1).ToString();
        xpText.text = currentXp + "/" + GetMaxXp();
        float fillAmount = (float)((float)currentXp / (float)GetMaxXp());
        xpBar.fillAmount = fillAmount;
    }
}
