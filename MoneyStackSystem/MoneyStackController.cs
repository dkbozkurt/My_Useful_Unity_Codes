using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoneyStackSystem
{
    [Serializable]
    public struct MoneyStackData
    {
        public MoneyStackData(string stackId, int stackAmount)
        {
            StackId = stackId;
            StackAmount = stackAmount;
        }

        public string StackId;
        public int StackAmount;
    }

    [Serializable]
    public class GridObject
    {
        public bool Enabled;
        public Vector3 Position;
        public Vector3 Rotation;
        public float EnabledScale;

        public Vector3 Scale
        {
            get { return Enabled ? Vector3.one * EnabledScale : Vector3.zero; }
        }

        public Matrix4x4 TRS
        {
            get { return Matrix4x4.TRS(Position, Quaternion.Euler(Rotation), Scale); }
        }
    }
    
    public class MoneyStackController : MonoBehaviour
    {
        [Header("Stack Size")] 
        [SerializeField] private int _stackSizeX;
        [SerializeField] private int _stackSizeY;
        [SerializeField] private int _stackSizeZ;

        [SerializeField] private float _scale = 0.1f;
        [SerializeField] private bool _scramble;
        
        [SerializeField] private MoneyStack _moneyStack;
        
        [Header("Offset Settings")]
        [SerializeField] private float _xOffset;
        [SerializeField] private float _zOffset;
        [SerializeField] private float _yOffset;
        
        [Header("Stack Object Settings")]
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;
        [SerializeField] private Transform _initialTransform;

        [Header("VFX")] [SerializeField] private ParticleSystem _moneyCollectParticle;
        
        private int _stackCount;

        public int StackCount
        {
            get => _stackCount;
            set
            {
                if (value < 0)
                {
                    _stackCount = 0;
                    UpdateGridElement(_stackCount);
                    return;
                }
                if (value >= 1023)
                {
                    _stackCount = 1023;
                    UpdateGridElement(_stackCount);
                    return;
                }
                if (value > _stackSizeX * _stackSizeY * _stackSizeZ)
                {
                    _stackCount = _stackSizeX * _stackSizeY * _stackSizeZ;
                    UpdateGridElement(_stackCount);
                    return;
                }
                
                _stackCount = value;
                UpdateGridElement(_stackCount); 

            }
        }
        
        private List<Matrix4x4> TRSList = new List<Matrix4x4>();
        private List<GridObject> _gridObjects = new List<GridObject>();
        
        private GridObject[,,] matrix;

        private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
        
        private void CreateStack()
        {
            matrix = new GridObject[_stackSizeX,_stackSizeY,_stackSizeZ];
            TRSList = new List<Matrix4x4>();
            _gridObjects = new List<GridObject>();
            
            for (int i = 0; i < matrix.GetLength(2); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix.GetLength(0); k++)
                    {
                        var localRotation = transform.localRotation;
                        matrix[k, j, i] = new GridObject()
                        {
                            Position = _initialTransform.position +
                                       localRotation * new Vector3(_xOffset * k, _yOffset * j, _zOffset * i) + 
                                       (_scramble ? new Vector3(Random.Range(-_xOffset/8, _xOffset/8), Random.Range(-_yOffset/8, _yOffset/8),Random.Range(-_zOffset/8, _zOffset/8)) : Vector3.zero),
                            Enabled = false,
                            EnabledScale = _scale,
                            Rotation = localRotation.eulerAngles
                        };
                        _gridObjects.Add(matrix[k,j,i]);
                    }
                }
            }

            TRSList = _gridObjects.Where(x => x.Enabled).ToList().ConvertAll(x => x.TRS);
        }

        public void Initialize(float delay, int count)
        {
            DoAfterGivenTime(delay, (() =>
            {
                CreateStack();
                ChangeGridElementStateInstant(count);
            }));
        }

        private Vector3Int GetMatrixValue(int count)
        {
            var indexX = 0;
            var indexY = 0;
            
            var a = count % (matrix.GetLength(0) * matrix.GetLength(2));
            var indexZ = count / (matrix.GetLength(0) * matrix.GetLength(2));
            if (count == 0)
            {
                return new Vector3Int(0,0,0);
            }
            if (a == 0)
            {
                return new Vector3Int(matrix.GetLength(0) - 1,indexZ - 1, matrix.GetLength(2) - 1);
            }
            else
            {
                indexX = a % (matrix.GetLength(0));
                if (indexX == 0){
                    indexX = matrix.GetLength(0) - 1;
                    indexY = a / (matrix.GetLength(0)) - 1;
                    return new Vector3Int(indexX, indexZ,indexY);;
                }
                else
                {
                    indexX -= 1;
                    indexY = a / (matrix.GetLength(0));

                }
            }

            return new Vector3Int(indexX, indexZ, indexY);
        }
        
        private void UpdateGridElement(int count)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeGridElementState(count));
            
        }
        
        public void ChangeGridElementStateInstant(int count)
        {
            if (count == TRSList.Count) return;
            
            if (count < 0)
            {
                _stackCount = 0;
            }
            else if (count >= 1022)
            {
                _stackCount = 1022;
            }
            else if (count > _stackSizeX * _stackSizeY * _stackSizeZ)
            {
                _stackCount = _stackSizeX * _stackSizeY * _stackSizeZ;
            }
            else
            {
                _stackCount = count;
            }
            
            if (_stackCount < TRSList.Count)
            {
                for (int i = TRSList.Count - 1; i > _stackCount - 1; i--)
                {
                    var value = GetMatrixValue(i + 1);
                    var gO = matrix[value.x, value.y, value.z];
                    var trs = TRSList.FirstOrDefault(x => x == gO.TRS);
                    gO.Enabled = false;
                    TRSList.Remove(trs);
                }
            }
            else
            {
                for (int i = TRSList.Count; i < _stackCount; i++)
                {
                    var value = GetMatrixValue(i + 1);
                    var gO = matrix[value.x, value.y, value.z];
                    gO.Enabled = true;
                    TRSList.Add(gO.TRS);
                }
            }

        }

        private IEnumerator ChangeGridElementState(int count)
        {
            if (count == TRSList.Count) yield break;
            if (count < TRSList.Count)
            {
                for (int i = TRSList.Count - 1; i > count - 1; i--)
                {
                    var value = GetMatrixValue(i + 1);
                    var gO = matrix[value.x, value.y, value.z];
                    var trs = TRSList.FirstOrDefault(x => x == gO.TRS);
                    gO.Enabled = false;
                    TRSList.Remove(trs);
                    yield return _waitForEndOfFrame;
                }
            }
            else
            {
                for (int i = TRSList.Count; i < count; i++)
                {
                    var value = GetMatrixValue(i + 1);
                    var gO = matrix[value.x, value.y, value.z];
                    gO.Enabled = true;
                    TRSList.Add(gO.TRS);
                    yield return _waitForEndOfFrame;
                }
            }
            yield return null;
        }

        private void Update()
        {
            if (matrix == null) return;
            
            foreach (var gridObject in matrix)
            {
                Graphics.DrawMesh(_mesh, Matrix4x4.TRS(gridObject.Position, Quaternion.Euler(gridObject.Rotation), gridObject.Scale), _material, 0);
            }
            
            //Graphics.DrawMeshInstanced(_mesh, 0, _material, TRSList);
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMoneyCollectController playerMoneyCollectController) && _stackCount != 0)
            {
                CollectMoney();
            }
        }

        private void CollectMoney()
        {
            _moneyCollectParticle.Play();
            var moneyAmount = _moneyStack.StackAmount;
            ChangeGridElementStateInstant(0);
            // PlayerData.Instance.CurrencyAmount += moneyAmount;
            PlayerMoneyCollectController.Instance.CurrencyAmount += moneyAmount;
            
            _moneyStack.StackAmount = 0;
        }

        public Vector3 GetJumpPosition()
        {
            if(StackCount >= _gridObjects.Count)
                return _gridObjects[_gridObjects.Count - 1].Position;
            else
                return _gridObjects[StackCount].Position;
        }

        private void DoAfterGivenTime(float delay,Action action = null)
        {
            StartCoroutine(Do());
            
            IEnumerator Do()
            {
                yield return new WaitForSeconds(delay);
                action?.Invoke();
            }
        }
        

    }
}
