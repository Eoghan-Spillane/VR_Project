using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI frameCountText;

    public float pollingTime = 1f;
    private float time;
    private int frameCount;

    void Update()
    {
        time += Time.deltaTime;
        frameCount++;

        if(time >= pollingTime) {
            int frameRate = Mathf.RoundToInt(frameCount/time);
            fpsText.text = "Fps: " + frameRate.ToString();
            frameCountText.text = "Framecount: " + frameCount.ToString();

            time -= pollingTime;
            frameCount = 0;
        }
    }
}
