using System.Linq;

namespace NumberPlaceSolver.Models
{
    class NumberPlaceModel
    {
        public static NumberPlaceCells All = new NumberPlaceCells(81);
        public static void Initialize(NumberPlaceQuestion q)
        {
            // ここで、問題を読み出してAllメンバを初期化する
        }

        public static NumberPlaceCells Solve()
        {
            // ここで、問題を解く処理を入れる

            return All;
        }
    }
}
