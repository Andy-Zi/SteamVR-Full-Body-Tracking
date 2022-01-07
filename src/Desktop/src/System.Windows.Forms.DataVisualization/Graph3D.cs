using PTSC.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartWin
{

    public class JointData
    {
        public JointData(string name, Point3D coordinates)
        {
            Name = name;
            Coordinates = coordinates;
            if (Graph3D.ColorLookup.ContainsKey(name))
                Color = Graph3D.ColorLookup[name];
            else
                Color = Color.Black;
        }

        public string Name { get; }
        public Point3D Coordinates { get; }
        public Color Color { get; }

        public PointF AbsoluteCoordinates { get; set; }
    }

    /// <summary>
    /// Based on https://stackoverflow.com/questions/40244699/how-to-plot-a-3d-graph-to-represent-an-object-in-space
    /// </summary>
    public class Graph3D : Chart
    {
        const float Interval = 0.5f;
        const float JointSize = 15;

        public readonly static Dictionary<string, Color> ColorLookup = new()
        {
            { "NOSE", Color.Green },
            { "LEFT_EAR", Color.Green },
            { "RIGHT_EAR", Color.Green },
            { "LEFT_EYE", Color.Green },
            { "RIGHT_EYE", Color.Green },

            { "LEFT_ELBOW", Color.Red },
            { "LEFT_WRIST", Color.Red },

            { "LEFT_KNEE", Color.Violet },
            { "LEFT_ANKLE", Color.Violet },

            { "RIGHT_ELBOW", Color.Blue },
            { "RIGHT_WRIST", Color.Blue },

            { "RIGHT_KNEE", Color.Yellow },
            { "RIGHT_ANKLE", Color.Yellow },
        };

        public readonly static List<(List<string>, Color)> PossibleBodies = new()
        {
            new(new() { "LEFT_SHOULDER", "LEFT_HIP", "RIGHT_HIP", "RIGHT_SHOULDER", "LEFT_SHOULDER" }, Color.Black),
            new(new() { "LEFT_SHOULDER", "LEFT_ELBOW", "LEFT_WRIST"}, Color.Red),
            new(new() { "RIGHT_SHOULDER", "RIGHT_ELBOW", "RIGHT_WRIST"}, Color.Blue),
            new(new() { "RIGHT_HIP", "RIGHT_KNEE", "RIGHT_ANKLE" }, Color.Yellow),
            new(new() { "LEFT_HIP", "LEFT_KNEE", "LEFT_ANKLE" }, Color.Violet),
            new(new() { "LEFT_EAR", "LEFT_EYE", "NOSE" , "RIGHT_EYE", "RIGHT_EAR"}, Color.Green),
        };
        public ChartArea ChartArea3D { get; protected set; }
        public Series Chard3DSeries { get; protected set; }

        public Dictionary<string, JointData> Points { get; protected set; } = new Dictionary<string, JointData>();
        public Graph3D()
        {
            ChartArea3D = new ChartArea();
            this.ChartAreas.Add(ChartArea3D);
            ChartArea3D.BackColor = Color.White;
            ChartArea3D.Area3DStyle.Enable3D = true;// set the chartarea to 3D!
            ChartArea3D.Area3DStyle.Rotation = 15;
            ChartArea3D.Area3DStyle.Inclination = 50;
            ChartArea3D.AxisX.Minimum = -1;
            ChartArea3D.AxisX.Maximum = 1;


            ChartArea3D.AxisX.MajorGrid.Enabled = false;
            ChartArea3D.AxisY.MajorGrid.Enabled = false;
            ChartArea3D.AxisX.MinorGrid.Enabled = false;
            ChartArea3D.AxisY.MinorGrid.Enabled = false;

            ChartArea3D.AxisX.MinorGrid.LineColor = Color.White;
            ChartArea3D.AxisY.MinorGrid.LineColor = Color.White;

            this.Series.Clear();
            Chard3DSeries = this.Series.Add("S0");
            Chard3DSeries.ChartType = SeriesChartType.Bubble;   // this ChartType has a YValue array
            Chard3DSeries.MarkerStyle = MarkerStyle.Circle;
            Chard3DSeries["PixelPointWidth"] = "10";
            Chard3DSeries["PixelPointGapDepth"] = "3";

            this.ApplyPaletteColors();
            this.PostPaint += Graph3D_PostPaint;

        }

        private void Graph3D_PostPaint(object sender, ChartPaintEventArgs e)
        {
            if (Points.Count < 1) return;

            GetPointsFrom3D(e.ChartGraphics);

            renderJoints(e.ChartGraphics.Graphics);
            renderBodies(e.ChartGraphics.Graphics);
        }

        public void Plot(IModuleDataModel moduleDataModel)
        {
            try
            {

                Build3DPoints(moduleDataModel.GetData());

                this.Refresh();
            }
            catch { }
        }

        private void Build3DPoints(Dictionary<string, List<double>> data)
        {
            Points?.Clear();
            Chard3DSeries.Points?.Clear();

            foreach (var (key, value) in data)
            {
                if (value == null)
                    continue;

                Points.Add(key, new(key, new((float)-value[0], (float)value[1], (float)-value[2])));

                var p = Chard3DSeries.Points?.AddXY((float)-value[0], (float)-value[1], (float)-value[2]);
                if(Chard3DSeries.Points != null)
                {
                    Chard3DSeries.Points[p ?? 0].Color = Color.Transparent;
                }
             
            }
        }

        void GetPointsFrom3D(ChartGraphics cg)
        {
            foreach (var (_, point) in Points)
            {
                var point3D = new Point3D(
                    (float)ChartArea3D.AxisX.ValueToPosition(point.Coordinates.X),
                    (float)ChartArea3D.AxisY.ValueToPosition(point.Coordinates.Y),
                    (float)ChartArea3D.AxisY.ValueToPosition(point.Coordinates.Z));

                ChartArea3D.TransformPoints(new Point3D[] { point3D });
                point.AbsoluteCoordinates = cg.GetAbsolutePoint(new PointF(point3D.X, point3D.Y));
            }
        }

     
        void renderJoints(Graphics graphics)
        {

            foreach (var (_, point) in Points)
            {
                using SolidBrush brush = new SolidBrush(point.Color);
                graphics.FillEllipse(brush, point.AbsoluteCoordinates.X - JointSize / 2,
                                     point.AbsoluteCoordinates.Y - JointSize / 2, JointSize, JointSize);
            }

        }


        void renderBodies(Graphics graphics)
        {

            foreach (var (parts, color) in PossibleBodies)
            {
                var bodypoints = new List<PointF>();

                foreach (var part in parts)
                {
                    if (Points.ContainsKey(part))
                        bodypoints.Add(Points[part].AbsoluteCoordinates);
                }
                if (bodypoints.Count != parts.Count)
                    continue;
                using Pen pen = new Pen(color, 2.5f);
                graphics.DrawLines(pen, bodypoints.ToArray());

            }
        }

        public void Clear()
        {
            try
            {
                this.Chard3DSeries.Points.Clear();
                this.Points.Clear();

                this.Refresh();
            }
            catch
            {

            }
        } 
    }
}

