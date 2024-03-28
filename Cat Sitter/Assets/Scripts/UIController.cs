using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject timerText;
    [SerializeField]
    private Gradient gradient;
    [SerializeField]
    private Slider cleanlinessSlider;

    void Start()
    {
        cleanlinessSlider.fillRect.GetComponent<Image>().color = gradient.Evaluate(100);
    }

    public void SetCleanliness(float cleanliness)
    {
        cleanlinessSlider.value = cleanliness;
        cleanlinessSlider.fillRect.GetComponent<Image>().color = gradient.Evaluate(cleanliness);
    }

    public void SetTime(float time)
    {
        var minutes = Mathf.FloorToInt(time / 60);
        var seconds = Mathf.FloorToInt(time % 60);

        timerText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
