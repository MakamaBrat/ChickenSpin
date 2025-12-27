using UnityEngine;
using TMPro;

public class CountManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;

    private void OnEnable()
    {
        // Обнуляем очки при включении
        score = 0;
        UpdateScoreUI();
    }

    /// <summary>
    /// Добавляет очки
    /// </summary>
    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreUI();
        SaveScore();
    }

    /// <summary>
    /// Обновляет текст в UI
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    /// <summary>
    /// Сохраняем очки в PlayerPrefs
    /// </summary>
    private void SaveScore()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Можно получить текущее значение очков
    /// </summary>
    public int GetScore()
    {
        return score;
    }
}
