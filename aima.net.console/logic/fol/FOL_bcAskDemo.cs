using aima.net.logic.fol.inference;

namespace aima.net.demo.logic.fol
{
    public class FOL_bcAskDemo : FolDemoBase
    {
        static void Main(params string[] args)
        {
            fOL_bcAskDemo();
        }

        private static void fOL_bcAskDemo()
        {
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("Backward Chain, Kings Demo 1");
            System.Console.WriteLine("----------------------------");
            kingsDemo1(new FOLBCAsk());
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("Backward Chain, Kings Demo 2");
            System.Console.WriteLine("----------------------------");
            kingsDemo2(new FOLBCAsk());
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("Backward Chain, Weapons Demo");
            System.Console.WriteLine("----------------------------");
            weaponsDemo(new FOLBCAsk());
        }
    }
}
