using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public GameObject weaponPrefab;
        public int cost;
        public Sprite icon;
    }

    public ShopItem[] shopItems;
    public GameObject itemButtonPrefab;
    public Transform shopContent;
    
    private GameManager gameManager;
    private PlayerController player;

    void Start()
    {
        gameManager = GameManager.Instance;
        player = FindObjectOfType<PlayerController>();
        InitializeShop();
    }

    void InitializeShop()
    {
        foreach (ShopItem item in shopItems)
        {
            GameObject buttonObj = Instantiate(itemButtonPrefab, shopContent);
            ShopItemButton itemButton = buttonObj.GetComponent<ShopItemButton>();
            
            itemButton.Initialize(item, () => PurchaseItem(item));
        }
    }

    void PurchaseItem(ShopItem item)
    {
        if (gameManager.TryPurchase(item.cost))
        {
            player.SetWeapon(item.weaponPrefab);
        }
    }
}
