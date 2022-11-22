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
    
    private FeedbackAnimationManager feedbackAnimationManager;

    private PlayerProperties currentProps;

    private float decimalResourceCount = 0f;
    private float decimalGoldResourceCount = 0f;

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
        currentProps.bombCount = 0;
        currentProps.resourceCount = 0;
        currentProps.goldResourceCount = 0;
        currentProps.bombStrengthLevel = 1;
        currentProps.bombCountLevel = 2;
        currentProps.resourceCountLevel = 1;

        inGameUI.HandlePlayerPropertyChange(currentProps);

        feedbackAnimationManager = FeedbackAnimationManager.instance;

        Bomb.bombStrength = currentProps.bombStrengthLevel;
    }
    //
    // Summary:
    //     Returns true if player has successfully bought. If player has not enough resources, it returns false.
    public bool IncrementBombs(int bombResourceCount)
    {
        if(bombResourceCount > currentProps.resourceCount || currentProps.bombCount >= currentProps.GetMaxBombCount())
        {
            return false;
        }

        currentProps.resourceCount -= bombResourceCount;
        currentProps.bombCount++;

        decimalResourceCount = (float)currentProps.resourceCount;
        
        feedbackAnimationManager.ExecFeedbackFX(FXFeedbackTypes.BOMB_PURCHASE_SUCCESSFUL, playerInputManager.transform);
        inGameUI.HandlePlayerPropertyChange(currentProps);
        return true;
    }

    public void IncrementResources()
    {
        if(currentProps.resourceCount >= currentProps.GetMaxResourceCount())
            return;
        
        decimalResourceCount += resourceIncrementRate;

        currentProps.resourceCount = (int)decimalResourceCount;
        inGameUI.HandlePlayerPropertyChange(currentProps);
    }


    public void IncrementGoldResources()
    {
        decimalGoldResourceCount += goldResourceIncrementRate;

        currentProps.goldResourceCount = (int)decimalGoldResourceCount;
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
}


public struct PlayerProperties
{
    public int bombCount;
    public int resourceCount;
    public int goldResourceCount;

    public int bombStrengthLevel;

    public int bombCountLevel;

    public int resourceCountLevel;

    public int GetMaxResourceCount()
    {
        return (int)((float)resourceCountLevel * 1.25f * 100f);
    }

    public int GetMaxBombCount()
    {
        return (int)((float)bombCountLevel * 0.05f * 100f);
    }

    public float GetBombStrength()
    {
          return 1 + ((float)bombStrengthLevel * 0.25f);
    }

}