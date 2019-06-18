using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CheckApp.Models;
using CheckApp.Services;
using Dart.Base;

namespace CheckApp
{
	public class MainViewModel : ViewModelBase
	{
		private List<int> _leftDarts;
		private int _score;
		private int _leftDartsSelected;
		private ObservableCollection<CheckViewModel> _solutions;
		private RelayCommand _calculateCommand;
		private RelayCommand _calculateAllCommand;
		private int _checkCnt;
		private int _calculationProgress;
		private readonly BackgroundWorker _worker;
		private List<CheckViewModel> _help;
		private CheckCalculator _calc;
		private Visibility _loadVisibility = Visibility.Collapsed;
		private bool _dart1Enabled = true;
		private bool _dart2Enabled = true;
		private bool _dart1AlleChecked = true;
		private bool _dart2AlleChecked = true;
		private readonly Config _config;
		private bool _runAll;

		public MainViewModel()
		{
			_config = new Config();
			My = 22;
			Sigma = 12;
			_worker = new BackgroundWorker
			{
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};
			_worker.DoWork += worker_DoWork;
			_worker.ProgressChanged += worker_ProgressChanged;
			_worker.RunWorkerCompleted += worker_RunWorkerCompleted;
			_leftDarts = new List<int> { 3, 2, 1 };
			_leftDartsSelected = _leftDarts.First();
		}

		void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			var worker = (BackgroundWorker)sender;
			_help = _runAll ? _calc.CalculateAll(LeftDartsSelected, worker) : _calc.CalculateChecks(Score, LeftDartsSelected, worker, true);

			CheckCnt = _help?.Count ?? 0;
			
		}

		void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			CalculationProgress = e.ProgressPercentage;
		}

		void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Solutions = new ObservableCollection<CheckViewModel>();
			if (_help != null)
			{
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

					Solutions.Add(new CheckViewModel(dart1, dart2, dart3, check.Check.Propability, check.Check.SubChecks));
				}
			}

			LoadVisibility = Visibility.Collapsed;
			CalculationProgress = 0;
		}

		public int Score
		{
			get => _score;
			set
			{
				_score = value;
				OnPropertyChanged(nameof(Score));
			}
		}

		public List<int> LeftDarts
		{
			get => _leftDarts;
			set
			{
				_leftDarts = value;
				OnPropertyChanged(nameof(LeftDarts));
			}
		}

		public int LeftDartsSelected
		{
			get => _leftDartsSelected;
			set
			{
				_leftDartsSelected = value;
				switch (_leftDartsSelected)
				{
					case 1: Dart1Enabled = false; Dart2Enabled = false; break;
					case 2: Dart1Enabled = true; Dart2Enabled = false; break;
					case 3: Dart1Enabled = true; Dart2Enabled = true; break;
				}
				OnPropertyChanged(nameof(LeftDartsSelected));
			}
		}

		public int CalculationProgress
		{
			get => _calculationProgress;
			set
			{
				_calculationProgress = value;
				OnPropertyChanged(nameof(CalculationProgress));
			}
		}

		public int CheckCnt
		{
			get => _checkCnt;
			set
			{
				_checkCnt = value;
				OnPropertyChanged(nameof(CheckCnt));
			}
		}

		public bool Dart1Enabled
		{
			get => _dart1Enabled;
			set
			{
				_dart1Enabled = value;
				OnPropertyChanged(nameof(Dart1Enabled));
			}
		}

		public bool Dart2Enabled
		{
			get => _dart2Enabled;
			set
			{
				_dart2Enabled = value;
				OnPropertyChanged(nameof(Dart2Enabled));
			}
		}

		public bool Dart1AlleChecked
		{
			get => _dart1AlleChecked;
			set
			{
				_dart1AlleChecked = value;
				if (_dart1AlleChecked)
				{
					Dart1SingleChecked = false;
					Dart1DoubleChecked = false;
					Dart1TripleChecked = false;
				}
				OnPropertyChanged(nameof(Dart1AlleChecked));
			}
		}

		public bool Dart1SingleChecked
		{
			get => _config.Dart1SingleChecked;
			set
			{
				_config.Dart1SingleChecked = value;
				if (_config.Dart1SingleChecked && Dart1AlleChecked)
					Dart1AlleChecked = false;
				OnPropertyChanged(nameof(Dart1SingleChecked));
			}
		}

		public bool Dart1DoubleChecked
		{
			get => _config.Dart1DoubleChecked;
			set
			{
				_config.Dart1DoubleChecked = value;
				if (_config.Dart1DoubleChecked && Dart1AlleChecked)
					Dart1AlleChecked = false;
				OnPropertyChanged(nameof(Dart1DoubleChecked));
			}
		}

		public bool Dart1TripleChecked
		{
			get => _config.Dart1TripleChecked;
			set
			{
				_config.Dart1TripleChecked = value;
				if (_config.Dart1TripleChecked && Dart1AlleChecked)
					Dart1AlleChecked = false;
				OnPropertyChanged(nameof(Dart1TripleChecked));
			}
		}

		public bool Dart2AlleChecked
		{
			get => _dart2AlleChecked;
			set
			{
				_dart2AlleChecked = value;
				if (_dart2AlleChecked)
				{
					Dart2SingleChecked = false;
					Dart2DoubleChecked = false;
					Dart2TripleChecked = false;
				}
				OnPropertyChanged(nameof(Dart2AlleChecked));
			}
		}

		public bool Dart2SingleChecked
		{
			get => _config.Dart2SingleChecked;
			set
			{
				_config.Dart2SingleChecked = value;
				if (_config.Dart2SingleChecked && Dart2AlleChecked)
					Dart2AlleChecked = false;
				OnPropertyChanged(nameof(Dart2SingleChecked));
			}
		}

		public bool Dart2DoubleChecked
		{
			get => _config.Dart2DoubleChecked;
			set
			{
				_config.Dart2DoubleChecked = value;
				if (_config.Dart2DoubleChecked && Dart2AlleChecked)
					Dart2AlleChecked = false;
				OnPropertyChanged(nameof(Dart2DoubleChecked));
			}
		}

		public bool Dart2TripleChecked
		{
			get => _config.Dart2TripleChecked;
			set
			{
				_config.Dart2TripleChecked = value;
				if (_config.Dart2TripleChecked && Dart2AlleChecked)
					Dart2AlleChecked = false;
				OnPropertyChanged(nameof(Dart2TripleChecked));
			}
		}

		public int My
		{
			get => _config.My;
			set
			{
				_config.My = value;
				OnPropertyChanged(nameof(My));
			}
		}

		public int Sigma
		{
			get => _config.Sigma;
			set
			{
				_config.Sigma = value;
				OnPropertyChanged(nameof(Sigma));
			}
		}

		public Visibility LoadVisibility
		{
			get => _loadVisibility;
			set
			{
				_loadVisibility = value;
				OnPropertyChanged(nameof(LoadVisibility));
			}
		}
		public ObservableCollection<CheckViewModel> Solutions
		{
			get => _solutions;
			set
			{
				_solutions = value;
				OnPropertyChanged(nameof(Solutions));
			}
		}

		public ICommand CalculateCommand
		{
			get
			{
				return _calculateCommand ?? (_calculateCommand = new RelayCommand(
					       param => StartWorker(false)
				       ));
			}
		}

		public ICommand CalculateAllCommand
		{
			get
			{
				return _calculateAllCommand ?? (_calculateAllCommand = new RelayCommand(
					       param => StartWorker(true)
				       ));
			}
		}

		public void StartWorker(bool runAll)
		{
			_runAll = runAll;
			LoadVisibility = Visibility.Visible;
			if (!_worker.IsBusy)
			{
				_calc = new CheckCalculator(_config);
				_worker.RunWorkerAsync(_calc);
			}
		}
	}
}
