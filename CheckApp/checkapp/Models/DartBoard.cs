using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckApp
{
	public class DartBoard
	{
		private List<List<Field>> dartBoard = null;

		private List<int> doubled = new List<int>() { 1, 2, 3, 4 };
		private int tripled = 1;
		private int singled = 1;
		private int bulld = 1;
		private int doublebulld = 1;

		public DartBoard()
		{
			InitializeDartboard();
		}


		private void InitializeDartboard()
		{
			dartBoard = new List<List<Field>>();

			List<Field> temp = new List<Field>();
			temp.Add(new Field(40, doubled[0], FieldType.Double));
			temp.Add(new Field(2, doubled[3], FieldType.Double));
			temp.Add(new Field(36, doubled[1], FieldType.Double));
			temp.Add(new Field(8, doubled[0], FieldType.Double));
			temp.Add(new Field(26, doubled[2], FieldType.Double));
			temp.Add(new Field(12, doubled[1], FieldType.Double));
			temp.Add(new Field(20, doubled[1], FieldType.Double));
			temp.Add(new Field(30, doubled[2], FieldType.Double));
			temp.Add(new Field(4, doubled[1], FieldType.Double));
			temp.Add(new Field(34, doubled[2], FieldType.Double));
			temp.Add(new Field(6, doubled[3], FieldType.Double));
			temp.Add(new Field(38, doubled[2], FieldType.Double));
			temp.Add(new Field(14, doubled[3], FieldType.Double));
			temp.Add(new Field(32, doubled[0], FieldType.Double));
			temp.Add(new Field(16, doubled[0], FieldType.Double));
			temp.Add(new Field(22, doubled[2], FieldType.Double));
			temp.Add(new Field(28, doubled[1], FieldType.Double));
			temp.Add(new Field(18, doubled[3], FieldType.Double));
			temp.Add(new Field(24, doubled[0], FieldType.Double));
			temp.Add(new Field(10, doubled[3], FieldType.Double));
			dartBoard.Add(temp);
			temp = new List<Field>();
			foreach (Field f in dartBoard[0])
			{
				temp.Add(new Field(f.Score / 2, singled, FieldType.Single));
			}
			dartBoard.Add(temp);
			temp = new List<Field>();
			foreach (Field f in dartBoard[1])
			{
				temp.Add(new Field(f.Score * 3, tripled, FieldType.Triple));
			}
			dartBoard.Add(temp);
			dartBoard.Add(new List<Field>() { new Field(25, bulld, FieldType.Single), new Field(50, doublebulld, FieldType.Double) });
		}

		public Field GetFirstField()
		{
			return this.dartBoard[1][0];
		}

		public Field GetBull()
		{
			return this.dartBoard[3][0];
		}

		public Field GetLeft(Field src)
		{
			Field ret = null;
			bool check = false;
			int x = 0;
			int y = 0;
			for (int i = 0; i < dartBoard.Count && !check; i++)
			{
				for (int j = 0; j < dartBoard[i].Count && !check; j++)
				{
					if (dartBoard[i][j].Equals(src))
					{
						x = i;
						y = j;
						check = true;
					}
				}
			}

			if (check)
			{
				if (x == 3)
				{
					if (y == 0)
						ret = dartBoard[x][y + 1];
					else
						ret = dartBoard[x][y - 1];
				}
				else
				{
					if (y == 0)
					{
						ret = dartBoard[x][dartBoard[x].Count - 1];
					}
					else
					{
						ret = dartBoard[x][y - 1];
					}
				}
			}
			return ret;
		}

		public Field GetRight(Field src)
		{
			Field ret = null;
			bool check = false;
			int x = 0;
			int y = 0;
			for (int i = 0; i < dartBoard.Count && !check; i++)
			{
				for (int j = 0; j < dartBoard[i].Count && !check; j++)
				{
					if (dartBoard[i][j].Equals(src))
					{
						x = i;
						y = j;
						check = true;
					}
				}
			}

			if (check)
			{
				if (x == 3)
				{
					if (y == 0)
						ret = dartBoard[x][y + 1];
					else
						ret = dartBoard[x][y - 1];
				}
				else
				{
					if (y == dartBoard[x].Count - 1)
					{
						ret = dartBoard[x][0];
					}
					else
					{
						ret = dartBoard[x][y + 1];
					}
				}
			}
			return ret;
		}

		public Field GetSingle(Field src)
		{
			Field ret = null;
			bool check = false;
			int x = 0;
			int y = 0;
			for (int i = 0; i < dartBoard.Count && !check; i++)
			{
				for (int j = 0; j < dartBoard[i].Count && !check; j++)
				{
					if (dartBoard[i][j].Equals(src))
					{
						x = i;
						y = j;
						check = true;
					}
				}
			}

			if (check)
			{
				if (x == 3)
				{
					ret = dartBoard[x][0];
				}
				else
				{
					ret = dartBoard[1][y];
				}
			}
			return ret;
		}

		public Field GetDouble(Field src)
		{
			Field ret = null;
			bool check = false;
			int x = 0;
			int y = 0;
			for (int i = 0; i < dartBoard.Count && !check; i++)
			{
				for (int j = 0; j < dartBoard[i].Count && !check; j++)
				{
					if (dartBoard[i][j].Equals(src))
					{
						x = i;
						y = j;
						check = true;
					}
				}
			}

			if (check)
			{
				if (x == 3)
				{
					ret = dartBoard[x][1];
				}
				else
				{
					ret = dartBoard[0][y];
				}
			}
			return ret;
		}

		public Field GetTriple(Field src)
		{
			Field ret = null;
			bool check = false;
			int x = 0;
			int y = 0;
			for (int i = 0; i < dartBoard.Count && !check; i++)
			{
				for (int j = 0; j < dartBoard[i].Count && !check; j++)
				{
					if (dartBoard[i][j].Equals(src))
					{
						x = i;
						y = j;
						check = true;
					}
				}
			}

			if (check)
			{
				if (x == 3)
				{
					ret = dartBoard[x][1];
				}
				else
				{
					ret = dartBoard[2][y];
				}
			}
			return ret;
		}

		public List<Field> GetAllFields()
		{
			List<Field> ret = new List<Field>();
			for (int i = 0; i < dartBoard.Count; i++)
			{
				for (int j = 0; j < dartBoard[i].Count; j++)
				{
					ret.Add(dartBoard[i][j]);
				}
			}
			return ret;
		}

		public List<Field> GetAllDoubles()
		{
			List<Field> ret = new List<Field>();
			for (int i = 0; i < dartBoard[0].Count; i++)
			{
				ret.Add(dartBoard[0][i]);
			}
			ret.Add(dartBoard[3][1]);
			return ret;
		}
	}
}
