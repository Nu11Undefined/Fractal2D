using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Fractal2D
{
    class SystemEnvironment
    {
        ComplexDynamicMap Model { get; set; }
        Controller Con { get; set; }
        RenderWindow Visualizator { get; set; }

        GraphicsParamCDM GraphParams { get; set; }
        static int width = 1920, height = 1080,margin = 10;
        public SystemEnvironment()
        {
            double w0 = 3;
            int k = 2;
            width /= k; height /= k;
            Complex c1 = new Complex(-2, -2);
            Color[] cols = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Black };
            GraphParams = new GraphicsParamCDM(
                new RectangleF(margin, margin, width - 2 * margin, height - 2 * margin), cols.Reverse().ToArray());
            Model = new ComplexDynamicMap(c1, new Complex(w0, w0 * height / width),
                new Complex(0.0, 0.01), (int)GraphParams.DrawRect.Width, (int)GraphParams.DrawRect.Height);
            //Model.FillRandom(0.5);
            Visualizator = new GDIRenderer(width, height);
            Con = new Controller(Visualizator.Controller);
            InitObjectConnection();
            Visualizator.Shown += (s, e) => Visualizator.DrawSystem(Model, GraphParams);
            Application.Run(Visualizator);
        }
        private void Update()
        {
            Model.CalcStage();
            Visualizator.DrawSystem(Model, GraphParams);
        }
        private void InitObjectConnection()
        {
            Con.ZoomIn += (x, y) =>
            {
                Model.ZoomIn(GraphParams.ScreenToModel(x, y, Model));
                Update();
            };
            Con.ZoomOut += (x, y) =>
            {
                Model.ZoomOut(GraphParams.ScreenToModel(x, y, Model));
                Update();
            };
            Con.ParamDown += () =>
            {
                Model.ParamDown();
                Update();
            };
            Con.ParamUp += () =>
            {
                Model.ParamUp();
                Update();
            };
            Con.CursorPositionChanged += (x, y) =>
            {
                Console.WriteLine($"CursorPosition = {GraphParams.ScreenToModel(x, y, Model)}");
            };
            Con.Exit += () => Visualizator.Close();

            // из плеера
            Visualizator.GraphicsSizeChanged += (w, h) =>
            {
                GraphParams.SetRect(w,h, margin);
                Update();
            };
        }
    }
}
