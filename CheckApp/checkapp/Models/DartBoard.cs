using System.Collections.Generic;
using System.Linq;

namespace CheckApp
{
	public class DartBoard
	{
		private List<Field> _dartBoard;

		private readonly List<int> _doubled = new List<int> { 1, 2, 3, 4 };
		private const int Tripled = 1;
		private const int Singled = 1;
		private const int Bulld = 1;
		private const int Doublebulld = 1;

		public DartBoard(double singleQuote, double doubleQuote, double tripleQuote)
		{
			InitializeDartboard(singleQuote, doubleQuote, tripleQuote);
		}


		private void InitializeDartboard(double singleQuote, double doubleQuote, double tripleQuote)
		{
			_dartBoard = new List<Field>();

			var doubleFields = new List<Field>
			{
				new Field(2, _doubled[3], FieldType.Double, doubleQuote),
				new Field(4, _doubled[1], FieldType.Double, doubleQuote),
				new Field(6, _doubled[3], FieldType.Double, doubleQuote),
				new Field(8, _doubled[0], FieldType.Double, doubleQuote),
				new Field(10, _doubled[3], FieldType.Double, doubleQuote),
				new Field(12, _doubled[1], FieldType.Double, doubleQuote),
				new Field(14, _doubled[3], FieldType.Double, doubleQuote),
				new Field(16, _doubled[0], FieldType.Double, doubleQuote),
				new Field(18, _doubled[3], FieldType.Double, doubleQuote),
				new Field(20, _doubled[1], FieldType.Double, doubleQuote),
				new Field(22, _doubled[2], FieldType.Double, doubleQuote),
				new Field(24, _doubled[0], FieldType.Double, doubleQuote),
				new Field(26, _doubled[2], FieldType.Double, doubleQuote),
				new Field(28, _doubled[1], FieldType.Double, doubleQuote),
				new Field(30, _doubled[2], FieldType.Double, doubleQuote),
				new Field(32, _doubled[0], FieldType.Double, doubleQuote),
				new Field(34, _doubled[2], FieldType.Double, doubleQuote),
				new Field(36, _doubled[1], FieldType.Double, doubleQuote),
				new Field(38, _doubled[2], FieldType.Double, doubleQuote),
				new Field(40, _doubled[0], FieldType.Double, doubleQuote),
			};
			var singleFields = new List<Field>();
			foreach (Field f in doubleFields)
			{
				singleFields.Add(new Field(f.Score / 2, Singled, FieldType.Single, singleQuote));
			}
			
			var tripleFields = new List<Field>();
			foreach (Field f in singleFields)
			{
				tripleFields.Add(new Field(f.Score * 3, Tripled, FieldType.Triple, tripleQuote));
			}
			
			var singleBull = new Field(25, Bulld, FieldType.Single, singleQuote/2);
			var doubleBull = new Field(50, Doublebulld, FieldType.Double, tripleQuote);
			var outside = new Field(0, Singled, FieldType.Single, singleQuote);

			for (int i = 0; i < doubleFields.Count; i++)
			{
				doubleFields[i].Neighbours.Add(singleFields[i], singleFields[i].HitRatio);
				singleFields[i].Neighbours.Add(tripleFields[i], tripleFields[i].HitRatio);
				tripleFields[i].Neighbours.Add(singleFields[i], singleFields[i].HitRatio);
				singleBull.Neighbours.Add(singleFields[i], singleFields[i].HitRatio);
				doubleBull.Neighbours.Add(singleFields[i], singleFields[i].HitRatio);
				doubleFields[i].Neighbours.Add(outside, outside.HitRatio);
			}

			singleBull.Neighbours.Add(doubleBull, doubleBull.HitRatio);
			doubleBull.Neighbours.Add(singleBull, singleBull.HitRatio);

			singleFields[0].Neighbours.Add(singleFields[19],  singleFields[19].HitRatio);
			singleFields[0].Neighbours.Add(singleFields[17],  singleFields[17].HitRatio);
			singleFields[1].Neighbours.Add(singleFields[16],  singleFields[16].HitRatio);
			singleFields[1].Neighbours.Add(singleFields[14],  singleFields[14].HitRatio);
			singleFields[2].Neighbours.Add(singleFields[18],  singleFields[18].HitRatio);
			singleFields[2].Neighbours.Add(singleFields[16],  singleFields[16].HitRatio);
			singleFields[3].Neighbours.Add(singleFields[17],  singleFields[17].HitRatio);
			singleFields[3].Neighbours.Add(singleFields[12],  singleFields[12].HitRatio);
			singleFields[4].Neighbours.Add(singleFields[19],  singleFields[19].HitRatio);
			singleFields[4].Neighbours.Add(singleFields[11],  singleFields[11].HitRatio);
			singleFields[5].Neighbours.Add(singleFields[12],  singleFields[12].HitRatio);
			singleFields[5].Neighbours.Add(singleFields[9],   singleFields[9].HitRatio);
			singleFields[6].Neighbours.Add(singleFields[18],  singleFields[18].HitRatio);
			singleFields[6].Neighbours.Add(singleFields[15],  singleFields[15].HitRatio);
			singleFields[7].Neighbours.Add(singleFields[15],  singleFields[15].HitRatio);
			singleFields[7].Neighbours.Add(singleFields[10],  singleFields[10].HitRatio);
			singleFields[8].Neighbours.Add(singleFields[11],  singleFields[11].HitRatio);
			singleFields[8].Neighbours.Add(singleFields[13],  singleFields[13].HitRatio);
			singleFields[9].Neighbours.Add(singleFields[14],  singleFields[14].HitRatio);
			singleFields[9].Neighbours.Add(singleFields[5],   singleFields[5].HitRatio);
			singleFields[10].Neighbours.Add(singleFields[13], singleFields[13].HitRatio);
			singleFields[10].Neighbours.Add(singleFields[7],  singleFields[7].HitRatio);
			singleFields[11].Neighbours.Add(singleFields[4],  singleFields[4].HitRatio);
			singleFields[11].Neighbours.Add(singleFields[8],  singleFields[8].HitRatio);
			singleFields[12].Neighbours.Add(singleFields[5],  singleFields[5].HitRatio);
			singleFields[12].Neighbours.Add(singleFields[3],  singleFields[3].HitRatio);
			singleFields[13].Neighbours.Add(singleFields[10], singleFields[10].HitRatio);
			singleFields[13].Neighbours.Add(singleFields[8],  singleFields[8].HitRatio);
			singleFields[14].Neighbours.Add(singleFields[9],  singleFields[9].HitRatio);
			singleFields[14].Neighbours.Add(singleFields[1],  singleFields[1].HitRatio);
			singleFields[15].Neighbours.Add(singleFields[6],  singleFields[6].HitRatio);
			singleFields[15].Neighbours.Add(singleFields[7],  singleFields[7].HitRatio);
			singleFields[16].Neighbours.Add(singleFields[2],  singleFields[2].HitRatio);
			singleFields[16].Neighbours.Add(singleFields[1],  singleFields[1].HitRatio);
			singleFields[17].Neighbours.Add(singleFields[0],  singleFields[0].HitRatio);
			singleFields[17].Neighbours.Add(singleFields[3],  singleFields[3].HitRatio);
			singleFields[18].Neighbours.Add(singleFields[6],  singleFields[6].HitRatio);
			singleFields[18].Neighbours.Add(singleFields[2],  singleFields[2].HitRatio);
			singleFields[19].Neighbours.Add(singleFields[0],  singleFields[0].HitRatio);
			singleFields[19].Neighbours.Add(singleFields[4],  singleFields[4].HitRatio);

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
	}
}
