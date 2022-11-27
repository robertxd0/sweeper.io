using UnityEngine;
using System;
using TMPro;

public class UpgradeShopUI : MonoBehaviour
{
    [SerializeField] private TMP_Text bombRadiusLevelInfo; 
    [SerializeField] private TMP_Text bombCountLevelInfo; 
    [SerializeField] private TMP_Text resourceCountInfo; 
    
    [SerializeField] private TMP_Text bombRadiusUpgradePrice;
    [SerializeField] private TMP_Text bombCountUpgradePrice;
    [SerializeField] private TMP_Text resourceCountUpgradePrice;
 
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

        bombRadiusUpgradePrice.text = bombRadiusUpgradePrice.text.Replace("%n", playerProperties.GetBombStrengthUpgradeUprice().ToString());
        bombCountUpgradePrice.text = bombCountUpgradePrice.text.Replace("%n", playerProperties.GetBombCountUpgradeUprice().ToString());
        resourceCountUpgradePrice.text = resourceCountUpgradePrice.text.Replace("%n", playerProperties.GetResourceCountUpgradeUprice().ToString());

    }

    public void ClearShopPropertyValues()
    {
        bombRadiusLevelInfo.text = bombRadiusLevelInfo.text.Replace(playerProperties.bombStrengthLevel.ToString(), "%n");

        bombCountLevelInfo.text = bombCountLevelInfo.text.Replace(playerProperties.bombCountLevel.ToString(), "%n");
        bombCountLevelInfo.text = bombCountLevelInfo.text.Replace(playerProperties.GetMaxBombCount().ToString(), "%c");

        resourceCountInfo.text = resourceCountInfo.text.Replace(playerProperties.resourceCountLevel.ToString(), "%n");
        resourceCountInfo.text = resourceCountInfo.text.Replace(playerProperties.GetMaxResourceCount().ToString(), "%c");

        bombRadiusUpgradePrice.text = bombRadiusUpgradePrice.text.Replace(playerProperties.GetBombStrengthUpgradeUprice().ToString(), "%n");
        bombCountUpgradePrice.text = bombCountUpgradePrice.text.Replace(playerProperties.GetBombCountUpgradeUprice().ToString(), "%n");
        resourceCountUpgradePrice.text = resourceCountUpgradePrice.text.Replace(playerProperties.GetResourceCountUpgradeUprice().ToString(), "%n");
    }


    public void CloseShop()
    {
        this.gameObject.SetActive(false);
        this.ClearShopPropertyValues();
        GameManager.instance.OnShopClosed();
    }
}
