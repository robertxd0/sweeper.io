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

    private InGameUI inGameUI;
    public void InitShop(PlayerProperties props, InGameUI inGameUIInstance)
    {
        this.playerProperties = props;
        this.LoadPropertyValuesIntoShop();
        this.gameObject.SetActive(true);

        this.inGameUI = inGameUIInstance;
    }


    public void LoadPropertyValuesIntoShop()
    {
        this.playerProperties = GameManager.instance.GetPlayerProperties();
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

    public void UpgradeBombRadius()
    {
        var playerProps = GameManager.instance.GetPlayerProperties();
        if (this.playerProperties.goldResourceCount >= this.playerProperties.GetBombStrengthUpgradeUprice())
        {
            playerProps.goldResourceCount -= this.playerProperties.GetBombStrengthUpgradeUprice();
            playerProps.bombStrengthLevel += 1;
            GameManager.instance.Upgrade(playerProps);
            this.ClearShopPropertyValues();
            this.LoadPropertyValuesIntoShop();
            this.inGameUI.HandlePlayerPropertyChange(playerProps);

            return;
        }

        //player's got no money lol. add ingameui feedback animation here
    }

    public void UpgradeBombCount()
    {
        var playerProps = GameManager.instance.GetPlayerProperties();
        if (this.playerProperties.goldResourceCount >= this.playerProperties.GetBombCountUpgradeUprice())
        {
            playerProps.goldResourceCount -= this.playerProperties.GetBombCountUpgradeUprice();
            playerProps.bombCountLevel += 1;
            GameManager.instance.Upgrade(playerProps);
            this.ClearShopPropertyValues();
            this.LoadPropertyValuesIntoShop();
            this.inGameUI.HandlePlayerPropertyChange(playerProps);

            return;
        }

        //player's got no money lol. add ingameui feedback animation here
    }

    public void UpgradeResourceCount()
    {
        var playerProps = GameManager.instance.GetPlayerProperties();
        if (this.playerProperties.goldResourceCount >= this.playerProperties.GetResourceCountUpgradeUprice())
        {
            playerProps.goldResourceCount -= this.playerProperties.GetResourceCountUpgradeUprice();
            playerProps.resourceCountLevel += 1;
            GameManager.instance.Upgrade(playerProps);
            this.ClearShopPropertyValues();
            this.LoadPropertyValuesIntoShop();
            this.inGameUI.HandlePlayerPropertyChange(playerProps);

            return;
        }

        //player's got no money lol. add ingameui feedback animation here


    }
}
