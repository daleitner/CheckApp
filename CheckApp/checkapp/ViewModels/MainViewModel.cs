using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace CheckApp
{
	public class MainViewModel : ViewModelBase
	{
		private string score = "";
		private List<string> leftDarts = null;
		private string leftDartsSelected = "";
		private ObservableCollection<CheckViewModel> solutions = null;
		private RelayCommand calculateCommand = null;
		private int checkCnt = 0;
		private int calculationProgress = 0;
		private BackgroundWorker worker = null;
		private List<CheckViewModel> help = null;
		private Calculator calc = null;
		private Visibility loadVisibility = Visibility.Collapsed;
		private bool dart1Enabled = true;
		private bool dart2Enabled = true;
		private bool dart1AlleChecked = true;
		private bool dart1SingleChecked = false;
		private bool dart1DoubleChecked = false;
		private bool dart1TripleChecked = false;
		private bool dart2AlleChecked = true;
		private bool dart2SingleChecked = false;
		private bool dart2DoubleChecked = false;
		private bool dart2TripleChecked = false;
		private string singleQuote = "55";
		private string doubleQuote = "20";
		private string tripleQuote = "15";
		public MainViewModel()
		{
			worker = new BackgroundWorker()
			{
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			worker.DoWork += new DoWorkEventHandler(worker_DoWork);
			worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
			worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
			calc = new Calculator(int.Parse(this.singleQuote), int.Parse(this.doubleQuote), int.Parse(this.doubleQuote));
			this.leftDarts = new List<string>() { "3", "2", "1" };
			this.leftDartsSelected = this.leftDarts[0];
		}

		void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			var _worker = (BackgroundWorker)sender;
			var _calc = (Calculator)e.Argument;
			int scores = 0;
			if (Int32.TryParse(Score, out scores))
			{
				int leftdarts = 3;
				Int32.TryParse(LeftDartsSelected, out leftdarts);
				List<bool> par = new List<bool>() { Dart1SingleChecked, Dart1DoubleChecked, Dart1TripleChecked, Dart2SingleChecked, Dart2DoubleChecked, Dart2TripleChecked };
				if (Dart1AlleChecked || !Dart1Enabled)
				{
					par[0] = true;
					par[1] = true;
					par[2] = true;
				}
				if (Dart2AlleChecked || !Dart2Enabled)
				{
					par[3] = true;
					par[4] = true;
					par[5] = true;
				}

				help = calc.GetAllChecks(scores, leftdarts, _worker, par);

				foreach (CheckViewModel check in help)
				{
					check.Check.Propability = Math.Round(check.Check.Propability * 100, 2);
					check.Check.Calculation = Math.Round(check.Check.Calculation * 100, 2);
				}

				CheckCnt = help.Count;
			}
		}

		void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			CalculationProgress = e.ProgressPercentage;
		}

		void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Solutions = new ObservableCollection<CheckViewModel>();
			Field dart1 = null;
			Field dart2 = null;
			Field dart3 = null;
			foreach (CheckViewModel check in help)
			{
				dart1 = null;
				dart2 = null;
				dart3 = null;
				if (check.Check.AufCheckDart == null)
				{
					dart1 = check.Check.CheckDart;
				}
				else
				{
					if (check.Check.ScoreDart == null)
					{
						dart1 = check.Check.AufCheckDart;
						dart2 = check.Check.CheckDart;
					}
					else
					{
						dart1 = check.Check.ScoreDart;
						dart2 = check.Check.AufCheckDart;
						dart3 = check.Check.CheckDart;
					}
				}
				Solutions.Add(new CheckViewModel(dart1, dart2, dart3, check.Check.Propability, check.Check.Calculation, check.Check.Message));
			}
			LoadVisibility = Visibility.Collapsed;
			CalculationProgress = 0;
		}

		public string Score
		{
			get
			{
				return this.score;
			}
			set
			{
				this.score = value;
				OnPropertyChanged("Score");
			}
		}

		public List<string> LeftDarts
		{
			get
			{
				return this.leftDarts;
			}
			set
			{
				this.leftDarts = value;
				OnPropertyChanged("LeftDarts");
			}
		}

		public string LeftDartsSelected
		{
			get
			{
				return this.leftDartsSelected;
			}
			set
			{
				this.leftDartsSelected = value;
				switch (this.leftDartsSelected)
				{
					case "1": Dart1Enabled = false; Dart2Enabled = false; break;
					case "2": Dart1Enabled = true; Dart2Enabled = false; break;
					case "3": Dart1Enabled = true; Dart2Enabled = true; break;
				}
				OnPropertyChanged("LeftDartsSelected");
			}
		}

		public int CalculationProgress
		{
			get
			{
				return this.calculationProgress;
			}
			set
			{
				this.calculationProgress = value;
				OnPropertyChanged("CalculationProgress");
			}
		}

		public int CheckCnt
		{
			get
			{
				return this.checkCnt;
			}
			set
			{
				this.checkCnt = value;
				OnPropertyChanged("CheckCnt");
			}
		}

		public bool Dart1Enabled
		{
			get
			{
				return this.dart1Enabled;
			}
			set
			{
				this.dart1Enabled = value;
				OnPropertyChanged("Dart1Enabled");
			}
		}

		public bool Dart2Enabled
		{
			get
			{
				return this.dart2Enabled;
			}
			set
			{
				this.dart2Enabled = value;
				OnPropertyChanged("Dart2Enabled");
			}
		}

		public bool Dart1AlleChecked
		{
			get
			{
				return this.dart1AlleChecked;
			}
			set
			{
				this.dart1AlleChecked = value;
				if (this.dart1AlleChecked)
				{
					Dart1SingleChecked = false;
					Dart1DoubleChecked = false;
					Dart1TripleChecked = false;
				}
				OnPropertyChanged("Dart1AlleChecked");
			}
		}

		public bool Dart1SingleChecked
		{
			get
			{
				return this.dart1SingleChecked;
			}
			set
			{
				this.dart1SingleChecked = value;
				if (this.dart1SingleChecked && Dart1AlleChecked)
					Dart1AlleChecked = false;
				OnPropertyChanged("Dart1SingleChecked");
			}
		}

		public bool Dart1DoubleChecked
		{
			get
			{
				return this.dart1DoubleChecked;
			}
			set
			{
				this.dart1DoubleChecked = value;
				if (this.dart1DoubleChecked && Dart1AlleChecked)
					Dart1AlleChecked = false;
				OnPropertyChanged("Dart1DoubleChecked");
			}
		}

		public bool Dart1TripleChecked
		{
			get
			{
				return this.dart1TripleChecked;
			}
			set
			{
				this.dart1TripleChecked = value;
				if (this.dart1TripleChecked && Dart1AlleChecked)
					Dart1AlleChecked = false;
				OnPropertyChanged("Dart1TripleChecked");
			}
		}

		public bool Dart2AlleChecked
		{
			get
			{
				return this.dart2AlleChecked;
			}
			set
			{
				this.dart2AlleChecked = value;
				if (this.dart2AlleChecked)
				{
					Dart2SingleChecked = false;
					Dart2DoubleChecked = false;
					Dart2TripleChecked = false;
				}
				OnPropertyChanged("Dart2AlleChecked");
			}
		}

		public bool Dart2SingleChecked
		{
			get
			{
				return this.dart2SingleChecked;
			}
			set
			{
				this.dart2SingleChecked = value;
				if (this.dart2SingleChecked && Dart2AlleChecked)
					Dart2AlleChecked = false;
				OnPropertyChanged("Dart2SingleChecked");
			}
		}

		public bool Dart2DoubleChecked
		{
			get
			{
				return this.dart2DoubleChecked;
			}
			set
			{
				this.dart2DoubleChecked = value;
				if (this.dart2DoubleChecked && Dart2AlleChecked)
					Dart2AlleChecked = false;
				OnPropertyChanged("Dart2DoubleChecked");
			}
		}

		public bool Dart2TripleChecked
		{
			get
			{
				return this.dart2TripleChecked;
			}
			set
			{
				this.dart2TripleChecked = value;
				if (this.dart2TripleChecked && Dart2AlleChecked)
					Dart2AlleChecked = false;
				OnPropertyChanged("Dart2TripleChecked");
			}
		}

		public string SingleQuote
		{
			get
			{
				return this.singleQuote;
			}
			set
			{
				this.singleQuote = value;
				OnPropertyChanged(nameof(this.SingleQuote));
			}
		}

		public string DoubleQuote
		{
			get
			{
				return this.doubleQuote;
			}
			set
			{
				this.doubleQuote = value;
				OnPropertyChanged(nameof(this.DoubleQuote));
			}
		}

		public string TripleQuote
		{
			get
			{
				return this.tripleQuote;
			}
			set
			{
				this.tripleQuote = value;
				OnPropertyChanged(nameof(this.tripleQuote));
			}
		}

		public Visibility LoadVisibility
		{
			get
			{
				return this.loadVisibility;
			}
			set
			{
				this.loadVisibility = value;
				OnPropertyChanged("LoadVisibility");
			}
		}
		public ObservableCollection<CheckViewModel> Solutions
		{
			get
			{
				return this.solutions;
			}
			set
			{
				this.solutions = value;
				OnPropertyChanged("Solutions");
			}
		}

		public ICommand CalculateCommand
		{
			get
			{
				if (this.calculateCommand == null)
				{
					this.calculateCommand = new RelayCommand(
						param => StartWorker()
							);
				}
				return this.calculateCommand;
			}
		}

		public void StartWorker()
		{
			LoadVisibility = Visibility.Visible;
			if (!worker.IsBusy)
			{
				this.calc = new Calculator(int.Parse(this.singleQuote), int.Parse(this.doubleQuote), int.Parse(this.doubleQuote));
				worker.RunWorkerAsync(calc);
			}
		}
	}
}
