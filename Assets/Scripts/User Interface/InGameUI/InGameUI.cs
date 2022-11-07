using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text bombCount;
    [SerializeField] private TMPro.TMP_Text resourceCount;

    public void HandlePlayerPropertyChange(PlayerProperties props)
    {
        this.bombCount.text = props.bombCount.ToString();
        this.resourceCount.text = props.resourceCount.ToString();
    }
}
