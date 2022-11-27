using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum FXFeedbackTypes
{
    BOMB_PURCHASE_SUCCESSFUL
}

[System.Serializable]
public struct FXFeedbackTemplate
{
    public FXFeedbackTypes feedbackType;
    public GameObject feedbackPrefab;
}

public class FeedbackAnimationManager : MonoBehaviour
{
    public static FeedbackAnimationManager instance;

    [SerializeField] private FXFeedbackTemplate[] fXFeedbackTemplates;

    [SerializeField] private int poolMagnitude = 5;

    private Dictionary<FXFeedbackTypes, ObjectPooler> fxPools;

    private void Awake() {
        FeedbackAnimationManager.instance = this;
    }

    private void OnDisable() {
        FeedbackAnimationManager.instance = null;
    }

    private void Start() {
        this.fxPools = new Dictionary<FXFeedbackTypes, ObjectPooler>();

        //initiate pools
        foreach(FXFeedbackTemplate template in fXFeedbackTemplates)
        {
            ObjectPooler pooler = new ObjectPooler(poolMagnitude, template.feedbackPrefab);

            fxPools.Add(template.feedbackType, pooler);
        }



        //set up pool objects
        foreach(FXFeedbackTypes type in this.fxPools.Keys)
        {
            GameObject typeParent = new GameObject(type.ToString());
            typeParent.transform.SetParent(this.transform);

            foreach(GameObject objectInstance in this.fxPools.GetValueOrDefault(type))
            {
                objectInstance.transform.parent = typeParent.transform;
            }
        }

    }

    public void ExecFeedbackFX(FXFeedbackTypes feedbackType, Transform origin)
    {
        var fxTypeInstance = this.fxPools.GetValueOrDefault(feedbackType).GetObject();
        
        var newPos = origin.position;
        newPos.y += 1.5f;
        fxTypeInstance.transform.parent.position = newPos;
    }

    public void RecycleFX(GameObject fxToRecycle)
    {
        object type;

        if(Enum.TryParse(typeof(FXFeedbackTypes), fxToRecycle.transform.parent.name, false, out type))
        {
            fxToRecycle.SetActive(false);
            this.fxPools.GetValueOrDefault((FXFeedbackTypes) type).PoolObject(fxToRecycle);
        }
    }
}
