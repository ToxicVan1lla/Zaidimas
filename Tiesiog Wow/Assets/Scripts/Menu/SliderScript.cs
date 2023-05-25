using UnityEngine.UI;
using UnityEngine;

public class SliderScript : MonoBehaviour
{
    public Slider volumeSlider;
    [SerializeField] private KeepData data;
    

    private void Start()
    {
        volumeSlider.value = data.volumeValue;
        soundManager.instance.adjustVolume(data.volumeValue);
        volumeSlider.onValueChanged.AddListener(val => soundManager.instance.adjustVolume(val));

    }
    private void Update()
    {
        data.volumeValue = volumeSlider.value;
    }


}
