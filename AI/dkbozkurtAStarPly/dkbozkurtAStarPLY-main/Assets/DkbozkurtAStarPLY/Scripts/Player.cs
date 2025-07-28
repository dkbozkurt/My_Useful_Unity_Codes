using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DkbozkurtAStarPLY.Scripts
{
    [RequireComponent(typeof(Collider),typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public static event Action OnNoPathFound;
        public static event Action OnPathCompletedByPassenger; 
        public Cell CurrentCell;

        [SerializeField] private float _passengerMovementSpeed = 2f;

        // private Animator _animator;

        private void Awake()
        {
            // _animator = GetComponentInChildren<Animator>();
        }
        
        private void OnEnable()
        {
            GridSystem.OnPathGenerated += Move;
            GridSystem.OnNoPathFound += TriggerNoPathCondition;
            InputController.OnStartCellSelected += SetStartLocation;
        }

        private void OnDisable()
        {
            GridSystem.OnPathGenerated -= Move;
            GridSystem.OnNoPathFound -= TriggerNoPathCondition;
            InputController.OnStartCellSelected -= SetStartLocation;
        }

        private void SetStartLocation(Cell cell)
        {
            if (CurrentCell != null) CurrentCell.IsValid = true;
            transform.position = cell.transform.position;
            CurrentCell = cell;
            CurrentCell.IsValid = false;
        }
        
        public void Move(List <Cell> cells)
        {
            var destinationCellToSet = cells[cells.Count - 1];
            // BoolAnimSetter("Walk",true);

            CurrentCell.IsValid = true;
            transform.DOPath(ConvertToArray(cells), _passengerMovementSpeed).SetSpeedBased().SetLookAt(0.01f)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    // BoolAnimSetter("Walk",false);
                    
                    CurrentCell = destinationCellToSet;
                    CurrentCell.IsValid = false;
                    destinationCellToSet.SetToStart();
                    GridSystem.Instance.TargetCell = null;
                    OnPathCompletedByPassenger?.Invoke();
                });
        }

        public void TriggerNoPathCondition()
        {
            OnNoPathFound?.Invoke();
            CurrentCell.IsValid = false;
        }
        
        private Vector3[] ConvertToArray(List<Cell> cells)
        {
            Vector3[] walkingPath = new Vector3[cells.Count];

            for (int i = 0; i < cells.Count; i++)
            {
                walkingPath[i] = cells[i].transform.position;
            }

            return walkingPath;  
        }

        // private void BoolAnimSetter(string animName, bool status)
        // {
        //     _animator.SetBool(animName,status);
        // }
        
        private void DebugPath(List<Cell> cells)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                Debug.Log(cells[i].gameObject.name);   
            }
        }
    }
}
