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
				doubleFields[i].Neighbours.Add(leftNeighbours[i], LeftNeighbourWhenDouble[i]);
				doubleFields[i].Neighbours.Add(rightNeighbours[i], RightNeighbourWhenDouble[i]);

				singleFields[i].Neighbours.Add(tripleFields[i], TripleWhenSingleQuotes[i]);
				singleFields[i].Neighbours.Add(leftNeighbours[i], LeftNeighbourWhenSingle[i]);
				singleFields[i].Neighbours.Add(rightNeighbours[i], RightNeighbourWhenSingle[i]);
				singleFields[i].Neighbours.Add(doubleFields[i], DoubleWhenSingleQuotes[i]);
				singleFields[i].Neighbours.Add(outside, OutsideWhenSingleQuotes[i]);

				tripleFields[i].Neighbours.Add(singleFields[i], SingleWhenTripleQuotes[i]);
				tripleFields[i].Neighbours.Add(leftNeighbours[i], LeftNeighbourWhenTriple[i]);
				tripleFields[i].Neighbours.Add(rightNeighbours[i], RightNeighbourWhenTriple[i]);

				singleBull.Neighbours.Add(singleFields[i], SingleWhenBull[i]);
				doubleBull.Neighbours.Add(singleFields[i], SingleWhenBull[i]);
				
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

		#region singlequotes
		private static readonly List<double> SingleQuotes = new List<double>
		{
			0.60, //1 x
			0.65, //2
			0.65, //3
			0.65, //4
			0.65, //5
			0.65, //6
			0.65, //7
			0.65, //8
			0.65, //9
			0.65, //10
			0.65, //11
			0.65, //12
			0.65, //13
			0.65, //14
			0.65, //15
			0.65, //16
			0.65, //17
			0.65, //18
			0.65, //19
			0.65  //20 x
		};

		private static readonly List<double> TripleWhenSingleQuotes = new List<double>
		{
			0.05, //1 x
			0.05, //2
			0.05, //3
			0.05, //4
			0.05, //5
			0.05, //6
			0.05, //7
			0.05, //8
			0.05, //9
			0.05, //10
			0.05, //11
			0.05, //12
			0.05, //13
			0.05, //14
			0.05, //15
			0.05, //16
			0.05, //17
			0.05, //18
			0.05, //19
			0.05  //20 x
		};

		private static readonly List<double> DoubleWhenSingleQuotes = new List<double>
		{
			0.05, //1 x
			0.0, //2
			0.0, //3
			0.0, //4
			0.0, //5
			0.0, //6
			0.0, //7
			0.0, //8
			0.0, //9
			0.0, //10
			0.0, //11
			0.0, //12
			0.0, //13
			0.0, //14
			0.0, //15
			0.0, //16
			0.0, //17
			0.0, //18
			0.0, //19
			0.0  //20 x
		};

		private static readonly List<double> LeftNeighbourWhenSingle = new List<double>
		{
			0.10, //20 x
			0.10, //15
			0.10, //17
			0.10, //18
			0.10, //12
			0.10, //13
			0.10, //19
			0.10, //16
			0.10, //14
			0.10, //6
			0.10, //8
			0.10, //9
			0.10, //4
			0.10, //11
			0.10, //10
			0.10, //7
			0.10, //2
			0.10, //1
			0.10, //3
			0.10  //5 x
		};

		private static readonly List<double> RightNeighbourWhenSingle = new List<double>
		{
			0.15, //18 x
			0.15, //17
			0.15, //19
			0.15, //13
			0.15, //20
			0.15, //10
			0.15, //16
			0.15, //11
			0.15, //12
			0.15, //15
			0.15, //14
			0.15, //5
			0.15, //6
			0.15, //9
			0.15, //2
			0.15, //8
			0.15, //3
			0.15, //4
			0.15, //7
			0.15 //1 x
		};

		private static readonly List<double> OutsideWhenSingleQuotes = new List<double>
		{
			0.05, //1 x
			0.0, //2
			0.0, //3
			0.0, //4
			0.0, //5
			0.0, //6
			0.0, //7
			0.0, //8
			0.0, //9
			0.0, //10
			0.0, //11
			0.0, //12
			0.0, //13
			0.0, //14
			0.0, //15
			0.0, //16
			0.0, //17
			0.0, //18
			0.0, //19
			0.0  //20 x
		};
		#endregion

		#region doublequotes
		private static readonly List<double> DoubleQuotes = new List<double>
		{
			0.22, //1
			0.16, //2 x
			0.22, //3
			0.22, //4
			0.22, //5
			0.22, //6
			0.22, //7
			0.22, //8
			0.22, //9
			0.16, //10 x
			0.22, //11
			0.22, //12
			0.22, //13
			0.22, //14
			0.22, //15
			0.22, //16
			0.22, //17
			0.22, //18
			0.22, //19
			0.22  //20 x
		};

		private static readonly List<double> SingleWhenDoubleQuotes = new List<double>
		{
			0.40, //1
			0.20, //2 x
			0.40, //3
			0.40, //4
			0.40, //5
			0.40, //6
			0.40, //7
			0.40, //8
			0.40, //9
			0.30, //10 x
			0.40, //11
			0.40, //12
			0.40, //13
			0.40, //14
			0.40, //15
			0.40, //16
			0.40, //17
			0.40, //18
			0.40, //19
			0.40  //20 x
		};

		private static readonly List<double> OutsideWhenDoubleQuotes = new List<double>
		{
			0.30, //1
			0.50, //2 x
			0.30, //3
			0.30, //4
			0.30, //5
			0.30, //6
			0.30, //7
			0.30, //8
			0.30, //9
			0.40, //10 x
			0.30, //11
			0.30, //12
			0.30, //13
			0.30, //14
			0.30, //15
			0.30, //16
			0.30, //17
			0.30, //18
			0.30, //19
			0.30  //20 x
		};

		private static readonly List<double> LeftNeighbourWhenDouble = new List<double>
		{
			0.10, //20
			0.10, //15 x
			0.10, //17
			0.10, //18
			0.10, //12
			0.10, //13
			0.10, //19
			0.10, //16
			0.10, //14
			0.10, //6
			0.10, //8
			0.10, //9
			0.10, //4
			0.10, //11
			0.10, //10
			0.10, //7
			0.10, //2
			0.10, //1
			0.10, //3
			0.10  //5
		};

		private static readonly List<double> RightNeighbourWhenDouble = new List<double>
		{
			0.15, //18
			0.04, //17 x
			0.15, //19
			0.15, //13
			0.15, //20
			0.15, //10
			0.15, //16
			0.15, //11
			0.15, //12
			0.15, //15
			0.15, //14
			0.15, //5
			0.15, //6
			0.15, //9
			0.15, //2
			0.15, //8
			0.15, //3
			0.15, //4
			0.15, //7
			0.15 //1
		};
		#endregion

		#region triplequotes
		private static readonly List<double> TripleQuotes = new List<double>
		{
			0.10, //1
			0.10, //2
			0.10, //3
			0.10, //4
			0.10, //5
			0.10, //6
			0.10, //7
			0.10, //8
			0.10, //9
			0.10, //10
			0.10, //11
			0.10, //12
			0.10, //13
			0.10, //14
			0.10, //15
			0.10, //16
			0.10, //17
			0.10, //18
			0.10, //19
			0.10  //20 x
		};

		private static readonly List<double> SingleWhenTripleQuotes = new List<double>
		{
			0.50, //1
			0.50, //2
			0.50, //3
			0.50, //4
			0.50, //5
			0.50, //6
			0.50, //7
			0.50, //8
			0.50, //9
			0.50, //10
			0.50, //11
			0.50, //12
			0.50, //13
			0.50, //14
			0.50, //15
			0.50, //16
			0.50, //17
			0.50, //18
			0.50, //19
			0.50  //20 x
		};

		private static readonly List<double> LeftNeighbourWhenTriple = new List<double>
		{
			0.10, //20
			0.10, //15
			0.10, //17
			0.10, //18
			0.10, //12
			0.10, //13
			0.10, //19
			0.10, //16
			0.10, //14
			0.10, //6
			0.10, //8
			0.10, //9
			0.10, //4
			0.10, //11
			0.10, //10
			0.10, //7
			0.10, //2
			0.10, //1
			0.10, //3
			0.10  //5
		};

		private static readonly List<double> RightNeighbourWhenTriple = new List<double>
		{
			0.15, //18
			0.15, //17
			0.15, //19
			0.15, //13
			0.15, //20
			0.15, //10
			0.15, //16
			0.15, //11
			0.15, //12
			0.15, //15
			0.15, //14
			0.15, //5
			0.15, //6
			0.15, //9
			0.15, //2
			0.15, //8
			0.15, //3
			0.15, //4
			0.15, //7
			0.15 //1
		};
		#endregion

		#region bullquotes
		private static readonly double SingleBullQuote = 0.23;
		private static readonly double DoubleBullQuote = 0.05;

		private static readonly List<double> SingleWhenBull = new List<double>
		{
			0.05, //1
			0.02, //2
			0.10, //3
			0.03, //4
			0.03, //5
			0.03, //6
			0.04, //7
			0.03, //8
			0.02, //9
			0.03, //10
			0.03, //11
			0.02, //12
			0.03, //13
			0.02, //14
			0.02, //15
			0.04, //16
			0.05, //17
			0.04, //18
			0.06, //19
			0.03  //20
		};
		#endregion
	}
}
