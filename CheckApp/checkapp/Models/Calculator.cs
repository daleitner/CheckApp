using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CheckApp
{
	public class Calculator
	{
		private readonly double _dartBoardRadius;
		private readonly double _bullRadius;
		private readonly double _doubleBullRadius;
		private readonly double _doubleHeight;
		private readonly double _tripleHeight;
		private readonly double _tripleRadius;
		private const double Pi = 3.14159;
		private readonly DartBoard _dBoard;
		private List<bool> _checkchecks;

		private readonly double _singleQuote;
		private readonly double _doubleQuote;
		private readonly double _tripleQuote;
		public double BullQuote;
		public double DoubleBullQuote;

		public Calculator(int singleQuote, int doubleQuote, int tripleQuote)
		{
			_dartBoardRadius = 170;
			_bullRadius = 15.9;
			_doubleBullRadius = 6.35;
			_doubleHeight = 8;
			_tripleHeight = 8;
			_tripleRadius = 107;

			_singleQuote = (double)singleQuote / 100;
			_doubleQuote = (double)doubleQuote / 100;
			_tripleQuote = (double)tripleQuote / 100;
			BullQuote = (_singleQuote + _doubleQuote) / 2;
			DoubleBullQuote = _doubleQuote / 2;
			_dBoard = new DartBoard();

			_checkchecks = new List<bool> { true, true, true, true, true, true };
		}

		public double GetSingleArea()
		{
			return GetSingleInArea() + GetSingleOutArea();
		}

		public double GetSingleOutArea()
		{
			double outer = (_dartBoardRadius - _doubleHeight) * (_dartBoardRadius - _doubleHeight) * Pi;
			double inner = _tripleRadius * _tripleRadius * Pi;
			return (outer - inner) / 20;
		}

		public double GetSingleInArea()
		{
			double outersmall = (_tripleRadius - _tripleHeight) * (_tripleRadius - _tripleHeight) * Pi;
			double innersmall = _bullRadius * _bullRadius * Pi;
			return (outersmall - innersmall) / 20;
		}

		public double GetDoubleArea()
		{
			double outer = _dartBoardRadius * _dartBoardRadius * Pi;
			double inner = (_dartBoardRadius - _doubleHeight) * (_dartBoardRadius - _doubleHeight) * Pi;
			return (outer - inner) / 20;
		}
		public double GetTripleArea()
		{
			double outer = _tripleRadius * _tripleRadius * Pi;
			double inner = (_tripleRadius - _tripleHeight) * (_tripleRadius - _tripleHeight) * Pi;
			return (outer - inner) / 20;
		}
		public double GetSingleBullArea()
		{
			return _bullRadius * _bullRadius * Pi - GetDoubleBullArea();
		}
		public double GetDoubleBullArea()
		{
			return _doubleBullRadius * _doubleBullRadius * Pi;
		}

		public double GetHitRatio(Field feld)
		{
			var ret = 0.0;
			switch (feld.Type)
			{
				case FieldType.Single:
					ret = feld.Score == 25 ? BullQuote : _singleQuote;
					break;
				case FieldType.Double:
					ret = feld.Score == 50 ? DoubleBullQuote : _doubleQuote;
					break;
				case FieldType.Triple:
					ret = _tripleQuote;
					break;
			}
			return ret;
		}

		public List<CheckViewModel> GetAllPossibleCheckouts(int scores, int leftdarts, BackgroundWorker worker, List<bool> checks)
		{
			_checkchecks = checks;
			List<CheckViewModel> ret = CalculateChecks(scores, leftdarts, worker);
			return ret;
		}

		private List<CheckViewModel> CalculateChecks(int scores, int leftdarts, BackgroundWorker worker)
		{
			var ret = new List<CheckViewModel>();
			List<Field> doubles = _dBoard.GetAllDoubles();
			List<Field> allFields = _dBoard.GetAllFields();
			foreach (Field doppel in doubles)
			{
				if (doppel.Score == scores)
				{
					ret.Add(GetCheck(scores, leftdarts, doppel));
				}
				else if (doppel.Score < scores && CanCheck(doppel, scores) && leftdarts > 1)
				{
					foreach (Field first in allFields)
					{
						var durchlauf = true;
						if (worker != null)
						{
							if (leftdarts == 3)
							{
								switch (first.Type)
								{
									case FieldType.Single: durchlauf = _checkchecks[3]; break;
									case FieldType.Double: durchlauf = _checkchecks[4]; break;
									case FieldType.Triple: durchlauf = _checkchecks[5]; break;
								}
							}
							else
							{
								switch (first.Type)
								{
									case FieldType.Single: durchlauf = _checkchecks[0]; break;
									case FieldType.Double: durchlauf = _checkchecks[1]; break;
									case FieldType.Triple: durchlauf = _checkchecks[2]; break;
								}
							}
						}

						if (!durchlauf)
							continue;

						if (first.Score + doppel.Score == scores)
						{
							ret.Add(GetCheck(scores, leftdarts, first, doppel));
						}
						else if (first.Score + doppel.Score < scores && CanCheck(first, doppel, scores) && leftdarts > 2)
						{
							var check = false;
							foreach (Field second in allFields)
							{
								if (!check && second.Equals(first))
									check = true;
								if (!check)
									continue;

								var durchlauf2 = true;
								if (worker != null)
								{
									switch (second.Type)
									{
										case FieldType.Single: durchlauf2 = _checkchecks[0]; break;
										case FieldType.Double: durchlauf2 = _checkchecks[1]; break;
										case FieldType.Triple: durchlauf2 = _checkchecks[2]; break;
									}
								}

								if (!durchlauf2)
									continue;

								if (second.Score + first.Score + doppel.Score != scores)
									continue;

								CheckViewModel one = GetCheck(scores, second, first, doppel);
								CheckViewModel two = GetCheck(scores, first, second, doppel);
								if (one.Check.Calculation > two.Check.Calculation)
									ret.Add(one);
								else if (one.Check.Calculation == two.Check.Calculation)
								{
									if (one.Check.CheckDart.Difficulty < two.Check.CheckDart.Difficulty)
										ret.Add(one);
									else if (one.Check.CheckDart.Difficulty == two.Check.CheckDart.Difficulty)
									{
										ret.Add(one.Check.Propability >= two.Check.Propability ? one : two);
									}
									else
										ret.Add(two);
								}
								else
								{
									ret.Add(two);
								}
							}
						}
					}
				}

				worker?.ReportProgress((doubles.IndexOf(doppel) + 1) * 100 / doubles.Count, "");
			}
			ret = ret.OrderByDescending(x => x.Check.Calculation).ThenBy(x => x.Check.CheckDart.Difficulty).ThenByDescending(x => x.Check.Propability).ThenBy(x => x.Check.CheckString).ToList();
			return ret;
		}

		private CheckViewModel GetCheck(int checkscore, int leftdarts, Field dart1)
		{
			double p = GetHitRatio(dart1);
			CheckViewModel ret = new CheckViewModel(dart1, null, null, p, p, "");
			if (leftdarts == 1) //wenn nur 1 dart übrig
				return ret;

			double notp = 1 - p;
			double pbounce = 0.0;
			double gesamtflaeche = GetNeighbourArea(dart1); //gesamtfläche der nachbarfelder
			List<Field> daneben = dart1.Neighbours;
			double danebenflaeche;
			int restscore;

			if (dart1.Score < 50)
			{
				pbounce = (GetSingleOutArea() / gesamtflaeche) * notp;
			}

			if (leftdarts == 2)
			{
				//!p * p
				double notpandp = 0.0;
				foreach (Field f in daneben)
				{
					if (f.Score < checkscore - 1)
					{
						danebenflaeche = GetNeighbourFieldArea(dart1, f);
						double pdaneben = notp * danebenflaeche / gesamtflaeche;

						restscore = checkscore - f.Score;

						List<CheckViewModel> possiblechecks = CalculateChecks(restscore, 1, null);
						if (possiblechecks.Count > 0)
							notpandp += pdaneben * possiblechecks[0].Check.Propability;
					}
				}

				ret.Check.Propability += pbounce * p; //bounce * p
				ret.Check.Propability += notpandp;
				return ret;
			}

			ret.Check.Propability += pbounce * GetCheck(checkscore, leftdarts - 1, dart1).Check.Propability; //bounce*p + bounce*!p*p + bounce*bounce*p

			//!p*!p*p + !p*bounce*p + !p*p
			foreach (Field f in daneben)
			{
				if (f.Score < checkscore - 1)
				{
					danebenflaeche = GetNeighbourFieldArea(dart1, f);
					double pdaneben = notp * danebenflaeche / gesamtflaeche;

					restscore = checkscore - f.Score;

					List<CheckViewModel> possiblechecks = CalculateChecks(restscore, leftdarts - 1, null);
					if (possiblechecks.Count > 0)
					{
						CheckViewModel easiestCheck = possiblechecks[0];
						ret.Check.Propability += pdaneben * easiestCheck.Check.Propability;
						if (dart1.Score % 25 != 0 || f.Score == 25 || f.Score == 50)
							ret.Check.Message += "wenn " + ret.Check.FieldToString(f) + "->" + easiestCheck.Check.CheckString + "; ";
					}
				}
			}
			return ret;
		}

		private CheckViewModel GetCheck(int checkscore, int leftdarts, Field dart1, Field dart2)
		{
			//3 darts
			//p(check) = p1*p2 + bounce*p2 + !p1*p2 + bounce*p1*p2 + p1*bounce*p2 + !p1*bounce*p2 + bounce*!p1*p2 + !p1*p1*p2  + !p1*!p1*p2 + p1*!p2*p2
			//2 darts
			//p(check) = p1*p2 + bounce*p2 + !p1*p2
			CheckViewModel ret = new CheckViewModel(dart1, dart2, null, 0.0, 0.0, "");
			double p1 = GetHitRatio(dart1);
			double p2 = GetHitRatio(dart2);
			ret.Check.Calculation = p1 * p2;
			double notp1 = 1 - p1;
			double pbounce = 0.0;
			double gesamtflaeche = GetNeighbourArea(dart1);
			List<CheckViewModel> checks = null;
			List<Field> neighbours = dart1.Neighbours;

			if (dart1.Type == FieldType.Single && dart1.Score < 21)
			{
				var singleNeighbours = neighbours.Where(x => x.Type == FieldType.Single).ToList();
				if (singleNeighbours.Count == 2)
					ret.Check.Message += GetCheckCode(checkscore, dart2.Score, singleNeighbours[0].Score, singleNeighbours[1].Score) + " ";
			}
			foreach (Field f in neighbours)
			{
				if (f.Score < checkscore - 1)
				{
					double danebenflaeche = GetNeighbourFieldArea(dart1, f);
					double pdaneben = notp1 * danebenflaeche / gesamtflaeche;
					checks = CalculateChecks(checkscore - f.Score, leftdarts - 1, null); //!p1*p2 for 1 dart left; !p1*p2 + !p1*bounce*p2 + !p1*p1*p2 + !p1*!p1*p2 for 2 darts left
					if (checks.Count > 0)
					{
						ret.Check.Propability += pdaneben * checks[0].Check.Propability;
						if (dart1.Score % 25 != 0 || f.Score == 25 || f.Score == 50)
							ret.Check.Message += "wenn " + ret.Check.FieldToString(f) + "->" + checks[0].Check.CheckString + "; ";
					}
				}
			}

			if (dart1.Type.Equals(FieldType.Double) && dart1.Score != 50)
			{
				pbounce = (GetSingleOutArea() / gesamtflaeche) * notp1;
			}


			if (leftdarts == 2)
			{
				ret.Check.Propability += p1 * p2;
				if (pbounce != 0.0)
				{
					checks = CalculateChecks(checkscore, 1, null);
					if (checks.Count > 0)
					{
						ret.Check.Propability += pbounce * checks[0].Check.Propability; //bounce*p2
					}
				}
				return ret;
			}

			ret.Check.Propability += p1 * GetCheck(checkscore - dart1.Score, leftdarts - 1, dart2).Check.Propability; //p1*p2 + p1*bounce*p2 + p1*!p2*p2

			if (pbounce != 0.0)
			{
				ret.Check.Propability += pbounce * GetCheck(checkscore, leftdarts - 1, dart1, dart2).Check.Propability; //bounce*!p1*p2 + bounce*p1*p2
			}

			return ret;
		}

		private string GetCheckCode(int checkscore, int dart2, int firstNeighbour, int secondNeighbour)
		{
			var ret = "";
			if (dart2 % 8 == 0)
				ret += "4";
			else if (dart2 % 4 == 0)
				ret += "3";
			else if (dart2 > 2)
				ret += "2";
			else
				ret += "1";

			if (firstNeighbour >= checkscore - 1)
			{
				if (secondNeighbour >= checkscore - 1)
				{
					ret += "1";
				}
				else if ((checkscore - secondNeighbour) % 2 == 0 && checkscore - secondNeighbour < 41)
				{
					ret += "3";
				}
				else
				{
					ret += "2";
				}
			}
			else if ((checkscore - firstNeighbour) % 2 == 0 && checkscore - firstNeighbour < 41)
			{
				if (secondNeighbour >= checkscore - 1)
				{
					ret += "3";
				}
				else if ((checkscore - secondNeighbour) % 2 == 0 && checkscore - secondNeighbour < 41)
				{
					ret += "6";
				}
				else
				{
					ret += "5";
				}
			}
			else
			{
				if (secondNeighbour >= checkscore - 1)
				{
					ret += "2";
				}
				else if ((checkscore - secondNeighbour) % 2 == 0 && checkscore - secondNeighbour < 41)
				{
					ret += "5";
				}
				else
				{
					ret += "4";
				}
			}
			return ret;
		}

		private CheckViewModel GetCheck(int checkscore, Field dart1, Field dart2, Field dart3)
		{
			//p(check) = p1*p2*p3 + p1*!p2*p3 + p1*bounce*p3 + !p1*p2*p3 + !p1*bounce*p3 + !p1*!p2*p3 + bounce*p2*p3 + bounce*!p2*p3 + bounce*bounce*p3
			CheckViewModel ret = new CheckViewModel(dart1, dart2, dart3, 0.0, 0.0, "");
			double p1 = GetHitRatio(dart1);
			double p2 = GetHitRatio(dart2);
			double p3 = GetHitRatio(dart3);
			ret.Check.Calculation = p1 * p2 * p3;
			double notp1 = 1 - p1;
			double pbounce = 0.0;
			double fullArea = GetNeighbourArea(dart1);
			List<Field> neighbours = dart1.Neighbours;
			List<CheckViewModel> checks;

			if (dart1.Type.Equals(FieldType.Double) && dart1.Score != 50)
				pbounce = (GetSingleOutArea() / fullArea) * notp1;

			ret.Check.Propability = p1 * GetCheck(checkscore - dart1.Score, 2, dart2, dart3).Check.Propability;//p1*p2*p3 + p1*!p2*p3 + p1*bounce*p3
			foreach (Field f in neighbours)
			{
				if (f.Score < checkscore - 1)
				{
					double danebenflaeche = GetNeighbourFieldArea(dart1, f);
					double pdaneben = notp1 * danebenflaeche / fullArea;
					int restscore = checkscore - f.Score;
					checks = CalculateChecks(restscore, 2, null);
					if (checks.Count > 0)
					{
						CheckViewModel easiestcheck = checks[0];
						ret.Check.Propability += pdaneben * easiestcheck.Check.Propability; //!p1*p2*p3 + !p1*bounce*p3 + !p1*!p2*p3
						if (dart1.Score % 25 != 0 || f.Score == 25 || f.Score == 50)
							ret.Check.Message += "wenn " + ret.Check.FieldToString(f) + "->" + easiestcheck.Check.CheckString + "; ";
					}
				}
			}

			if (dart1.Type.Equals(FieldType.Double) && dart1.Score != 50)
			{
				checks = CalculateChecks(checkscore, 2, null);
				if (checks.Count > 0)
				{
					CheckViewModel easiestcheck = checks[0];
					ret.Check.Propability += pbounce * easiestcheck.Check.Propability; //bounce*p2*p3 + bounce*!p2*p3 + bounce*bounce*p3
					ret.Check.Message += "wenn bounce->" + easiestcheck.Check.CheckString + "; ";
				}
			}

			return ret;
		}

		private double GetNeighbourArea(Field f)
		{
			double ret = 0.0;
			switch (f.Type)
			{
				case FieldType.Double:
					if (f.Score == 50)
					{
						ret += GetSingleBullArea();
						ret += GetSingleInArea() * 20 / 10;
					}
					else
					{
						ret += GetSingleOutArea() * 2; //single/2 + singleleft/4 + singleright/4 + gleiche fläche für bounce
						ret += GetDoubleArea(); //doubleleft/2 + doubleright/2
					}
					break;
				case FieldType.Single:
					if (f.Score == 25)
					{
						ret += GetDoubleBullArea();
						ret += GetSingleInArea() * 20 / 10;
					}
					else
					{
						ret += GetSingleArea() * 2;
						ret += GetTripleArea() * 3;
						ret += GetDoubleArea() * 3;
					}
					break;
				case FieldType.Triple:
					ret += GetSingleArea() / 2 * 3;
					ret += GetTripleArea() * 2; break;
			}
			return ret;
		}

		private double GetNeighbourFieldArea(Field f, Field neighbour)
		{
			double res;
			if (f.Score % 25 == 0)
			{
				if (neighbour.Type.Equals(FieldType.Double))
					res = GetDoubleBullArea();
				else if (neighbour.Score == 25)
					res = GetSingleBullArea();
				else
					res = GetSingleInArea() / 10;
			}
			else
			{
				if (f.Type.Equals(FieldType.Double))
				{
					if (neighbour.Type.Equals(FieldType.Double))
						res = GetDoubleArea() / 2;
					else if (neighbour.Score == f.Score / 2)
						res = GetSingleOutArea() / 2;
					else
						res = GetSingleOutArea() / 4;
				}
				else if (f.Type.Equals(FieldType.Single))
				{
					if (neighbour.Type.Equals(FieldType.Single))
						res = GetSingleArea();
					else if (neighbour.Type.Equals(FieldType.Double))
						res = GetDoubleArea();
					else
						res = GetTripleArea();
				}
				else
				{
					if (neighbour.Type.Equals(FieldType.Single))
						res = GetSingleArea() / 2;
					else
						res = GetTripleArea();
				}
			}
			return res;
		}

		private bool CanCheck(Field dart1, Field dart2, int score)
		{
			int erg = dart1.Score + dart2.Score;
			var ret = false;
			if (erg < score)
			{
				int rest = score - erg;
				if (rest <= 60 && (rest <= 20 || rest == 25 || rest % 3 == 0 || rest % 2 == 0))
					ret = true;
			}
			return ret;
		}

		private bool CanCheck(Field dart1, int score)
		{
			if (dart1.Score >= score)
				return false;

			var ret = false;
			int rest = score - dart1.Score;
			if (rest < 103 || (rest <= 120 && (rest % 3 == 0 || (rest - 50) % 3 == 0)))
				ret = true;
			return ret;
		}
	}
}
