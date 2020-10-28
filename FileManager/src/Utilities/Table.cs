using System;
using System.Linq;
using System.Net;
using System.Text;

namespace Utilities
{
    class Table
    {
        public Table(int columnsCount, string[,] data, string name = null,
            string[] header = null, int[] maxLengthInColumn = null)
        {
            this.columnsCount = columnsCount;
            this.data = data;
            this.name = name;
            this.header = header;
            this.maxLengthInColumn = maxLengthInColumn;

            GenerateColumnLengths();
        }

        public void Print()
        {
            var printData = new string[data.GetLength(0) + (header == null ? 0 : 1), data.GetLength(1)];
            if (header != null)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    printData[0, j] = CutString(header[j], columnLengths[j]);
                }
            }
            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    printData[i + (header == null ? 0 : 1), j] = CutString(data[i, j], columnLengths[j]);
                }
            }

            if (name != null)
            {
                var printName = CutString(name, globalLength);
                PrintSeparator('╔', '╗', '═', '═');
                Logger.PrintLine($"║{printName.PadRight(globalLength)}║");
            }

            for (int i = 0; i < printData.GetLength(0); i++)
            {
                bool firstLine = name == null && i == 0;
                PrintSeparator(firstLine ? '╔' : '╠', firstLine ? '╗' : '╣', '═', i == 0 ? '╦' : '╬');

                for (int j = 0; j < printData.GetLength(1); j++)
                {
                    Logger.Print($"║{printData[i, j].PadRight(columnLengths[j])}");
                }
                Logger.PrintLine("║");
            }

            PrintSeparator('╚', '╝', '═', printData.GetLength(0) == 0 ? '═' : '╩');
        }

        private void PrintSeparator(char left, char right, char horizontal, char vertical)
        {
            var sb = new StringBuilder();

            sb.Append(left);
            for (int j = 0; j < columnsCount; j++)
            {
                sb.Append(horizontal, columnLengths[j]);
                if (j != columnsCount - 1)
                    sb.Append(vertical);
            }
            sb.Append(right);

            Console.WriteLine(sb.ToString());
        }

        private string CutString(string s, int maxLength)
        {
            if (s.Length <= maxLength)
                return s;

            s = s.Remove(Math.Max(0, maxLength - 3));
            return s + new string('.', Math.Min(3, maxLength));
        }

        private void GenerateColumnLengths()
        {
            rowsCount = data.GetLength(0);
            columnLengths = new int[columnsCount];

            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    columnLengths[j] = Math.Max(columnLengths[j], data[i, j].Length);
                }
            }

            if (maxLengthInColumn != null)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    columnLengths[j] = Math.Min(columnLengths[j], maxLengthInColumn[j]);
                }
            }

            globalLength = columnLengths.Sum() + columnsCount - 1;
        }

        private int columnsCount;
        private string[,] data;
        private string name;
        private string[] header;
        private int[] maxLengthInColumn;

        private int rowsCount;
        private int[] columnLengths;
        private int globalLength;
    }
}
