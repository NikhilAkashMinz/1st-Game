using UnityEngine;
using UnityEngine.UI;

public class HalthBarControl : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    public void SetSliderValue(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
