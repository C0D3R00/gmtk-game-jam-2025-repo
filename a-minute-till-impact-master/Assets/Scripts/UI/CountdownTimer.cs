using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public UIDocument uiDocument;
    public int startingSeconds = 60;
    public AudioSource beepAudioSource;

    private Label countdownLabel;
    private float currentTime;
    private bool isRunning;
    private bool isFlashing;
    private int lastSecondBeeped = -1;

    void OnEnable()
    {
        countdownLabel = uiDocument.rootVisualElement.Q<Label>("countdown-label");
        ResetTimer();
        StartCoroutine(UpdateTimer());
    }

    void ResetTimer()
    {
        currentTime = startingSeconds;
        isRunning = true;
        isFlashing = false;
        lastSecondBeeped = -1;
    }

    IEnumerator UpdateTimer()
    {
        while (isRunning)
        {
            if (currentTime > 0f)
            {
                currentTime -= Time.deltaTime;
                int totalSeconds = Mathf.CeilToInt(currentTime);
                int hours = totalSeconds / 3600;
                int minutes = (totalSeconds % 3600) / 60;
                int seconds = totalSeconds % 60;

                countdownLabel.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";

                if (totalSeconds <= 10)
                {
                    // 🔊 Play beep if not already played for this second
                    if (lastSecondBeeped != totalSeconds)
                    {
                        beepAudioSource?.Play();
                        lastSecondBeeped = totalSeconds;
                    }

                    // Start flashing if not already
                    if (!isFlashing)
                    {
                        isFlashing = true;
                        StartCoroutine(FlashTextColor());
                    }
                }
                else
                {
                    countdownLabel.style.color = Color.white;
                    isFlashing = false;
                }
            }
            else
            {
                countdownLabel.text = "00:00:00";
                countdownLabel.style.color = Color.red;
                isRunning = false;
                isFlashing = false;
                OnTimerEnd();
            }

            yield return null;
        }
    }

    IEnumerator FlashTextColor()
    {
        while (isFlashing)
        {
            countdownLabel.style.color = countdownLabel.style.color == Color.red ? Color.white : Color.red;
            yield return new WaitForSeconds(0.5f);
        }

        countdownLabel.style.color = Color.white; // reset after flashing
    }

    void OnTimerEnd()
    {
        Debug.Log("⏰ Timer finished!");
        // Add end-of-timer logic here
    }
}
