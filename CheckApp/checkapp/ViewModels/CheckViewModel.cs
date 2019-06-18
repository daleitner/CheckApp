using System.Collections.Generic;
using System.Windows.Media;
using Dart.Base;

namespace CheckApp
{
	public class CheckViewModel : ViewModelBase
	{
		private Check chk;
		private Brush background;

		public CheckViewModel(Field dart1, Field dart2, Field dart3, double propability, List<Check> subChecks = null)
		{
			this.chk = new Check(dart1, dart2, dart3, propability, subChecks);
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
						case FieldEnum.SingleOut: Background = new SolidColorBrush(Colors.DeepSkyBlue); break; //2
						case FieldEnum.Double: Background = new SolidColorBrush(Colors.LightGreen); break; //4
						case FieldEnum.Triple: Background = new SolidColorBrush(Colors.GreenYellow); break; //5
					}
				}
				else
				{
					switch (dart1.Type)
					{
						case FieldEnum.SingleOut:
							switch (dart2.Type)
							{
								case FieldEnum.SingleOut: Background = new SolidColorBrush(Colors.LightSeaGreen); break; //3
								case FieldEnum.Double: Background = new SolidColorBrush(Colors.Yellow); break; //6
								case FieldEnum.Triple: Background = new SolidColorBrush(Colors.Gold); break; //7
							}
							break;
						case FieldEnum.Double:
							switch (dart2.Type)
							{
								case FieldEnum.SingleOut: Background = new SolidColorBrush(Colors.Yellow); break; //6
								case FieldEnum.Double: Background = new SolidColorBrush(Colors.Orange); break; //8
								case FieldEnum.Triple: Background = new SolidColorBrush(Colors.OrangeRed); break; //9
							}
							break;
						case FieldEnum.Triple:
							switch (dart2.Type)
							{
								case FieldEnum.SingleOut: Background = new SolidColorBrush(Colors.Gold); break; //7
								case FieldEnum.Double: Background = new SolidColorBrush(Colors.OrangeRed); break; //9
								case FieldEnum.Triple: Background = new SolidColorBrush(Colors.Red); break; //10
							}
							break;
					}
				}
			}
		}
	}
}
