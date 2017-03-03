namespace OneBreak.Helpers
{
    public static class DeviceFamilyHelper
    {
        public static string CurrentDeviceFamily => Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
    }
}