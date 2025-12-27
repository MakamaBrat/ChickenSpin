using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeLevel : MonoBehaviour
{
    public Transform[] levels;
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(LongRecreate());
    }
    public void RecreateLevel()
    {
        var k = FindAnyObjectByType<CircleRotator>();
        if (k != null)
        { Destroy(k.gameObject); }
        int d=Random.Range(0, levels.Length);
        Instantiate(levels[d], new Vector3(transform.position.x,transform.position.y+13,transform.position.z),Quaternion.identity,transform);
    }

    IEnumerator LongRecreate()
    {
        yield return new WaitForSeconds(0.01f);
        RecreateLevel();
    }
}
