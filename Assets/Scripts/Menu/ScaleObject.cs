using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    public float scaleSpeed = 1f;
    private float targetScaleY;
    private bool isScaling = false;

    public void StartScaleTo(float targetY)
    {
        targetScaleY = targetY;
        isScaling = true;
    }

    private void Update()
    {
        if (!isScaling) return;

        Vector3 scale = transform.localScale;
        float newY = Mathf.MoveTowards(scale.y, targetScaleY, scaleSpeed * Time.deltaTime);
        transform.localScale = new Vector3(scale.x, newY, scale.z);

        if (Mathf.Approximately(newY, targetScaleY))
        {
            isScaling = false;
        }
    }
}
