using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace 扫雷
{
    public class SliderShower : MonoBehaviour
    {
        private void Start()
        {
            var slider = transform.GetChild(1).GetComponent<Slider>();
            var display = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            display.SetText(slider.value + "");
            slider.onValueChanged.AddListener((e) => { display.SetText(e + ""); });
        }
    }
}