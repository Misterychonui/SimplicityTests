using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace WpfSimplicityTests
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

        private void button_Click(object sender, RoutedEventArgs e)
        {            
            label1.Visibility = Visibility.Hidden;
            label2.Visibility = Visibility.Hidden;
            label3.Visibility = Visibility.Hidden;
            label4.Visibility = Visibility.Hidden;
            label5.Visibility = Visibility.Hidden;
            label6.Visibility = Visibility.Hidden;
            label7.Visibility = Visibility.Hidden;
            if (textBox.Text.Count()!=0)
            {
                long s = long.Parse(textBox.Text.ToString());
                var watch1 = new Stopwatch();
                Number n = new Number(s);
                if (n.Check()) // проверка числа на простоту и при положительном результате замер времени
                {
                    watch1.Start();
                    bool flag = n.NightingaleStrassen();
                    watch1.Stop();
                    label2.Content = watch1.ElapsedMilliseconds;
                    watch1.Reset();

                    watch1.Start();
                    flag = n.MillerRabin();
                    watch1.Stop();
                    label4.Content = watch1.ElapsedMilliseconds;
                    watch1.Reset();

                    watch1.Start();
                    flag = n.ProbabilisticAlgorithm();
                    watch1.Stop();
                    label6.Content = watch1.ElapsedMilliseconds;

                    Final_Label.Content = ("Число вероятно простое");
                    Final_Label.Visibility = Visibility.Visible;
                    label1.Visibility = Visibility.Visible;
                    label2.Visibility = Visibility.Visible;
                    label3.Visibility = Visibility.Visible;
                    label4.Visibility = Visibility.Visible;
                    label5.Visibility = Visibility.Visible;
                    label6.Visibility = Visibility.Visible;
                    label7.Visibility = Visibility.Visible;
                }
                else
                {
                    Final_Label.Visibility = Visibility.Visible;
                    Final_Label.Content = ("Число составное однозначно");
                }
            }
            else
            {
                MessageBox.Show("Введите число для проверки");
            }
        }
    }

    public class Number
        {
            private long _number;
            public Number(long number)
            {
                _number = number;

            }
            public bool Check()
            {
                Number n = new Number(_number);
                bool flag1 = n.NightingaleStrassen();
                bool flag2 = n.MillerRabin();
                bool flag3 = n.ProbabilisticAlgorithm();
                if (flag1 || flag2 && flag3) return true;
                return false;
            }
            #region Methods
            public bool NightingaleStrassen()
            {
                long n = _number;
                int k = 1000;
                long a = 17;
                var rand = new Random();
                int flag = 0;
                if (n == 2)
                    return true;
                else if (n % 2 == 0)
                    return false;
                for (int i = 1; i <= k; i++)
                {                  
                    a = rand.NextInt64(2, (int)n-1);
                if (nod(a, n) > 1 || mod(a, (n - 1) / 2, n) <= jacob_char(a, n))
                    {
                        flag = 1;
                    }
                }
                if (flag == 0) return true;
                else return false;
            }
            public bool MillerRabin()
            {
                long temp;
                long n = _number;
                bool not_prime = false;
                int s = 0, m, j, k;
                double q;
                long[] p = new long[1008];
                long[] pq = new long[1008];

                string text;
                using (var sr = new StreamReader("input.txt"))
                {
                    text = sr.ReadToEnd();
                }
                var separators = new char[] { ' ', '\r', '\n', '\t' };
                var words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                for (long i = 0; i < 1008; i++)
                {
                    p[i] = int.Parse(words[i]);
                }
                if (n == 2)
                    return true;
                else if (n % 2 == 0)
                    return false;

                for (int i = 3; i <= Math.Sqrt(n); i += 2) // проверка n на то, является ли оно степенью числа
                {
                    temp = n;
                    int power = 0;
                    while (temp / i > 0 && temp % i == 0)
                    {
                        temp /= i;
                        power++;
                    }
                    if (temp == 1 && power > 1)
                    {
                        return false;
                    }
                }

                if (!not_prime)
                // определяем первые m простых чисел p[m]
                {
                    for (m = 0; m < 1007; m++)
                        if (p[m] <= Math.Pow(n, 0.133) && Math.Pow(n, 0.133) <= p[m + 1])
                        {
                            m++;
                            break;
                        }

                    temp = n - 1;
                    while (temp / 2 > 0 && temp % 2 == 0)
                    {
                        temp /= 2;
                        s++;
                    }
                    q = (n - 1) / Math.Pow(2, s);

                    for (int i = 1; i <= m; i++) // проверка является ли p[i] делителем n
                    {
                        if (n % p[i - 1] == 0)
                        {
                            return false;
                        }
                        for (j = 0; j <= s; j++)
                            pq[j] = mod(p[i - 1], (long)(q * Math.Pow(2, j)), n);
                        if (pq[s] != 1)
                        {
                            return false;
                        }
                        if (pq[0] == 1)
                            continue;
                        temp = 0;
                        for (j = 0; j <= s; j++)
                            if (pq[j] != 1 && pq[j] > temp)
                            {
                                temp = pq[j];
                                k = j;
                            }
                        if (pq[0] == n - 1)
                            continue;
                    }
                    not_prime = true;
                }
                return true;
            }
            public bool ProbabilisticAlgorithm()
            {
                long n = _number;
                var rnd = new Random();
                if (n == 2)
                    return true;
                else if (n % 2 == 0)
                    return false;
                for (int i = 0; i < 1000; i++)
                {
                    int rndNumber = rnd.Next(0, (int)n);
                    long a = (rndNumber % (n - 2)) + 2;
                    if (gcd(a, n) != 1) // вычисление НОДа для n
                        return false;
                    if (pows(a, n - 1, n) != 1) // возведение в степень по модулю
                        return false;
                }
                return true;
            }
            #endregion
            #region PrivateProperties
            private long gcd(long a, long b) // другое вычисление НОД
            {
                if (b == 0)
                    return a;
                return gcd(b, a % b);
            }
            private long mul(long a, long b, long m)
            {
                if (b == 1)
                    return a;
                if (b % 2 == 0)
                {
                    long t = mul(a, b / 2, m);
                    return (2 * t) % m;
                }
                return (mul(a, b - 1, m) + a) % m;
            } // 1 функция для быстрого возведения по модулю

            private long pows(long a, long b, long m)
            {
                if (b == 0)
                    return 1;
                if (b % 2 == 0)
                {
                    long t = pows(a, b / 2, m);
                    return mul(t, t, m) % m;
                }
                return (mul(pows(a, b - 1, m), a, m)) % m;
            } // 2 функция для быстрого возведения по модулю
            private long mod(long number, long power, long n) // нахождение остатка от деления на n числа number, возведенного в степень power
            {
                long res = 1;
                while (power > 0)
                {
                    if (power % 2 == 1)
                        res = (res * number) % n;
                    number = (number * number) % n;
                    power /= 2;
                }
                return res;
            }
            private long nod(long m, long n) // вычисление НОД
            {
                while (m != 0 && n != 0)
                {
                    if (m >= n)
                        m %= n;
                    else
                        n %= m;
                }
                return m + n;
            }
            private int jacob_char(long a, long b) // нахождение символа Якоби
            {
                if (nod(a, b) != 1)
                    return 0;
                else
                {
                    int r = 1;
                    if (a < 0)
                    {
                        a = -a;
                        if (b % 4 == 3)
                            r = -r;
                    }
                    do
                    {
                        int t = 0;
                        while (a % 2 == 0)
                        {
                            t += 1;
                            a /= 2;
                        }
                        if (t % 2 == 1 && (b % 8 == 3 || b % 8 == 5))
                            r = -r;
                        if (a % 4 == 3 && b % 4 == 3)
                            r = -r;
                        long c = a;
                        a = b % c;
                        b = c;
                    }
                    while (a != 0);
                    return r;
                }
            }
            #endregion
        }
    
}
