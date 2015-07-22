using System;

namespace Moen.SystemInterop
{
    public class VolumeChangedEventArgs : EventArgs
    {
        public bool IsMute { get; set; }
        public int Volume { get; set; }
    }
}
