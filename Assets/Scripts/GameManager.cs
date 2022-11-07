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
        currentProps = new PlayerProperties();
        currentProps.bombCount = 5;
        currentProps.resourceCount = 0;

        inGameUI.HandlePlayerPropertyChange(currentProps);
    }

    public void UpdateResources()
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
}