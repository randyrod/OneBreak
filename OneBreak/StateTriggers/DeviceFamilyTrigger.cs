using Windows.UI.Xaml;

namespace OneBreak.StateTriggers
{
    public class DeviceFamilyTrigger : StateTriggerBase
    {
        private string _currentDeviceFamily, _queriedDeviceFamily;

        public string DeviceFamily
        {
            get { return _queriedDeviceFamily; }
            set
            {
                _queriedDeviceFamily = value;
                _currentDeviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;

                SetActive(_queriedDeviceFamily == _currentDeviceFamily);
            }
        }
    }
}
