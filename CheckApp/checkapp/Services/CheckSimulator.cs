using System.Collections.Generic;
using System.Linq;
using Dart.Base;
using DartBot.Player;

namespace CheckApp.Services
{
	public static class CheckSimulator
	{
		private static Dictionary<Field, double> Cache = new Dictionary<Field, double>();
		private static Dictionary<Field, Dictionary<Field, double>> NeighborCache = new Dictionary<Field, Dictionary<Field, double>>();
		public static double GetSuccessRate(Field target, int my, int sigma)
		{
			if (Cache.ContainsKey(target))
				return Cache[target];

			var bot = new PlayerHand();
			bot.AssignHitQuotes(my, sigma);

			var hits = 0;
			var tries = 100000;
			var neighbors = new Dictionary<Field, double>();
			for (int i = 0; i < tries; i++)
			{
				var hit = bot.ThrowDart(target);
				if (hit == target || (hit.Value == target.Value && hit.Type == FieldEnum.SingleIn))
					hits++;
				else
				{
					if(!neighbors.ContainsKey(hit))
						neighbors.Add(hit, 0);
					neighbors[hit]++;
				}
					
			}

			var keys = neighbors.Keys.ToList();

			foreach (var key in keys)
			{
				neighbors[key] = neighbors[key] / tries;
			}
			NeighborCache.Add(target, neighbors);

			var rate = (double) hits / tries;
			Cache.Add(target, rate);
			return rate;
		}

		public static Dictionary<Field, double> GetNeighbors(Field target, int my, int sigma)
		{
			if(!NeighborCache.ContainsKey(target))
				GetSuccessRate(target, my, sigma);
			return NeighborCache[target];
		}

		public static void ClearCache()
		{
			Cache = new Dictionary<Field, double>();
			NeighborCache = new Dictionary<Field, Dictionary<Field, double>>();
		}
	}
}
