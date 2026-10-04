using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Image image;

    private Button _btn;
    
    public Button Button
    {
        get
        {
            if(_btn == null)
                _btn = GetComponent<Button>();
            return _btn;
        }
    }

    public void SetText(string text)
    {
        textField.text = text;
    }
    
    public void SetInteractable(bool interactable)
    {
        _btn.interactable = interactable;
    }

    public void SetImage(Sprite newIcon)
    {
        if (image == null) return;
        image.sprite = newIcon;
    }
}
