using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CheckApp.Models;
using Dart.Base;

namespace CheckApp.Services
{
	public class CheckCalculator
	{
		private readonly DartBoard _dBoard;
		private readonly Config _config;
		public CheckCalculator(Config config)
		{
			_config = config;
			_dBoard = DartBoard.Instance;
			CheckSimulator.ClearCache();
		}

		public List<CheckViewModel> CalculateAll(int leftDarts, BackgroundWorker worker)
		{
			CheckSimulator.ClearCache();
			List<CheckViewModel> checks = new List<CheckViewModel>();
			var border = 170;
			if (leftDarts == 2)
				border = 110;
			else if (leftDarts == 1)
				border = 50;
			for (int i = 1; i <= border; i++)
			{
				var current = CalculateChecks(i, leftDarts, null);
				if (current == null)
					continue;

				checks.Add(current.First());

				worker.ReportProgress(i*100/border);
			}

			return checks;
		}

		public List<CheckViewModel> CalculateChecks(int score, int leftDarts, BackgroundWorker worker, bool clearCache = false)
		{
			if (!IsAFinish(score, leftDarts))
				return null;
			if(clearCache)
				CheckSimulator.ClearCache();

			if (leftDarts == 1)
				return HandleLastDart(score);

			if (leftDarts == 2)
				return HandleTwoDartFinishes(score);

			var checks = new List<CheckViewModel>();
			var list = GetRelevantFields();
			for (var index = 0; index < list.Count; index++)
			{
				worker?.ReportProgress(index * 100 / list.Count);
				var field = list[index];
				if (field.Value > score)
				{
					continue;
				}

				var oneDartFinish = score == field.Value && field.Type == FieldEnum.Double;
				List<CheckViewModel> currentChecks;
				var prop = 0.0;
				if (oneDartFinish)
				{
					currentChecks = CalculateChecks(score, leftDarts - 2, worker);
				}
				else
				{
					currentChecks = CalculateChecks(score - field.Value, leftDarts - 1, worker);
					if (currentChecks == null)
						continue;
				}

				var neighborSubChecks = new List<Check>();
				var dict = CheckSimulator.GetNeighbors(field, _config.My, _config.Sigma);
				foreach (var neighbor in dict.Keys)
				{
					if (score - neighbor.Value == 0 &&
					    (neighbor.Type == FieldEnum.Double || neighbor.Type == FieldEnum.DoubleBull))
					{
						var randomCheck = new CheckViewModel(neighbor, null, null, dict[neighbor]);
						prop += dict[neighbor];
						neighborSubChecks.Add(randomCheck.Check);
						continue;
					}
					var subCheck = CalculateChecks(score - neighbor.Value, leftDarts - 1, worker)
						?.FirstOrDefault();
					if (subCheck == null)
						continue;
					if (subCheck.Check.AufCheckDart == null)
						subCheck.Check.AufCheckDart = neighbor;
					else
						subCheck.Check.ScoreDart = neighbor;
					subCheck.Check.Propability = subCheck.Check.Propability * dict[neighbor];
					prop += subCheck.Check.Propability;
					neighborSubChecks.Add(subCheck.Check);
					subCheck.Check.SubChecks.ForEach(x =>
					{
						if (x.AufCheckDart == null)
							x.AufCheckDart = neighbor;
						else
							x.ScoreDart = neighbor;
						x.Propability = x.Propability * dict[neighbor];
					});
					neighborSubChecks.AddRange(subCheck.Check.SubChecks);
				}

				foreach (var currentCheck in currentChecks)
				{
					var propx = prop;
					var subChecks = new List<Check>(neighborSubChecks);
					if (!oneDartFinish)
					{
						currentCheck.Check.SubChecks.ForEach(x =>
						{
							x.ScoreDart = field;
							x.Propability = x.Propability * CheckSimulator.GetSuccessRate(field, _config.My, _config.Sigma);
							propx += x.Propability;
						});
						subChecks.AddRange(currentCheck.Check.SubChecks);
					}
						
					if (oneDartFinish)
					{
						var propa = CheckSimulator.GetSuccessRate(field, _config.My, _config.Sigma) + propx;
						checks.Add(new CheckViewModel(field, null, null, propa, subChecks));
					}
					else if (currentCheck.Check.AufCheckDart != null)
					{
						var propa = CheckSimulator.GetSuccessRate(field, _config.My, _config.Sigma) * CheckSimulator.GetSuccessRate(currentCheck.Check.AufCheckDart, _config.My, _config.Sigma) *
						            CheckSimulator.GetSuccessRate(currentCheck.Check.CheckDart, _config.My, _config.Sigma) + propx;
						checks.Add(new CheckViewModel(field, currentCheck.Check.AufCheckDart,
							currentCheck.Check.CheckDart, propa, subChecks));
					}
					else
					{
						var propa = CheckSimulator.GetSuccessRate(field, _config.My, _config.Sigma) * CheckSimulator.GetSuccessRate(currentCheck.Check.CheckDart, _config.My, _config.Sigma) + propx;
						checks.Add(new CheckViewModel(field, currentCheck.Check.CheckDart, null, propa, subChecks));
					}
				}
			}

			return checks.OrderByDescending(x => x.Check.Propability).ToList();
		}

		private List<CheckViewModel> HandleLastDart(int score)
		{
			if (!IsAFinish(score, 1))
				return null;
			var doubleField = GetAllDoubles().Single(x => x.Value == score);
			var doubleProp = CheckSimulator.GetSuccessRate(doubleField, _config.My, _config.Sigma);
			var check = new CheckViewModel(doubleField, null, null, doubleProp);
			return new List<CheckViewModel> {check};
		}

		private List<CheckViewModel> HandleTwoDartFinishes(int score)
		{
			var checks = new List<CheckViewModel>();
			foreach (var field in GetRelevantFields())
			{
				CheckViewModel check;
				var prop = 0.0;
				var oneDartFinish = score == field.Value && field.Type == FieldEnum.Double;
				if (oneDartFinish)
				{
					check = HandleLastDart(score).Single();
					prop = check.Check.Propability;
				}
				else
				{
					check = HandleLastDart(score - field.Value)?.FirstOrDefault();
					if (check == null)
						continue;
					prop = check.Check.Propability * CheckSimulator.GetSuccessRate(field, _config.My, _config.Sigma);
				}


				var subChecks = new List<Check>();
				var dict = CheckSimulator.GetNeighbors(field, _config.My, _config.Sigma);
				foreach (var neighbor in dict.Keys)
				{
					if (score - neighbor.Value == 0 && (neighbor.Type == FieldEnum.Double || neighbor.Type == FieldEnum.DoubleBull))
					{
						var randomCheck = new CheckViewModel(neighbor, null, null, dict[neighbor]);
						prop += dict[neighbor];
						subChecks.Add(randomCheck.Check);
						continue;
					}

					var subCheck = HandleLastDart(score - neighbor.Value)
						?.FirstOrDefault();
					if (subCheck == null)
						continue;
					subCheck.Check.AufCheckDart = neighbor;
					subCheck.Check.Propability = subCheck.Check.Propability * dict[neighbor];
					prop += subCheck.Check.Propability;
					subChecks.Add(subCheck.Check);
				}

				var newCheck = new CheckViewModel(field, check.Check.CheckDart, null, prop, subChecks);
				if (oneDartFinish)
					newCheck = new CheckViewModel(field, null, null, prop, subChecks);
				checks.Add(newCheck);
			}

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

		private List<Field> GetAllDoubles()
		{
			var list = _dBoard.GetFieldsByType(FieldEnum.Double).ToList();
			list.Add(_dBoard.GetDoubleBull());
			return list;
		}

		private List<Field> GetRelevantFields()
		{
			var list = _dBoard.GetFieldsByType(FieldEnum.SingleOut).ToList();
			list.AddRange(_dBoard.GetFieldsByType(FieldEnum.Double));
			list.AddRange(_dBoard.GetFieldsByType(FieldEnum.Triple));
			list.Add(_dBoard.GetSingleBull());
			list.Add(_dBoard.GetDoubleBull());
			return list;
		}
	}
}
