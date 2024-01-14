using MatchGame.Data;
using MatchGame.Models;
using MatchGame.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Specialized;
using System.IO;
using System.Xml.Serialization;




namespace MatchGame.ViewModels
{
    internal class GameViewModel : ViewModel
    {


        #region Field Observable Collection
        private ObservableCollection<FieldModel> f = new();
        public ObservableCollection<FieldModel> F { get => f; set => Set(ref f, value); }
        #endregion

        #region Field Observable Collection
        private ObservableCollection<FieldModel> f1 = new();
        public ObservableCollection<FieldModel> F1 { get => f1; set => Set(ref f1, value); }
        #endregion

        #region Field Observable Collection
        private ObservableCollection<FieldModel> f2 = new();
        public ObservableCollection<FieldModel> F2 { get => f2; set => Set(ref f2, value); }
        #endregion
        //Кол-во спичек в коробке
        private int matches;

        #region Step
        private string step;
        public string Step { get => step; set => Set(ref step, value); }
        #endregion

        private bool stepForFirst = false;
        public bool StepForFirst {
            get => stepForFirst;
            set {
                Step = stepForFirst ? "первый" : "второй";
                Set(ref stepForFirst, value);
            }
        }

        private int pickupCount;
        public int PickupCount { get => pickupCount; set => Set(ref pickupCount, value); }

        public GameViewModel()
        {
            StartGame();
        }



        /// <summary>
        /// Ходит первый игрок; Счетчик взятых спичек = 0; Вызывается метод GenerateField();
        /// </summary>
        public void StartGame()
        {
            StepForFirst = false;
            Step = "первый";
            PickupCount = 0;
            GenerateField();
        }
        public void Rules()
        {
            MessageBox.Show("Играют два человека. Перед ними на столе в ряд выложено 20 спичек." +
                " Каждый в свой ход может взять от 1 до 3 спичек. Ходы выполняются по очереди. Проигрывает участник, взявший последнюю спичку (или спички).");
        }

        /// <summary>
        /// Вызывает MessageBox, при нажатии "Yes" вызывается метод StartGame(), при нажатии "No" закрывается программа
        /// </summary>
        public void RestartGame()
        {
            MessageBoxResult mbr = MessageBox.Show("Начать заново?", "", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                StartGame();
            }
            else {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Проверяет, если нажали на спичку и если спичек <3, то кол-во взятых спичек увеличивается и вместо картинки спички, вставляется пустая картинка
        /// Если нажали не на картинку спички, и количество спичек >0, то вместо пустой картинки вставляется картинка спички, и счетчик взятых спичек уменьшается
        /// </summary>
        /// <param name="match"></param>
        public void MatchClick(FieldModel match)
        {

            if(match.Image == ImagesPaths.Match)
            {
                if (pickupCount < 3)
                {
                    match.Image = ImagesPaths.None;
                    PickupCount++;
                }

            } 
            else
            {
                if (pickupCount > 0)
                {
                    match.Image = ImagesPaths.Match;
                    PickupCount--;
                }
            }
        }

        /// <summary>
        /// Если количество взятых спичек <1 - ничего не происходит
        /// Если ход первого игрока, добавляет к спичкам первого игрока выбранные им спички
        /// Если ход второго игрока, добавляет к спичкам второго игрока выбранные им спички
        /// Если спичек не выбрано, и коробок спичек пуст пишется кто выиграл, и вызывается метод рестарта игры
        /// </summary>
        public void Accept()
        {
            if (PickupCount < 1) return;

            if (StepForFirst)
            {
                int c = GetMatchCount(f1);

                for (int i = 0; i < PickupCount + c; i++)
                {
                    f1[i].Image = ImagesPaths.Match;
                }
            }
            else {
                int c = GetMatchCount(f2);

                for (int i = 0; i < PickupCount + c; i++)
                {
                    f2[i].Image = ImagesPaths.Match;
                }
            }

            PickupCount = 0;
            if (GetMatchCount(f) < 1)
            {
                MessageBox.Show(StepForFirst ? "Выйграл первый игрок" : "Выйграл второй игрок");
                RestartGame();
            }
            else {
                StepForFirst = !StepForFirst;
            }

        }

        /// <summary>
        /// Определяет сколько спичек было выложено на поле
        /// </summary>
        /// <param name="f">коллекция спичек</param>
        /// <returns></returns>
        public int GetMatchCount(ObservableCollection<FieldModel> f)
        {
            var appSettings = ConfigurationManager.AppSettings;
            matches = Convert.ToInt32(appSettings["matches"]);
            int c = 0;
            for (int i = 0; i < matches; i++) {
                if (f[i].Image == ImagesPaths.Match)
                    c++;
            }
            return c;
        }

        /// <summary>
        /// Обнуляет поля игроков, и заполняет общий коробок спичек
        /// </summary>
        public void GenerateField()
        {
            f.Clear();
            f1.Clear();
            f2.Clear();
            var appSettings = ConfigurationManager.AppSettings;
            matches = Convert.ToInt32(appSettings["matches"]);
            for (int i = 0; i < matches; i++)
            {
                f.Add(new FieldModel() { I = i, Image = ImagesPaths.Match });
                f1.Add(new FieldModel() { I = i, Image = ImagesPaths.None });
                f2.Add(new FieldModel() { I = i, Image = ImagesPaths.None });
            }
        }
        public void SaveGame()
        {
            var fiel = GetMatchCount(f);
            var fiel1 = GetMatchCount(f1);
            var fiel2 = GetMatchCount(f2);
            var step1 = StepForFirst;
            var count = PickupCount;
            var fileName = "saveGame.txt";

            using StreamWriter sw = File.CreateText(fileName);
            {
                sw.Write(fiel);
                sw.Write("\n");
                sw.Write(fiel1);
                sw.Write("\n");
                sw.Write(fiel2);
                sw.Write("\n");
                sw.Write(step1);
                sw.Write("\n");
                sw.Write(count);
                sw.Write("\n");
            }
        }

        //Загрузка игры
        public void LoadGame()
        {
            f.Clear();
            f1.Clear();
            f2.Clear();
            var inputFileName = "saveGame.txt";
            string fileContents;
            using (StreamReader sr = File.OpenText(inputFileName))
            {
                int fileContents0 = int.Parse(sr.ReadLine());
                int fileContents1 = int.Parse(sr.ReadLine());
                int fileContents2 = int.Parse(sr.ReadLine());
                for (int i = 0; i < 20; i++)
                {
                    if (i < fileContents0) f.Add(new FieldModel() { I = i, Image = ImagesPaths.Match });
                    else f.Add(new FieldModel() { I = i, Image = ImagesPaths.None });
                    if (i < fileContents1) f1.Add(new FieldModel() { I = i, Image = ImagesPaths.Match });
                    else f1.Add(new FieldModel() { I = i, Image = ImagesPaths.None });
                    if (i < fileContents2) f2.Add(new FieldModel() { I = i, Image = ImagesPaths.Match });
                    else f2.Add(new FieldModel() { I = i, Image = ImagesPaths.None });
                }
                fileContents = sr.ReadLine();
                if (fileContents == "True") { StepForFirst = true; Step = "второй"; }
                else { StepForFirst = false; Step = "первый"; }
                PickupCount = int.Parse(sr.ReadLine());

            }
        }
    }
}
