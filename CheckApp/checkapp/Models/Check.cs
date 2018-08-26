using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckApp
{
	public class Check
	{
		public Check()
		{
			CheckString = "";
			Propability = 0;
			Message = "";
			Calculation = 0;
		}

		public Check(Field dart1, Field dart2, Field dart3, double propability, double calculation, string message)
		{
			CheckString = GetCheckString(dart1, dart2, dart3);
			if (dart2 == null)
			{
				CheckDart = dart1;
			}
			else
			{
				if (dart3 == null)
				{
					AufCheckDart = dart1;
					CheckDart = dart2;
				}
				else
				{
					ScoreDart = dart1;
					AufCheckDart = dart2;
					CheckDart = dart3;
				}
			}
			Propability = propability;
			Calculation = calculation;
			Message = message;
		}
		public string CheckString { get; set; }
		public double Propability { get; set; }
		public string PropabilityString => Propability + "%";
		public double Calculation { get; set; }
		public string Message { get; set; }
		public Field CheckDart { get; set; }
		public Field AufCheckDart { get; set; }
		public Field ScoreDart { get; set; }

		private string GetCheckString(Field dart1, Field dart2, Field dart3)
		{
			string ret = FieldToString(dart1);
			if (dart2 != null)
				ret += " + " + FieldToString(dart2);
			if (dart3 != null)
				ret += " + " + FieldToString(dart3);
			return ret;
		}

		public string FieldToString(Field f)
		{
			switch (f.Type)
			{
				case FieldType.Double: return "D" + (f.Score / 2).ToString();
				case FieldType.Triple: return "T" + (f.Score / 3).ToString();
				default: return f.Score.ToString();
			}
		}
	}
}
