using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Font_Breathing : MonoBehaviour

{
    public TextMeshProUGUI Text;
    public float Size1;
    public float Size2;
    public AnimationCurve Ease;
    public float Speed;

    // Update is called once per frame
    void Update()
    {
        var minSize = Mathf.Min(Size1, Size2);
        var totalOffset = Mathf.Abs(Size1 - Size2);
        var factor = Ease.Evaluate(Mathf.PingPong(Time.time * Speed, 1));
        Text.fontSize = minSize + totalOffset * factor;
    }
}

