using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private static int sortingOrder = 0;

    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        GameObject damagePopupPrefab = Resources.Load<GameObject>("DamagePopup");
        GameObject damagePopupObject = Instantiate(damagePopupPrefab, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupObject.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);
        return damagePopup;
    }

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit)
    {
        textMesh.SetText(damageAmount.ToString());
        if (isCriticalHit)
        {
            textMesh.fontSize = 6;
            textColor = Color.red;
        }
        else
        {
            textMesh.fontSize = 4;
            textColor = Color.yellow;
        }
        textMesh.color = textColor;
        disappearTimer = 1f;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(Random.Range(-1f, 1f), 1, 0) * 3f;
    }

    void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > 0.5f)
        {
            transform.localScale += Vector3.one * 1f * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * 1f * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= 3f * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
