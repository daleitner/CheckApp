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

		public DartBoard()
		{
			InitializeDartboard();
		}


		private void InitializeDartboard()
		{
			_dartBoard = new List<Field>();

			var doubleFields = new List<Field>
			{
				new Field(2, _doubled[3], FieldType.Double),
				new Field(4, _doubled[1], FieldType.Double),
				new Field(6, _doubled[3], FieldType.Double),
				new Field(8, _doubled[0], FieldType.Double),
				new Field(10, _doubled[3], FieldType.Double),
				new Field(12, _doubled[1], FieldType.Double),
				new Field(14, _doubled[3], FieldType.Double),
				new Field(16, _doubled[0], FieldType.Double),
				new Field(18, _doubled[3], FieldType.Double),
				new Field(20, _doubled[1], FieldType.Double),
				new Field(22, _doubled[2], FieldType.Double),
				new Field(24, _doubled[0], FieldType.Double),
				new Field(26, _doubled[2], FieldType.Double),
				new Field(28, _doubled[1], FieldType.Double),
				new Field(30, _doubled[2], FieldType.Double),
				new Field(32, _doubled[0], FieldType.Double),
				new Field(34, _doubled[2], FieldType.Double),
				new Field(36, _doubled[1], FieldType.Double),
				new Field(38, _doubled[2], FieldType.Double),
				new Field(40, _doubled[0], FieldType.Double),
			};
			var singleFields = new List<Field>();
			foreach (Field f in doubleFields)
			{
				singleFields.Add(new Field(f.Score / 2, Singled, FieldType.Single));
			}
			
			var tripleFields = new List<Field>();
			foreach (Field f in singleFields)
			{
				tripleFields.Add(new Field(f.Score * 3, Tripled, FieldType.Triple));
			}
			
			var singleBull = new Field(25, Bulld, FieldType.Single);
			var doubleBull = new Field(50, Doublebulld, FieldType.Double);

			for (int i = 0; i < doubleFields.Count; i++)
			{
				doubleFields[i].Neighbours.Add(singleFields[i]);
				singleFields[i].Neighbours.Add(tripleFields[i]);
				tripleFields[i].Neighbours.Add(singleFields[i]);
				singleBull.Neighbours.Add(singleFields[i]);
				doubleBull.Neighbours.Add(singleFields[i]);
			}

			singleBull.Neighbours.Add(doubleBull);
			doubleBull.Neighbours.Add(singleBull);

			singleFields[0].Neighbours.Add(singleFields[19]);
			singleFields[0].Neighbours.Add(singleFields[17]);
			singleFields[1].Neighbours.Add(singleFields[16]);
			singleFields[1].Neighbours.Add(singleFields[14]);
			singleFields[2].Neighbours.Add(singleFields[18]);
			singleFields[2].Neighbours.Add(singleFields[16]);
			singleFields[3].Neighbours.Add(singleFields[17]);
			singleFields[3].Neighbours.Add(singleFields[12]);
			singleFields[4].Neighbours.Add(singleFields[19]);
			singleFields[4].Neighbours.Add(singleFields[11]);
			singleFields[5].Neighbours.Add(singleFields[12]);
			singleFields[5].Neighbours.Add(singleFields[9]);
			singleFields[6].Neighbours.Add(singleFields[18]);
			singleFields[6].Neighbours.Add(singleFields[15]);
			singleFields[7].Neighbours.Add(singleFields[15]);
			singleFields[7].Neighbours.Add(singleFields[10]);
			singleFields[8].Neighbours.Add(singleFields[11]);
			singleFields[8].Neighbours.Add(singleFields[13]);
			singleFields[9].Neighbours.Add(singleFields[14]);
			singleFields[9].Neighbours.Add(singleFields[5]);
			singleFields[10].Neighbours.Add(singleFields[13]);
			singleFields[10].Neighbours.Add(singleFields[7]);
			singleFields[11].Neighbours.Add(singleFields[4]);
			singleFields[11].Neighbours.Add(singleFields[8]);
			singleFields[12].Neighbours.Add(singleFields[5]);
			singleFields[12].Neighbours.Add(singleFields[3]);
			singleFields[13].Neighbours.Add(singleFields[10]);
			singleFields[13].Neighbours.Add(singleFields[8]);
			singleFields[14].Neighbours.Add(singleFields[9]);
			singleFields[14].Neighbours.Add(singleFields[1]);
			singleFields[15].Neighbours.Add(singleFields[6]);
			singleFields[15].Neighbours.Add(singleFields[7]);
			singleFields[16].Neighbours.Add(singleFields[2]);
			singleFields[16].Neighbours.Add(singleFields[1]);
			singleFields[17].Neighbours.Add(singleFields[0]);
			singleFields[17].Neighbours.Add(singleFields[3]);
			singleFields[18].Neighbours.Add(singleFields[6]);
			singleFields[18].Neighbours.Add(singleFields[2]);
			singleFields[19].Neighbours.Add(singleFields[0]);
			singleFields[19].Neighbours.Add(singleFields[4]);

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
