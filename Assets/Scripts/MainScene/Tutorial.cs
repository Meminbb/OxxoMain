using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Sprite[] tutorialSprites; 
    public GameObject tutorialPanel; 
    public Image displayImage;

    private int currentIndex = 0;
    private bool isOpen = false;

    void Start()
    {
        tutorialPanel.SetActive(false);
    }

    public void ToggleTutorial()
    {
        isOpen = !isOpen;
        tutorialPanel.SetActive(isOpen);

        if (isOpen)
        {
            Time.timeScale = 0f;
            currentIndex = 0;
            UpdateImage();
        }else
        {
            Time.timeScale = 1f;

        }
    }

    public void NextImage()
    {
        if (!isOpen) return;

        currentIndex = (currentIndex + 1) % tutorialSprites.Length;
        UpdateImage();
    }

    public void PreviousImage()
    {
        if (!isOpen) return;

        currentIndex = (currentIndex - 1 + tutorialSprites.Length) % tutorialSprites.Length;
        UpdateImage();
    }

    private void UpdateImage()
    {
        if (tutorialSprites.Length > 0 && currentIndex >= 0 && currentIndex < tutorialSprites.Length)
        {
            displayImage.sprite = tutorialSprites[currentIndex];
        }
    }
}
