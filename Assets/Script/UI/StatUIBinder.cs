using UnityEngine;

public class StatUIBinder : MonoBehaviour
{
    [SerializeField] private MonoBehaviour statSource; // IStat
    [SerializeField] private StatBarView statView;

    private IStat stat;

    private void Awake()
    {
        stat = statSource as IStat;

        if (stat == null)
        {
            Debug.LogError("Stat is Null");
            return;
        }

        stat.OnValueChanged += statView.UpdateView;
        statView.UpdateView(stat.CurrentValue, stat.MaxValue);
    }

    private void OnDestroy()
    {
        if (stat != null)
            stat.OnValueChanged -= statView.UpdateView;
    }
}
