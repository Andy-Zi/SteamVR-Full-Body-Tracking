using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartWin
{
    /// <summary>
    /// Based on https://stackoverflow.com/questions/40244699/how-to-plot-a-3d-graph-to-represent-an-object-in-space
    /// </summary>
    public class Graph3D : Chart
    {
        public ChartArea ChartArea3D { get; protected set; }
        public Series Chard3DSeries { get; protected set; }
        public Graph3D()
        {
            ChartArea3D = new ChartArea();
            this.ChartAreas.Add(ChartArea3D);
            ChartArea3D.BackColor = Color.White;
            ChartArea3D.Area3DStyle.Enable3D = true;// set the chartarea to 3D!
            ChartArea3D.Area3DStyle.Rotation = 35;
            ChartArea3D.Area3DStyle.Inclination = 20;
            ChartArea3D.AxisX.Minimum = -1;
            ChartArea3D.AxisY.Minimum = -1;
            ChartArea3D.AxisX.Maximum = 1;
            ChartArea3D.AxisY.Maximum = 1;
            ChartArea3D.AxisX.Interval = 0.25;
            ChartArea3D.AxisY.Interval = 0.25;
            ChartArea3D.AxisX.Title = "X-Achse";
            ChartArea3D.AxisY.Title = "Y-Achse";
            ChartArea3D.AxisX.MajorGrid.Interval = 1;
            ChartArea3D.AxisY.MajorGrid.Interval = 1;
            ChartArea3D.AxisX.MinorGrid.Enabled = true;
            ChartArea3D.AxisY.MinorGrid.Enabled = true;
            ChartArea3D.AxisX.MinorGrid.Interval = 0.25;
            ChartArea3D.AxisY.MinorGrid.Interval = 0.25;
            ChartArea3D.AxisX.MinorGrid.LineColor = Color.White;
            ChartArea3D.AxisY.MinorGrid.LineColor = Color.White;

            // we add two series:
            this.Series.Clear();
            Chard3DSeries = this.Series.Add("S0");
            Chard3DSeries.ChartType = SeriesChartType.Bubble;   // this ChartType has a YValue array
            Chard3DSeries.MarkerStyle = MarkerStyle.Circle;
            Chard3DSeries["PixelPointWidth"] = "100";
            Chard3DSeries["PixelPointGapDepth"] = "1";

            this.ApplyPaletteColors();
            this.PostPaint += Graph3D_PostPaint;

        }

        private void Graph3D_PostPaint(object sender, ChartPaintEventArgs e)
        {


            if (this.Series.Count < 1) return;
            if (Chard3DSeries.Points.Count < 1) return;


            e.ChartGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            List<List<PointF>> data = new List<List<PointF>>();

            data.Add(GetPointsFrom3D(ChartArea3D, Chard3DSeries, Chard3DSeries.Points.ToList(), e.ChartGraphics));

            //renderLines(data, e.ChartGraphics.Graphics, true);
            renderPoints(data, e.ChartGraphics.Graphics, 12);   
        }

        public int AddXY3d(double xVal, double yVal, double zVal)
        {
            int p = Chard3DSeries.Points.AddXY(xVal, yVal, zVal);
            // the DataPoint are transparent to the regular chart drawing:
            Chard3DSeries.Points[p].Color = Color.Transparent;
            return p;
        }

        List<PointF> GetPointsFrom3D(ChartArea ca, Series s,
                     List<DataPoint> dPoints, ChartGraphics cg)
        {
            var p3t = dPoints.Select(x => new Point3D((float)ca.AxisX.ValueToPosition(x.XValue),
                (float)ca.AxisY.ValueToPosition(x.YValues[0]),
                (float)ca.AxisY.ValueToPosition(x.YValues[1]))).ToArray();
            ca.TransformPoints(p3t.ToArray());

            return p3t.Select(x => cg.GetAbsolutePoint(new PointF(x.X, x.Y))).ToList();
        }

        void renderPoints(List<List<PointF>> data, Graphics graphics, float width)
        {
                for (int p = 0; p < Chard3DSeries.Points.Count; p++)
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, Chard3DSeries.Color)))
                        graphics.FillEllipse(brush, data[0][p].X - width / 2,
                                             data[0][p].Y - width / 2, width, width); 
        }

        void renderLines(List<List<PointF>> data, Graphics graphics, bool curves)
        {
                if (data[0].Count > 1)
                    using (Pen pen = new Pen(Color.FromArgb(64, Chard3DSeries.Color), 2.5f))
                        if (curves) graphics.DrawCurve(pen, data[0].ToArray());
                        else graphics.DrawLines(pen, data[0].ToArray());
        }


    }
}
