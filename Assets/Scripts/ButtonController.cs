using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    [Header("UI References")]
    public Button button;
    public TextMeshProUGUI clickCounter;
    public TextMeshProUGUI buttonMessage;
    public GameManager gameManager;

    [Header("Settings")]
    public bool canPlay = true;

    private int clickCount = 0;
    private Vector2 startingPosition;
    private bool clickLocked = false;

    private Coroutine messageCoroutine;
    private Coroutine movementCoroutine;

    // Cache Edilmiş Bileşenler (Performans Optimizasyonu)
    private RectTransform buttonRectTransform;
    private RectTransform canvasRectTransform;
    private Transform bubbleTransform;

    private string[][] levels =
    {
        new string[] { "Buradayım..." },
        new string[] { "Beni mi arıyorsun?", "Daha yeni başladık..." },
        new string[] { "Bırak artık!", "Yorulmadın mı?", "Yine mi sen?" },
        new string[] { "Kaçtım ki!", "Yakalayamazsın!", "Hızlı mısın bakalım?", "Boşuna uğraşma." },
        new string[] { "Sıkılmadın mı?", "Pes et bence.", "O kadar kolay değil.", "Biraz daha hızlı!", "Neredeyse tutuyordun..." },
        new string[] { "Aha buradayım!", "Dokunamadın ki!", "Ciddi olamazsın.", "Hahaha!", "Devam et bakalım.", "Azimlisin ama yetmez." },
        new string[] { "Bir sağda bir solda!", "Hızlandım mı ne?", "Reflekslerin zayıf.", "Oraya değil buraya!", "Neredeyim bil bakalım?", "Çok yakın!", "Ama yine kaçtım." },
        new string[] { "Ağlama ama...", "Işınlanıyorum sanki?", "Tıklamak imkansız!", "Parmağın yoruldu mu?", "Benim pilim bitmez.", "Biraz mola ver.", "Şaka şaka, kaçtım!", "Iı-ıh, tutamadın." },
        new string[] { "Sınırları zorluyorsun.", "Hala buradayım.", "Belki de sen kazanırsın?", "Yok ya, zor biraz.", "Sabrını takdir ettim.", "Ama pes etmelisin.", "Az kaldı sanıyorsun...", "Ama bitmeyecek!", "Hahaha!" },
        new string[] { "Efsane misin sen?", "Son seviye sanma!", "Tamam tamam, hızlısın.", "Ama ben daha hızlıyım!", "Pes et artik!", "Buraya kadar geleceğini düşünmemiştim.", "Seni tebrik ederim...", "Ama kazanamayacaksın!", "Görüşürüz!", "Bye bye!" },
        new string[] { "Hop! Buradayım.", "Hızlısın ama yeterli değil.", "Beni yakalayamazsın!" },
        new string[] { "Aaaa, yine mi sen?", "Bırak artık şu telefonu.", "Parmağın acımadı mı?" },
        new string[] { "Sağ, sol, bam!", "Nereye baktığını sanıyorsun?", "Hala buradayım!" },
        new string[] { "Işınlanma modumu açtım.", "Bir oradayım bir burada.", "Beni asla yakalayamazsın!" },
        new string[] { "Gözlerini kapa ve dene.", "Belki o zaman tutarsın.", "Hahaha!" },
        new string[] { "Çok yaklaştın...", "Ama yetersiz!", "Devam et bakalım." },
        new string[] { "Sabır taşı mısın sen?", "Yeter artık, dur!", "Oyun bitti sayılır." },
        new string[] { "Kafam döndü!", "Seni izlemekten ben yoruldum.", "Pes et gitsin." },
        new string[] { "Bunu başarabileceğini mi sanıyorsun?", "Daha çok yolun var.", "İmkansız!" },
        new string[] { "20. Seviyeye geldin ha?", "Buradan ötesi yok!", "Sonuna yaklaşıyorsun..." },
        new string[] { "Hala pes etmedin mi?", "Gerçekten ilginç.", "Bakalım nereye kadar gideceksin." },
        new string[] { "Işık hızındayım!", "Beni görebiliyor musun?", "Rüzgarımı hisset!" },
        new string[] { "Bir tık daha hızlı olmalısın.", "Çok yavaş kaldın.", "Hahaha!" },
        new string[] { "Ooo, tam ısınıyordum.", "Sen de ısındın mı?", "Hadi bakalım!" },
        new string[] { "Sol elinle mi oynuyorsun?", "Biraz ciddiyet lütfen.", "Beni yakala!" },
        new string[] { "Ekrana çok basma, kıracaksın!", "Sakin ol şampiyon.", "Yine kaçtım." },
        new string[] { "Ritim tutturdun sanki?", "Ama ben ritmi bozarım!", "Bam!" },
        new string[] { "Rekor mu kıracaksın?", "İzin vermem!", "Buradayım!" },
        new string[] { "Gözlerin yoruldu mu?", "Biraz dinlen istersen.", "Ben kaçmaya devam ediyorum." },
        new string[] { "30. Seviye! İnanılmaz.", "Ama yolun yarısı bile değil.", "Devam!" },
        new string[] { "Format mı atsam ne yapsam?", "Beni silmeden durduramazsın!", "Kaçtım!" },
        new string[] { "Sanal alemde benden hızlısı yok.", "Kabullensen iyi edersin.", "Hıh!" },
        new string[] { "Tıklama sesleri buraya kadar geliyor.", "Hırslı seni!", "Tutamadın." },
        new string[] { "Sürpriz! Buradayım.", "Hayır, şuradayım!", "Hahaha!" },
        new string[] { "Ekran koruyucu gibi süzülüyorum.", "Çok estetiğim değil mi?", "Kaçtım!" },
        new string[] { "Pil durumuna baktın mı?", "Şarjın bitmeden beni tutamazsın.", "Devam!" },
        new string[] { "Bir profesyonel gibi oynuyorsun.", "Ama ben bir profesyonel kaçıcıyım!", "Bye!" },
        new string[] { "Arkana bak! Şaka şaka, ekrandayım.", "Ama başka yerde!", "Yakalayamadın." },
        new string[] { "Bunu arkadaşlarına gösteriyor musun?", "Rezil olacaksın, geçemiyorsun!", "Hehe." },
        new string[] { "40. Seviye! Çıldırmış olmalısın.", "Buraya kadar gelen ilk kişisin (belki de).", "Durmak yok!" },
        new string[] { "Kafamda deli sorular...", "Neden hala tıklıyorsun?", "Devam et bakalım." },
        new string[] { "Hızlı ve Öfkeli: Buton Efsanesi.", "Ben hızlıyım, sen öfkeli!", "Kaçtım!" },
        new string[] { "Aha buradayım... Tıkla hadi!", "Tıklayamadın ki!", "Hahaha!" },
        new string[] { "Reflekslerin fena değil.", "Ama benim algoritmam daha iyi!", "Hop!" },
        new string[] { "Bu seviye çok zor, söyleyeyim.", "Pes etmek için harika bir an.", "Bırak gitsin." },
        new string[] { "Göz kırpma!", "Kaçırırsın söyleyeyim.", "Bak, yine kaçtım!" },
        new string[] { "Parmağın kas yaptı mı?", "Skorboarda adını yazdıracağız bu gidişle.", "Tıkla!" },
        new string[] { "Işınlanma cihazım ısındı ama...", "Hala çalışıyor!", "Görüşürüz." },
        new string[] { "Bana dokunma!", "Alerjim var sana.", "Kaçtım ki!" },
        new string[] { "Yarı yoldayız: 50. SEVİYE!", "Büyük başarı. Ama dava bitmedi!", "Devam!" },
        new string[] { "Destan yazıyorsun.", "Şiir gibi kaçıyorum ben de.", "Tıkla hadi!" },
        new string[] { "Yerinde olsam çay molası verirdim.", "Ben buradayım, bir yere gitmem.", "(Kaçtı)" },
        new string[] { "Sana bir sır vereyim mi?", "Aslında tutması çok kolay.", "Ama yapamıyorsun! Hahaha!" },
        new string[] { "Sanal parmağın var sanki.", "Çok seri basıyorsun.", "Yine de yetmez!" },
        new string[] { "Gözlerim üzerinde.", "Senin de gözlerin bende olsun.", "Neredeyim?" },
        new string[] { "Hata 404: Buton Bulunamadı.", "Şaka şaka, buradayım!", "Tıkla!" },
        new string[] { "Beni yakalayana ödül varmış.", "Ödül ne mi? Bir sonraki seviye!", "Kaçtım!" },
        new string[] { "Saniyenin onda biri hızındayım.", "Sen hangi hızdasın?", "Yavaş kaldın!" },
        new string[] { "Parmak kasların gelişti.", "Oyun bitince bana teşekkür edersin.", "Hop!" },
        new string[] { "60. SEVİYE! İnanılmaz bir azim.", "Pes etmeyenlerde bugün sensin.", "Başlayalım!" },
        new string[] { "Matrix gibiyim değil mi?", "Mavi hapı mı seçtin kırmızı hapı mı?", "Ben kaçtım!" },
        new string[] { "Dokunmatik ekranın isyan edecek.", "Biraz yavaş bas!", "Kaçtım ki." },
        new string[] { "Sola kaçtım, sağa kaçtım.", "Ortadayım! Yok yok, yine kaçtım.", "Hahaha!" },
        new string[] { "Pes etme kelimesini biliyor musun?", "Sözlüğünden silmiş gibisin.", "Devam!" },
        new string[] { "Telefonu sallama, düşüreceksin!", "Sadece ekrana odaklan.", "Tıkla!" },
        new string[] { "Beni yakalamak bir sanattır.", "Sen henüz bir çıraksın!", "Yakala bakalım." },
        new string[] { "Bunu başaran ilk kişi sen olabilirsin.", "Ama daha çok var!", "Devam!" },
        new string[] { "Rüzgar gibi geçtim.", "Sesimi duydun mu?", "Kaçtım!" },
        new string[] { "Bunu oynarken zaman nasıl geçiyor?", "Farkında bile değilsin değil mi?", "Hehe!" },
        new string[] { "70. SEVİYE! Şapka çıkarıyorum.", "Ama ben pes etmem, sen de etme!", "Hadi!" },
        new string[] { "Son sürat devam!", "Frenlerim patladı!", "Duramıyorum!" },
        new string[] { "Aha tutuyordun!", "Kıl payı kaçırdın.", "Bir daha dene." },
        new string[] { "Sanki biraz yavaşladım mı?", "Yooo, kandırdım!", "Hızlıyım hala!" },
        new string[] { "Bu seviyeyi geçmek imkansız deniyordu.", "Bakalım geçebilecek misin?", "Tıkla!" },
        new string[] { "Odaklan... Nefes al...", "Ve... TIKLAYAMADIN!", "Hahaha!" },
        new string[] { "Büyücü gibiyim.", "Şimdi varım, şimdi yokum!", "Neredeyim?" },
        new string[] { "Tebrikler, buraya kadar gelmek büyük başarı.", "Ama hevesin kursağında kalacak!", "Kaçtım!" },
        new string[] { "Işıklar, kamera, aksiyon!", "Sahne benim, kaçıyorum!", "Bye!" },
        new string[] { "Beni tutarsan oyun biter mi acaba?", "Deneyip gör!", "Yakala!" },
        new string[] { "80. SEVİYE! Efsaneler arasındasın.", "Son 10 seviye! (Acaba?)", "Devam!" },
        new string[] { "Bütün gücünü topla.", "Son kozlarını oyna!", "Kaçtım!" },
        new string[] { "Parmağın ekrana yapıştı sanki.", "Çok hızlısın!", "Ama ben daha hızlıyım!" },
        new string[] { "Nefes kesici bir mücadele!", "Kazanan kim olacak?", "Tabii ki ben!" },
        new string[] { "Son virajlar...", "Dikkatli ol, kaza yapma!", "Hop!" },
        new string[] { "Artık kelimeler yetmiyor.", "Sadece hız konuşur!", "Tıkla!" },
        new string[] { "Beni tutman için mucize lazım.", "Mucizelere inanır mısın?", "Kaçtım!" },
        new string[] { "Neredeyse bitti...", "Yoksa yeni mi başlıyor?", "Hahaha!" },
        new string[] { "Sona çok yaklaştın!", "Heyecan var mı?", "Ben çok rahatım!" },
        new string[] { "Zirveye az kaldı!", "Tırmanmaya devam et!", "Son hamleler!" },
        new string[] { "90. SEVİYE! FİNALA GEÇTİN!", "Buraya kadar geldiğin için tebrikler!", "Şimdi GERÇEK ZORLUK başlıyor!" }
    };

    private string[] finalMessages =
    {
        "BURAYA KADAR MIYDI?", "Pes etme!", "Son adımlar...", "İnanılmazsın!", "Çok az kaldı!", "ŞAMPİYON OLMAMA AZ KALDI!", "BİTİYOR!", "SON BİR KAÇ TIK!", "VE...", "EFSANEVİ ZAFER!"
    };

    private void Start()
    {
        if (button == null)
        {
            Debug.LogError("Button reference missing on ButtonController!");
            return;
        }

        buttonRectTransform = button.GetComponent<RectTransform>();
        startingPosition = buttonRectTransform.anchoredPosition;

        Canvas parentCanvas = button.GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasRectTransform = parentCanvas.GetComponent<RectTransform>();
        }

        button.onClick.AddListener(MoveButton);

        if (buttonMessage != null)
        {
            buttonMessage.raycastTarget = false;
            buttonMessage.gameObject.SetActive(false);

            bubbleTransform = buttonMessage.transform.parent;
            if (bubbleTransform != null)
            {
                bubbleTransform.gameObject.SetActive(false);
                Image bubbleImage = bubbleTransform.GetComponent<Image>();
                if (bubbleImage != null)
                {
                    bubbleImage.raycastTarget = false;
                }
            }
        }

        UpdateClickCounterUI();
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(MoveButton);
        }
    }

    // GameManager Tarafından Çağrılan Eksik Metod
    public void ResetClicks()
    {
        clickCount = 0;
        clickLocked = false;
        UpdateClickCounterUI();

        if (buttonRectTransform != null)
        {
            buttonRectTransform.anchoredPosition = startingPosition;
        }
    }

    private void MoveButton()
    {
        if (!canPlay || gameManager == null || clickLocked) return;

        clickLocked = true;
        clickCount++;

        UpdateClickCounterUI();
        ShowLevelMessage();

        if (clickCount >= 10)
        {
            clickCount = 0;
            UpdateClickCounterUI();

            if (gameManager.currentLevel < 200)
            {
                gameManager.NextLevel();
            }

            if (gameManager.currentLevel >= 200)
            {
                if (movementCoroutine != null) StopCoroutine(movementCoroutine);
                movementCoroutine = StartCoroutine(FinalLevelBehavior());
                return;
            }
        }

        if (movementCoroutine != null) StopCoroutine(movementCoroutine);
        movementCoroutine = StartCoroutine(MoveWithLevelBehavior());
    }

    private void UpdateClickCounterUI()
    {
        if (clickCounter != null)
        {
            clickCounter.text = "DOKUNUŞ: " + clickCount;
        }
    }

    private void ShowLevelMessage()
    {
        if (buttonMessage == null) return;

        string currentMsg = "";

        if (gameManager.currentLevel >= 191)
        {
            int index = Mathf.Clamp(gameManager.currentLevel - 191, 0, finalMessages.Length - 1);
            currentMsg = finalMessages[index];
        }
        else
        {
            int levelIndex = Mathf.Clamp(gameManager.currentLevel - 1, 0, levels.Length - 1);
            string[] currentLevelMessages = levels[levelIndex];
            currentMsg = currentLevelMessages[Random.Range(0, currentLevelMessages.Length)];
        }

        buttonMessage.text = currentMsg;

        if (bubbleTransform != null)
            bubbleTransform.gameObject.SetActive(true);

        buttonMessage.gameObject.SetActive(true);

        if (messageCoroutine != null) StopCoroutine(messageCoroutine);
        messageCoroutine = StartCoroutine(HideMessageAfterDelay(1.2f));
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (buttonMessage != null)
            buttonMessage.gameObject.SetActive(false);

        if (bubbleTransform != null)
            bubbleTransform.gameObject.SetActive(false);
    }

    private IEnumerator MoveWithLevelBehavior()
    {
        int level = gameManager.currentLevel;

        if (level <= 10)
        {
            yield return StartCoroutine(MoveSmoothly(GetRandomCanvasPosition(), 0.18f));
        }
        else if (level <= 30)
        {
            yield return StartCoroutine(FakeMove());
            yield return StartCoroutine(MoveSmoothly(GetRandomCanvasPosition(), 0.15f));
        }
        else if (level <= 60)
        {
            int moveCount = Random.Range(2, 4);
            for (int i = 0; i < moveCount; i++)
            {
                yield return StartCoroutine(MoveSmoothly(GetRandomCanvasPosition(), 0.12f));
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if (level <= 90)
        {
            yield return StartCoroutine(ZigZagMove());
        }
        else if (level < 200)
        {
            int moveCount = Random.Range(3, 5);
            for (int i = 0; i < moveCount; i++)
            {
                yield return StartCoroutine(MoveSmoothly(GetRandomCanvasPosition(), 0.08f));
                yield return new WaitForSeconds(0.02f);
            }
        }

        clickLocked = false;
    }

    private IEnumerator MoveSmoothly(Vector2 targetPos, float duration)
    {
        if (buttonRectTransform == null) yield break;

        Vector2 startPos = buttonRectTransform.anchoredPosition;

        if (canvasRectTransform != null)
        {
            float minX = -canvasRectTransform.rect.width / 2 + buttonRectTransform.rect.width / 2;
            float maxX = canvasRectTransform.rect.width / 2 - buttonRectTransform.rect.width / 2;
            float minY = -canvasRectTransform.rect.height / 2 + buttonRectTransform.rect.height / 2;
            float maxY = canvasRectTransform.rect.height / 2 - buttonRectTransform.rect.height / 2;

            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            buttonRectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        buttonRectTransform.anchoredPosition = targetPos;
    }

    private IEnumerator FakeMove()
    {
        Vector2 fakePos = GetRandomCanvasPosition();
        yield return StartCoroutine(MoveSmoothly(fakePos, 0.08f));
        yield return new WaitForSeconds(0.05f);
    }

    private IEnumerator ZigZagMove()
    {
        Vector2 pos1 = GetRandomCanvasPosition();
        Vector2 pos2 = GetRandomCanvasPosition();

        yield return StartCoroutine(MoveSmoothly(pos1, 0.08f));
        yield return StartCoroutine(MoveSmoothly(pos2, 0.08f));
    }

    private IEnumerator FinalLevelBehavior()
    {
        clickLocked = true;

        if (buttonMessage != null)
        {
            buttonMessage.text = "TEBRİKLER! OYUNU BİTİRDİN!";
            if (bubbleTransform != null) bubbleTransform.gameObject.SetActive(true);
            buttonMessage.gameObject.SetActive(true);
        }

        yield return StartCoroutine(MoveSmoothly(startingPosition, 0.5f));

        // GameManager'da WinGame metodu yoksa derleme hatası vermesini engeller
        if (gameManager != null)
        {
            gameManager.SendMessage("WinGame", SendMessageOptions.DontRequireReceiver);
        }
    }

    private Vector2 GetRandomCanvasPosition()
    {
        if (canvasRectTransform == null || buttonRectTransform == null) return Vector2.zero;

        float width = canvasRectTransform.rect.width - buttonRectTransform.rect.width;
        float height = canvasRectTransform.rect.height - buttonRectTransform.rect.height;

        float randomX = Random.Range(-width / 2, width / 2);
        float randomY = Random.Range(-height / 2, height / 2);

        return new Vector2(randomX, randomY);
    }
}