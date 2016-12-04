using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace ML_lab2
{
    class Classifier
    {
        private static readonly string _filepath = "D:\\std\\Бигдата Павловский\\ML-lab2\\ML-lab2\\Iris.csv";
        private Dictionary<double[], bool> train = new Dictionary<double[], bool>();
        private Dictionary<double[], bool> test = new Dictionary<double[], bool>();

        private const int _trainCount = 20;
        private const int _classElementCount = 50;
        private const int _iterationCount = 10000;

        private const double _alpha = 0.01;
        private double[] thetas = new double[4];

        private void ReadData()
        {
            var count = 0;
            string str;
            bool isFirstClass = true;
            using (var stream = new StreamReader(_filepath))
            {
                while ((str = stream.ReadLine()) != null)
                {
                    var elements = str.Split(',');
                    var features = new double[4];
                    for (int i = 0; i < 4; i++)
                    {
                        features[i] = double.Parse(elements[i], System.Globalization.CultureInfo.InvariantCulture);
                    }
                    if (count < _trainCount)
                        train.Add(features, isFirstClass);
                    else
                        test.Add(features, isFirstClass);
                    count++;
                    if (count == _classElementCount)
                    {
                        count = 0;
                        isFirstClass = false;
                    }
                }
            }
        }

        private double LogisticCurve(double z)
        {
            return 1.0 / (1.0 + Math.Exp(-z));
        }

        private double ScalarMultiply(double[] v1, double[] v2)
        {
            var res = 0.0;
            for (var i = 0; i < v1.Count(); i++)
                res += v1[i] * v2[i];
            return res;
        } 

        private void TrainRegression()
        {
            for (var i = 0; i < _iterationCount; i++)
            {
                double[] currThetas = new double[4];
                Array.Copy(thetas, currThetas, thetas.Count());
                for (var j = 0; j < 4; j++)
                {
                    double sum = 0.0;
                    for (var k = 0; k < train.Count; k++)
                    {
                        var iris = train.ElementAt(k);
                        var rightAns = (iris.Value) ? 1.0 : 0.0;
                        sum += (rightAns - LogisticCurve(ScalarMultiply(currThetas, iris.Key))) * iris.Key.ElementAt(j);
                    }
                    sum /= train.Count;
                    sum *= _alpha;
                    thetas[j] = currThetas[j] + sum;
                }
            }
        }

        public LinkedList<Point> Classify()
        {
            ReadData();
            TrainRegression();

            var result = new LinkedList<Point>();

            for (var threshhold = 0.0; threshhold <= 1.0; threshhold += 0.0001)
            {
                var testSize = (_classElementCount - _trainCount) * 3;
                double FPR = 0.0, TPR = 0.0;
                for (var j = 0; j < testSize; j++)
                {
                    var currAns = LogisticCurve(ScalarMultiply(thetas, test.ElementAt(j).Key));
                    if (currAns >= threshhold)
                        if (test.ElementAt(j).Value)
                            TPR++;
                        else
                            FPR++;
                }
                double p = _classElementCount - _trainCount;
                double n = p * 2;
                TPR /= p;
                FPR /= n;
                result.AddLast(new Point(FPR, TPR));
            }

            return result;
        }
    }
}
