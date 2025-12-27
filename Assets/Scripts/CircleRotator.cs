using UnityEngine;

public class CircleRotator : MonoBehaviour
{
    [SerializeField] private Transform[] objects;        // объекты, которые вращаются
    [SerializeField] private float radius = 5f;
    [SerializeField] private float speed = 1f;           // радианы в секунду
    [SerializeField] private float yOffset = 0f;         // дополнительное смещение центра по Y

    private float[] startAngles;
    private float rotationTime = 0f;
    private bool isRotating = true;
    private bool anglesInitialized = false;

    private void Update()
    {
        if (objects == null || objects.Length == 0) return;

        // Ленивая инициализация углов (только один раз, когда объекты уже активны)
        if (!anglesInitialized && isRotating)
        {
            InitializeStartAngles();
            anglesInitialized = true;
        }

        if (isRotating)
            rotationTime += Time.deltaTime * speed;

        // Центр — всегда текущая позиция родителя + смещение по Y
        Vector3 currentCenter = transform.position + Vector3.up * yOffset;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] == null) continue;

            float angle = startAngles[i] + rotationTime;
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );

            objects[i].position = currentCenter + offset;
        }
    }

    private void InitializeStartAngles()
    {
        startAngles = new float[objects.Length];

        // Берем центр на момент первой инициализации (когда объекты уже активны)
        Vector3 initCenter = transform.position + Vector3.up * yOffset;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] == null) continue;

            Vector3 direction = objects[i].position - initCenter;
            startAngles[i] = Mathf.Atan2(direction.y, direction.x);
        }
    }

    public void StopRotation()
    {
        isRotating = false;
    }

    public void ResumeRotation()
    {
        isRotating = true;

        // Если объекты появились позже и углы ещё не инициализированы
        if (!anglesInitialized)
        {
            InitializeStartAngles();
            anglesInitialized = true;
        }
    }

    // Полезно для ресета/рестарта
    public void Reset()
    {
        rotationTime = 0f;
        anglesInitialized = false;
    }

    private void OnEnable()
    {
        InitializeStartAngles();
    }
}