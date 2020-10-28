using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnScript : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform cameraFadePanel;
    private Rigidbody RB;
    bool lerpColor;
    Image image;
    Color curColor;
    public float fadeRate = 0.1f, targetAlpha = 0;
    // Start is called before the first frame update
    void Start()
    {
        image = cameraFadePanel.GetComponent<Image>();
        RB = GetComponent<Rigidbody>();
        SpawnPlayer(spawnPoint);
    }
    private void Update()
    {
        if(lerpColor)
        {
            curColor = image.color;
            float alphaDiff = Mathf.Abs(curColor.a - targetAlpha);
            if (alphaDiff > 0.0001f)
            {
                curColor.a = Mathf.Lerp(curColor.a, targetAlpha, fadeRate * Time.deltaTime);
                image.color = curColor;
            }
            else lerpColor = false;
        }
    }
    public void SetSpawnPoint(Transform _spawnPoint)
    {
        spawnPoint = _spawnPoint;
    }
    public void SpawnPlayer(Transform _spawnPoint)
    {
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        RB.velocity = Vector3.zero;
        RB.angularVelocity = Vector3.zero;
        StartCoroutine(FadeIn());
        
    }
    IEnumerator FadeIn()
    {
        targetAlpha = 0.0f;
        cameraFadePanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        lerpColor = true;
    }
    
}
