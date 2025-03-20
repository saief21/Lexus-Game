using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponShopUI : MonoBehaviour
{
    [System.Serializable]
    public class WeaponButton
    {
        public Button button;
        public TextMeshProUGUI priceText;
        public int weaponIndex;
    }

    [SerializeField] private WeaponButton[] weaponButtons;
    private WeaponManager weaponManager;

    private void Start()
    {
        weaponManager = FindFirstObjectByType<WeaponManager>();
        
        if (weaponManager != null)
        {
            // Configurer les boutons
            foreach (var weaponButton in weaponButtons)
            {
                weaponButton.button.onClick.AddListener(() => TryBuyWeapon(weaponButton.weaponIndex));
                
                // Mettre à jour l'état initial des boutons
                if (weaponManager.IsWeaponUnlocked(weaponButton.weaponIndex))
                {
                    weaponButton.button.interactable = false;
                    weaponButton.priceText.text = "ACHETÉ";
                }
            }
        }
        else
        {
            Debug.LogError("WeaponManager non trouvé !");
        }
    }

    private void TryBuyWeapon(int weaponIndex)
    {
        if (weaponManager != null && weaponManager.UnlockWeapon(weaponIndex))
        {
            // Désactiver le bouton une fois l'arme achetée
            foreach (var wb in weaponButtons)
            {
                if (wb.weaponIndex == weaponIndex)
                {
                    wb.button.interactable = false;
                    wb.priceText.text = "ACHETÉ";
                    break;
                }
            }
        }
    }
}
