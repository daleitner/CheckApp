﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CheckApp
{
	public class NewCalculator
	{
		private readonly DartBoard _dBoard;
		private List<CheckViewModel> _doubleChecks;
		public NewCalculator()
		{
			_dBoard = new DartBoard();
		}

		public List<CheckViewModel> CalculateAll(BackgroundWorker worker)
		{
			List<CheckViewModel> checks = new List<CheckViewModel>();
			for (int i = 1; i <= 170; i++)
			{
				var current = CalculateChecks(i, 3, null, null, true);
				if (current == null)
					continue;

				checks.Add(current.First());

				worker.ReportProgress(i*100/170);
			}

			return checks;
		}

		public List<CheckViewModel> CalculateChecks(int score, int leftDarts, BackgroundWorker worker, List<bool> sth, bool setDoubleProp)
		{
			if (!IsAFinish(score, leftDarts))
				return null;

			if (leftDarts == 1)
			{
				var doubleField = _dBoard.GetAllDoubles().Single(x => x.Score == score);
				var doubleProp = 0.0;
				if (setDoubleProp)
				{
					if (_doubleChecks == null)
					{
						CalculateDoubles();
					}

					doubleProp = _doubleChecks.Single(x => (x.Check.CheckDart.Score + x.Check.AufCheckDart?.Score ?? x.Check.CheckDart.Score) == score).Check.Propability;
				}
				var check = new CheckViewModel(doubleField, null, null, doubleField.HitRatio, doubleProp, doubleField.HitRatio, "", null);
				if (check.Check.Propability <= 0.0)
					return null;

				return new List<CheckViewModel> {check};
			}
			var checks = new List<CheckViewModel>();
			if (leftDarts == 2)
			{
				foreach (var field in _dBoard.GetAllFields())
				{
					CheckViewModel check;
					var prop = 0.0;
					var oneDartFinish = score == field.Score && field.Type == FieldType.Double;
					if (oneDartFinish)
					{
						check = CalculateChecks(score, leftDarts - 1, worker, sth, setDoubleProp).Single();
						prop = check.Check.Propability;
					}
					else
					{
						check = CalculateChecks(score - field.Score, leftDarts - 1, worker, sth, setDoubleProp)?.FirstOrDefault();
						if (check == null)
							continue;
						prop = check.Check.Propability * field.HitRatio;
					}

					
					var subChecks = new List<Check>();
					foreach (var neighbour in field.Neighbours.Keys)
					{
						if (field.Neighbours[neighbour] <= 0.0)
							continue;
						var subCheck = CalculateChecks(score - neighbour.Score, leftDarts - 1, worker, sth, setDoubleProp)
							?.FirstOrDefault();
						if(subCheck == null)
							continue;
						subCheck.Check.AufCheckDart = neighbour;
						subCheck.Check.Propability = subCheck.Check.Propability * field.Neighbours[neighbour];
						subCheck.Check.Calculation = subCheck.Check.Calculation * field.Neighbours[neighbour];
						prop += subCheck.Check.Propability;
						subChecks.Add(subCheck.Check);
					}
					var newCheck = new CheckViewModel(field, check.Check.CheckDart, null, prop, check.Check.DoublePropability, prop, "", subChecks);
					if(oneDartFinish)
						newCheck = new CheckViewModel(field, null, null, prop, check.Check.DoublePropability, prop, "", subChecks);
					checks.Add(newCheck);
				}

				return checks.OrderByDescending(x => x.Check.Propability).ThenByDescending(x => x.Check.DoublePropability).ToList();
			}

			var list = _dBoard.GetAllFields();
			for (var index = 0; index < list.Count; index++)
			{
				var field = list[index];
				var oneDartFinish = score == field.Score && field.Type == FieldType.Double;
				List<CheckViewModel> currentChecks;
				var prop = 0.0;
				if (oneDartFinish)
				{
					currentChecks = CalculateChecks(score, leftDarts - 2, worker, sth, setDoubleProp);
				}
				else
				{
					currentChecks = CalculateChecks(score - field.Score, leftDarts - 1, worker, sth, setDoubleProp);
					if (currentChecks == null)
					{
						worker?.ReportProgress(index * 100 / list.Count);
						continue;
					}
				}

				var neighbourSubChecks = new List<Check>();
				foreach (var neighbour in field.Neighbours.Keys)
				{
					if(field.Neighbours[neighbour] <= 0.0)
						continue;
					var subCheck = CalculateChecks(score - neighbour.Score, leftDarts - 1, worker, sth, setDoubleProp)
						?.FirstOrDefault();
					if (subCheck == null)
						continue;
					if (subCheck.Check.AufCheckDart == null)
						subCheck.Check.AufCheckDart = neighbour;
					else
						subCheck.Check.ScoreDart = neighbour;
					subCheck.Check.Propability = subCheck.Check.Propability * field.Neighbours[neighbour];
					subCheck.Check.Calculation = subCheck.Check.Calculation * field.Neighbours[neighbour];
					prop += subCheck.Check.Propability;
					neighbourSubChecks.Add(subCheck.Check);
					subCheck.Check.SubChecks.ForEach(x =>
					{
						if (x.AufCheckDart == null)
							x.AufCheckDart = neighbour;
						else
							x.ScoreDart = neighbour;
						x.Propability = x.Propability * field.Neighbours[neighbour];
						x.Calculation = x.Calculation * field.Neighbours[neighbour];
					});
					neighbourSubChecks.AddRange(subCheck.Check.SubChecks);
				}

				foreach (var currentCheck in currentChecks)
				{
					var propx = prop;
					var subChecks = new List<Check>(neighbourSubChecks);
					if (!oneDartFinish)
					{
						currentCheck.Check.SubChecks.ForEach(x =>
						{
							x.ScoreDart = field;
							x.Propability = x.Propability * field.HitRatio;
							x.Calculation = x.Calculation * field.HitRatio;
							propx += x.Propability;
						});
						subChecks.AddRange(currentCheck.Check.SubChecks);
					}
						
					if (oneDartFinish)
					{
						var propa = field.HitRatio + propx;
						checks.Add(new CheckViewModel(field, null, null, propa, currentCheck.Check.DoublePropability, propa, "", subChecks));
					}
					else if (currentCheck.Check.AufCheckDart != null)
					{
						var propa = field.HitRatio * currentCheck.Check.AufCheckDart.HitRatio *
									currentCheck.Check.CheckDart.HitRatio + propx;
						checks.Add(new CheckViewModel(field, currentCheck.Check.AufCheckDart,
							currentCheck.Check.CheckDart, propa, currentCheck.Check.DoublePropability, propa, "", subChecks));
					}
					else
					{
						var propa = field.HitRatio * currentCheck.Check.CheckDart.HitRatio + propx;
						checks.Add(new CheckViewModel(field, currentCheck.Check.CheckDart, null,
							propa, currentCheck.Check.DoublePropability, propa, "", subChecks));
					}
				}

				worker?.ReportProgress(index*100/list.Count);
			}

			return checks.OrderByDescending(x => x.Check.Propability).ThenByDescending(x => x.Check.DoublePropability).ToList();
		}

		private void CalculateDoubles()
		{
			_doubleChecks = new List<CheckViewModel>();
			for (int i = 2; i < 51; i++)
			{
				if (!IsAFinish(i, 1))
					continue;
				_doubleChecks.Add(CalculateChecks(i, 3, null, null, false).First());
			}
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
