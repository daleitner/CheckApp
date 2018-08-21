using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CheckApp
{
    public class CheckViewModel : ViewModelBase
    {
        private Check chk = null;
        private Brush background = null;
        public CheckViewModel()
        {
            this.chk = new Check();
        }
        public CheckViewModel(Field dart1, Field dart2, Field dart3, double Propability, double Calculation, string Message)
        {
            this.chk = new Check(dart1, dart2, dart3, Propability, Calculation, Message);
            SetBackground(dart1, dart2, dart3);
        }

        public Check Check
        {
            get
            {
                return this.chk;
            }
            set
            {
                this.chk = value;
                OnPropertyChanged("Check");
            }
        }

        public Brush Background
        {
            get
            {
                return this.background;
            }
            set
            {
                this.background = value;
                OnPropertyChanged("Background");
            }
        }

        private void SetBackground(Field dart1, Field dart2, Field dart3)
        {
            if (dart2 == null)
            {
                Background = new SolidColorBrush(Colors.DodgerBlue); //1
            }
            else
            {
                if (dart3 == null)
                {
                    switch (dart1.Type)
                    {
                        case FieldType.Single: Background = new SolidColorBrush(Colors.DeepSkyBlue); break; //2
                        case FieldType.Double: Background = new SolidColorBrush(Colors.LightGreen); break; //4
                        case FieldType.Triple: Background = new SolidColorBrush(Colors.GreenYellow); break; //5
                    }
                }
                else
                {
                    switch (dart1.Type)
                    {
                        case FieldType.Single:
                            switch(dart2.Type)
                            {
                                case FieldType.Single: Background = new SolidColorBrush(Colors.LightSeaGreen); break; //3
                                case FieldType.Double: Background = new SolidColorBrush(Colors.Yellow); break; //6
                                case FieldType.Triple: Background = new SolidColorBrush(Colors.Gold); break; //7
                            }break;
                        case FieldType.Double: 
                        switch (dart2.Type)
                            {
                                case FieldType.Single: Background = new SolidColorBrush(Colors.Yellow); break; //6
                                case FieldType.Double: Background = new SolidColorBrush(Colors.Orange); break; //8
                                case FieldType.Triple: Background = new SolidColorBrush(Colors.OrangeRed); break; //9
                            } break;
                        case FieldType.Triple: 
                        switch (dart2.Type)
                            {
                                case FieldType.Single: Background = new SolidColorBrush(Colors.Gold); break; //7
                                case FieldType.Double: Background = new SolidColorBrush(Colors.OrangeRed); break; //9
                                case FieldType.Triple: Background = new SolidColorBrush(Colors.Red); break; //10
                            } break;
                    }
                }
            }
        }
    }
}
