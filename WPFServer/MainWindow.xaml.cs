using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace WPFServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        PieSeries scissors;
        PieSeries paper;
        PieSeries rock;
        PieChart chart;
        TextBox console;
        Button startServer;

        double currPaper;
        double currRock;
        double currScissors;

        delegate void WriteToConsolePtr();
        

        public MainWindow()
        {
            InitializeComponent();


            chart = this.FindName("Chart") as PieChart;
            scissors = this.FindName("Scissors") as PieSeries;
            paper = this.FindName("Paper") as PieSeries;
            rock = this.FindName("Rock") as PieSeries;
            console = this.FindName("Console") as TextBox;
            startServer = this.FindName("StartServer") as Button;

           
          
          
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            

            DataContext = this;
            
            
        }
        
        
        public Func<ChartPoint, string> PointLabel { get; set; }

        private void StartServer_Click(object sender, RoutedEventArgs e)
        {
            Server serv = new Server();

            serv.StartServer(9999, WriteToConsole);
            WriteToConsole("Server Started!");
            startServer.Content = "Connected";
            startServer.Background = Brushes.Green;
            
        }

        public void WriteToConsole(String msg)
        {

            this.Dispatcher.Invoke(() =>
            {
                console.AppendText("\n" + msg);

                console.ScrollToEnd();

                if(msg.Contains("Scissors") == true)
                {
                    UpdateChart("Scissors");
                }
                if (msg.Contains("Rock") == true)
                {
                    UpdateChart("Rock");
                }
                if (msg.Contains("Paper") == true)
                {
                    UpdateChart("Paper");
                }


            });
        }

        public void UpdateChart(String type)
        {


            switch (type)
            {
                case "Paper":
                    paper.Values = new ChartValues<double> { currPaper + 1 };
                    currPaper += 1;
                    break;
                case "Rock":
                    rock.Values = new ChartValues<double> { currRock + 1 };
                    currRock += 1;
                    break;
                case "Scissors":
                    scissors.Values = new ChartValues<double> { currScissors + 1 };
                    currScissors += 1;
                    break;
            }



        }


        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

    }
}
