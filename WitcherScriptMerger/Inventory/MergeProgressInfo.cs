using System.ComponentModel;

namespace WitcherScriptMerger.Inventory
{
    public class MergeProgressInfo : INotifyPropertyChanged
    {
        private string _currentAction;
        public string CurrentAction
        {
            get
            {
                return _currentAction;
            }
            set
            {
                _currentAction = value;
                OnPropertyChanged();
            }
        }

        private string _currentPhase;
        public string CurrentPhase
        {
            get
            {
                return _currentPhase;
            }
            set
            {
                _currentPhase = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, null);
        }
    }
}
