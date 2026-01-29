using UnityEngine;
using UnityEngine.UI;

public class StatBarView : MonoBehaviour, IStatView
{
    [SerializeField] private Image fillImage;

    private float value;

    public void UpdateView(float current, float max)
    {
        value = current / max;
    }

    private void Update()
    {
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, value, 20 * Time.deltaTime);
    }
}
