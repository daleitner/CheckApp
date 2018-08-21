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
        }
        public Field(int score, int difficulty, FieldType type)
        {
            Score = score;
            Difficulty = difficulty;
            Type = type;
        }

        public int Score { get; set; }
        public int Difficulty { get; set; }
        public FieldType Type { get; set; }
    }
}
