using UnityEngine;
using UnityEngine.UI;

public class StatBarView : MonoBehaviour, IStatView
{
    [SerializeField] private Image fillImage;

    public void UpdateView(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}
