using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 360f;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Combat")]
    public Transform weaponHolder;
    public GameObject currentWeapon;
    
    private Rigidbody rb;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleShooting();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
    }

    void HandleRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y;
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleShooting()
    {
        if (Input.GetMouseButton(0) && currentWeapon != null)
        {
            WeaponController weapon = currentWeapon.GetComponent<WeaponController>();
            if (weapon != null)
            {
                weapon.Shoot();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameManager.GameOver();
        gameObject.SetActive(false);
    }

    public void SetWeapon(GameObject newWeapon)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
        
        currentWeapon = Instantiate(newWeapon, weaponHolder.position, weaponHolder.rotation, weaponHolder);
    }
}
