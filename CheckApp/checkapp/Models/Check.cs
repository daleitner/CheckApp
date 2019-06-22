using System;
using System.Collections.Generic;
using Dart.Base;

namespace CheckApp
{
	public class Check
	{
		public Check()
		{
			Propability = 0;
			SubChecks = new List<Check>();
		}

		public Check(Field dart1, Field dart2, Field dart3, double propability, double exactPropability, List<Check> subChecks = null)
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
			ExactPropability = exactPropability;
			SubChecks = subChecks ?? new List<Check>();
		}

		public string CheckString => GetCheckString();
		public double Propability { get; set; }
		public string PropabilityString => Math.Round(Propability * 100, 2) + "%";
		public double ExactPropability { get; set; }
		public string ExactPropabilityString => Math.Round(ExactPropability * 100, 2) + "%";
		public Field CheckDart { get; set; }
		public Field AufCheckDart { get; set; }
		public Field ScoreDart { get; set; }
		public List<Check> SubChecks { get; set; }

		private string GetCheckString()
		{
			string ret = "";
			if (ScoreDart != null)
				ret += ScoreDart + " + ";
			if (AufCheckDart != null)
				ret += AufCheckDart + " + ";
			if (CheckDart != null)
				ret += CheckDart;
			return ret;
		}
	}
}
