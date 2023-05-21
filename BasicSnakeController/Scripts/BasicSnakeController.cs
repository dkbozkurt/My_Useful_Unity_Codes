// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Mechanics.BasicSnakeController.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=iuz7aUHYC_E
    /// </summary>

    public class BasicSnakeController : MonoBehaviour
    {
        [SerializeField] private int _initialBodyLenght = 5;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _steerSpeed = 180f;
        [SerializeField] private float _bodySpeed = 5;
        [SerializeField] private int _gap = 20;

        [SerializeField] private GameObject _bodyPrefab;

        private List<GameObject> _bodyParts = new List<GameObject>();
        private List<Vector3> _positionsHistory = new List<Vector3>();

        private void Start()
        {
            for (int i = 0; i < _initialBodyLenght; i++)
            {
                GrowSnake();
            }
        }

        private void Update()
        {
            Movement();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                GrowSnake();
            }
        }

        private void Movement()
        {
            // move forward
            transform.position += transform.forward * _moveSpeed * Time.deltaTime;
            
            // steer
            float steerDirection = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up * steerDirection *_steerSpeed * Time.deltaTime);
            
            PositionHistoryRecorder();
        }

        private void PositionHistoryRecorder()
        {
            // store position history
            _positionsHistory.Insert(0,transform.position);
            
            // move body parts
            int index = 0;
            foreach (var body in _bodyParts)
            {
                Vector3 point = _positionsHistory[Mathf.Min(index * _gap, _positionsHistory.Count - 1)];
                Vector3 moveDirection = point - body.transform.position;
                body.transform.position += moveDirection * _bodySpeed * Time.deltaTime;
                body.transform.LookAt(point);
                index++;
            }

        }

        public void GrowSnake()
        {
            GameObject body = Instantiate(_bodyPrefab);
            _bodyParts.Add(body);
        }
    }
}
