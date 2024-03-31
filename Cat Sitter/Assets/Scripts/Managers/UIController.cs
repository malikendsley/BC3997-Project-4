using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Slider cleanlinessSlider;
    [SerializeField] TextMeshProUGUI currentToolText;
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

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void OnCatGrabberSelected()
    {
        Debug.Log("Cat Grabber selected");
        LevelManager.Instance.ToolManager.HandleToolSelected(CatTool.CatGrabber);
    }

    public void OnLaserPointerSelected()
    {
        Debug.Log("Laser Pointer selected");
        LevelManager.Instance.ToolManager.HandleToolSelected(CatTool.LaserPointer);
    }

    public void OnFireExtinguisherSelected()
    {
        Debug.Log("Fire Extinguisher selected");
        LevelManager.Instance.ToolManager.HandleToolSelected(CatTool.FireExtinguisher);
    }

    public void OnWrenchSelected()
    {
        Debug.Log("Wrench selected");
        LevelManager.Instance.ToolManager.HandleToolSelected(CatTool.Wrench);
    }

    public void OnDustpanSelected()
    {
        Debug.Log("Dustpan selected");
        LevelManager.Instance.ToolManager.HandleToolSelected(CatTool.Dustpan);
    }

    public void SetToolText(CatTool tool)
    {
        var toolText = "None";
        switch (tool)
        {
            case CatTool.CatGrabber:
                toolText = "Cat Grabber";
                break;
            case CatTool.LaserPointer:
                toolText = "Laser Pointer";
                break;
            case CatTool.FireExtinguisher:
                toolText = "Fire Extinguisher";
                break;
            case CatTool.Wrench:
                toolText = "Wrench";
                break;
            case CatTool.Dustpan:
                toolText = "Dustpan";
                break;
        }
        currentToolText.text = toolText;
    }

}
