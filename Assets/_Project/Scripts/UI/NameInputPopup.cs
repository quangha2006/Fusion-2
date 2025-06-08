using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInputPopup : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private List<ColorButton> colorList;
    
    private Action<string, Color> onSubmit;
    private Color colorSelected = Color.red;
    private void Start()
    {
        foreach (var button in colorList)
        {
            button.OnColorSelected += OnColorSelected;
        }
        colorList.First().button.onClick.Invoke();
        inputField.text = $"Player"+ UnityEngine.Random.Range(1000, 9999);
    }
    public void Init(Action<string, Color> onSubmitCallback)
    {
        onSubmit = onSubmitCallback;
        submitButton.onClick.AddListener(HandleSubmit);
    }

    void HandleSubmit()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            onSubmit?.Invoke(inputField.text, colorSelected);
            Destroy(gameObject);
        }
    }

    public void OnColorSelected(Color color, ColorButton button)
    {
        colorSelected = color;
        foreach (var btn in colorList)
        {
            btn.SetActive(false);
        }
        button.SetActive(true);
    }
}
