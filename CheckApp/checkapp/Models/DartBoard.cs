using System.Collections.Generic;
using System.Linq;

namespace CheckApp
{
	public class DartBoard
	{
		private List<Field> _dartBoard;

		private const int Doubled =  1;
		private const int Tripled = 1;
		private const int Singled = 1;
		private const int Bulld = 1;
		private const int Doublebulld = 1;

		public DartBoard()
		{
			InitializeDartboard();
		}


		private void InitializeDartboard()
		{
			_dartBoard = new List<Field>();

			var singleFields = new List<Field>();
			var doubleFields = new List<Field>();
			var tripleFields = new List<Field>();
			for (var i = 1; i <= 20; i++)
			{
				singleFields.Add(new Field(i, Singled, FieldType.Single, SingleQuotes[i-1]));
				doubleFields.Add(new Field(i * 2, Doubled, FieldType.Double, DoubleQuotes[i-1]));
				tripleFields.Add(new Field(i * 3, Tripled, FieldType.Triple, TripleQuotes[i-1]));
			}

			var singleBull = new Field(25, Bulld, FieldType.Single, SingleBullQuote);
			var doubleBull = new Field(50, Doublebulld, FieldType.Double, DoubleBullQuote);

			var outside = new Field(0, Singled, FieldType.Single, 0.0);

			var leftNeighbours = new List<Field>
			{
				singleFields[19],
				singleFields[14],
				singleFields[16],
				singleFields[17],
				singleFields[11],
				singleFields[12],
				singleFields[18],
				singleFields[15],
				singleFields[13],
				singleFields[5],
				singleFields[7],
				singleFields[8],
				singleFields[3],
				singleFields[10],
				singleFields[9],
				singleFields[6],
				singleFields[1],
				singleFields[0],
				singleFields[2],
				singleFields[4],
			};
			var rightNeighbours = new List<Field>
			{
				singleFields[17],
				singleFields[16],
				singleFields[18],
				singleFields[12],
				singleFields[19],
				singleFields[9],
				singleFields[15],
				singleFields[10],
				singleFields[11],
				singleFields[14],
				singleFields[13],
				singleFields[4],
				singleFields[5],
				singleFields[8],
				singleFields[1],
				singleFields[7],
				singleFields[2],
				singleFields[3],
				singleFields[6],
				singleFields[0],
			};

			for (int i = 0; i < doubleFields.Count; i++)
			{
				doubleFields[i].Neighbours.Add(singleFields[i], SingleWhenDoubleQuotes[i]);
				doubleFields[i].Neighbours.Add(outside, OutsideWhenDoubleQuotes[i]);

				singleFields[i].Neighbours.Add(tripleFields[i], TripleWhenSingleQuotes[i]);
				singleFields[i].Neighbours.Add(leftNeighbours[i], LeftNeighbourWhenSingle[i]);
				singleFields[i].Neighbours.Add(rightNeighbours[i], RightNeighbourWhenSingle[i]);

				tripleFields[i].Neighbours.Add(singleFields[i], SingleWhenTripleQuotes[i]);
				tripleFields[i].Neighbours.Add(leftNeighbours[i], LeftNeighbourWhenTriple[i]);
				tripleFields[i].Neighbours.Add(rightNeighbours[i], RightNeighbourWhenTriple[i]);

				singleBull.Neighbours.Add(singleFields[i], (1-singleBull.HitRatio - doubleBull.HitRatio)/20);
				doubleBull.Neighbours.Add(singleFields[i], (1-doubleBull.HitRatio - singleBull.HitRatio)/20);
				
			}

			singleBull.Neighbours.Add(doubleBull, doubleBull.HitRatio);
			doubleBull.Neighbours.Add(singleBull, singleBull.HitRatio);

			_dartBoard.AddRange(singleFields);
			_dartBoard.AddRange(doubleFields);
			_dartBoard.AddRange(tripleFields);
			_dartBoard.Add(singleBull);
			_dartBoard.Add(doubleBull);
			_dartBoard = _dartBoard.OrderBy(x => x.Score).ToList();
		}

		public List<Field> GetAllFields()
		{
			return _dartBoard;
		}

		public List<Field> GetAllDoubles()
		{
			return _dartBoard.Where(x => x.Type == FieldType.Double).ToList();
		}

		private static readonly List<double> SingleQuotes = new List<double>
		{
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65,
			0.65
		};
		private static readonly List<double> DoubleQuotes = new List<double>
		{
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15
		};
		private static readonly List<double> TripleQuotes = new List<double>
		{
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10
		};

		private static readonly double SingleBullQuote = 0.20;
		private static readonly double DoubleBullQuote = 0.10;

		private static readonly List<double> SingleWhenDoubleQuotes = new List<double>
		{
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425
		};

		private static readonly List<double> OutsideWhenDoubleQuotes = new List<double>
		{
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425,
			0.425
		};

		private static readonly List<double> TripleWhenSingleQuotes = new List<double>
		{
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05,
			0.05
		};

		private static readonly List<double> SingleWhenTripleQuotes = new List<double>
		{
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50,
			0.50
		};

		private static readonly List<double> LeftNeighbourWhenSingle = new List<double>
		{
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10
		};

		private static readonly List<double> RightNeighbourWhenSingle = new List<double>
		{
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15
		};

		private static readonly List<double> LeftNeighbourWhenTriple = new List<double>
		{
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10
		};

		private static readonly List<double> RightNeighbourWhenTriple = new List<double>
		{
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15,
			0.15
		};
	}
}
