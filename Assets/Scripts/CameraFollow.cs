using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    private Transform target; // Ссылка на трансформ игрока
    public float smoothing = 5f; // Коэффициент сглаживания

    Vector3 offset; // Расстояние между камерой и игроком
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        offset = transform.position - target.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;

        // Плавно перемещаем камеру к новой позиции с помощью коэффициента сглаживания
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    void Update()
    {
        
    }
}
