using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
