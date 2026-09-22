
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MineManager : MonoBehaviour
{
    [Header("Mayın Görselleri")]
    public Sprite[] mineSprites;

    [Header("Patlama Görselleri")]
    public Sprite[] explosionSprites;

    [Header("Mayın Ayarları")]
    public int mineCount = 3;
    public float mineSize = 80f;

    [Header("Hareket Ayarları")]
    public float moveSpeed = 80f;
    public float changeDirectionTime = 1.5f;

    [Header("Oyun Alanı")]
    public RectTransform gameArea;

    [Header("Mayın Çarpma Efekti")]
    public GameObject hitEffect;

    [Header("Mayın Patlama Sesi")]
    public AudioSource audioSource;
    public AudioClip explosionSound;


    // ============================================================
    // BAŞLANGIÇ
    // ============================================================

    void Start()
    {
        if (hitEffect != null)
        {
            hitEffect.SetActive(false);
        }
    }


    // ============================================================
    // MAYINLARI OLUŞTUR
    // ============================================================

    void CreateMines()
    {
        if (mineSprites == null || mineSprites.Length == 0)
        {
            Debug.LogWarning("Mayın görselleri eklenmemiş!");
            return;
        }

        if (gameArea == null)
        {
            Debug.LogWarning("Game Area atanmadı!");
            return;
        }

        for (int i = 0; i < mineCount; i++)
        {
            CreateMine();
        }
    }


    // ============================================================
    // TEK MAYIN OLUŞTUR
    // ============================================================

    void CreateMine()
    {
        GameObject mineObject =
            new GameObject("Mine");

        mineObject.transform.SetParent(
            gameArea,
            false
        );

        Image image =
            mineObject.AddComponent<Image>();

        image.sprite =
            mineSprites[
                Random.Range(
                    0,
                    mineSprites.Length
                )
            ];

        image.preserveAspect = true;
        image.raycastTarget = true;

        RectTransform rect =
            mineObject.GetComponent<RectTransform>();

        rect.sizeDelta =
            new Vector2(
                mineSize,
                mineSize
            );

        rect.anchoredPosition =
            GetRandomPosition();

        Button button =
            mineObject.AddComponent<Button>();

        button.transition =
            Selectable.Transition.None;

        button.onClick.AddListener(() =>
        {
            ExplodeMine(mineObject);
        });

        MineMovement movement =
            mineObject.AddComponent<MineMovement>();

        movement.gameArea = gameArea;
        movement.moveSpeed = moveSpeed;
        movement.changeDirectionTime =
            changeDirectionTime;
        movement.mineSize = mineSize;
    }


    // ============================================================
    // RASTGELE MAYIN KONUMU
    // ============================================================

    Vector2 GetRandomPosition()
    {
        float halfWidth =
            gameArea.rect.width / 2f;

        float halfHeight =
            gameArea.rect.height / 2f;

        float x =
            Random.Range(
                -halfWidth + mineSize / 2f,
                halfWidth - mineSize / 2f
            );

        float y =
            Random.Range(
                -halfHeight + mineSize / 2f,
                halfHeight - mineSize / 2f
            );

        return new Vector2(x, y);
    }


    // ============================================================
    // MAYINA BASILDI
    // ============================================================

    void ExplodeMine(GameObject mineObject)
    {
        ShowHitEffect();

        PlayExplosionSound();

        TriggerVibration();

        if (explosionSprites == null ||
            explosionSprites.Length == 0)
        {
            Destroy(mineObject);

            LoseLife();

            return;
        }

        Image mineImage =
            mineObject.GetComponent<Image>();

        if (mineImage != null)
        {
            mineImage.sprite =
                explosionSprites[
                    Random.Range(
                        0,
                        explosionSprites.Length
                    )
                ];

            mineImage.preserveAspect = true;
        }

        Button button =
            mineObject.GetComponent<Button>();

        if (button != null)
        {
            button.interactable = false;
        }

        MineMovement movement =
            mineObject.GetComponent<MineMovement>();

        if (movement != null)
        {
            movement.enabled = false;
        }

        LoseLife();

        StartCoroutine(
            RemoveExplosion(mineObject)
        );
    }


    // ============================================================
    // TİTREŞİM
    // ============================================================

    void TriggerVibration()
    {
        bool vibrationOn =
            PlayerPrefs.GetInt("VibrationOn", 1) == 1;

        if (vibrationOn)
        {
            Handheld.Vibrate();
        }
    }


    // ============================================================
    // PATLAMA SESİ
    // ============================================================

    void PlayExplosionSound()
    {
        if (audioSource == null ||
            explosionSound == null)
            return;

        audioSource.PlayOneShot(
            explosionSound
        );
    }


    // ============================================================
    // KIRMIZI EFEKT
    // ============================================================

    void ShowHitEffect()
    {
        if (hitEffect == null)
            return;

        StopCoroutine(
            "HideHitEffect"
        );

        hitEffect.SetActive(true);

        StartCoroutine(
            HideHitEffect()
        );
    }


    IEnumerator HideHitEffect()
    {
        yield return new WaitForSeconds(
            0.15f
        );

        if (hitEffect != null)
        {
            hitEffect.SetActive(false);
        }
    }


    // ============================================================
    // PATLAMA GÖRSELİNİ SİL
    // ============================================================

    IEnumerator RemoveExplosion(
        GameObject mineObject
    )
    {
        yield return new WaitForSeconds(
            0.5f
        );

        if (mineObject != null)
        {
            Destroy(mineObject);
        }
    }


    // ============================================================
    // CAN KAYBET
    // ============================================================

    void LoseLife()
    {
        GameManager gameManager =
            FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.LoseLife();
        }
    }


    // ============================================================
    // MAYIN SAYISI
    // ============================================================

    public void UpdateMineCount(int level)
    {
        if (level <= 5)
        {
            mineCount = 3;
        }
        else if (level <= 10)
        {
            mineCount = 4;
        }
        else if (level <= 15)
        {
            mineCount = 5;
        }
        else if (level <= 20)
        {
            mineCount = 6;
        }
        else if (level <= 25)
        {
            mineCount = 7;
        }
        else if (level <= 30)
        {
            mineCount = 8;
        }
        else if (level <= 35)
        {
            mineCount = 9;
        }
        else
        {
            mineCount = 10;
        }
    }


    // ============================================================
    // MAYIN HIZI
    // ============================================================

    public void UpdateMineSpeed(int level)
    {
        if (level <= 10)
        {
            moveSpeed = 80f;
        }
        else if (level <= 20)
        {
            moveSpeed = 100f;
        }
        else if (level <= 50)
        {
            moveSpeed = 130f;
        }
        else if (level <= 100)
        {
            moveSpeed = 160f;
        }
        else if (level <= 150)
        {
            moveSpeed = 190f;
        }
        else
        {
            moveSpeed = 220f;
        }
    }


    // ============================================================
    // LEVEL DEĞİŞTİĞİNDE MAYINLARI YENİLE
    // ============================================================

    public void RefreshMines(int level)
    {
        if (gameArea == null)
            return;

        ClearMines();

        UpdateMineCount(level);
        UpdateMineSpeed(level);

        CreateMines();
    }


    // ============================================================
    // MAYINLARI TEMİZLE
    // ============================================================

    void ClearMines()
    {
        if (gameArea == null)
            return;

        for (int i =
             gameArea.childCount - 1;
             i >= 0;
             i--)
        {
            Transform child =
                gameArea.GetChild(i);

            if (child.name == "Mine")
            {
                Destroy(child.gameObject);
            }
        }
    }


    // ============================================================
    // ANA MENÜYE DÖNÜŞ
    // ============================================================

    public void StopMines()
    {
        ClearMines();

        StopAllCoroutines();

        if (hitEffect != null)
        {
            hitEffect.SetActive(false);
        }
    }


    // ============================================================
    // OYUN YENİDEN BAŞLADI
    // ============================================================

    public void RestartMines()
    {
        ClearMines();

        StopAllCoroutines();

        if (hitEffect != null)
        {
            hitEffect.SetActive(false);
        }

        UpdateMineCount(1);
        UpdateMineSpeed(1);

        CreateMines();
    }
}


// ============================================================
// MAYIN HAREKET SİSTEMİ
// ============================================================

public class MineMovement : MonoBehaviour
{
    public RectTransform gameArea;

    public float moveSpeed = 80f;
    public float changeDirectionTime = 1.5f;
    public float mineSize = 80f;

    private RectTransform rectTransform;
    private Vector2 direction;
    private float timer;


    void Start()
    {
        rectTransform =
            GetComponent<RectTransform>();

        direction =
            Random.insideUnitCircle.normalized;

        if (direction == Vector2.zero)
        {
            direction = Vector2.right;
        }

        timer =
            Random.Range(
                0.5f,
                changeDirectionTime
            );
    }


    void Update()
    {
        if (rectTransform == null ||
            gameArea == null)
            return;

        if (!gameArea.gameObject.activeInHierarchy)
            return;

        rectTransform.anchoredPosition +=
            direction *
            moveSpeed *
            Time.deltaTime;

        KeepInsideScreen();

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            direction =
                Random.insideUnitCircle.normalized;

            if (direction == Vector2.zero)
            {
                direction = Vector2.right;
            }

            timer =
                changeDirectionTime;
        }
    }


    void KeepInsideScreen()
    {
        float halfWidth =
            gameArea.rect.width / 2f;

        float halfHeight =
            gameArea.rect.height / 2f;

        Vector2 position =
            rectTransform.anchoredPosition;

        float minX =
            -halfWidth +
            mineSize / 2f;

        float maxX =
            halfWidth -
            mineSize / 2f;

        float minY =
            -halfHeight +
            mineSize / 2f;

        float maxY =
            halfHeight -
            mineSize / 2f;

        if (position.x <= minX)
        {
            position.x = minX;
            direction.x =
                Mathf.Abs(direction.x);
        }

        if (position.x >= maxX)
        {
            position.x = maxX;
            direction.x =
                -Mathf.Abs(direction.x);
        }

        if (position.y <= minY)
        {
            position.y = minY;
            direction.y =
                Mathf.Abs(direction.y);
        }

        if (position.y >= maxY)
        {
            position.y = maxY;
            direction.y =
                -Mathf.Abs(direction.y);
        }

        rectTransform.anchoredPosition =
            position;
    }
}
