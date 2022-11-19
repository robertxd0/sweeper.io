using UnityEngine;
using System;
using TMPro;

public class UpgradeShopUI : MonoBehaviour
{
    [SerializeField] private TMP_Text bombRadiusLevelInfo; 
    [SerializeField] private TMP_Text bombCountLevelInfo; 
    [SerializeField] private TMP_Text resourceCountInfo; 

    private PlayerProperties playerProperties;

    public void InitShop(PlayerProperties props)
    {
        this.playerProperties = props;
        this.LoadPropertyValuesIntoShop();
        this.gameObject.SetActive(true);
    }


    public void LoadPropertyValuesIntoShop()
    {
        bombRadiusLevelInfo.text = bombRadiusLevelInfo.text.Replace("%n", playerProperties.bombStrengthLevel.ToString());

        bombCountLevelInfo.text = bombCountLevelInfo.text.Replace("%n", playerProperties.bombCountLevel.ToString());
        bombCountLevelInfo.text = bombCountLevelInfo.text.Replace("%c", playerProperties.GetMaxBombCount().ToString());

        resourceCountInfo.text = resourceCountInfo.text.Replace("%n", playerProperties.resourceCountLevel.ToString());
        resourceCountInfo.text = resourceCountInfo.text.Replace("%c", playerProperties.GetMaxResourceCount().ToString());
    }

    public void ClearShopPropertyValues()
    {
        bombRadiusLevelInfo.text = bombRadiusLevelInfo.text.Replace(playerProperties.bombStrengthLevel.ToString(), "%n");

        bombCountLevelInfo.text = bombCountLevelInfo.text.Replace(playerProperties.bombCountLevel.ToString(), "%n");
        bombCountLevelInfo.text = bombCountLevelInfo.text.Replace(playerProperties.GetMaxBombCount().ToString(), "%c");

        resourceCountInfo.text = resourceCountInfo.text.Replace(playerProperties.resourceCountLevel.ToString(), "%n");
        resourceCountInfo.text = resourceCountInfo.text.Replace(playerProperties.GetMaxResourceCount().ToString(), "%c");
    }


    public void CloseShop()
    {
        this.gameObject.SetActive(false);
        this.ClearShopPropertyValues();
    }
}
