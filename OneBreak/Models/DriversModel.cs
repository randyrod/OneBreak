using System;
using OneBreak.ViewModels;

namespace OneBreak.Models
{
    public class DriversModel : ViewModelBase
    {
        private int _driversNumber;
        public int DriversNumber
        {
            get { return _driversNumber; }
            set
            {
                if (_driversNumber == value) return;
                _driversNumber = value;
                OnPropertyChanged();
            }
        }

        private string _driversName;
        public string DriversName
        {
            get { return _driversName; }
            set
            {
                if (_driversName == value) return;
                _driversName = value;
                OnPropertyChanged();
            }
        }

        private string _driversLastName;
        public string DriversLastName
        {
            get { return _driversLastName; }
            set
            {
                if (_driversLastName == value) return;
                _driversLastName = value;
                OnPropertyChanged();
            }
        }

        private string _debutRace;
        public string DebutRace
        {
            get { return _debutRace; }
            set
            {
                if (_debutRace == value) return;
                _debutRace = value;
                OnPropertyChanged();
            }
        }

        private string _driversBio;
        public string DriversBiography
        {
            get { return _driversBio; }
            set
            {
                if (_driversBio == value) return;
                _driversBio = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                if (_dateOfBirth == value) return;
                _dateOfBirth = value;
                OnPropertyChanged();
            }
        }

        private int _highestGridPosition;
        public int HighestGridPosition
        {
            get { return _highestGridPosition; }
            set
            {
                if (_highestGridPosition == value) return;
                _highestGridPosition = value;
                OnPropertyChanged();
            }
        }

        private int _highestFinishPosition;
        public int HighestFinishPosition
        {
            get { return _highestFinishPosition; }
            set
            {
                if (_highestFinishPosition == value) return;
                _highestFinishPosition = value;
                OnPropertyChanged();
            }
        }

        private string _firstWin;
        public string FirstWin
        {
            get { return _firstWin; }
            set
            {
                if (_firstWin == value) return;
                _firstWin = value;
                OnPropertyChanged();
            }
        }

        private string _country;
        public string Country
        {
            get { return _country; }
            set
            {
                if (_country == value) return;
                _country = value;
                OnPropertyChanged();
            }
        }

        private string _placeOfBirth;
        public string PlaceOfBirth
        {
            get { return _placeOfBirth; }
            set
            {
                if (_placeOfBirth == value) return;
                _placeOfBirth = value;
                OnPropertyChanged();
            }
        }

        private string _team;
        public string Team
        {
            get { return _team; }
            set
            {
                if (_team == value) return;
                _team = value;
                OnPropertyChanged();
            }
        }

        private string _teamMateName;
        public string TeammateName
        {
            get { return _teamMateName; }
            set
            {
                if (_teamMateName == value) return;
                _teamMateName = value;
                OnPropertyChanged();
            }
        }

        private int _championships;
        public int Championships
        {
            get { return _championships; }
            set
            {
                if (_championships == value) return;
                _championships = value;
                OnPropertyChanged();
            }
        }

        private string _imageSrc;
        public string ImageSource
        {
            get { return _imageSrc; }
            set
            {
                if (_imageSrc == value) return;
                _imageSrc = value;
                OnPropertyChanged();
            }
        }

    }
}
