using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Sprite[] tutorialSprites;   // Tus sprites (4 en total)
    public GameObject tutorialPanel;   // El panel que contiene la imagen y botones
    public Image displayImage;         // El componente Image que mostrará los sprites

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
            currentIndex = 0;
            UpdateImage();
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
