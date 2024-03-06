using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const double dt = .1;   // 0.1
        

        double cosa, sina;
        double x, y, v0, m, S, k, vx, vy, vend, t;

        int series = 1;

        private void btNew_Click(object sender, EventArgs e)
        {
            chart1.Series.Add("Series" + (series+1).ToString());
            chart1.Series["Series" + (series + 1).ToString()].XValueType = ChartValueType.Auto;
            chart1.Series["Series" + (series+1).ToString()].ChartType = SeriesChartType.Line;
            chart1.Series["Series" + (series + 1).ToString()].BorderWidth = 3; 
        }

        int index = 0;

        const double g = 9.81;  // Gravity
        const double C = .15;
        const double rho = 1.29;  // Rho = P

        List<double> arrayY;


        private void resetButton_Click(object sender, EventArgs e)
        {
            resultTime.Text = "...";
            resultDistance.Text = "...";
            resultMaxHeight.Text = "...";
            resultSpeedattheendpoint.Text = "...";

            timer1.Stop();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            t = t + dt;
            double vxold = vx;
            double vyold = vy;
            double vroot = Math.Sqrt(vx * vx + vy * vy);

            vx = vxold - k * vxold * vroot * dt;
            vy = vyold - (g + k * vyold * vroot) * dt;

            x = x * vx * dt;
            y = y + vy * dt;


            chart1.Series["Series" + (series).ToString()].Points.AddXY(x, y);
            if (y > 0)
            {
                arrayY.Add(y);
            }
            if(y <= 0)
            {
                timer1.Stop();
                index += 1;
                vend = Math.Sqrt(vx * vx + vy * vy);
                resultTime.Text = (Math.Round(t, 2)).ToString();
                resultDistance.Text = (Math.Round(x, 2)).ToString();
                resultSpeedattheendpoint.Text = (Math.Round(vend, 2)).ToString();
                resultMaxHeight.Text = (Math.Round(arrayY.Max(), 2)).ToString();
            }
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            t = 0;
            x = 0;

            y = (double)edHeight.Value;
            v0 = (double)edSpeed.Value;
            arrayY = new List<double>();
 
            // Change angle to radiant
            double a = (double)edAngle.Value * Math.PI / 180;


            cosa = Math.Cos(a);
            sina = Math.Sin(a);
            S = (double)edArea.Value;
            m = (double)edWeight.Value;
            k = .5 * C * S * rho / m;

            vx = v0 * cosa;
            vy = v0 * sina;

            chart1.Series["Series" + (series).ToString()].Points.AddXY(x, y);


            timer1.Start();
        }

        private void chart1_Click(object sender, EventArgs e)  // btStart  ==   chart1
        {

        }
    }
}
