using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DkbozkurtAStarPLY.Scripts
{
    public class GridSystem : MonoBehaviour
    {
        public static GridSystem Instance;

        public static event Action<List<Cell>> OnPathGenerated;
        public static event Action OnNoPathFound;

        [Header("Core Properties")]
        [SerializeField] private GameObject _cellPrefab;
        public int CellsPerRow = 10;
        public int CellsPerColumn = 10;
        public bool CanMoveDiagonal = false;

        [Space]
        public Cell StartCell;
        public Cell TargetCell;
        
        private List<Cell> _allCells;
        private List<Cell> _openList;
        private List<Cell> _closedList;

        public int NumberOfCells => CellsPerColumn * CellsPerRow;
        
        private bool _isCalculating = false;
        private LineRenderer _lineRenderer;
        
        private void Awake()
        {
            Instance = this;
            _lineRenderer = GetComponent<LineRenderer>();
        }
        private void Start()
        {
            CreateCells();
        }

        private void Update()
        {
            if(_isCalculating) return;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CalculatePathExternal();
            }
        }

        private void SetCellsExternalAndStartPath(Cell startCell, Cell targetCell)
        {
            if(_isCalculating) return;
            
            StartCell = startCell;
            TargetCell = targetCell;
            
            CalculatePathExternal();
        }

        #region Obstacle Movement By Mouse

        // private void DetectAndMoveObstacles()
        // {
        //     if (Input.GetMouseButton(0)) {
        //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //         RaycastHit hit;
        //     
        //         if (Physics.Raycast(ray, out hit, 100)) {
        //             if (otherCube) {
        //                 otherCube.GetComponent<Collider>().enabled = false;
        //                 otherCube.transform.position = FindClosestCell(hit.point) + new Vector3(0, 1, 0);
        //             }
        //         }
        //     } else if (Input.GetMouseButtonUp(0)) {
        //         if (otherCube) {
        //             otherCube.GetComponent<Collider>().enabled = true;
        //             otherCube = null;
        //     
        //             grid.CalculatePathExternal();
        //         }
        //     }
        // }
        
        // private Vector3 FindClosestCell(Vector3 startPosition) {
        //     Cell closest = null;
        //     float distance = Mathf.Infinity;
        //     Vector3 position = startPosition;
        //     foreach (Cell cell in _allCells) {
        //         Vector3 diff = cell.transform.position - position;
        //         float curDistance = diff.sqrMagnitude;
        //         if (curDistance < distance) {
        //             closest = cell;
        //             distance = curDistance;
        //         }
        //     }
        //     return closest.transform.position;
        // }

        #endregion
        
        private void CreateCells()
        {
            _allCells = new List<Cell>();

            int xOffset = 0;
            int zOffset = 0;

            int counter = 0;

            for (int i = 0; i < NumberOfCells; i++)
            {
                counter += 1;
                xOffset += 1;
                
                Cell newCell = Instantiate(_cellPrefab,
                    new Vector3(transform.position.x + xOffset, transform.position.y,
                        transform.position.z+zOffset),transform.rotation,transform).GetComponent<Cell>();
                newCell.Id = i;
                _allCells.Add(newCell);

                if (counter >= CellsPerRow)
                {
                    counter = 0;
                    xOffset = 0;
                    zOffset += 1;
                }
            }
        }

        public void CalculatePathExternal()
        {
            if(!PreCheckerForCellsBasedCalculatePath()) return;
            
            ResetAllCells();
            StartCoroutine(CalculatePath());
        }
        
        private bool PreCheckerForCellsBasedCalculatePath()
        {
            if (StartCell == null || TargetCell == null)
            {
                Debug.LogError("You have to assign both \"StartCell\" and \"TargetCell\" for Calculate Path from Cells!");
                return false;
            }
            else if (!TargetCell.IsValid)
            {
                Debug.LogError("\"TargetCell\" is not a valid cell to move!");
                return false;
            }
            else if (StartCell.Id == TargetCell.Id)
            {
                Debug.LogError("\"StartCell\" and \"TargetCell\" are can not be the same cell.");
                return false;
            }

            return true;
        }
        
        private IEnumerator CalculatePath()
        {
            _isCalculating = true;
            
            _openList = new List<Cell>();
            _closedList = new List<Cell>();

            Cell currentCell = StartCell;
            AddCellToClosedList(currentCell);

            float cycleDelay = 0.0f;
            int cycleCounter = 0;
            while (currentCell.Id != TargetCell.Id)
            {
                yield return new WaitForSeconds(cycleDelay);

                // Safety-abort in case of endless loop
                cycleCounter++;
                if (cycleCounter >= NumberOfCells)
                {
                    OnNoPathFound?.Invoke();
                    Debug.Log("No Path Found");
                    _isCalculating = false;
                    yield break;
                }
                
                // Add all cells adjacent to currentCell to openList
                foreach (Cell cell in GetAdjacentCells(currentCell)) {
                    float tentativeG = currentCell.G + Vector3.Distance(currentCell.transform.position, cell.transform.position);
                    //if cell is on closed list skip to next cycle
                    if (cell.OnClosedList && tentativeG > cell.G) {
                        continue;
                    }

                    if (!cell.OnOpenList || tentativeG < cell.G) {
                        cell.CalculateH(TargetCell);
                        cell.G = tentativeG;
                        cell.F = cell.G + cell.H;
                        cell.ParentCell = currentCell;

                        if (!cell.OnClosedList)
                            AddCellToOpenList(cell);
                    }
                }

                yield return new WaitForSeconds(cycleDelay);
                
                // Get cell with lowest F value from openList, set it to currentCell
                float lowestFValue = 99999.9f;
                foreach (Cell cell in _openList) {
                    if (cell.F < lowestFValue) {
                        lowestFValue = cell.F;
                        currentCell = cell;
                    }
                }

                // remove currentCell from openList, add to closedList
                _openList.Remove(currentCell);
                AddCellToClosedList(currentCell);
            }
            
            // Get Path
            List<Cell> path = new List<Cell>();
            currentCell = TargetCell;
            while (currentCell.Id != StartCell.Id)
            {
                path.Add(currentCell);
                currentCell = currentCell.ParentCell;
            }
            path.Add(currentCell);
            path.Reverse();

            // DrawPath
            _lineRenderer.SetVertexCount(path.Count);
            int i = 0;
            foreach (Cell cell in path)
            {
                _lineRenderer.SetPosition(i,cell.transform.position + new Vector3(0,1,0));
                i++;
            }
            
            OnPathGenerated?.Invoke(path);
            
            _isCalculating = false;
        }
        
        private List<Cell> GetAdjacentCells(Cell currentCell) {
            return currentCell.GetAdjacentCells(_allCells, CellsPerRow);
        }

        private void AddCellToClosedList(Cell currentCell) {
            _closedList.Add(currentCell);
            currentCell.SetToClosedList();
        }

        private void AddCellToOpenList(Cell currentCell) {
            _openList.Add(currentCell);
            currentCell.SetToOpenList();
        }

        private void CreateStart(int id)
        {
            StartCell = _allCells[id];
            StartCell.SetToStart();
        }

        private void CreateTarget(int id)
        {
            TargetCell = _allCells[id];
            TargetCell.SetToTarget();
        }
        
        private void ResetAllCells()
        {
            foreach (Cell cell in _allCells)
            {
                cell.Reset();
            }
        }

        
    }
}