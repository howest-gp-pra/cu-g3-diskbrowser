using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Diskbrowser.Wpf
{
    public class ListFile
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime Date { get; set; }
        public ListFile(string name, long size, DateTime date)
        {
            Name = name;
            Size = size;
            Date = date;
        }
    }
}
