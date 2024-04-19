using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class ColorChange : MonoBehaviour
{
    public float duration = 2.5f;
    public Color targetColor = Color.red;

    private Renderer colorRenderer;
    private Color startColor;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        colorRenderer = GetComponent<Renderer>();
        startColor = colorRenderer.material.color;


        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        while (true)
        {
            yield return ResetColor();

            yield return LerpColor(startColor, targetColor, duration);
        }
    }

    IEnumerator LerpColor(Color start, Color end, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float t = Mathf.Clamp01((Time.time - startTime) / duration);
            colorRenderer.material.color = Color.Lerp(start, end, t);
            yield return null;
        }
    }
    IEnumerator ResetColor()
    {
        colorRenderer.material.color = startColor;
        yield return new WaitForSeconds(5.0f);
    }
}
