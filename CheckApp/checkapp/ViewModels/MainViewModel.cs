using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CheckApp
{
	public class MainViewModel : ViewModelBase
	{
		private string _score = "";
		private List<string> _leftDarts;
		private string _leftDartsSelected;
		private ObservableCollection<CheckViewModel> _solutions;
		private RelayCommand _calculateCommand;
		private int _checkCnt;
		private int _calculationProgress;
		private readonly BackgroundWorker _worker;
		private List<CheckViewModel> _help;
		private Calculator _calc;
		private Visibility _loadVisibility = Visibility.Collapsed;
		private bool _dart1Enabled = true;
		private bool _dart2Enabled = true;
		private bool _dart1AlleChecked = true;
		private bool _dart1SingleChecked;
		private bool _dart1DoubleChecked;
		private bool _dart1TripleChecked;
		private bool _dart2AlleChecked = true;
		private bool _dart2SingleChecked;
		private bool _dart2DoubleChecked;
		private bool _dart2TripleChecked;
		private string _singleQuote = "55";
		private string _doubleQuote = "15";
		private string _tripleQuote = "10";
		public MainViewModel()
		{
			_worker = new BackgroundWorker
			{
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			_worker.DoWork += worker_DoWork;
			_worker.ProgressChanged += worker_ProgressChanged;
			_worker.RunWorkerCompleted += worker_RunWorkerCompleted;
			_calc = new Calculator(int.Parse(_singleQuote), int.Parse(_doubleQuote), int.Parse(_doubleQuote));
			_leftDarts = new List<string> { "3", "2", "1" };
			_leftDartsSelected = _leftDarts[0];
		}

		void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			var worker = (BackgroundWorker)sender;
			int scores;
			if (!int.TryParse(Score, out scores))
				return;

			int leftdarts;
			int.TryParse(LeftDartsSelected, out leftdarts);
			var par = new List<bool> { Dart1SingleChecked, Dart1DoubleChecked, Dart1TripleChecked, Dart2SingleChecked, Dart2DoubleChecked, Dart2TripleChecked };
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

			_help = _calc.GetAllPossibleCheckouts(scores, leftdarts, worker, par);

			foreach (var check in _help)
			{
				check.Check.Propability = Math.Round(check.Check.Propability * 100, 2);
				check.Check.Calculation = Math.Round(check.Check.Calculation * 100, 2);
			}

			CheckCnt = _help.Count;
		}

		void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			CalculationProgress = e.ProgressPercentage;
		}

		void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Solutions = new ObservableCollection<CheckViewModel>();
			foreach (CheckViewModel check in _help)
			{
				Field dart1;
				Field dart2 = null;
				Field dart3 = null;
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
				Solutions.Add(new CheckViewModel(dart1, dart2, dart3, check.Check.Propability, check.Check.Calculation, check.Check.Message, check.Check.SubChecks));
			}
			LoadVisibility = Visibility.Collapsed;
			CalculationProgress = 0;
		}

		public string Score
		{
			get
			{
				return _score;
			}
			set
			{
				_score = value;
				OnPropertyChanged("Score");
			}
		}

		public List<string> LeftDarts
		{
			get
			{
				return _leftDarts;
			}
			set
			{
				_leftDarts = value;
				OnPropertyChanged("LeftDarts");
			}
		}

		public string LeftDartsSelected
		{
			get
			{
				return _leftDartsSelected;
			}
			set
			{
				_leftDartsSelected = value;
				switch (_leftDartsSelected)
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
				return _calculationProgress;
			}
			set
			{
				_calculationProgress = value;
				OnPropertyChanged("CalculationProgress");
			}
		}

		public int CheckCnt
		{
			get
			{
				return _checkCnt;
			}
			set
			{
				_checkCnt = value;
				OnPropertyChanged("CheckCnt");
			}
		}

		public bool Dart1Enabled
		{
			get
			{
				return _dart1Enabled;
			}
			set
			{
				_dart1Enabled = value;
				OnPropertyChanged("Dart1Enabled");
			}
		}

		public bool Dart2Enabled
		{
			get
			{
				return _dart2Enabled;
			}
			set
			{
				_dart2Enabled = value;
				OnPropertyChanged("Dart2Enabled");
			}
		}

		public bool Dart1AlleChecked
		{
			get
			{
				return _dart1AlleChecked;
			}
			set
			{
				_dart1AlleChecked = value;
				if (_dart1AlleChecked)
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
				return _dart1SingleChecked;
			}
			set
			{
				_dart1SingleChecked = value;
				if (_dart1SingleChecked && Dart1AlleChecked)
					Dart1AlleChecked = false;
				OnPropertyChanged("Dart1SingleChecked");
			}
		}

		public bool Dart1DoubleChecked
		{
			get
			{
				return _dart1DoubleChecked;
			}
			set
			{
				_dart1DoubleChecked = value;
				if (_dart1DoubleChecked && Dart1AlleChecked)
					Dart1AlleChecked = false;
				OnPropertyChanged("Dart1DoubleChecked");
			}
		}

		public bool Dart1TripleChecked
		{
			get
			{
				return _dart1TripleChecked;
			}
			set
			{
				_dart1TripleChecked = value;
				if (_dart1TripleChecked && Dart1AlleChecked)
					Dart1AlleChecked = false;
				OnPropertyChanged("Dart1TripleChecked");
			}
		}

		public bool Dart2AlleChecked
		{
			get
			{
				return _dart2AlleChecked;
			}
			set
			{
				_dart2AlleChecked = value;
				if (_dart2AlleChecked)
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
				return _dart2SingleChecked;
			}
			set
			{
				_dart2SingleChecked = value;
				if (_dart2SingleChecked && Dart2AlleChecked)
					Dart2AlleChecked = false;
				OnPropertyChanged("Dart2SingleChecked");
			}
		}

		public bool Dart2DoubleChecked
		{
			get
			{
				return _dart2DoubleChecked;
			}
			set
			{
				_dart2DoubleChecked = value;
				if (_dart2DoubleChecked && Dart2AlleChecked)
					Dart2AlleChecked = false;
				OnPropertyChanged("Dart2DoubleChecked");
			}
		}

		public bool Dart2TripleChecked
		{
			get
			{
				return _dart2TripleChecked;
			}
			set
			{
				_dart2TripleChecked = value;
				if (_dart2TripleChecked && Dart2AlleChecked)
					Dart2AlleChecked = false;
				OnPropertyChanged("Dart2TripleChecked");
			}
		}

		public string SingleQuote
		{
			get
			{
				return _singleQuote;
			}
			set
			{
				_singleQuote = value;
				OnPropertyChanged(nameof(SingleQuote));
			}
		}

		public string DoubleQuote
		{
			get
			{
				return _doubleQuote;
			}
			set
			{
				_doubleQuote = value;
				OnPropertyChanged(nameof(DoubleQuote));
			}
		}

		public string TripleQuote
		{
			get
			{
				return _tripleQuote;
			}
			set
			{
				_tripleQuote = value;
				OnPropertyChanged(nameof(_tripleQuote));
			}
		}

		public Visibility LoadVisibility
		{
			get
			{
				return _loadVisibility;
			}
			set
			{
				_loadVisibility = value;
				OnPropertyChanged("LoadVisibility");
			}
		}
		public ObservableCollection<CheckViewModel> Solutions
		{
			get
			{
				return _solutions;
			}
			set
			{
				_solutions = value;
				OnPropertyChanged("Solutions");
			}
		}

		public ICommand CalculateCommand
		{
			get
			{
				if (_calculateCommand == null)
				{
					_calculateCommand = new RelayCommand(
						param => StartWorker()
							);
				}
				return _calculateCommand;
			}
		}

		public void StartWorker()
		{
			LoadVisibility = Visibility.Visible;
			if (!_worker.IsBusy)
			{
				_calc = new Calculator(int.Parse(_singleQuote), int.Parse(_doubleQuote), int.Parse(_doubleQuote));
				_worker.RunWorkerAsync(_calc);
			}
		}
	}
}
