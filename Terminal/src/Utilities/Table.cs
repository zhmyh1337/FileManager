﻿using System;
using System.Linq;
using System.Text;

namespace Utilities
{
    class Table
    {
        public Table(int columnsCount, string[][] data, string name = null,
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
            int headerOffset = header == null ? 0 : 1;

            var printData = new string[rowsCount + headerOffset][];
            if (header != null)
            {
                printData[0] = header;
            }
            Array.Copy(data, 0, printData, headerOffset, rowsCount);

            if (name != null)
            {
                var printName = name.CutWithDots(globalLength);
                PrintSeparator('╔', '╗', '═', '═');
                Logger.PrintLine($"║{printName.PadRight(globalLength)}║");
            }

            for (int i = 0; i < printData.Length; i++)
            {
                bool firstLine = name == null && i == 0;
                PrintSeparator(firstLine ? '╔' : '╠', firstLine ? '╗' : '╣', '═', i == 0 ? '╦' : '╬');

                for (int j = 0; j < columnsCount; j++)
                {
                    Logger.Print("║{0}", printData[i][j].CutWithDots(columnLengths[j]).PadRight(columnLengths[j]));
                }

                Logger.PrintLine("║");
            }

            PrintSeparator('╚', '╝', '═', printData.Length == 0 ? '═' : '╩');
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

        private void GenerateColumnLengths()
        {
            rowsCount = data.Length;
            columnLengths = Enumerable.Range(0, columnsCount).Select(x => header == null ? 0 : header[x].Length).ToArray();

            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    columnLengths[j] = Math.Max(columnLengths[j], data[i][j].Length);
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
        private string[][] data;
        private string name;
        private string[] header;
        private int[] maxLengthInColumn;

        private int rowsCount;
        private int[] columnLengths;
        private int globalLength;
    }
}
