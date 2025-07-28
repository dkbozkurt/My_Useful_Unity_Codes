using System;
using UnityEngine;

namespace DkbozkurtAStarPLY.Scripts
{
    public class InputController : MonoBehaviour
    {
        public static event Action<Cell> OnStartCellSelected;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DetectStartCell();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                DetectTargetCell();
            }
        }
        
        
        private void DetectStartCell()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.TryGetComponent(out Cell cell))
                {
                    if(!cell.IsValid) return;
                    OnStartCellSelected?.Invoke(cell);
                    cell.SetToStart();
                }
            }
        }
        
        private void DetectTargetCell()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.TryGetComponent(out Cell cell))
                {
                    if(!cell.IsValid) return;
                    cell.SetToTarget();
                }
            }
        }
    }
}
