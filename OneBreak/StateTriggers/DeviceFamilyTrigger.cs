using OneBreak.Helpers;
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
                SetActive(_queriedDeviceFamily == DeviceFamilyHelper.CurrentDeviceFamily);
            }
        }
    }
}
