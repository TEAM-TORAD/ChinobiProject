using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    public float rise;
    public Vector3 goalVector;
    public Transform portal;
    public Transform tree;

    // Start is called before the first frame update
    void Start()
    {
        goalVector = transform.position;
        goalVector.y += rise;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            WaterRise();
        }
    }
    private void WaterRiseCompleted()
    {
        // Activate the portal
        foreach (Transform t in portal)
        {
            if (!t.gameObject.activeSelf) t.gameObject.SetActive(true);
        }
        tree.gameObject.SetActive(true);
    }
    public void WaterRise()
    {
        var conditions = iTween.Hash("position", goalVector, "time", 15.0f, "easetype", iTween.EaseType.easeInOutSine, "oncomplete", "WaterRiseCompleted");
        //iTween.MoveTo(water.gameObject, goalVector, 5);
        iTween.MoveTo(this.gameObject, conditions);
    }
    private void CalmWater()
    {
        Renderer r = GetComponent<Renderer>();
        Material mat = r.material;

        float[] startNormal = mat.GetFloatArray("_NormalDirection1");
        float redPerSec = (startNormal[1] - 0.1f) / 2;

        float timer = 0;
        while (timer < 2)
        {
            timer += Time.deltaTime;
            mat.SetFloatArray("_NormalDirection1", new float[2] { 0, startNormal[1] - redPerSec * timer });
            mat.SetFloatArray("_NormalDirection2", new float[2] { 0, -startNormal[1] + redPerSec * timer });
        }
    }
}
