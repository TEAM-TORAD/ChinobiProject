using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCrosshair : MonoBehaviour
{
    #region Fields
        public bool drawCrosshair = true;
        public Color crosshairColor = Color.green;
        public float width = 1;
        public float height = 3;

        [System.Serializable]
        public class Spreading
        {
            public float sSpread = 200;
            public float maxSpread = 100;
            public float minSpread = 1;
            public float spreadPerSecond = 20;
            public float decreasePerSecond = 5;
        }

        public Spreading spread = new Spreading();

        Texture2D tex;
        float newHeight;
        GUIStyle lineStyle;

        #endregion

    #region Functions
        void Awake()
        {
            tex = new Texture2D(1, 1);
            lineStyle = new GUIStyle();
            lineStyle.normal.background = tex;
        }

        void OnGUI()
        {
            Vector2 centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);
            float screenRatio = Screen.height / 100;

            newHeight = height * screenRatio;

            if (drawCrosshair)
            {
                GUI.Box(new Rect(centerPoint.x - (width / 2), centerPoint.y - (newHeight + spread.sSpread), width, newHeight), GUIContent.none, lineStyle);
                GUI.Box(new Rect(centerPoint.x - (width / 2), (centerPoint.y + spread.sSpread), width, newHeight), GUIContent.none, lineStyle);
                GUI.Box(new Rect((centerPoint.x + spread.sSpread), (centerPoint.y - (width / 2)), newHeight, width), GUIContent.none, lineStyle);
                GUI.Box(new Rect(centerPoint.x - (newHeight + spread.sSpread), (centerPoint.y - (width / 2)), newHeight, width), GUIContent.none, lineStyle);
            }

        }

        public void SetColor(Texture2D myTexture, Color myColor)
        {
            for (int y = 0; y < myTexture.height; y++)
            {
                for (int x = 0; x < myTexture.width; x++)
                    myTexture.SetPixel(x, y, myColor);
                myTexture.Apply();
            }
        }
        #endregion 
}
