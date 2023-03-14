using UnityEngine;

namespace MoneyStackSystem
{
    public class MoneyStack : MonoBehaviour
    {
        private int _stackAmount;

        public string StackId;

        public int StackAmount
        {
            get => _stackAmount;
            set
            {
                _stackAmount = value;
                SetStackAmount();
            }
        }

        private MoneyStackController _moneyStackController;

        private void Start()
        {
            _moneyStackController = GetComponent<MoneyStackController>();
            // if (Load(out MoneyStackData stackData))
            // {
            //     _stackAmount = stackData.StackAmount;
            //     _moneyStackController.Initialize(0, stackData.StackAmount / 50);
            // }
            // else
            // {
            //     _stackAmount = 0;
            //     PlayerData.Instance.MoneyStackData.Add(new MoneyStackData(StackId, StackAmount));
            //     _moneyStackController.Initialize(1.5f, 0);
            //
            // }

            _stackAmount = 0;
            _moneyStackController.Initialize(1.5f, 0);
        }

        public void ChangeStackAmount(int amount)
        {
            _stackAmount += amount;
            // Save();
        }

        public void IncreaseStackAmount()
        {
            _moneyStackController.StackCount++;
        }

        public void UpdateStackAmount()
        {
            _moneyStackController.StackCount = StackAmount / 50;
        }
        
        public void SetStackAmount()
        {
            _moneyStackController.StackCount = StackAmount / 50;
            // Save();
        }

        public Vector3 JumpPosition
        {
            get => _moneyStackController.GetJumpPosition();
        }

        // public void Save()
        // {
        //     var currentSaveData = PlayerData.Instance.MoneyStackData.FirstOrDefault(x => x.StackId == StackId);
        //     PlayerData.Instance.MoneyStackData.Remove(currentSaveData);
        //     PlayerData.Instance.MoneyStackData.Add(new MoneyStackData(StackId, StackAmount));
        // }

        // public bool Load(out MoneyStackData currentSaveData)
        // {
        //     currentSaveData = PlayerData.Instance.MoneyStackData.FirstOrDefault(x => x.StackId == StackId);
        //     bool dataLoaded = !currentSaveData.StackId.IsNullOrWhitespace();
        //     return dataLoaded;
        // }
    }
}
