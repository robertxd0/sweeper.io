using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text bombCount;
    [SerializeField] private TMPro.TMP_Text resourceCount;
    [SerializeField] private TMPro.TMP_Text goldResourceCount;

    [SerializeField] private UpgradeShopUI upgradeShopUI;

    public void HandlePlayerPropertyChange(PlayerProperties props)
    {
        this.bombCount.text = props.bombCount.ToString() + "/" + props.GetMaxBombCount().ToString();
        this.resourceCount.text = props.resourceCount.ToString() + "/" + props.GetMaxResourceCount().ToString();
        this.goldResourceCount.text = props.goldResourceCount.ToString();
    }


    public void TriggerUpgradeShop(PlayerProperties props)
    {
        this.upgradeShopUI.InitShop(props);
    }
}
