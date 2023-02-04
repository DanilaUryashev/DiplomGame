using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootMover : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 NewTarget { get; set; }


    [SerializeField] private Transform _targetPoint;
    [SerializeField] private float _distance;
    [SerializeField] private float _maxHeightDistance;


    [SerializeField] private float _countLerpPosition = 0.4f;
    [SerializeField] private float _countLerpHeight = 0.5f;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _amplutide = 0.4f;


    private float currentTime = 1f;

    private void Start()
    {
        NewTarget = _targetPoint.position;
    }

    private void Update()
    {
        RaycastHit hit; // Переменная, в которой храним информацию о попадании луча.

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {

            if ((Vector3.Distance(hit.point, _targetPoint.position) > _distance)) // Проверяем расстояние между попаданием рейкаста и текущим таргетом.
            {

                currentTime = 0;
                NewTarget = hit.point; // Задаем в свойство NewTarget  координаты новой точки (Это нужно будет нам далее)
            }
            if (currentTime < 1)
            {

                Vector3 footPosition = Vector3.Lerp(_targetPoint.position, hit.point, _countLerpPosition); // С помощью линейной интерполяции плавненько переходим из текущей точки, в новую. 
                footPosition.y = Mathf.Lerp(footPosition.y, hit.point.y, _countLerpHeight) + (Mathf.Sin(currentTime * Mathf.PI) * _amplutide); // С помощью линейной интерполяции и синуса задаем новую высоту лапки. 
                _targetPoint.position = footPosition; // Меняем позицию таргета на новую. 
                currentTime += Time.deltaTime * _speed;
            }

        }



    }
}
