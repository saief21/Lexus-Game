using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    [System.Serializable]
    public class WeaponSlot
    {
        public GameObject weaponPrefab;
        public bool isUnlocked;
    }

    [Header("Armes")]
    [SerializeField] private List<WeaponSlot> weapons = new List<WeaponSlot>();
    [SerializeField] private Transform weaponHolder;

    private int currentWeaponIndex = 0;
    private Weapon currentWeapon;

    private void Start()
    {
        // Débloquer la première arme par défaut
        if (weapons.Count > 0)
        {
            weapons[0].isUnlocked = true;
            EquipWeapon(0);
        }
    }

    private void Update()
    {
        // Changer d'arme avec la molette
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int newIndex = currentWeaponIndex;
            if (scroll > 0)
                newIndex = (currentWeaponIndex + 1) % weapons.Count;
            else
                newIndex = (currentWeaponIndex - 1 + weapons.Count) % weapons.Count;

            if (weapons[newIndex].isUnlocked)
                EquipWeapon(newIndex);
        }

        // Tirer
        if (Input.GetButton("Fire1") && currentWeapon != null)
        {
            currentWeapon.Shoot();
        }
    }

    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count || !weapons[index].isUnlocked)
            return;

        // Détruire l'arme actuelle
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        // Créer la nouvelle arme
        GameObject weaponObject = Instantiate(weapons[index].weaponPrefab, weaponHolder);
        currentWeapon = weaponObject.GetComponent<Weapon>();
        currentWeaponIndex = index;
    }

    public bool UnlockWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count || weapons[index].isUnlocked)
            return false;

        Weapon weaponToUnlock = weapons[index].weaponPrefab.GetComponent<Weapon>();
        if (GameManager.Instance.TrySpendCurrency(weaponToUnlock.GetCost()))
        {
            weapons[index].isUnlocked = true;
            return true;
        }

        return false;
    }

    public bool IsWeaponUnlocked(int index)
    {
        return index >= 0 && index < weapons.Count && weapons[index].isUnlocked;
    }

    public string GetCurrentWeaponName()
    {
        return currentWeapon != null ? currentWeapon.GetWeaponName() : "Aucune arme";
    }

    public int GetWeaponCost(int index)
    {
        if (index < 0 || index >= weapons.Count)
            return 0;

        Weapon weapon = weapons[index].weaponPrefab.GetComponent<Weapon>();
        return weapon != null ? weapon.GetCost() : 0;
    }
}
