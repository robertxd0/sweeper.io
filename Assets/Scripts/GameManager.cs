using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private InGameUI inGameUI;

    [SerializeField] private InputManager playerInputManager;

    [Range(0f, 1f)]
    [SerializeField] private float resourceIncrementRate = 0.1f;
    
    [Range(0f, 1f)]
    [SerializeField] private float goldResourceIncrementRate = 0.1f;

    [SerializeField] private AnimationCurve bombStrengthSkillProgress;
    
    private FeedbackAnimationManager feedbackAnimationManager;

    private PlayerProperties currentProps;


    private void Awake() {
        instance = this;
    }

    private void OnDisable() {
        instance = null;
    }

    void Start()
    {
        //load from playerprefs
        currentProps = new PlayerProperties();
        currentProps.gameLevel = 1;
        currentProps.bombCount = 5;
        currentProps.resourceCount = 0;
        currentProps.goldResourceCount = 250;
        currentProps.bombStrengthLevel = 1;
        currentProps.bombCountLevel = 1;
        currentProps.resourceCountLevel = 1;

        inGameUI.HandlePlayerPropertyChange(currentProps);

        feedbackAnimationManager = FeedbackAnimationManager.instance;

        Bomb.bombStrength = currentProps.GetBombStrength(bombStrengthSkillProgress);
    }
    //
    // Summary:
    //     Returns true if player has successfully bought. If player has not enough resources, it returns false.
    public bool IncrementBombs(int bombResourceCount)
    {
        if(currentProps.bombCount >= currentProps.GetMaxBombCount())
        {
            inGameUI.EnableBombCntLimitFeedback();
            return false;
        }

        if(bombResourceCount > currentProps.resourceCount)
        {
            inGameUI.EnableNotEnoughResFeedback();
            return false;
        }

        currentProps.resourceCount -= bombResourceCount;
        currentProps.bombCount++;
        
        feedbackAnimationManager.ExecFeedbackFX(FXFeedbackTypes.BOMB_PURCHASE_SUCCESSFUL, playerInputManager.transform);
        inGameUI.HandlePlayerPropertyChange(currentProps);
        return true;
    }

    public void IncrementResources()
    {
        if(currentProps.resourceCount >= currentProps.GetMaxResourceCount())
        {
            return;
        }

        currentProps.resourceCount += resourceIncrementRate;
        inGameUI.HandlePlayerPropertyChange(currentProps);
    }


    public void IncrementGoldResources()
    {
        currentProps.goldResourceCount += goldResourceIncrementRate;
        inGameUI.HandlePlayerPropertyChange(currentProps);
    }

    public void ConsumeBomb()
    {
        currentProps.bombCount--;
        inGameUI.HandlePlayerPropertyChange(currentProps);
    }

    public bool HasBombs()
    {
        return currentProps.bombCount > 0;
    }

    public bool ResourceLimitReached()
    {
        if(currentProps.resourceCount >= currentProps.GetMaxResourceCount())
        {
            inGameUI.EnableResLimitFeedback();

            currentProps.resourceCount = currentProps.GetMaxResourceCount();
            inGameUI.HandlePlayerPropertyChange(currentProps);

            return true;
        }
        return false;
    }

    public void OpenShop()
    {
        this.playerInputManager.DisableInput();
        this.inGameUI.TriggerUpgradeShop(this.currentProps);
    }

    public void OnShopClosed()
    {
        this.playerInputManager.EnableInput();
    }

    public void UpdateLevelProgress(float percentQuota)
    {
        this.inGameUI.SetLevelProgressBarQuota(percentQuota);
    }

    public void Upgrade(PlayerProperties newProps)
    {
        this.currentProps = newProps;
    }

    public PlayerProperties GetPlayerProperties()
    {
        return this.currentProps;
    }
}


public struct PlayerProperties
{
    public int gameLevel;
    public int bombCount;
    public float resourceCount;
    public float goldResourceCount;

    public int bombStrengthLevel;

    public int bombCountLevel;

    public int resourceCountLevel;

    //--------Calibration needed here-----------

    public int GetMaxResourceCount()
    {
        return (int)((float)resourceCountLevel * 1.25f * 100f);
    }

    public int GetMaxBombCount()
    {
        return (int)((float)bombCountLevel * 0.05f * 100f);
    }

    public float GetBombStrength(AnimationCurve predefinedStrengthProgress)
    {
          return predefinedStrengthProgress.Evaluate((float)bombStrengthLevel-1 / predefinedStrengthProgress[predefinedStrengthProgress.length - 1].time);
    }


    public int GetBombStrengthUpgradeUprice()
    {
        return bombStrengthLevel * 250;
    }

    public int GetBombCountUpgradeUprice()
    {
        return bombCountLevel * 25;
    }

    public int GetResourceCountUpgradeUprice()
    {
        return resourceCountLevel * 25;
    }
    //---------------------------------------------
}