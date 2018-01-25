using aima.net.environment.eightpuzzle;

namespace aima.net.demo.search.eightpuzzle
{
    public abstract class EightPuzzleDemoBase : SearchDemoBase
    {
        protected static EightPuzzleBoard boardWithThreeMoveSolution =
                new EightPuzzleBoard(new int[] { 1, 2, 5, 3, 4, 0, 6, 7, 8 });

        protected static EightPuzzleBoard random1 =
                new EightPuzzleBoard(new int[] { 1, 4, 2, 7, 5, 8, 3, 0, 6 });

        protected static EightPuzzleBoard extreme =
                new EightPuzzleBoard(new int[] { 0, 8, 7, 6, 5, 4, 3, 2, 1 });
    }
}
