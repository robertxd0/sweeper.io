using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private InGameUI inGameUI;

    [SerializeField] InputManager playerInputManager;
    
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
        currentProps.bombCount = 5;
        currentProps.resourceCount = 0;
        currentProps.bombStrengthLevel = 1;
        currentProps.bombCountLevel = 1;
        currentProps.resourceCountLevel = 1;

        inGameUI.HandlePlayerPropertyChange(currentProps);

        feedbackAnimationManager = FeedbackAnimationManager.instance;
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
        
        feedbackAnimationManager.ExecFeedbackFX(FXFeedbackTypes.BOMB_PURCHASE_SUCCESSFUL, playerInputManager.transform);
        inGameUI.HandlePlayerPropertyChange(currentProps);
        return true;
    }

    public void IncrementResources()
    {
        if(currentProps.resourceCount >= currentProps.GetMaxResourceCount())
            return;
        
        currentProps.resourceCount++;
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