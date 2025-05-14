using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public bool isGreen; // Set to true on the designated "green" button
    public Button[] otherButtons; // Assign the other two TMP buttons here

    private Button thisButton;
    private Image thisImage;

    void Start()
    {
        thisButton = GetComponent<Button>();
        thisImage = GetComponent<Image>();

        thisButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (isGreen)
        {
            // Turn self green
            SetColor(thisImage, Color.green);

            // Turn all others red
            foreach (Button btn in otherButtons)
            {
                Image img = btn.GetComponent<Image>();
                if (img != null)
                {
                    SetColor(img, Color.red);
                }
            }
        }
        else
        {
            // Just turn self red
            SetColor(thisImage, Color.red);
        }
    }

    void SetColor(Image img, Color color)
    {
        img.color = color;
    }
}
