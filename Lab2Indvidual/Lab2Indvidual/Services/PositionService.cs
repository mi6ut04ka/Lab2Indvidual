using Lab2Indvidual.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Collections.ObjectModel;


namespace Lab2Indvidual.Services
{
    public class PositionService
    {
        private const string PositionsKey = "positions";
        public List<Position> positions = new List<Position>();
        public ObservableCollection<string> Positions { get; set; }

        public PositionService()
        {
            Positions = LoadPositions();
        }

        public void SavePositions()
        {
            string json = JsonConvert.SerializeObject(Positions);
            Preferences.Set(PositionsKey, json);
        }

        private ObservableCollection<string> LoadPositions()
        {
            string json = Preferences.Get(PositionsKey, string.Empty);

            if (string.IsNullOrEmpty(json))
            {
                return new ObservableCollection<string>();
            }
            var positionsList = JsonConvert.DeserializeObject<List<string>>(json);
            return new ObservableCollection<string>(positionsList);
        }


        public void AddPosition(string position)
        {
            Positions.Add(position);
            SavePositions();
        }


    }

}
