using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float waitAtTargetTime = 1.5f;
    [SerializeField] private Transform startPos;

    [Header("Effects")]
    public GameObject niceEffect;
    public GameObject badEffect;

    private Transform target;
    private CircleRotator targetCircle;
    private bool isBusy = false;

    private int hunterHits = 0;          // Счётчик попаданий на Hunter
    private int clickCount = 0;          // Счётчик кликов для LevelRegenerate
    private bool pendingLevelRegenerate = false; // Флаг отложенной генерации уровня

    public MenuTravel menuTravel;

    public AudioSource niceShot;
    public AudioSource badShot;

    public CountManager countManager;

    public Animator animator;

    public RandomizeLevel randomize;

    private void OnEnable()
    {
        transform.position = startPos.position;
        hunterHits = 0;
        clickCount = 0;
        pendingLevelRegenerate = false;
        isBusy = false;
    }

    public void OnBackgroundClick(PointerEventData eventData)
    {
        if (isBusy)
            return;

        Vector2 localPoint;
        RectTransform rect = eventData.pointerPress.GetComponent<RectTransform>();
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out localPoint))
            return;

        Vector3 clickWorldPos = rect.TransformPoint(localPoint);
        clickWorldPos.z = 0f;

        FindNearestAnimal(clickWorldPos);

        // 🔹 Увеличиваем счётчик кликов
        clickCount++;
        if (clickCount % 2 == 0)
        {
            // Устанавливаем флаг, чтобы пересоздать уровень после возвращения курицы
            pendingLevelRegenerate = true;
        }
    }

    private void FindNearestAnimal(Vector3 clickPos)
    {
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
        if (animals.Length == 0) return;

        float minDistance = float.MaxValue;
        GameObject nearest = null;

        foreach (GameObject animal in animals)
        {
            float dist = Vector2.Distance(clickPos, animal.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = animal;
            }
        }

        if (nearest != null)
        {
            target = nearest.transform;
            targetCircle = nearest.GetComponentInParent<CircleRotator>();
            if (targetCircle != null)
                targetCircle.StopRotation();

            StartCoroutine(ChickenRoutine());
        }
    }

    private IEnumerator ChickenRoutine()
    {
        isBusy = true;

        // Летим к цели
        while (Vector2.Distance(transform.position, target.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Курица достигла цели — проверяем скрипт Hunter
        Hunter hunterScript = target.GetComponent<Hunter>();
        if (hunterScript != null)
        {
            Instantiate(badEffect, transform.position, Quaternion.identity, transform.parent);
            hunterHits++;
            animator.Play("MessageGo");
            badShot.Play();
            if (hunterHits % 3 == 0)
            {
                GameOver();
            }
        }
        else
        {
            niceShot.Play();
            countManager.AddPoints(5);
            Instantiate(niceEffect, transform.position, Quaternion.identity, transform.parent);
        }

        // Ждём у цели
        yield return new WaitForSeconds(waitAtTargetTime);

        // Возвращаемся
        while (Vector2.Distance(transform.position, startPos.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        if (targetCircle != null)
            targetCircle.ResumeRotation();

        target = null;
        targetCircle = null;
        isBusy = false;

        // 🔹 Проверяем флаг и вызываем генерацию уровня после возвращения
        if (pendingLevelRegenerate)
        {
            pendingLevelRegenerate = false;
            LevelRegenerate();
        }
    }

    // Метод GameOver
    private void GameOver()
    {
        Debug.Log("Game Over! Попаданий на Hunter: " + hunterHits);
        menuTravel.makeMenu(5);
    }

    // Метод для перегенерации уровня
    private void LevelRegenerate()
    {
        if (randomize != null)
        {
            randomize.RecreateLevel();
            Debug.Log("Level regenerated after 2 clicks!");
        }
    }
}
