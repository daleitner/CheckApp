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
			Propability = 0;
			Message = "";
			Calculation = 0;
			SubChecks = new List<Check>();
		}

		public Check(Field dart1, Field dart2, Field dart3, double propability, double calculation, string message, List<Check> subChecks)
		{
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
			SubChecks = subChecks;
			if(SubChecks == null)
				SubChecks = new List<Check>();
		}

		public string CheckString => GetCheckString();
		public double Propability { get; set; }
		public string PropabilityString => Math.Round(Propability * 100, 2) + "%";
		public double Calculation { get; set; }
		public string Message { get; set; }
		public Field CheckDart { get; set; }
		public Field AufCheckDart { get; set; }
		public Field ScoreDart { get; set; }
		public List<Check> SubChecks { get; set; }

		private string GetCheckString()
		{
			string ret = "";
			if (ScoreDart != null)
				ret += FieldToString(ScoreDart) + " + ";
			if (AufCheckDart != null)
				ret += FieldToString(AufCheckDart) + " + ";
			if (CheckDart != null)
				ret += FieldToString(CheckDart);
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
