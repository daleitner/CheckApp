using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CheckApp
{
    public class Calculator
    {
        private double dartBoardRadius;
        private double bullRadius;
        private double doubleBullRadius;
        private double doubleHeight;
        private double tripleHeight;
        private double tripleRadius;
        private const double PI = 3.14159;
        private DartBoard dBoard = null;
        private List<bool> checkchecks = null;

		public double singleQuote;
		public double doubleQuote;
		public double tripleQuote;
		public double bullQuote;
		public double doubleBullQuote;

        public Calculator(int singleQuote, int doubleQuote, int tripleQuote)
        {
            this.dartBoardRadius = 170;
            this.bullRadius = 15.9;
            this.doubleBullRadius = 6.35;
            this.doubleHeight = 8;
            this.tripleHeight = 8;
            this.tripleRadius = 107;

			this.singleQuote = (double)singleQuote / 100;
			this.doubleQuote = (double)doubleQuote / 100;
			this.tripleQuote = (double)tripleQuote / 100;
			this.bullQuote = (this.singleQuote + this.doubleQuote) / 2;
			this.doubleBullQuote = this.doubleQuote / 2;
			dBoard = new DartBoard();

            checkchecks = new List<bool>() { true, true, true, true, true, true };
        }

        public double GetSingleFlaeche()
        {
            return GetSingleInFlaeche() + GetSingleOutFlaeche();
        }

        public double GetSingleOutFlaeche()
        {
            double outer = (dartBoardRadius - doubleHeight) * (dartBoardRadius - doubleHeight) * PI;
            double inner = tripleRadius * tripleRadius * PI;
            return (outer - inner) / 20;
        }

        public double GetSingleInFlaeche()
        {
            double outersmall = (tripleRadius - tripleHeight) * (tripleRadius - tripleHeight) * PI;
            double innersmall = bullRadius * bullRadius * PI;
            return (outersmall - innersmall) / 20;
        }

        public double GetDoubleFlaeche()
        {
            double outer = dartBoardRadius * dartBoardRadius * PI;
            double inner = (dartBoardRadius - doubleHeight) * (dartBoardRadius - doubleHeight) * PI;
            return (outer-inner)/20;
        }
        public double GetTripleFlaeche()
        {
            double outer = tripleRadius * tripleRadius * PI;
            double inner = (tripleRadius - tripleHeight) * (tripleRadius - tripleHeight) * PI;
            return (outer - inner) / 20;
        }
        public double GetSingleBullFlaeche()
        {
            return bullRadius * bullRadius * PI - GetDoubleBullFlaeche();
        }
        public double GetDoubleBullFlaeche()
        {
            return doubleBullRadius * doubleBullRadius * PI;
        }

        public double GetTrefferWahrscheinlichkeit(Field feld)
        {
            double ret = 0.0;
            switch (feld.Type)
            {
                case FieldType.Single:
					if (feld.Score == 25)
						ret = this.bullQuote;
                    else
                        ret = this.singleQuote;
                       break;
                case FieldType.Double:
                    if(feld.Score == 50)
                        ret = this.doubleBullQuote;
                    else
                        ret = this.doubleQuote; 
                        break;
                case FieldType.Triple:
                        ret = this.tripleQuote;
                    break;
            }
            return ret;
        }

        public List<CheckViewModel> GetAllChecks(int scores, int leftdarts, BackgroundWorker worker, List<bool> checks)
        {
            //List<Field> all = dBoard.GetAllDoubles();
            //Field test = null;
            //Field single = null;
            //foreach (Field f in all)
            //{
            //    if (f.Score == 12)
            //        test = f;
            //    if (f.Score == 8)
            //        single = f;
            //}
            checkchecks = checks;
            List<CheckViewModel> ret = CalculateChecks(scores, leftdarts, worker);// new List<CheckViewModel>();
           // ret.Add(GetCheck(32, test, single, test));
            return ret;
        }

        private List<CheckViewModel> CalculateChecks(int scores, int leftdarts, BackgroundWorker worker)
        {
            List<CheckViewModel> ret = new List<CheckViewModel>();
            List<Field> doubles = dBoard.GetAllDoubles();
            List<Field> allFields = dBoard.GetAllFields();
            bool check = false;
            foreach (Field doppel in doubles)
            {
                if (doppel.Score == scores)
                {
                    ret.Add(GetCheck(scores, leftdarts, doppel));
                }
                else if (doppel.Score < scores && CheckNochMoeglich(doppel, scores) && leftdarts > 1)
                {
                    foreach (Field first in allFields)
                    {
                        bool durchlauf = true;
                        if (worker != null)
                        {
                            if (leftdarts == 3)
                            {
                                switch (first.Type)
                                {
                                    case FieldType.Single: durchlauf = checkchecks[3]; break;
                                    case FieldType.Double: durchlauf = checkchecks[4]; break;
                                    case FieldType.Triple: durchlauf = checkchecks[5]; break;
                                }
                            }
                            else
                            {
                                switch (first.Type)
                                {
                                    case FieldType.Single: durchlauf = checkchecks[0]; break;
                                    case FieldType.Double: durchlauf = checkchecks[1]; break;
                                    case FieldType.Triple: durchlauf = checkchecks[2]; break;
                                }
                            }
                        }
                        if (durchlauf)
                        {
                            if (first.Score + doppel.Score == scores)
                            {
                                ret.Add(GetCheck(scores, leftdarts, first, doppel));
                            }
                            else if (first.Score + doppel.Score < scores && CheckNochMoeglich(first, doppel, scores) && leftdarts > 2)
                            {
                                check = false;
                                foreach (Field second in allFields)
                                {
                                    if (!check && second.Equals(first))
                                        check = true;
                                    if (check)
                                    {
                                        bool durchlauf2 = true;
                                        if (worker != null)
                                        {
                                            switch (second.Type)
                                            {
                                                case FieldType.Single: durchlauf2 = checkchecks[0]; break;
                                                case FieldType.Double: durchlauf2 = checkchecks[1]; break;
                                                case FieldType.Triple: durchlauf2 = checkchecks[2]; break;
                                            }
                                        }
                                        if (durchlauf2)
                                        {
                                            if (second.Score + first.Score + doppel.Score == scores)
                                            {
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
                                                        if (one.Check.Propability >= two.Check.Propability)
                                                            ret.Add(one);
                                                        else
                                                            ret.Add(two);
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
                            }
                        }
                    }
                }
                if (worker != null)
                {
                    worker.ReportProgress((doubles.IndexOf(doppel) + 1) * 100 / doubles.Count, "");
                }
            }
            ret = ret.OrderByDescending(x => x.Check.Calculation).ThenBy(x => x.Check.CheckDart.Difficulty).ThenByDescending(x => x.Check.Propability).ThenBy(x => x.Check.CheckString).ToList();
            return ret;
        }

        private CheckViewModel GetCheck(int checkscore, int leftdarts, Field dart1)
        {
            //3 darts
            // P(x) = p + bounce*p + bounce*bounce*p + bounce*!p*p + !p*p + !p*bounce*p + !p*!p*p
            //2 darts
            // P(x) = p + bounce*p + !p*p
            //1 dart
            // P(x) = p
            double p = GetTrefferWahrscheinlichkeit(dart1);
            CheckViewModel ret = new CheckViewModel(dart1, null, null, p, p, "");
            if (leftdarts == 1) //wenn nur 1 dart übrig
                return ret;

            double notp = 1-p;
            double pbounce = 0.0;
            double gesamtflaeche = GetNeighbourFlaeche(dart1); //gesamtfläche der nachbarfelder
            List<Field> daneben = GetNeighbourFields(dart1);
            double danebenflaeche = 0.0;
            int restscore = 0;

            if (dart1.Score < 50)
            {
                pbounce = (GetSingleOutFlaeche() / gesamtflaeche) * notp;
            }

            if (leftdarts == 2)
            {
                //!p * p
                double notpandp = 0.0;
                foreach (Field f in daneben)
                {
                    if (f.Score < checkscore - 1)
                    {
                        danebenflaeche = GetNeighbourFieldFlaeche(dart1, f);
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

            ret.Check.Propability += pbounce * GetCheck(checkscore, leftdarts-1, dart1).Check.Propability; //bounce*p + bounce*!p*p + bounce*bounce*p

            //!p*!p*p + !p*bounce*p + !p*p
            restscore = 0;
            foreach (Field f in daneben)
            {
                if (f.Score < checkscore - 1)
                {
                    danebenflaeche = GetNeighbourFieldFlaeche(dart1, f);
                    double pdaneben = notp * danebenflaeche / gesamtflaeche;

                    restscore = checkscore - f.Score;

                    List<CheckViewModel> possiblechecks = CalculateChecks(restscore, leftdarts-1, null);
                    if (possiblechecks.Count > 0)
                    {
                        CheckViewModel easiestCheck = possiblechecks[0];
                        ret.Check.Propability += pdaneben * easiestCheck.Check.Propability;
                        if(dart1.Score % 25 != 0 || f.Score == 25 || f.Score == 50)
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
            double p1 = GetTrefferWahrscheinlichkeit(dart1);
            double p2 = GetTrefferWahrscheinlichkeit(dart2);
            ret.Check.Calculation = p1 * p2;
            double notp1 = 1 - p1;
            double pbounce = 0.0;
            double gesamtflaeche = GetNeighbourFlaeche(dart1);
            List<CheckViewModel> checks = null;
            List<Field> neighbours = GetNeighbourFields(dart1);

	        if (dart1.Type == FieldType.Single && dart1.Score < 21)
	        {
		        var singleNeighbours = neighbours.Where(x => x.Type == FieldType.Single).ToList();
				if(singleNeighbours.Count == 2)
					ret.Check.Message += GetCheckCode(checkscore, dart2.Score, singleNeighbours[0].Score, singleNeighbours[1].Score) + " ";
	        }
            foreach (Field f in neighbours)
            {
                if (f.Score < checkscore - 1)
                {
                    double danebenflaeche = GetNeighbourFieldFlaeche(dart1, f);
                    double pdaneben = notp1 * danebenflaeche / gesamtflaeche;
                    checks = CalculateChecks(checkscore - f.Score, leftdarts-1, null); //!p1*p2 for 1 dart left; !p1*p2 + !p1*bounce*p2 + !p1*p1*p2 + !p1*!p1*p2 for 2 darts left
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
                pbounce = (GetSingleOutFlaeche() / gesamtflaeche) * notp1;
            }


            if(leftdarts == 2)
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
            
            ret.Check.Propability += p1 * GetCheck(checkscore-dart1.Score, leftdarts-1, dart2).Check.Propability; //p1*p2 + p1*bounce*p2 + p1*!p2*p2

            if (pbounce != 0.0)
            {
                ret.Check.Propability += pbounce * GetCheck(checkscore, leftdarts-1, dart1, dart2).Check.Propability; //bounce*!p1*p2 + bounce*p1*p2
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
            double p1 = GetTrefferWahrscheinlichkeit(dart1);
            double p2 = GetTrefferWahrscheinlichkeit(dart2);
            double p3 = GetTrefferWahrscheinlichkeit(dart3);
            ret.Check.Calculation = p1 * p2 * p3;
            double notp1 = 1 - p1;
            double pbounce = 0.0;
            double gesamtflaeche = GetNeighbourFlaeche(dart1);
            List<Field> neighbours = GetNeighbourFields(dart1);
            List<CheckViewModel> checks = null;

            if(dart1.Type.Equals(FieldType.Double) && dart1.Score != 50)
                pbounce = (GetSingleOutFlaeche() / gesamtflaeche) * notp1;

            ret.Check.Propability = p1 * GetCheck(checkscore - dart1.Score, 2, dart2, dart3).Check.Propability;//p1*p2*p3 + p1*!p2*p3 + p1*bounce*p3
            foreach (Field f in neighbours)
            {
                if (f.Score < checkscore - 1)
                {
                    double danebenflaeche = GetNeighbourFieldFlaeche(dart1, f);
                    double pdaneben = notp1 * danebenflaeche / gesamtflaeche;
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

        private double GetNeighbourFlaeche(Field f)
        {
            double ret = 0.0;
            switch (f.Type)
            {
                case FieldType.Double:
                    if(f.Score == 50)
                    {
                        ret += GetSingleBullFlaeche();
                        ret += GetSingleInFlaeche() * 20 / 10;
                    }
                    else   
                    {
                        ret += GetSingleOutFlaeche() * 2; //single/2 + singleleft/4 + singleright/4 + gleiche fläche für bounce
                        ret += GetDoubleFlaeche(); //doubleleft/2 + doubleright/2
                    }
                    break;
                case FieldType.Single:
                    if(f.Score == 25)
                    {
                        ret += GetDoubleBullFlaeche();
                        ret += GetSingleInFlaeche() * 20 / 10;
                    }
                    else
                    {
                        ret += GetSingleFlaeche() * 2;
                        ret += GetTripleFlaeche() * 3;
                        ret += GetDoubleFlaeche() * 3;
                    }
                    break;
                case FieldType.Triple: 
                    ret += GetSingleFlaeche() / 2 * 3;
                    ret += GetTripleFlaeche() * 2; break;
            }
            return ret;
        }

        private double GetNeighbourFieldFlaeche(Field f, Field neighbour)
        {
            double erg = 0.0;
            if (f.Score % 25 == 0)
            {
                if (neighbour.Type.Equals(FieldType.Double))
                    erg = GetDoubleBullFlaeche();
                else if (neighbour.Score == 25)
                    erg = GetSingleBullFlaeche();
                else
                    erg= GetSingleInFlaeche() / 10;
            }
            else
            {
                if (f.Type.Equals(FieldType.Double))
                {
                    if (neighbour.Type.Equals(FieldType.Double))
                        erg = GetDoubleFlaeche() / 2;
                    else if(neighbour.Score == f.Score / 2)
                        erg = GetSingleOutFlaeche() / 2;
                    else
                        erg = GetSingleOutFlaeche() / 4;
                }
                else if (f.Type.Equals(FieldType.Single))
                {
                    if(neighbour.Type.Equals(FieldType.Single))
                        erg = GetSingleFlaeche();
                    else if(neighbour.Type.Equals(FieldType.Double))
                        erg = GetDoubleFlaeche();
                    else
                        erg = GetTripleFlaeche();
                }
                else
                {
                    if(neighbour.Type.Equals(FieldType.Single))
                        erg = GetSingleFlaeche() / 2;
                    else
                        erg = GetTripleFlaeche();
                }
            }
            return erg;
        }
        private List<Field> GetNeighbourFields(Field f)
        {
            List<Field> ret = new List<Field>();
            if (f.Score % 25 == 0)
            {
                ret.Add(dBoard.GetLeft(f));

                Field start = dBoard.GetFirstField();
                Field tmp = dBoard.GetRight(start);
                ret.Add(start);
                while(tmp != start)
                {
                    ret.Add(tmp);
                    tmp = dBoard.GetRight(tmp);
                }
            }
            else
            {
                if (f.Type.Equals(FieldType.Double))
                {
                    Field single = dBoard.GetSingle(f);
                    ret.Add(single);
                    ret.Add(dBoard.GetLeft(single));
                    ret.Add(dBoard.GetRight(single));
                    ret.Add(dBoard.GetLeft(f));
                    ret.Add(dBoard.GetRight(f));
                }
                else if (f.Type.Equals(FieldType.Single))
                {
                    ret.Add(dBoard.GetLeft(f));
                    ret.Add(dBoard.GetRight(f));
                    ret.Add(dBoard.GetTriple(f));
                    ret.Add(dBoard.GetDouble(f));
                    ret.Add(dBoard.GetTriple(dBoard.GetLeft(f)));
                    ret.Add(dBoard.GetTriple(dBoard.GetRight(f)));
                    ret.Add(dBoard.GetDouble(dBoard.GetLeft(f)));
                    ret.Add(dBoard.GetDouble(dBoard.GetRight(f)));
                }
                else
                {
                    ret.Add(dBoard.GetSingle(f));
                    ret.Add(dBoard.GetRight(dBoard.GetSingle(f)));
                    ret.Add(dBoard.GetLeft(dBoard.GetSingle(f)));
                    ret.Add(dBoard.GetLeft(f));
                    ret.Add(dBoard.GetRight(f));
                }
            }
            return ret;
        }

        private bool CheckNochMoeglich(Field dart1, Field dart2, int score)
        {
            int erg = dart1.Score + dart2.Score;
            bool ret = false;
            if (erg < score)
            {
                int rest = score - erg;
                if (rest <= 60 && (rest <= 20 || rest == 25 || rest % 3 == 0 || rest % 2 == 0))
                    ret = true;
            }
            return ret;
        }

        private bool CheckNochMoeglich(Field dart1, int score)
        {
            if (dart1.Score >= score)
                return false;

            bool ret = false;
            int rest = score - dart1.Score;
            if (rest < 103 || (rest <= 120 && (rest % 3 == 0 || (rest - 50) % 3 == 0)))
                ret = true;
            return ret;
        }
    }
}
