using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace Biorhythms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        List<string> Labels = new List<string>();

        private void Check_Allow_Checked(object sender, RoutedEventArgs e)
        {
            Custom_Date.IsEnabled = true;
        }

        private void Check_Allow_Unchecked(object sender, RoutedEventArgs e)
        {
            Custom_Date.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Biorhythm> biorhythms = new List<Biorhythm>();
            int arbitrarys = 0;
            DateTime birthDate = Convert.ToDateTime(BirthDate.Text);
                
            const int phys = 23;
            const int emot = 28;
            const int intel = 33;

            //Создаем таблицу

            DateTime dateCountDown = Convert.ToDateTime(countDownTime.Text);

            if (Custom_Date.IsEnabled == true)
            {
                 arbitrarys = Convert.ToInt32(Custom_Date.Text);
            }
            else
            {
                 arbitrarys = Convert.ToInt32(cmbCountdown.Text);
            }
            for (int i = 0; i < arbitrarys; i++)
            {
                dateCountDown = Convert.ToDateTime(countDownTime.Text);
                var bior = new Biorhythm()
                {
                    Date = dateCountDown.AddDays(i).ToShortDateString(),
                    Emotional = Math.Round((Math.Sin(2 * Math.PI * ((dateCountDown - birthDate).Days + i) / emot)) * 100, 2),
                    Intellectual = Math.Round((Math.Sin(2 * Math.PI * ((dateCountDown - birthDate).Days + i) / intel)) * 100, 2),
                    Physical = Math.Round((Math.Sin(2 * Math.PI * ((dateCountDown - birthDate).Days + i) / phys)) * 100, 2),
                };
                bior.Total = Math.Round(bior.Emotional + bior.Intellectual + bior.Physical, 2);

                biorhythms.Add(bior);
            }
            Dates.ItemsSource = biorhythms;

            // Создание графика

            ChartValues<double> PhysicalValues = new ChartValues<double>();
            ChartValues<double> EmotionalValues = new ChartValues<double>();
            ChartValues<double> IntellectualValues = new ChartValues<double>();
            SeriesCollection series = new SeriesCollection();

            Labels.Clear();

            foreach (Biorhythm bior in biorhythms)
            {
                PhysicalValues.Add(bior.Physical);
                EmotionalValues.Add(bior.Emotional);
                IntellectualValues.Add(bior.Intellectual);
                Labels.Add(bior.Date.ToString());
            }

            series.Add(new LineSeries
            {
                Title = "Физические ритмы",
                Values = PhysicalValues,
            });
            series.Add(new LineSeries
            {
                Title = "Эмоциональные ритмы",
                Values = EmotionalValues
            });
            series.Add(new LineSeries
            {
                Title = "Интеллектуальные ритмы",
                Values = IntellectualValues
            });
            chart.AxisY = new AxesCollection()
            {
                new Axis()
                {
                    Title = "Значения",
                    MinValue = -100,
                    MaxValue = 100,
                }
            };
            chart.Series = series;
            chart.Update();

            //Создание статистики

            double maxEm = double.MinValue;
            double maxInt = double.MinValue;
            double maxPhys = double.MinValue;
            double maxSum = double.MinValue;
            string maxEmDate = String.Empty;
            string maxIntDate = String.Empty;
            string maxPhysDate = String.Empty;
            string maxSumDate = String.Empty;


            foreach (Biorhythm bior in biorhythms)
            {
                if (bior.Physical > maxPhys)
                {
                    maxPhys = bior.Physical;
                    maxPhysDate = bior.Date;
                }
                if (bior.Emotional > maxEm)
                {
                    maxEm = bior.Emotional;
                    maxEmDate = bior.Date;
                }
                if (bior.Intellectual > maxInt)
                {
                    maxInt = bior.Intellectual;
                    maxIntDate = bior.Date;
                }
                if (bior.Total > maxSum)
                {
                    maxSum = bior.Total;
                    maxSumDate = bior.Date;
                }
            }

            list.Items.Clear();
            list.Items.Add($"Дата рождения - {birthDate.ToShortDateString()}");
            list.Items.Add($"Длительность прогноза - {arbitrarys}");
            list.Items.Add($"Период с - {dateCountDown.ToShortDateString()} - {dateCountDown.AddDays(arbitrarys).ToShortDateString()}");
            list.Items.Add($"Эмоциональный максимум - {maxEm}: {maxEmDate}");
            list.Items.Add($"Интеллектуальный максимум - {maxInt}: {maxIntDate}");
            list.Items.Add($"Физический максимум - {maxPhys}: {maxPhysDate}");
        }

        private void dateon_Checked(object sender, RoutedEventArgs e)
        {
            chart.AxisX = new AxesCollection()
                {
                    new Axis()
                    {
                        Title = "Дата",
                        Labels = Labels,
                    }
                };
        }

        private void dateon_Unchecked(object sender, RoutedEventArgs e)
        {
            chart.AxisX = new AxesCollection()
            {
                new Axis()
                {
                        Title = "Дни",
                        MinValue = 0,
                }
            };
        }
    }
}
