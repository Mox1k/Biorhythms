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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
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


            if (Custom_Date.IsEnabled == true)
            {
                 arbitrarys = Convert.ToInt32(Custom_Date.Text);
            }
            else
            {
                 arbitrarys = Convert.ToInt32(cmbCountdown.Text);
            }
            DateTime dateCountDown;
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



        }

    }
}
