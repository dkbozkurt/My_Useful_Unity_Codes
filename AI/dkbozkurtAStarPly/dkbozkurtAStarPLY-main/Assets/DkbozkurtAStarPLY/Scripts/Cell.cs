using System.Collections.Generic;
using UnityEngine;

namespace DkbozkurtAStarPLY.Scripts
{
    public class Cell : MonoBehaviour, IObstacle
    {
        public int Id = 0;
        public bool IsValid { get; set; }

        [SerializeField] private List<Cell> _blockedCells;
        
        [SerializeField] private Material _materialValid;
        [SerializeField] private Material _materialInValid;
        [SerializeField] private Material _materialStart;
        [SerializeField] private Material _materialTarget;
        [SerializeField] private Material _materialOnClosedList;
        [SerializeField] private Material _materialOnOpenList;
        
        [HideInInspector] public Cell ParentCell;
        
        [HideInInspector] public bool OnOpenList;
        [HideInInspector] public bool OnClosedList;

        [HideInInspector] public float F= 0f;
        [HideInInspector] public float G= 0f;
        [HideInInspector] public float H= 0f;

        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            IsValid = true;
        }

        private void OnDestroy()
        {
            _meshRenderer.material = _materialValid;
        }

        public void Reset()
        {
            ParentCell = null;
            
            F = 0.0f;
            G = 0.0f;
            H = 0.0f;

            OnOpenList = false;
            OnClosedList = false;
            
            _meshRenderer.material = IsValid ? _materialValid : _materialInValid;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out IObstacle obstacle))
            {
                if(this.IsValid == obstacle.IsValid) return;
                
                this.IsValid = obstacle.IsValid;
                _meshRenderer.material = IsValid ? _materialValid : _materialInValid;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IObstacle obstacle))
            {
                this.IsValid = true;
                _meshRenderer.material = _materialValid;
            }
        }

        public void SetToStart()
        {
            ClearCell(GridSystem.Instance.StartCell);
            GridSystem.Instance.StartCell = this;
            _meshRenderer.material = _materialStart;
        }

        public void SetToTarget()
        {
            ClearCell(GridSystem.Instance.TargetCell);
            GridSystem.Instance.TargetCell = this;
            _meshRenderer.material = _materialTarget;
        }
        
        public void SetToClosedList()
        {
            OnClosedList = true;
            OnOpenList = false;
            _meshRenderer.material = _materialOnClosedList;
        }

        public void SetToOpenList()
        {
            OnOpenList = true;
            _meshRenderer.material = _materialOnOpenList;
        }

        public void ClearCell(Cell cellToPreCheck)
        {
            if (cellToPreCheck != null)
            {
                cellToPreCheck._meshRenderer.material = cellToPreCheck.IsValid ? _materialValid : _materialInValid;
            }
        }

        public List<Cell> GetAdjacentCells(List<Cell> allCells,int cellsPerRow)
        {
            List<Cell> adjacentCells = new List<Cell>();

            Cell neighbourUpper = null;
            Cell neighbourRight = null;
            Cell neighbourLower = null;
            Cell neighbourLeft = null;

            Cell neighbourUpperLeft = null;
            Cell neighbourUpperRight = null;
            Cell neighbourLowerLeft = null;
            Cell neighbourLowerRight = null;
            
            #region Check each Neighbour

            // Check each neighbour
            if (GridSystem.Instance.CanMoveDiagonal &&
                (Id % cellsPerRow != 0 && IsInBounds(Id + cellsPerRow - 1, allCells)))
                neighbourUpperLeft = allCells[Id + cellsPerRow - 1];
            
            if (IsInBounds(Id + cellsPerRow, allCells))
                neighbourUpper = allCells[Id + cellsPerRow];

            if (GridSystem.Instance.CanMoveDiagonal &&
                ((Id + 1) % cellsPerRow != 0 && IsInBounds(Id + cellsPerRow + 1, allCells)))
                neighbourUpperRight = allCells[Id + cellsPerRow + 1];
            
            if ((Id + 1) % cellsPerRow != 0)
                neighbourRight = allCells[Id + 1];
            
            if (Id % cellsPerRow != 0)
                neighbourLeft = allCells[Id - 1];

            if (GridSystem.Instance.CanMoveDiagonal && (Id % cellsPerRow != 0 && IsInBounds(Id - cellsPerRow - 1, allCells)))
                neighbourLowerLeft = allCells[Id - cellsPerRow - 1];
            
            if (IsInBounds(Id - cellsPerRow, allCells))
                neighbourLower = allCells[Id - cellsPerRow];
            
            if (GridSystem.Instance.CanMoveDiagonal && ((Id + 1) % cellsPerRow != 0 && IsInBounds(Id - cellsPerRow + 1, allCells)))
                neighbourLowerRight = allCells[Id - cellsPerRow + 1];
            
            // If neighbour exists and is valid, add to neighbour-list
            if (IsCellValid(neighbourRight) && CheckIfNeighbourCellIsBlocked(neighbourRight) == false)
                adjacentCells.Add(neighbourRight);

            if (IsCellValid(neighbourLeft) && CheckIfNeighbourCellIsBlocked(neighbourLeft) == false)
                adjacentCells.Add(neighbourLeft);

            if (GridSystem.Instance.CanMoveDiagonal && (IsCellValid(neighbourUpperRight) &&
                CheckIfNeighbourCellIsBlocked(neighbourUpperRight) == false))
            {
                adjacentCells.Add(neighbourUpperRight);   
            }
            
            if (IsCellValid(neighbourUpper) && CheckIfNeighbourCellIsBlocked(neighbourUpper) == false)
                adjacentCells.Add(neighbourUpper);
            
            if (GridSystem.Instance.CanMoveDiagonal && (IsCellValid(neighbourUpperLeft) &&
                                                        CheckIfNeighbourCellIsBlocked(neighbourUpperLeft) == false))
            {
                adjacentCells.Add(neighbourUpperLeft);   
            }
            
            if (GridSystem.Instance.CanMoveDiagonal && (IsCellValid(neighbourLowerRight) &&
                                                        CheckIfNeighbourCellIsBlocked(neighbourLowerRight) == false))
            {
                adjacentCells.Add(neighbourLowerRight);   
            }
            
            if (IsCellValid(neighbourLower) && CheckIfNeighbourCellIsBlocked(neighbourLower) == false)
                adjacentCells.Add(neighbourLower);
            
            if (GridSystem.Instance.CanMoveDiagonal && (IsCellValid(neighbourLowerLeft) &&
                                                        CheckIfNeighbourCellIsBlocked(neighbourLowerLeft) == false))
            {
                adjacentCells.Add(neighbourLowerLeft);   
            }
            
            #endregion

            return adjacentCells;
        }
        
        public void CalculateH(Cell targetCell) {
            H = Mathf.Abs(targetCell.transform.position.x - transform.position.x) 
                + Mathf.Abs(targetCell.transform.position.z - transform.position.z);
        }

        private bool IsCellInvalid (Cell inputCell)
        {
            return (inputCell != null && !inputCell.IsValid);
        }

        private bool IsCellValid (Cell inputCell)
        {
            return (inputCell != null && inputCell.IsValid);
        }

        private bool IsInBounds(int i, List<Cell> cells)
        {
            return (i >= 0 && i < cells.Count);
        }

        private bool CheckIfNeighbourCellIsBlocked(Cell targetCell)
        {
            if (_blockedCells.Count <= 0) return false;

            if (_blockedCells.Contains(targetCell)) return true;

            return false;
        }
    }
}
