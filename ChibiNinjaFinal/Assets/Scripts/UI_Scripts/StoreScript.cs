using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreScript : MonoBehaviour
{
    public void CloseStore()
    {
        Economy.economy.storePanel.gameObject.SetActive(false);
        CursorScript.instance.storeOpen = false;
        CameraLookAt.instance.Deactivate();
    }
}
