using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text bombCount;
    [SerializeField] private TMPro.TMP_Text resourceCount;
    [SerializeField] private TMPro.TMP_Text goldResourceCount;

    [SerializeField] private Slider levelProgressSlider;

    [SerializeField] private UpgradeShopUI upgradeShopUI;
    
    [SerializeField] private Animator collectibleManagementAnimator;

    private Coroutine autoStopResFeedbackCoroutine;
    private Coroutine autoStopBombFeedbackCoroutine;
    private Coroutine autoStopNotEnoughResFeedbackCoroutine;

    public void HandlePlayerPropertyChange(PlayerProperties props)
    {
        this.bombCount.text = props.bombCount.ToString() + "/" + props.GetMaxBombCount().ToString();
        this.resourceCount.text = props.resourceCount.ToString() + "/" + props.GetMaxResourceCount().ToString();
        this.goldResourceCount.text = props.goldResourceCount.ToString();
    }

    public void SetLevelProgressBarQuota(float percent)
    {
        this.levelProgressSlider.value = percent;
    }


    public void TriggerUpgradeShop(PlayerProperties props)
    {
        this.upgradeShopUI.InitShop(props);
    }

    public void EnableResLimitFeedback()
    {
        this.collectibleManagementAnimator.SetBool(Constants.ANIMATOR_RES_LIMIT_REACHED_FEEDBACK, true);
        if(autoStopResFeedbackCoroutine != null)
        {
            StopCoroutine(autoStopResFeedbackCoroutine);
        }
        autoStopResFeedbackCoroutine = StartCoroutine(AutoStopFeedbackAnim(Constants.ANIMATOR_RES_LIMIT_REACHED_FEEDBACK));
    }

    public void EnableBombCntLimitFeedback()
    {
        this.collectibleManagementAnimator.SetBool(Constants.ANIMATOR_BOMB_CNT_LIMIT_REACHED_FEEDBACK, true);
        if(autoStopBombFeedbackCoroutine != null)
        {
            StopCoroutine(autoStopBombFeedbackCoroutine);
        }
        autoStopBombFeedbackCoroutine = StartCoroutine(AutoStopFeedbackAnim(Constants.ANIMATOR_BOMB_CNT_LIMIT_REACHED_FEEDBACK));
    }

    public void EnableNotEnoughResFeedback()
    {
        this.collectibleManagementAnimator.SetBool(Constants.ANIMATOR_NOT_ENOUGH_RES_FEEDBACK, true);
        if(autoStopNotEnoughResFeedbackCoroutine != null)
        {
            StopCoroutine(autoStopNotEnoughResFeedbackCoroutine);
        }
        autoStopNotEnoughResFeedbackCoroutine = StartCoroutine(AutoStopFeedbackAnim(Constants.ANIMATOR_NOT_ENOUGH_RES_FEEDBACK));
    }

    private IEnumerator AutoStopFeedbackAnim(string animClip)
    {
        var end = Time.time + 0.25f;
        while(Time.time < end)
        {
            yield return null;
        }
        this.collectibleManagementAnimator.SetBool(animClip, false);
    }

    
}
