using UnityEngine;

public interface IStat
{
    float CurrentValue { get; }
    float MaxValue { get; }

    event System.Action<float, float> OnValueChanged;
}
