using UnityEngine;
using TMPro;
using System;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI frameCountText;

    public float pollingTime = 1f;
    private float time;
    private int frameCount;

    private float lastFrameTime = 0;

    void Update()
    {
        time += Time.deltaTime;
        frameCount++;

        //Frame Time
        frameCountText.text = "Frametime: " + MathF.Round(Time.smoothDeltaTime * 1000, 2) + "m/s per frame";

        //Display Change Once per second
        if(time >= pollingTime) {
            int frameRate = Mathf.RoundToInt(frameCount/time);
            fpsText.text = "Fps: " + frameRate.ToString();

            time -= pollingTime;
            frameCount = 0;
        }

        lastFrameTime = Time.time;
    }

}
