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

namespace ML_lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            drawGraph(new Classifier().Classify());
        }

        public double getAuc(LinkedList<Point> points)
        {
            var result = 0.0;
            for (var i = 0; i < points.Count - 1; i++)
            {
                var curr = points.ElementAt(i);
                var next = points.ElementAt(i + 1);

                result += Math.Abs(curr.X - next.X) * (curr.Y + next.Y) * 0.5;
            }
            return result;
        }

        public void setText(string text)
        {
            this.text.AppendText(text);
        }

        public void drawGraph(LinkedList<Point> points)
        {
            var arrow = new Line();
            arrow.Stroke = Brushes.Gray;
            arrow.X1 = 40;
            arrow.Y1 = 20;
            arrow.X2 = 40;
            arrow.Y2 = 1 * (image.Height - 80) + 40;
            image.Children.Add(arrow);
            var arrow1 = new Line();
            arrow1.Stroke = Brushes.Gray;
            arrow1.X1 = 1 * (image.Width - 80) + 60;
            arrow1.Y1 = 1 * (image.Height - 80) + 40;
            arrow1.X2 = 40;
            arrow1.Y2 = 1 * (image.Height - 80) + 40;
            image.Children.Add(arrow1);

            for (int i = 0; i < points.Count - 1; i++)
            {
                var line = new Line();
                line.Stroke = Brushes.CadetBlue;
                //line.Fill = Brushes.Magenta;
                line.StrokeThickness = 3;
                line.X1 = points.ElementAt(i).X * (image.Width - 80) + 40;
                line.Y1 = (1 - points.ElementAt(i).Y) * (image.Height - 80) + 40;
                line.X2 = points.ElementAt(i + 1).X * (image.Width - 80) + 40;
                line.Y2 = (1 - points.ElementAt(i + 1).Y) * (image.Height - 80) + 40;
                image.Children.Add(line);
            }
            setText(string.Concat(getAuc(points)));
        }
    }
}
