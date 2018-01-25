using aima.net.logic.fol.inference;

namespace aima.net.demo.logic.fol
{
    public class FOL_ModelEliminationDemo : FolDemoBase
    {
        static void Main(params string[] args)
        {
            fOL_ModelEliminationDemo();
        }

        static void fOL_ModelEliminationDemo()
        {
            System.Console.WriteLine("-------------------------------");
            System.Console.WriteLine("Model Elimination, Kings Demo 1");
            System.Console.WriteLine("-------------------------------");
            kingsDemo1(new FOLModelElimination());
            System.Console.WriteLine("-------------------------------");
            System.Console.WriteLine("Model Elimination, Kings Demo 2");
            System.Console.WriteLine("-------------------------------");
            kingsDemo2(new FOLModelElimination());
            System.Console.WriteLine("-------------------------------");
            System.Console.WriteLine("Model Elimination, Weapons Demo");
            System.Console.WriteLine("-------------------------------");
            weaponsDemo(new FOLModelElimination());
            System.Console.WriteLine("------------------------------------");
            System.Console.WriteLine("Model Elimination, Loves Animal Demo");
            System.Console.WriteLine("------------------------------------");
            lovesAnimalDemo(new FOLModelElimination());
            System.Console.WriteLine("------------------------------------------");
            System.Console.WriteLine("Model Elimination, ABC Equality Axiom Demo");
            System.Console.WriteLine("-------------------------------------------");
            abcEqualityAxiomDemo(new FOLModelElimination());
        }
    }
}
