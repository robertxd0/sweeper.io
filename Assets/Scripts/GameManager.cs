using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private InGameUI inGameUI;

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
        currentProps.resourceCount = 500;

        inGameUI.HandlePlayerPropertyChange(currentProps);
    }
    //
    // Summary:
    //     Returns true if player has successfully bought. If player has not enough resources, it returns false.
    public bool IncrementBombs(int bombPrice)
    {
        if(bombPrice > currentProps.resourceCount)
        {
            return false;
        }

        currentProps.resourceCount -= bombPrice;
        currentProps.bombCount++;

        inGameUI.HandlePlayerPropertyChange(currentProps);
        return true;
    }

    public void IncrementResources()
    {
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
}


public struct PlayerProperties
{
    public int bombCount;
    public int resourceCount;
    public int money;
}