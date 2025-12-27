using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableBackground : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    public ChickenController chicken; // ссылка на курицу

    public void OnPointerClick(PointerEventData eventData)
    {
        if (chicken != null)
        {
            chicken.OnBackgroundClick(eventData);
        }
    }
}
