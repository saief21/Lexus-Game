using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopItemButton : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Button purchaseButton;

    public void Initialize(ShopController.ShopItem item, Action onPurchase)
    {
        iconImage.sprite = item.icon;
        nameText.text = item.itemName;
        costText.text = $"${item.cost}";
        purchaseButton.onClick.AddListener(() => onPurchase?.Invoke());
    }
}
