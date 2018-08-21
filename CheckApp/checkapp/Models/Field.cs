using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckApp
{
	public enum FieldType
	{
		Single,
		Double,
		Triple
	}
	public class Field
	{
		public Field()
		{
			Score = 0;
			Difficulty = 0;
			Type = FieldType.Single;
			Neighbours = new List<Field>();
		}
		public Field(int score, int difficulty, FieldType type)
		{
			Score = score;
			Difficulty = difficulty;
			Type = type;
			Neighbours = new List<Field>();
		}

		public int Score { get; set; }
		public int Difficulty { get; set; }
		public FieldType Type { get; set; }
		public List<Field> Neighbours { get; set; }
	}
}
