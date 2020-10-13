using System;
using System.Collections;
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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Beginning of PValue Calc
        int txtNum1 = 0, txtNum2 = 0;
        TextBox[] inputBox1;
        TextBox[] inputBox2;

        public MainWindow()
        {
            InitializeComponent();
        }

        static public double GetPValue(int trialCount, double tStat)
        {
            double[,] pValueTable = 
            {
                     //- The row 0 has the pValue
                     {  0.50, 0.20, 0.10,  0.05,  0.02,  0.01,   0.005,   0.002, 0.001  } //-  0

                    , { 1.00, 3.08, 6.31, 12.71, 31.82, 63.66, 127.32 , 318.31 , 636.62 } //-  1
                    , { 0.82, 1.89, 2.92,  4.30,  6.97,  9.93,  14.09 ,  22.33 ,  31.56 } //-  2
                    , { 0.77, 1.64, 2.35,  3.18,  4.54,  5.84,   7.45 ,  10.22 ,  12.92 } //-  3
                    , { 0.74, 1.53, 2.13,  2.78,  3.57,  4.60,   5.60 ,   7.17 ,   8.61 } //-  4
                    , { 0.73, 1.48, 2.02,  2.57,  3.37,  4.03,   4.77 ,   5.89 ,   6.87 } //-  5
                    , { 0.72, 1.44, 1.94,  2.45,  3.14,  3.71,   4.32 ,   5.21 ,   5.96 } //-  6
                    , { 0.71, 1.42, 1.90,  2.37,  3.00,  3.50,   4.03 ,   4.79 ,   5.41 } //-  7
                    , { 0.71, 1.40, 1.86,  2.31,  2.90,  3.36,   3.83 ,   4.50 ,   5.04 } //-  8
                    , { 0.70, 1.38, 1.83,  2.26,  2.82,  3.25,   3.69 ,   4.30 ,   4.78 } //-  9
                    , { 0.70, 1.37, 1.81,  2.23,  2.76,  3.17,   3.58 ,   4.14 ,   4.59 } //- 10
                    , { 0.70, 1.36, 1.80,  2.20,  2.72,  3.11,   3.50 ,   4.03 ,   4.44 } //- 11 : 11 -  15
                    , { 0.70, 1.34, 1.75,  2.13,  2.60,  2.95,   3.29 ,   3.73 ,   4.07 } //- 12 : 15 -  20
                    , { 0.69, 1.33, 1.73,  2.09,  2.53,  2.85,   3.15 ,   3.55 ,   4.85 } //- 13 : 20 -  50
                    , { 0.68, 1.30, 1.68,  2.01,  2.40,  2.68,   2.94 ,   3.26 ,   3.50 } //- 14 : 50 - 100
            };

            double pValue = 0;
            bool isFound = false;
            int rowToUse = trialCount - 2;

            if (rowToUse < 11)
            {
            }
            else if (rowToUse < 15)
            {
                rowToUse = 11;
            }
            else if (rowToUse < 20)
            {
                rowToUse = 12;
            }
            else if (rowToUse < 50)
            {
                rowToUse = 13;
            }
            else if (rowToUse <= 100)
            {
                rowToUse = 14;
            }
            else
            {
                throw new NotSupportedException("TrialCount not supported: " + trialCount);
            }

            int pValueTableColumnCount = pValueTable.GetLength(1);
            for (int i = 1; i < pValueTableColumnCount; i++)
            {
                if (tStat < pValueTable[rowToUse, i])
                {
                    pValue = pValueTable[0, i - 1];
                    isFound = true;
                    break;
                }
            }
            if (false == isFound)
            {
                pValue = pValueTable[0, pValueTableColumnCount - 1];
            }
            return pValue;
        }

        public void calculate()
        {
            double array1Sum = 0, array2Sum = 0;

            double array1Average = 0, array2Average = 0;

            double sumSquares1 = 0, sumSquares2 = 0;

            double pooledVariance = 0, ugh = 0, tStatistic = 0, pValue = 0, percentChance = 0;

            double[] tArray1 = new double[txtNum1];
            double[] tArray2 = new double[txtNum2];

            for (int i = 0; i <= txtNum1 - 1; i++)
            {
                tArray1[i] = double.Parse(inputBox1[i].Text);
            }

            array1Sum = tArray1.Sum();
            array1Average = tArray1.Average();

            for (int i = 0; i <= txtNum1 - 1; i++)
            {
                double tmp1 = (tArray1[i] - array1Average) * (tArray1[i] - array1Average);
                sumSquares1 = sumSquares1 + tmp1;
            }


            for (int i = 0; i <= txtNum2 - 1; i++)
            {
                tArray2[i] = double.Parse(inputBox2[i].Text);
            }

            array2Sum = tArray2.Sum();
            array2Average = tArray2.Average();

            for (int i = 0; i <= txtNum2 - 1; i++)
            {
                double tmp2 = (tArray2[i] - array2Average) * (tArray2[i] - array2Average);
                sumSquares2 = sumSquares2 + tmp2;
            }

            pooledVariance = (sumSquares1 + sumSquares2) / (txtNum1 + txtNum2 - 2);

            ugh = (pooledVariance / txtNum1) + (pooledVariance / txtNum2);

            tStatistic = (array1Average - array2Average) / Math.Sqrt(ugh);

            pValue = GetPValue(txtNum1 + txtNum2, tStatistic);

            percentChance = pValue * 100;

            if (percentChance == 50)
            {
                pResults.Text =
                    "Sum of Section 1 = " + array1Sum +
                    "\nSum of Section 2 = " + array2Sum +
                    "\nAverage of Section 1 = " + array1Average +
                    "\nAverage of Section 2 = " + array2Average +
                    "\nSum of Squares for Section 1 = " + sumSquares1 +
                    "\nSum of Squares for Section 2 = " + sumSquares2 +
                    "\nPooled Variance = " + pooledVariance +
                    "\nUGH = " + ugh +
                    "\nT-Statistic = " + tStatistic +
                    "\nP-Value = " + pValue +
                    "\nChance that H0 is true = greater than or equal to " + percentChance + "%";
            }
            else
            {
                pResults.Text =
                    "Sum of Section 1 = " + array1Sum +
                    "\nSum of Section 2 = " + array2Sum +
                    "\nAverage of Section 1 = " + array1Average +
                    "\nAverage of Section 2 = " + array2Average +
                    "\nSum of Squares for Section 1 = " + sumSquares1 +
                    "\nSum of Squares for Section 2 = " + sumSquares2 +
                    "\nPooled Variance  = " + pooledVariance +
                    "\nUGH = " + ugh +
                    "\nT-Statistic = " + tStatistic +
                    "\nP-Value = " + pValue +
                    "\nChance that H0 is true = " + percentChance + "%";
            }
        }

        public void apply()
        {
            inputBox1 = new TextBox[txtNum1];
            inputBox2 = new TextBox[txtNum2];

            for (int i = 0; i <= txtNum1 - 1; i++)
            {
                int j = i + 1;
                inputBox1[i] = new TextBox();
                inputBox1[i].Text = "enter trial " + j + " data here";
                inputBox1[i].Width = 200;
                inputBox1[i].Margin = new Thickness(0, 3, 0, 3);
                data1.Children.Add(inputBox1[i]);
            }

            for (int i = 0; i <= txtNum2 - 1; i++)
            {
                int j = i + 1;
                inputBox2[i] = new TextBox();
                inputBox2[i].Text = "enter trial " + j + " data here";
                inputBox2[i].Width = 200;
                inputBox2[i].Margin = new Thickness(0, 3, 0, 3);
                data2.Children.Add(inputBox2[i]);
            }
        }

        bool isCorrect = true;
        bool isDone = false;

        public void check()
        {
            isDone = false;
            data1.Children.Clear();
            data2.Children.Clear();

            int lol;

            if (int.TryParse(infobox1.Text, out lol) == false || int.TryParse(infobox2.Text, out lol) == false)
            {
                MessageBox.Show("Invalid Trial Number Input", "Error");
                infobox1.Text = "";
                infobox2.Text = "";

                isCorrect = false;
            }

            if (isCorrect == true)
            {
                if (int.Parse(infobox1.Text) + int.Parse(infobox2.Text) > 100 || int.Parse(infobox1.Text) + int.Parse(infobox2.Text) < 3)
                {
                    MessageBox.Show("Trial Number Must Be Between 3 and 100", "Error");
                    infobox1.Text = "";
                    infobox2.Text = "";

                    isCorrect = false;
                }
            }

            if (isCorrect == true)
            {
                txtNum1 = int.Parse(infobox1.Text);
                txtNum2 = int.Parse(infobox2.Text);
                isDone = true;
            }
        }

        public void button_click(object sender, RoutedEventArgs e)
        {
            check();

            if (isCorrect == true)
            {
                apply();
            }

            isCorrect = true;
        }

        public void button1_click(object sender, RoutedEventArgs e)
        {
            if (isDone == true)
            {
                int test;
                bool isReady = true;

                for (int i = 0; i <= txtNum1 - 1; i++)
                {
                    isReady = int.TryParse(inputBox1[i].Text, out test);
                }

                if (isReady == true)
                {
                    for (int i = 0; i <= txtNum2 - 1; i++)
                    {
                        isReady = int.TryParse(inputBox2[i].Text, out test);
                    }
                }

                if (isReady == true)
                {
                    calculate();
                }

                else
                {
                    MessageBox.Show("Invalid Data Input", "Error");
                }
            }

            else
            {
                MessageBox.Show("No Data Entered", "Error");
            }
        }
        //End of PValue Calc
        
        //Beginning of ArabicRoman
        public enum Roman
        {
            I = 1,
            V = 5,
            X = 10,
            L = 50,
            C = 100,
            D = 500,
            M = 1000
        }

        static public string RomanToNumber(string roman)
        {
            roman = roman.ToUpper().Trim();

            if (roman == "N")
            {
                return "0";
            }

            if (roman.Split('V').Length > 2 || roman.Split('L').Length > 2 || roman.Split('D').Length > 2)
            {
                MessageBox.Show("V, L, and D cannot be used more than once");
                return "";
            }

            int i = 1;
            char last = 'Z';
            foreach (char numeral in roman)
            {
                if ("IVXLCDM".IndexOf(numeral) == -1)
                {
                    MessageBox.Show("Only letters I, V, X, L, C, D, and M can be used");
                    return "";
                }

                if (numeral == last)
                {
                    i++;
                    if (i == 4)
                    {
                        MessageBox.Show("Only 3 of I, X, C, and M can be used");
                        return "";
                    }
                }
                else
                {
                    i = 1;
                    last = numeral;
                }
            }

            int ptr = 0;
            ArrayList values = new ArrayList();
            int maxDigit = 1000;
            while (ptr < roman.Length)
            {
                char numeral = roman[ptr];
                int digit = (int)Enum.Parse(typeof(Roman), numeral.ToString());

                if (digit > maxDigit)
                {
                    MessageBox.Show("Maximum digit value exceeded");
                    return "";
                }
                int nextDigit = 0;
                if (ptr < roman.Length - 1)
                {
                    char nextNumeral = roman[ptr + 1];
                    nextDigit = (int)Enum.Parse(typeof(Roman), nextNumeral.ToString());

                    if (nextDigit > digit)
                    {
                        if ("IXC".IndexOf(numeral) == -1 || nextDigit > (digit * 10) || roman.Split(numeral).Length > 3)
                        {
                            MessageBox.Show("Maximum digit value exceeded");
                            return "";
                        }

                        maxDigit = digit - 1;
                        digit = nextDigit - digit;
                        ptr++;
                    }
                }

                values.Add(digit);

                ptr++;
            }

            for (int j = 0; j < values.Count - 1; j++)
            {
                if ((int)values[j] < (int)values[j + 1])
                {
                    MessageBox.Show("The numeral's values do not proceed in descending order");
                    return "";
                }
            }

            int total = 0;
            foreach (int digit in values)
            {
                total += digit;
            }
            return total.ToString();

        }

        static public string RomanNumber(int number)
        {
            if (number < 0 || number > 3999)
            {
                MessageBox.Show("Value must be in the range 0 – 3999");
                return "";
            }

            if (number == 0)
            {
                return "N";

            }

            int[] decimalValues = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            string[] romanNumerals = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", 
                                               "IV", "I" };

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < 13; i++)
            {

                while (number >= decimalValues[i])
                {
                    number = number - decimalValues[i];
                    result.Append(romanNumerals[i]);
                }

            }

            return result.ToString();
        }

        public void start()
        {
            int decimalNumber = 0;
            string userInputNumber = "1";
                
            if (conChoice.SelectedItem == RtoA)   
            {
                userInputNumber = infobox3.Text;
                Result.Text = RomanToNumber(userInputNumber);
            }

            else if (conChoice.SelectedItem == AtoR)
            {
                decimalNumber = int.Parse(infobox3.Text);

                Result.Text = RomanNumber(decimalNumber);
            }

            else
            {
                MessageBox.Show("Invalid Choice");
            }
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            bool ready = true;
            if (conChoice.SelectedItem == AtoR)
            {
                int test;
                ready = int.TryParse(infobox3.Text, out test);
            }

            if (ready == true)
            {
                start();
            }

            else 
            {
                MessageBox.Show("Invalid Input");
            }
        }
        //End of ArabicRoman

        //Beginning of Prime Checker
        ulong input;
        ulong output;

        bool checker()
        {
            if (input == 0 || input == 1)
            {
                return false;
            }

            for (ulong i = (ulong)Math.Sqrt(input); i > 1; i--)
            {
                output = input % i;
                if (output == 0)
                {
                    return false;
                }
            }
            return true;
        }

        bool proSmart()
        {             
            if(ulong.TryParse(infobox4.Text, out input) == true)
            {
                return true;
            }
            
            return false;
        }
        
        public void begin()
        {
            bool response;
            
            if (proSmart())
            {
                response = checker();  
                switch (response)
                {
                    case true:
                        answer.Text = "The number is a prime";
                        break;

                    case false:
                       answer.Text = "The number is not a prime";
                        break;
                }
            }

            else
            {
                MessageBox.Show("Invalid Input");
            }
        }
            
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            begin();
        }
        //End of Prime Checker

        //Beginning of Matrix Tool

        TextBox[][] inputMatrix;
        double[][] matrix;

        static public double getDeterminant(double[][] data)
        {
            int length = data.GetLength(0);
            double result = 0;

            if (length == 2)
            {
                result = data[0][0] * data[1][1] - data[0][1] * data[1][0];
                return result;
            }

            for (int i = 0; i < length; i++)
            {
                double[][] temp = new double[length - 1][];

                for (int j = 0; j < length - 1; j++)
                {
                    temp[j] = new double[length - 1];
                }

                for (int j = 1; j < length; j++)
                {
                    Array.Copy(data[j], 0, temp[j - 1], 0, i);
                    Array.Copy(data[j], i + 1, temp[j - 1], i, length - i - 1);
                }

                result += data[0][i] * Math.Pow(-1, i) * getDeterminant(temp);
            }

            return result;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MatrixName.Children.Clear();
            MatrixName.ColumnDefinitions.Clear();
            MatrixName.RowDefinitions.Clear();
            int length;
            if (!int.TryParse(infobox5.Text, out length))
            {
                MessageBox.Show("Invalid length input");
            }

            else
            {
                inputMatrix = new TextBox[length][];

                RowDefinition[] rows = new RowDefinition[length];
                ColumnDefinition[] columns = new ColumnDefinition[length];

                for (int i = 0; i < length; i++)
                {
                    rows[i] = new RowDefinition();
                    rows[i].Height = new GridLength(26);
                    columns[i] = new ColumnDefinition();
                    columns[i].Width = new GridLength(54);
                    
                    MatrixName.ColumnDefinitions.Add(columns[i]);
                    MatrixName.RowDefinitions.Add(rows[i]);
                }
                
                for (int i = 0; i < length; i++)
                {
                    inputMatrix[i] = new TextBox[length];

                    for (int j = 0; j < length; j++)
                    {
                        inputMatrix[i][j] = new TextBox();
                    }
                }

                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        Grid.SetRow(inputMatrix[i][j], j);
                        Grid.SetColumn(inputMatrix[i][j], i);
                        inputMatrix[i][j].Height = 20;
                        inputMatrix[i][j].Width = 50;
                        inputMatrix[i][j].Margin = new Thickness(1, 3, 1, 3);
                        MatrixName.Children.Add(inputMatrix[i][j]);
                    }
                }
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (inputMatrix == null)
            {
                MessageBox.Show("No length entered");
            }

            else
            {
                bool isOK = true;
                matrix = new double[inputMatrix.GetLength(0)][];
                for (int i = 0; i < inputMatrix.GetLength(0); i++)
                {
                    matrix[i] = new double[inputMatrix.GetLength(0)];
                    for (int j = 0; j < inputMatrix.GetLength(0); j++)
                    {
                        if (!double.TryParse(inputMatrix[i][j].Text, out matrix[i][j]))
                        {
                            MessageBox.Show("Invalid input");
                            isOK = false;
                            break;
                        }
                    }

                    if (!isOK)
                    {
                        break;
                    }
                }

                if (isOK)
                {
                    mResults.Text = "Determinant: " + getDeterminant(matrix);
                }
            }
        }
        //End of Matrix Tool
    }
}
