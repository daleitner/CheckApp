using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CheckApp
{
	public class NewCalculator
	{
		private readonly DartBoard _dBoard;
		private readonly double _singleQuote;
		private readonly double _doubleQuote;
		private readonly double _tripleQuote;
		public NewCalculator(int singleQuote, int doubleQuote, int tripleQuote)
		{
			_singleQuote = (double)singleQuote / 100;
			_doubleQuote = (double)doubleQuote / 100;
			_tripleQuote = (double)tripleQuote / 100;
			_dBoard = new DartBoard(_singleQuote, _doubleQuote, _tripleQuote);
		}

		public List<CheckViewModel> CalculateChecks(int score, int leftDarts, BackgroundWorker worker, List<bool> sth)
		{
			if (!IsAFinish(score, leftDarts))
				return null;

			if (leftDarts == 1)
			{
				var doubleField = _dBoard.GetAllDoubles().Single(x => x.Score == score);
				return new List<CheckViewModel> {new CheckViewModel(doubleField, null, null, doubleField.HitRatio, doubleField.HitRatio, "abc", null)};
			}
			var checks = new List<CheckViewModel>();
			if (leftDarts == 2)
			{
				foreach (var field in _dBoard.GetAllFields())
				{
					var check = CalculateChecks(score - field.Score, leftDarts - 1, worker, sth)?.FirstOrDefault();
					if(check == null)
						continue;

					var prop = check.Check.Propability * field.HitRatio;
					var subChecks = new List<Check>();
					foreach (var neighbour in field.Neighbours.Keys)
					{
						var subCheck = CalculateChecks(score - neighbour.Score, leftDarts - 1, worker, sth)
							?.FirstOrDefault();
						if(subCheck == null)
							continue;
						subCheck.Check.AufCheckDart = neighbour;
						subCheck.Check.Propability = subCheck.Check.Propability * field.Neighbours[neighbour];
						subCheck.Check.Calculation = subCheck.Check.Calculation * field.Neighbours[neighbour];
						prop += subCheck.Check.Propability;
						subChecks.Add(subCheck.Check);
					}
					var newCheck = new CheckViewModel(field, check.Check.CheckDart, null, prop, prop, "", subChecks);

					checks.Add(newCheck);
				}

				if (IsAFinish(score, leftDarts - 1))
				{
					var oneDartFinish = CalculateChecks(score, leftDarts - 1, worker, sth).Single();
					foreach (var neighbour in oneDartFinish.Check.CheckDart.Neighbours.Keys)
					{
						var subCheck = CalculateChecks(score - neighbour.Score, leftDarts - 1, worker, sth)
							?.FirstOrDefault();
						if (subCheck == null)
							continue;
						subCheck.Check.AufCheckDart = neighbour;
						subCheck.Check.Propability = subCheck.Check.Propability * oneDartFinish.Check.CheckDart.Neighbours[neighbour];
						subCheck.Check.Calculation = subCheck.Check.Calculation * oneDartFinish.Check.CheckDart.Neighbours[neighbour];
						oneDartFinish.Check.Propability += subCheck.Check.Propability;
						oneDartFinish.Check.Calculation += subCheck.Check.Calculation;
						oneDartFinish.Check.SubChecks.Add(subCheck.Check);
					}
					checks.Add(oneDartFinish);
				}

				return checks.OrderByDescending(x => x.Check.Propability).ToList();
			}

			var list = _dBoard.GetAllFields();
			for (var index = 0; index < list.Count; index++)
			{
				var field = list[index];
				var twoDartChecks = CalculateChecks(score - field.Score, leftDarts - 1, worker, sth);
				if (twoDartChecks == null)
					continue;
				foreach (var twoDartCheck in twoDartChecks)
				{
					if(twoDartCheck.Check.AufCheckDart != null)
						checks.Add(new CheckViewModel(field, twoDartCheck.Check.AufCheckDart, twoDartCheck.Check.CheckDart,
							twoDartCheck.Check.Propability * field.HitRatio,
							twoDartCheck.Check.Propability * field.HitRatio, "", null));
					else
						checks.Add(new CheckViewModel(field, twoDartCheck.Check.CheckDart, null,
							twoDartCheck.Check.Propability * field.HitRatio,
							twoDartCheck.Check.Propability * field.HitRatio, "", null));
				}

				worker.ReportProgress(index*100/list.Count);
			}
			if(IsAFinish(score, leftDarts-2))
				checks.AddRange(CalculateChecks(score, leftDarts - 2, worker, sth));
			return checks.OrderByDescending(x => x.Check.Propability).ToList();
		}

		private bool IsAFinish(int score, int leftDarts)
		{
			if (score < 2 || score > 170 || score == 159 || score == 162 || score == 163 || score == 165 ||
				score == 166 || score == 168 || score == 169)
				return false;
			if (leftDarts <= 2 && (score > 110 || score == 99 || score == 102 || score == 103 || score == 105 ||
				score == 106 || score == 108 || score == 109))
				return false;
			if (leftDarts == 1 && !(score < 41 && score % 2 == 0 || score == 50))
				return false;
			return true;
		}
	}
}
