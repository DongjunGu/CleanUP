using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public Texture2D crosshairImage;
    public float crosshairSize = 32f;

    private void OnGUI()
    {
        
        Vector3 mousePosition = Input.mousePosition;

        
        float crosshairX = mousePosition.x - (crosshairSize * 0.5f);
        float crosshairY = Screen.height - mousePosition.y - (crosshairSize * 0.5f);

        
        GUI.DrawTexture(new Rect(crosshairX, crosshairY, crosshairSize, crosshairSize), crosshairImage);
    }
}
