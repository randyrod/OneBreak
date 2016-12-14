using OneBreak.ViewModels;
using System;

namespace OneBreak.Models
{
    public class RaceModel : ViewModelBase
    {
        private int _roundNumber;
        
        public int RoundNumber {
            get { return _roundNumber; }
            set
            {
                if (value == _roundNumber) return;
                _roundNumber = value;
                OnPropertyChanged();
            }
        }

        private string _roundName;
        public string RoundName
        {
            get
            {
                return _roundName;
            }
            set
            {
                if (value == _roundName) return;
                _roundName = value;
                OnPropertyChanged();
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                if (_location == value) return;
                _location = value;
                OnPropertyChanged();
            }
        }

        private string _country;
        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                if (value == _country) return;
                _country = value;
                OnPropertyChanged();
            }
        }

        private DateTime _practiceOneDate;
        public DateTime PracticeOneDate
        {
            get { return _practiceOneDate; }
            set
            {
                if (value == _practiceOneDate) return;
                _practiceOneDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _practiceTwoDate;
        public DateTime PracticeTwoDate
        {
            get { return _practiceTwoDate; }
            set
            {
                if (value == _practiceTwoDate) return;
                _practiceTwoDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _practiceThreeDate;
        public DateTime PracticeThreeDate
        {
            get { return _practiceThreeDate; }
            set
            {
                if (value == _practiceThreeDate) return;
                _practiceThreeDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _qualificationTime;
        public DateTime QualificationTime
        {
            get { return _qualificationTime; }
            set
            {
                if (value == _qualificationTime) return;
                _qualificationTime = value;
                OnPropertyChanged();
            }
        }

        private DateTime _raceTime;
        public DateTime RaceTime
        {
            get { return _raceTime; }
            set
            {
                if (value == _raceTime) return;
                _raceTime = value;
                OnPropertyChanged();
            }
        }

        private double _raceDistance;
        public double RaceDistance
        {
            get { return _raceDistance; }
            set
            {
                if (value == _raceDistance) return;
                _raceDistance = value;
                OnPropertyChanged();
            }
        }

        private double _circuitLength;
        public double CircuitLength
        {
            get { return _circuitLength; }
            set
            {
                if (value == _circuitLength) return;
                _circuitLength = value;
                OnPropertyChanged();
            }
        }

        private int _laps;
        public int Laps
        {
            get { return _laps; }
            set
            {
                if (value == _laps) return;
                _laps = value;
                OnPropertyChanged();
            }
        }

        private string _raceDescription;
        public string RaceDescription
        {
            get { return _raceDescription; }
            set
            {
                if (value == _raceDescription) return;
                _raceDescription = value;
                OnPropertyChanged();
            }
        }

        private string _raceLayoutSrc;
        public string RaceLayoutSource
        {
            get { return _raceLayoutSrc; }
            set
            {
                if (value == _raceLayoutSrc) return;
                _raceLayoutSrc = value;
                OnPropertyChanged();
            }
        }


    }
}
