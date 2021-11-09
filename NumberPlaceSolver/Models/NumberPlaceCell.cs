using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NumberPlaceSolver.Models
{
    public enum CellProperty
    {
        Initial,
        FixedAnswer,
        EstimatedAnswer,
        NoAnswer,
    }

    public enum CellSetFormat
    {
        HorizontalLine,
        VerticalLine,
        Rectangle,
    }

    class NumberPlaceQuestion
    {

    }

    class NumberPlaceCell
    {
        public int Id { get; private set; }
        public int Number { get; private set; }
        public CellProperty CellProperty { get; private set; }

        public bool SetAnswer(int value, CellProperty property)
        {
            if (value <= 0 || value > 9) return false;
            if (property == CellProperty.Initial) return false;

            switch (property)
            {
                case CellProperty.Initial:
                    return false;
                case CellProperty.FixedAnswer:
                    Number = value;
                    CellProperty = CellProperty.FixedAnswer;
                    break;
                case CellProperty.EstimatedAnswer:
                    if (CellProperty == CellProperty.NoAnswer)
                    {
                        Number = value;
                        CellProperty = CellProperty.EstimatedAnswer;
                    }
                    break;
            }

            return true;
        }

        public void ClearAnswer()
        {
            if (CellProperty == CellProperty.Initial) return;
            Number = 0;
            CellProperty = CellProperty.NoAnswer;
        }
    }

    class NumberPlaceCells : List<NumberPlaceCell>
    {
        public NumberPlaceCells() : base() { }
        public NumberPlaceCells(int capacity) : base(capacity) { }

        private int[] Numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        /// <summary>
        /// 引数で指定した数字がいくつ含まれているかを返す
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public int CountNum(int num)
        {
            return this.Count(item => item.Number == num);
        }

        /// <summary>
        /// 指定した数字が含まれているかどうかを返す
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool Contains(int num)
        {
            return CountNum(num) > 0;
        }

        /// <summary>
        /// 1～9の数値が一つずつ含まれているかどうかを返す
        /// </summary>
        /// <returns></returns>
        public bool Completed()
        {
            return LackNumber().Count == 0;
        }

        /// <summary>
        /// 1～9のうち、含まれていない数値のリストを返す
        /// </summary>
        /// <returns></returns>
        public List<int> LackNumber()
        {
            return Numbers.Where(n => !Contains(n)).ToList();
        }

        /// <summary>
        /// 1～9のうち、複数個含まれている数値のリストを返す
        /// </summary>
        /// <returns></returns>
        public List<int> DupeNumber()
        {
            var ret = new List<int>();
            ret.AddRange(Numbers.Where(n => this.Count(item => item.Number == n) > 1));
            return ret;
        }

        /// <summary>
        /// ゲーム盤全面でコールすると、全てのセットがOKかどうかをチェックする
        /// 全面でないインスタンスでコールするとFalseを返す
        /// </summary>
        /// <returns></returns>
        public bool FullCompleted()
        {
            if (Count != 9 * 9) return false;

            foreach (int i in Numbers)
            {
                if (!GetCellSet(CellSetFormat.HorizontalLine, i - 1).Completed())
                    return false;
                if (!GetCellSet(CellSetFormat.VerticalLine, i - 1).Completed())
                    return false;
                if (!GetCellSet(CellSetFormat.Rectangle, i - 1).Completed())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// ゲーム盤全面から、指定の向きのセルのコレクションを取得する
        /// </summary>
        /// <param name="format">横・縦・四角</param>
        /// <param name="number">行・列・位置番号（0-8)</param>
        /// <returns></returns>
        public NumberPlaceCells GetCellSet(CellSetFormat format, int number)
        {
            if (Count != 9 * 9) return null;

            var ret = format switch
            {
                CellSetFormat.HorizontalLine => (NumberPlaceCells)this.Where(item => item.Id / 9 == number).ToList(),
                CellSetFormat.VerticalLine => (NumberPlaceCells)this.Where(item => item.Id % 9 == number).ToList(),
                CellSetFormat.Rectangle => (NumberPlaceCells)this.Where(item =>
                {
                    int lt = (27 * (number / 3)) + (3 * (number % 3));
                    if (item.Id >= lt && item.Id < lt + 3) return true;
                    lt += 9;
                    if (item.Id >= lt && item.Id < lt + 3) return true;
                    lt += 9;
                    if (item.Id >= lt && item.Id < lt + 3) return true;
                    return false;
                }).ToList(),
                _ => null,
            };

            if (ret.Count == 9) return ret;

            return null; // some logic error occurred.
        }
    }
}
