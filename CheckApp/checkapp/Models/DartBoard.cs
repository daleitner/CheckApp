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
			
			var singleBull = new Field(25, Bulld, FieldType.Single, 0.2);
			var doubleBull = new Field(50, Doublebulld, FieldType.Double, tripleQuote);
			var outside = new Field(0, Singled, FieldType.Single, (1-doubleQuote)/2);

			for (int i = 0; i < doubleFields.Count; i++)
			{
				doubleFields[i].Neighbours.Add(singleFields[i], (1 - doubleQuote) / 2);
				singleFields[i].Neighbours.Add(tripleFields[i], tripleFields[i].HitRatio/2);
				tripleFields[i].Neighbours.Add(singleFields[i], singleFields[i].HitRatio);
				singleBull.Neighbours.Add(singleFields[i], (1-singleBull.HitRatio - doubleBull.HitRatio)/20);
				doubleBull.Neighbours.Add(singleFields[i], (1-doubleBull.HitRatio - singleBull.HitRatio)/20);
				doubleFields[i].Neighbours.Add(outside, outside.HitRatio);
			}

			singleBull.Neighbours.Add(doubleBull, doubleBull.HitRatio);
			doubleBull.Neighbours.Add(singleBull, singleBull.HitRatio);

			singleFields[0].Neighbours.Add(singleFields[19],  (1-singleFields[19].HitRatio-0.05)/2);
			singleFields[0].Neighbours.Add(singleFields[17],  (1-singleFields[17].HitRatio-0.05)/2);
			singleFields[1].Neighbours.Add(singleFields[16],  (1-singleFields[16].HitRatio-0.05)/2);
			singleFields[1].Neighbours.Add(singleFields[14],  (1-singleFields[14].HitRatio-0.05)/2);
			singleFields[2].Neighbours.Add(singleFields[18],  (1-singleFields[18].HitRatio-0.05)/2);
			singleFields[2].Neighbours.Add(singleFields[16],  (1-singleFields[16].HitRatio-0.05)/2);
			singleFields[3].Neighbours.Add(singleFields[17],  (1-singleFields[17].HitRatio-0.05)/2);
			singleFields[3].Neighbours.Add(singleFields[12],  (1-singleFields[12].HitRatio-0.05)/2);
			singleFields[4].Neighbours.Add(singleFields[19],  (1-singleFields[19].HitRatio-0.05)/2);
			singleFields[4].Neighbours.Add(singleFields[11],  (1-singleFields[11].HitRatio-0.05)/2);
			singleFields[5].Neighbours.Add(singleFields[12],  (1-singleFields[12].HitRatio-0.05)/2);
			singleFields[5].Neighbours.Add(singleFields[9],   (1-singleFields[9].HitRatio -0.05)/2);
			singleFields[6].Neighbours.Add(singleFields[18],  (1-singleFields[18].HitRatio-0.05)/2);
			singleFields[6].Neighbours.Add(singleFields[15],  (1-singleFields[15].HitRatio-0.05)/2);
			singleFields[7].Neighbours.Add(singleFields[15],  (1-singleFields[15].HitRatio-0.05)/2);
			singleFields[7].Neighbours.Add(singleFields[10],  (1-singleFields[10].HitRatio-0.05)/2);
			singleFields[8].Neighbours.Add(singleFields[11],  (1-singleFields[11].HitRatio-0.05)/2);
			singleFields[8].Neighbours.Add(singleFields[13],  (1-singleFields[13].HitRatio-0.05)/2);
			singleFields[9].Neighbours.Add(singleFields[14],  (1-singleFields[14].HitRatio-0.05)/2);
			singleFields[9].Neighbours.Add(singleFields[5],   (1-singleFields[5].HitRatio -0.05)/2);
			singleFields[10].Neighbours.Add(singleFields[13], (1-singleFields[13].HitRatio-0.05)/2);
			singleFields[10].Neighbours.Add(singleFields[7],  (1-singleFields[7].HitRatio -0.05)/2);
			singleFields[11].Neighbours.Add(singleFields[4],  (1-singleFields[4].HitRatio -0.05)/2);
			singleFields[11].Neighbours.Add(singleFields[8],  (1-singleFields[8].HitRatio -0.05)/2);
			singleFields[12].Neighbours.Add(singleFields[5],  (1-singleFields[5].HitRatio -0.05)/2);
			singleFields[12].Neighbours.Add(singleFields[3],  (1-singleFields[3].HitRatio -0.05)/2);
			singleFields[13].Neighbours.Add(singleFields[10], (1-singleFields[10].HitRatio-0.05)/2);
			singleFields[13].Neighbours.Add(singleFields[8],  (1-singleFields[8].HitRatio -0.05)/2);
			singleFields[14].Neighbours.Add(singleFields[9],  (1-singleFields[9].HitRatio -0.05)/2);
			singleFields[14].Neighbours.Add(singleFields[1],  (1-singleFields[1].HitRatio -0.05)/2);
			singleFields[15].Neighbours.Add(singleFields[6],  (1-singleFields[6].HitRatio -0.05)/2);
			singleFields[15].Neighbours.Add(singleFields[7],  (1-singleFields[7].HitRatio -0.05)/2);
			singleFields[16].Neighbours.Add(singleFields[2],  (1-singleFields[2].HitRatio -0.05)/2);
			singleFields[16].Neighbours.Add(singleFields[1],  (1-singleFields[1].HitRatio -0.05)/2);
			singleFields[17].Neighbours.Add(singleFields[0],  (1-singleFields[0].HitRatio -0.05)/2);
			singleFields[17].Neighbours.Add(singleFields[3],  (1-singleFields[3].HitRatio -0.05)/2);
			singleFields[18].Neighbours.Add(singleFields[6],  (1-singleFields[6].HitRatio -0.05)/2);
			singleFields[18].Neighbours.Add(singleFields[2],  (1-singleFields[2].HitRatio -0.05)/2);
			singleFields[19].Neighbours.Add(singleFields[0],  (1-singleFields[0].HitRatio -0.05)/2);
			singleFields[19].Neighbours.Add(singleFields[4],  (1-singleFields[4].HitRatio -0.05)/2);

			tripleFields[0].Neighbours.Add(singleFields[19], (1 - singleFields[19].HitRatio - 0.1) / 2);
			tripleFields[0].Neighbours.Add(singleFields[17], (1 - singleFields[17].HitRatio - 0.1) / 2);
			tripleFields[1].Neighbours.Add(singleFields[16], (1 - singleFields[16].HitRatio - 0.1) / 2);
			tripleFields[1].Neighbours.Add(singleFields[14], (1 - singleFields[14].HitRatio - 0.1) / 2);
			tripleFields[2].Neighbours.Add(singleFields[18], (1 - singleFields[18].HitRatio - 0.1) / 2);
			tripleFields[2].Neighbours.Add(singleFields[16], (1 - singleFields[16].HitRatio - 0.1) / 2);
			tripleFields[3].Neighbours.Add(singleFields[17], (1 - singleFields[17].HitRatio - 0.1) / 2);
			tripleFields[3].Neighbours.Add(singleFields[12], (1 - singleFields[12].HitRatio - 0.1) / 2);
			tripleFields[4].Neighbours.Add(singleFields[19], (1 - singleFields[19].HitRatio - 0.1) / 2);
			tripleFields[4].Neighbours.Add(singleFields[11], (1 - singleFields[11].HitRatio - 0.1) / 2);
			tripleFields[5].Neighbours.Add(singleFields[12], (1 - singleFields[12].HitRatio - 0.1) / 2);
			tripleFields[5].Neighbours.Add(singleFields[9],  (1 - singleFields[9].HitRatio -  0.1) / 2);
			tripleFields[6].Neighbours.Add(singleFields[18], (1 - singleFields[18].HitRatio - 0.1) / 2);
			tripleFields[6].Neighbours.Add(singleFields[15], (1 - singleFields[15].HitRatio - 0.1) / 2);
			tripleFields[7].Neighbours.Add(singleFields[15], (1 - singleFields[15].HitRatio - 0.1) / 2);
			tripleFields[7].Neighbours.Add(singleFields[10], (1 - singleFields[10].HitRatio - 0.1) / 2);
			tripleFields[8].Neighbours.Add(singleFields[11], (1 - singleFields[11].HitRatio - 0.1) / 2);
			tripleFields[8].Neighbours.Add(singleFields[13], (1 - singleFields[13].HitRatio - 0.1) / 2);
			tripleFields[9].Neighbours.Add(singleFields[14], (1 - singleFields[14].HitRatio - 0.1) / 2);
			tripleFields[9].Neighbours.Add(singleFields[5],  (1 - singleFields[5].HitRatio -  0.1) / 2);
			tripleFields[10].Neighbours.Add(singleFields[13],(1 - singleFields[13].HitRatio - 0.1) / 2);
			tripleFields[10].Neighbours.Add(singleFields[7], (1 - singleFields[7].HitRatio -  0.1) / 2);
			tripleFields[11].Neighbours.Add(singleFields[4], (1 - singleFields[4].HitRatio -  0.1) / 2);
			tripleFields[11].Neighbours.Add(singleFields[8], (1 - singleFields[8].HitRatio -  0.1) / 2);
			tripleFields[12].Neighbours.Add(singleFields[5], (1 - singleFields[5].HitRatio -  0.1) / 2);
			tripleFields[12].Neighbours.Add(singleFields[3], (1 - singleFields[3].HitRatio -  0.1) / 2);
			tripleFields[13].Neighbours.Add(singleFields[10],(1 - singleFields[10].HitRatio - 0.1) / 2);
			tripleFields[13].Neighbours.Add(singleFields[8], (1 - singleFields[8].HitRatio -  0.1) / 2);
			tripleFields[14].Neighbours.Add(singleFields[9], (1 - singleFields[9].HitRatio -  0.1) / 2);
			tripleFields[14].Neighbours.Add(singleFields[1], (1 - singleFields[1].HitRatio -  0.1) / 2);
			tripleFields[15].Neighbours.Add(singleFields[6], (1 - singleFields[6].HitRatio -  0.1) / 2);
			tripleFields[15].Neighbours.Add(singleFields[7], (1 - singleFields[7].HitRatio -  0.1) / 2);
			tripleFields[16].Neighbours.Add(singleFields[2], (1 - singleFields[2].HitRatio -  0.1) / 2);
			tripleFields[16].Neighbours.Add(singleFields[1], (1 - singleFields[1].HitRatio -  0.1) / 2);
			tripleFields[17].Neighbours.Add(singleFields[0], (1 - singleFields[0].HitRatio -  0.1) / 2);
			tripleFields[17].Neighbours.Add(singleFields[3], (1 - singleFields[3].HitRatio -  0.1) / 2);
			tripleFields[18].Neighbours.Add(singleFields[6], (1 - singleFields[6].HitRatio -  0.1) / 2);
			tripleFields[18].Neighbours.Add(singleFields[2], (1 - singleFields[2].HitRatio -  0.1) / 2);
			tripleFields[19].Neighbours.Add(singleFields[0], (1 - singleFields[0].HitRatio -  0.1) / 2);
			tripleFields[19].Neighbours.Add(singleFields[4], (1 - singleFields[4].HitRatio -  0.1) / 2);
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
