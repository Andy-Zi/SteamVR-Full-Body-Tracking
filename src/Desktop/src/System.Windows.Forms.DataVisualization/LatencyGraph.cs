using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartWin
{
    public class LatencyGraph :  Chart
    {
        ChartArea ChartArea;
        Queue<double> Latencies = new();
        Series series;
        const int maxLength = 25;
        const int windowSize = 15;
        int counter = 0;
        double currentValue = 0;

        public LatencyGraph()
        {
            ChartArea= new ChartArea();
            this.ChartAreas.Add(ChartArea);
            ChartArea.AxisX.MajorGrid.Enabled = false;
            ChartArea.AxisX.MinorGrid.Enabled = false;
            ChartArea.AxisY.MajorGrid.Enabled = false;
            ChartArea.AxisY.MinorGrid.Enabled = false;
            ChartArea.AxisX.IsStartedFromZero = true;
            ChartArea.AxisX.LabelStyle.Enabled = false;
            ChartArea.AxisY.Title = "ms";

           series = this.Series.Add("Series1");
            series.ChartType = SeriesChartType.Line;
            series.Color = Color.Red;
        }

        public void Update(double value)
        {
            if(counter < windowSize)
            {
                currentValue += value;
                counter++;
            }
            else{
                if (Latencies.Count > maxLength)
                    Latencies.Dequeue();
                Latencies.Enqueue(currentValue/ windowSize);

                try
                {
                    int i = 0;
                    series.Points?.Clear();
                    foreach (var latencie in Latencies)
                    {
                        series.Points?.AddXY(i, latencie);
                        i++;
                    }

                    currentValue = 0;
                    counter = 0;
                    this?.Refresh();
                }catch{

                }
  
            }
        }

        public void Clear()
        {
            try
            {
                series.Points?.Clear();
                this?.Refresh();
            }
            catch
            {

            }
        }
    }
}
