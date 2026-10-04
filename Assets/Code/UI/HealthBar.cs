using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] protected Slider slider;
    
    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
    }
    
    public void SetValue(float value)
    {
        slider.value = value;
    }
}
