using System.Collections.Generic;
using System.Linq;
using Dart.Base;
using DartBot.Player;

namespace CheckApp.Services
{
	public static class CheckSimulator
	{
		private class CacheObject
		{
			public CacheObject(int my, int sigma)
			{
				My = my;
				Sigma = sigma;
			}
			public int My { get; set; }
			public int Sigma { get; set; }
			public Dictionary<Field, double> Cache { get; set; } = new Dictionary<Field, double>();
			public Dictionary<Field, Dictionary<Field, double>> NeighborCache { get; set; } = new Dictionary<Field, Dictionary<Field, double>>();
		}

		private class SimulatorCache
		{
			private List<CacheObject> _objects = new List<CacheObject>();

			public SimulatorCache()
			{
			}

			public CacheObject GetCache(int my, int sigma)
			{
				foreach (var cache in _objects)
				{
					if (cache.My == my && cache.Sigma == sigma)
						return cache;
				}
				var newCache = new CacheObject(my, sigma);
				_objects.Add(newCache);
				return newCache;
			}
		}

		private static SimulatorCache CacheCollection = new SimulatorCache();
		//private static Dictionary<Field, double> Cache = new Dictionary<Field, double>();
		//private static Dictionary<Field, Dictionary<Field, double>> NeighborCache = new Dictionary<Field, Dictionary<Field, double>>();
		public static double GetSuccessRate(Field target, int my, int sigma)
		{
			var cache = CacheCollection.GetCache(my, sigma);
			if (cache.Cache.ContainsKey(target))
				return cache.Cache[target];

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
			cache.NeighborCache.Add(target, neighbors);

			var rate = (double) hits / tries;
			cache.Cache.Add(target, rate);
			return rate;
		}

		public static Dictionary<Field, double> GetNeighbors(Field target, int my, int sigma)
		{
			var cache = CacheCollection.GetCache(my, sigma);
			if (!cache.NeighborCache.ContainsKey(target))
				GetSuccessRate(target, my, sigma);
			return cache.NeighborCache[target];
		}
	}
}
