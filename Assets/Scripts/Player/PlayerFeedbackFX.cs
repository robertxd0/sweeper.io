using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedbackFX : MonoBehaviour
{
    [SerializeField] private Animation playerPurchasedBombFX;

    public void ExecutePlayerPurchasedBombFX()
    {
        if(playerPurchasedBombFX.isPlaying)
        {
            //this.playerPurchasedBombFX.Rewind();
        }else
        {
            this.playerPurchasedBombFX.Play();
        }

        Vector3 newPos = this.transform.position;
        newPos.y += 2f;
        this.playerPurchasedBombFX.transform.parent.transform.position = newPos;
    }
}