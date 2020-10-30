﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public static CursorScript instance = null;
    public bool cursorLocked;
    public bool storeOpen, conversationOpen, menuOpen;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if(storeOpen || conversationOpen || menuOpen)
        {
            if (cursorLocked) UnlockCursor();
        }
        else
        {
            if (!cursorLocked) LockCursor();
        }
    }
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
    }
}
