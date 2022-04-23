//
// 
//
// https://www.codeproject.com/Articles/30180/Simple-Signal-Generator
//
//


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGenOscill
{
    public partial class Form1 : Form
    {
        private string strSignalType = "SINE";
        private float frequency = 1f;
        private float amplitude = 75f;
        float t;  // время
        float v_prev;   // Значение сигнала в предыдущий момент вермени. Необходимо для прорсовки графика
        Pen pen = new Pen(Brushes.Lime, 5);  // Ширина линии 3 пикселя

        int x;    // Координата развертки сигнала в пределах компонента panel1



        public Form1()
        {
            InitializeComponent();
        }

        private void btnSin_Click(object sender, EventArgs e) { strSignalType = "SINE"; pen = new Pen(Brushes.Lime, 5); }
        private void btnTriangle_Click(object sender, EventArgs e) { strSignalType = "TRIANGLE"; pen = new Pen(Brushes.Orange, 5); }
        private void btnSquare_Click(object sender, EventArgs e)   { strSignalType = "SQUARE"; pen = new Pen(Brushes.Yellow, 5); }
        private void btnSawtooth_Click(object sender, EventArgs e) { strSignalType = "SAWTOOTH"; pen = new Pen(Brushes.Cyan, 5); }


        private void nudFrequency_ValueChanged(object sender, EventArgs e)
        {
            frequency = (float)nudFrequency.Value;
        }

        private void nudAmplitude_ValueChanged(object sender, EventArgs e)
        {
            amplitude = (float)nudAmplitude.Value;
        }

        private float GetValue(float time)
        {
            float value = 0f;
            float t = frequency * time;

            switch (strSignalType)
            {
                case "SINE": // sin( 2 * pi * t )
                    value = (float)Math.Sin(2f * Math.PI * t);
                    break;
                case "SQUARE": // sign( sin( 2 * pi * t ) )
                    value = Math.Sign( Math.Sin(2f * Math.PI * t));   // если у синусоиды знак плюс, то на выходе 1, если минус, то -1
                    break;
                case "TRIANGLE":
                    // 2 * abs( t - 2 * floor( t / 2 ) - 1 ) - 1
                    value = 1f - 4f * (float)Math.Abs( Math.Round(t - 0.25f) - (t - 0.25f));
                    break;
                case "SAWTOOTH":
                    // 2 * ( t/a - floor( t/a + 1/2 ) )
                    value = 2f * (t - (float)Math.Floor(t + 0.5f));
                    break;
            }

            return (amplitude * value);
        }

        /*
        public float GetValue()
        {
            float time = (float)(Stopwatch.GetTimestamp() - startTime)
                            / ticksPerSecond;
            return GetValue(time);
        }
        */
        //public float GetValue()
        //{
        //    //float time = (float)(Stopwatch.GetTimestamp() - startTime)
        //    //                / ticksPerSecond;
        //    return GetValue( 0);
        //}


        private void timer1_Tick(object sender, EventArgs e)
        {
            //
            //  RNN: С точным значением частоты сигнала и значением дискретизации
            //  Результат виден на экране. 

            // Частота сигнала 1 Гц
            //timer1.Interval = 100;   // Но панели свойств компонента timer1 задано значение 100 мс.
            //t += 0.1f;   // Дискретизацию (оцифровку сигнала) делаем 10 раз в секунду
            //t += 0.01f;

            // Частота сигнала 10 Гц
            timer1.Interval = 10;
            //t += 0.1f;   // Дискретизацию (оцифровку сигнала) делаем 100 раз в секунду
            t += 0.01f;


            //float A = 50f;

            float v = GetValue( t);     // Передаем функции заданное время и получаем значение, выбранного сигнала для этого момента времени

            //Console.WriteLine( Math.Round(t, 2) + ":   " + v);    // Вывод фазы и значениt сигнала
            Console.WriteLine( v);     // Вывод только значения сигнала

            Graphics g = panel1.CreateGraphics();
            
            //Pen pen = Pens.Lime;    // ширина линии будет один пиксель

            //g.DrawLine(pen, 0, 0, 100, 100);  // DEBUG
            //g.Clear( panel1.BackColor);   // Перед тем как рисовать новую линию, стираем старую, стирая всю панел paenl1. Очень грубо, но просто
            //g.DrawLine(pen, 0, 0, x, v* A + panel1.Height/2 );

            //g.DrawLine( pen, x, 0, x, v + panel1.Height / 2);

            int OFF = panel1.Height / 2;
            g.DrawLine(pen, x-1, v_prev + OFF, x, v + OFF);
            // panel1.Height/2 -  это центр панели по вертикали
            // x - 1 - это координата X для предыдущей v_prev точки сигнала

            v_prev = v;  // Сохраняем текущее значение сигнала. На следующем круге оно будет предыдущим

            x += 1;   // Развертка по горизонтали
            if (x > panel1.Width)
            {
                x = 0;
                g.Clear(panel1.BackColor);  // Очисить панель
                //panel1.Invalidate();  // Очисить панель можно и так

            }
            
        }

        private void btnClearConsole_Click(object sender, EventArgs e)
        {
            Console.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Invalidate();  // DEBUG
        }
    }
}
