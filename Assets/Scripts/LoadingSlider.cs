using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingSlider : MonoBehaviour
{
    [SerializeField] private Image loadingImage;
    [SerializeField] private float loadDuration = 3f;
    public MenuTravel travel;
    private void Start()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        float elapsed = 0f;
        loadingImage.fillAmount = 0f;

        while (elapsed < loadDuration)
        {
            elapsed += Time.deltaTime;
            loadingImage.fillAmount = elapsed / loadDuration;
            yield return null;
        }

        loadingImage.fillAmount = 1f;
        OnLoadComplete();
    }

    private void OnLoadComplete()
    {
        travel.makeMenu(1);
    }
}
