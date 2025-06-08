using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    [SerializeField] public Button button;
    [SerializeField] public Image image;
    public Action<Color, ColorButton> OnColorSelected;
    void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        OnColorSelected?.Invoke(image.color, this);
    }
    public void SetActive(bool isActive)
    {
        Vector3 scale = isActive ? Vector3.one * 1.2f : Vector3.one;
        scale.z = 1f;
        transform.localScale = scale;
    } 
}
