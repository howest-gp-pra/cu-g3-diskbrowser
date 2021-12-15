using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Pra.Diskbrowser.Wpf
{
    public class Disk 
    {
        public DriveInfo DriveInfo { get; set; }
        public Disk(DriveInfo driveInfo)
        {
            DriveInfo = driveInfo;
        }
        public override string ToString()
        {            
            return $"{DriveInfo.Name} - {DriveInfo.VolumeLabel} - {DriveInfo.TotalSize/1000000000}GB";
        }
    }
}
