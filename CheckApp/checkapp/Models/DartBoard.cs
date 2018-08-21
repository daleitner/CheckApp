using System.Collections.Generic;

namespace CheckApp
{
	public class DartBoard
	{
		private List<List<Field>> _dartBoard;

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
			_dartBoard = new List<List<Field>>();

			var temp = new List<Field>
			{
				new Field(40, _doubled[0], FieldType.Double),
				new Field(2, _doubled[3], FieldType.Double),
				new Field(36, _doubled[1], FieldType.Double),
				new Field(8, _doubled[0], FieldType.Double),
				new Field(26, _doubled[2], FieldType.Double),
				new Field(12, _doubled[1], FieldType.Double),
				new Field(20, _doubled[1], FieldType.Double),
				new Field(30, _doubled[2], FieldType.Double),
				new Field(4, _doubled[1], FieldType.Double),
				new Field(34, _doubled[2], FieldType.Double),
				new Field(6, _doubled[3], FieldType.Double),
				new Field(38, _doubled[2], FieldType.Double),
				new Field(14, _doubled[3], FieldType.Double),
				new Field(32, _doubled[0], FieldType.Double),
				new Field(16, _doubled[0], FieldType.Double),
				new Field(22, _doubled[2], FieldType.Double),
				new Field(28, _doubled[1], FieldType.Double),
				new Field(18, _doubled[3], FieldType.Double),
				new Field(24, _doubled[0], FieldType.Double),
				new Field(10, _doubled[3], FieldType.Double)
			};
			_dartBoard.Add(temp);
			temp = new List<Field>();
			foreach (Field f in _dartBoard[0])
			{
				temp.Add(new Field(f.Score / 2, Singled, FieldType.Single));
			}
			_dartBoard.Add(temp);
			temp = new List<Field>();
			foreach (Field f in _dartBoard[1])
			{
				temp.Add(new Field(f.Score * 3, Tripled, FieldType.Triple));
			}
			_dartBoard.Add(temp);
			_dartBoard.Add(new List<Field> { new Field(25, Bulld, FieldType.Single), new Field(50, Doublebulld, FieldType.Double) });
		}

		public Field GetFirstField()
		{
			return _dartBoard[1][0];
		}

		public Field GetBull()
		{
			return _dartBoard[3][0];
		}

		public Field GetLeft(Field src)
		{
			Field ret;
			var check = false;
			var x = 0;
			var y = 0;
			for (var i = 0; i < _dartBoard.Count && !check; i++)
			{
				for (var j = 0; j < _dartBoard[i].Count && !check; j++)
				{
					if (!_dartBoard[i][j].Equals(src))
						continue;

					x = i;
					y = j;
					check = true;
				}
			}

			if (!check)
				return null;

			if (x == 3)
			{
				ret = y == 0 ? _dartBoard[x][y + 1] : _dartBoard[x][y - 1];
			}
			else
			{
				ret = y == 0 ? _dartBoard[x][_dartBoard[x].Count - 1] : _dartBoard[x][y - 1];
			}
			return ret;
		}

		public Field GetRight(Field src)
		{
			Field ret;
			var check = false;
			var x = 0;
			var y = 0;
			for (var i = 0; i < _dartBoard.Count && !check; i++)
			{
				for (var j = 0; j < _dartBoard[i].Count && !check; j++)
				{
					if (!_dartBoard[i][j].Equals(src))
						continue;

					x = i;
					y = j;
					check = true;
				}
			}

			if (!check)
				return null;

			if (x == 3)
			{
				ret = y == 0 ? _dartBoard[x][y + 1] : _dartBoard[x][y - 1];
			}
			else
			{
				ret = y == _dartBoard[x].Count - 1 ? _dartBoard[x][0] : _dartBoard[x][y + 1];
			}
			return ret;
		}

		public Field GetSingle(Field src)
		{
			Field ret = null;
			var check = false;
			var x = 0;
			var y = 0;
			for (var i = 0; i < _dartBoard.Count && !check; i++)
			{
				for (var j = 0; j < _dartBoard[i].Count && !check; j++)
				{
					if (!_dartBoard[i][j].Equals(src))
						continue;

					x = i;
					y = j;
					check = true;
				}
			}

			if (check)
			{
				ret = x == 3 ? _dartBoard[x][0] : _dartBoard[1][y];
			}
			return ret;
		}

		public Field GetDouble(Field src)
		{
			Field ret = null;
			var check = false;
			var x = 0;
			var y = 0;
			for (var i = 0; i < _dartBoard.Count && !check; i++)
			{
				for (var j = 0; j < _dartBoard[i].Count && !check; j++)
				{
					if (!_dartBoard[i][j].Equals(src))
						continue;

					x = i;
					y = j;
					check = true;
				}
			}

			if (check)
			{
				ret = x == 3 ? _dartBoard[x][1] : _dartBoard[0][y];
			}
			return ret;
		}

		public Field GetTriple(Field src)
		{
			Field ret = null;
			var check = false;
			var x = 0;
			var y = 0;
			for (var i = 0; i < _dartBoard.Count && !check; i++)
			{
				for (var j = 0; j < _dartBoard[i].Count && !check; j++)
				{
					if (!_dartBoard[i][j].Equals(src))
						continue;

					x = i;
					y = j;
					check = true;
				}
			}

			if (check)
			{
				ret = x == 3 ? _dartBoard[x][1] : _dartBoard[2][y];
			}
			return ret;
		}

		public List<Field> GetAllFields()
		{
			var ret = new List<Field>();
			foreach (var fields in _dartBoard)
			{
				foreach (var field in fields)
				{
					ret.Add(field);
				}
			}
			return ret;
		}

		public List<Field> GetAllDoubles()
		{
			var ret = new List<Field>();
			for (var i = 0; i < _dartBoard[0].Count; i++)
			{
				ret.Add(_dartBoard[0][i]);
			}
			ret.Add(_dartBoard[3][1]);
			return ret;
		}
	}
}
