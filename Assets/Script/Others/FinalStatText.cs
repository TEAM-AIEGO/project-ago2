using TMPro;
using UnityEngine;

public class FinalStatText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!text) return;
        text.text = $"걸린 시간: {StaticValueManager.GameTime.ToString("0.##")}초   찾은 고양이: {StaticValueManager.CatsFound}/8"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
