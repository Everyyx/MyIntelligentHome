using MyIntelligentHomeSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyIntelligentHomeSystem.ViewModels
{
    public class TimeViewModel
    {
        private string _now;
        public string Now
        {
            get { return _now; }
            set
            {
                _now = value;
                TimeRaisePropertyChanged();
            }
        }
        private string _date;
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                TimeRaisePropertyChanged();
            }
        }

        private string _week;
        public string Week
        {
            get { return _week; }
            set
            {
                _week = value;
                TimeRaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void TimeRaisePropertyChanged([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public SystemTime systemTimemodel { get; set; }
        public TimeViewModel()
        {
            systemTimemodel = new SystemTime();
            systemTimemodel.now = DateTime.Now.ToString("t");
            Now = systemTimemodel.now;
            systemTimemodel.date = DateTime.Now.ToString("D");
            Date = systemTimemodel.date;
            systemTimemodel.week = DateTime.Now.DayOfWeek.ToString();
            Week = systemTimemodel.week;
        }
    }
}
