using System.ComponentModel;

namespace WitcherScriptMerger.Inventory
{
    public class MergeProgressInfo : INotifyPropertyChanged
    {
        string _currentAction;
        public string CurrentAction
        {
            get { return _currentAction; }
            set { Set(ref _currentAction, value); }
        }

        string _currentPhase;
        public string CurrentPhase
        {
            get { return _currentPhase; }
            set { Set(ref _currentPhase, value); }
        }

        int _currentMergeNum;
        public int CurrentMergeNum
        {
            get { return _currentMergeNum; }
            set
            {
                _currentMergeNum = value;
                UpdatePhase();
            }
        }

        int _totalMergeCount;
        public int TotalMergeCount
        {
            get { return _totalMergeCount; }
            set
            {
                _totalMergeCount = value;
                UpdatePhase();
            }
        }

        int _currentFileNum;
        public int CurrentFileNum
        {
            get { return _currentFileNum; }
            set
            {
                _currentFileNum = value;
                UpdatePhase();
            }
        }

        string _currentFileName;
        public string CurrentFileName
        {
            get { return _currentFileName; }
            set
            {
                _currentFileName = value;
                UpdatePhase();
            }
        }

        int _totalFileCount;
        public int TotalFileCount
        {
            get { return _totalFileCount; }
            set
            {
                _totalFileCount = value;
                UpdatePhase();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, null);
        }

        void Set<T>(ref T property, T value)
        {
            property = value;
            OnPropertyChanged();
        }

        void UpdatePhase()
        {
            CurrentPhase =
                "Resolving mod conflict" +
                (
                    TotalMergeCount > 1
                    ? $" {CurrentMergeNum} of {TotalMergeCount}" : ""
                ) +
                "\nFile" +
                (
                    TotalFileCount > 1 && TotalFileCount != TotalMergeCount
                    ? $" {CurrentFileNum} of {TotalFileCount}" : ""
                ) +
                $": {CurrentFileName}";
        }
    }
}
