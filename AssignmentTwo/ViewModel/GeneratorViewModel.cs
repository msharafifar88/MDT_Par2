using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using AssignmentTwo.Model;
using Newtonsoft.Json;

namespace AssignmentTwo.ViewModel
{
    public class GeneratorViewModel
    {

        private readonly string path2 = @"D:\OneDrive - York University\Desktop\MDT\Part2\Assignments\Assignments\AssignmentTwo\Data\generator.json";
        public GeneratorViewModel()
        {
            this._generators = new ObservableCollection<GeneratorModel>();            
            _data = new ObservableCollection<dataDTO>();
            _dataResult = new ObservableCollection<dataDTO>();
            getData();
        }

        private void getData()
        {
            var data = File.ReadAllText(path2);
            Generator generator = JsonConvert.DeserializeObject<Generator>(data);
            this._generators.Clear();
            foreach (var item in generator.generators)
                this._generators.Add(item);
            int i = 0;
            _data.Clear();
            foreach (var num in generator.datasets)
            {
                _data.Add(new dataDTO() { data = string.Join(",", num) ,id=i});
                i++;
            }
          
        }

       /* private void setData()
        {
            var path1 = @"";
            var generatorJson = System.IO.File.ReadAllText(path1);
            Generator generator = JsonConvert.DeserializeObject<Generator>(generatorJson);
            this._generators.Clear();
            foreach (var item in generator.generators)
                this._generators.Add(item);
        }*/

        private GeneratorModel _selectedGenerator;
        public GeneratorModel SelectedGenerator
        {
            get {return _selectedGenerator;}
            set {_selectedGenerator = value;}
        }
        private dataDTO _selectedData;
        public dataDTO SelectedData
        {
            get { return _selectedData; }
            set { _selectedData = value; }
        }

        private ObservableCollection<GeneratorModel> _generators;
        public ObservableCollection<GeneratorModel> Generators
        {
            get { return _generators; }
            set { _generators = value; }
        }

        private ObservableCollection<dataDTO> _data;
        public ObservableCollection<dataDTO> Data
        {
            get {return _data;}
            set {_data = value;}
        }

        private ObservableCollection<dataDTO> _dataResult;
        public ObservableCollection<dataDTO> DataResult
        {
            get { return _dataResult; }
            set { _dataResult = value; }
        }

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => MyAction(), true));
            }
        }
        private ICommand _clickCommand1;
        public ICommand ClickCommand1
        {
            get
            {
                return _clickCommand1 ?? (_clickCommand1 = new CommandHandler(() => MyAction1(), true));
            }
        }
        public void MyAction()
        {
            Generator model = new Generator();
            model.generators = new List<GeneratorModel>();
            foreach (var generator in this._generators)
                model.generators.Add(generator);
            model.datasets = new List<List<double>>();
            foreach (var dataset in this._data)
            {
                if (!string.IsNullOrEmpty(dataset.data))
                {
                    List<double> temp = new List<double>();
                    try
                    {
                        foreach (var d in dataset.data.Split(","))
                            temp.Add(double.Parse(d));
                    }
                    catch { }
                    model.datasets.Add(temp);
                }
            }
            var m = JsonConvert.SerializeObject(model, Formatting.Indented);
            File.WriteAllText(path2, m);


            _dataResult.Clear();
            TaskViewModel taskViewModel = new TaskViewModel();
            List<string> result = taskViewModel.doTask(model);
            int counter = 0;
            foreach (var item in result)
            {
                _dataResult.Add(new dataDTO { data = item, id = counter });
                counter++;
            }
        }
        private string _addableData;


        public string AddableData
        {
            get { return _addableData; }
            set {_addableData = value;}
        }
        public void MyAction1()
        {
        }

    }

    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }


    }

}
