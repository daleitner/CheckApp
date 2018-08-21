using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace CheckApp
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected string path = "";
        public event ErrorEventHandler SendError;
        private List<string> errors = new List<string>();
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void Save(String sLine)
        {
            Save(sLine, this.path);
        }

        public void Save(String sLine, String sPath)
        {
            StreamWriter myFile = new StreamWriter(sPath, true);
            myFile.WriteLine(sLine);
            myFile.Close();
        }

        protected string ReadFile()
        {
            return ReadFile(this.path);
        }

        protected string ReadFile(String sPath)
        {
            string sContent = "";

            if (File.Exists(sPath))
            {
                StreamReader myFile = new StreamReader(sPath, System.Text.Encoding.Default);
                sContent = myFile.ReadToEnd();
                myFile.Close();
            }
            else
            {
                Log("File not found!!! " + sPath);
            }
            return sContent;
        }

        protected void Log(string message)
        {
            if(message != "")
                errors.Add(message);

            if (SendError != null)
            {
                foreach(string m in errors)
                    SendError(m, null);
                errors = new List<string>();
            }
        }

        public void SendTestLog(string message)
        {
            Log(message);
        }
    }
}