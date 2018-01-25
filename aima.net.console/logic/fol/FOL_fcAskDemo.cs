using aima.net.logic.fol.inference;

namespace aima.net.demo.logic.fol
{
    public class FOL_fcAskDemo : FolDemoBase
    {
        static void Main(params string[] args)
        {
            fOL_fcAskDemo();
        }

        static void fOL_fcAskDemo()
        {
            System.Console.WriteLine("---------------------------");
            System.Console.WriteLine("Forward Chain, Kings Demo 1");
            System.Console.WriteLine("---------------------------");
            kingsDemo1(new FOLFCAsk());
            System.Console.WriteLine("---------------------------");
            System.Console.WriteLine("Forward Chain, Kings Demo 2");
            System.Console.WriteLine("---------------------------");
            kingsDemo2(new FOLFCAsk());
            System.Console.WriteLine("---------------------------");
            System.Console.WriteLine("Forward Chain, Weapons Demo");
            System.Console.WriteLine("---------------------------");
            weaponsDemo(new FOLFCAsk());
        }
    }
}
