using aima.net.logic.fol.inference;

namespace aima.net.demo.logic.fol
{
    public class FOL_OTTERDemo : FolDemoBase
    {
        static void Main(params string[] args)
        {
            fOL_OTTERDemo();
        }

        static void fOL_OTTERDemo()
        {
            System.Console.WriteLine("---------------------------------------");
            System.Console.WriteLine("OTTER Like Theorem Prover, Kings Demo 1");
            System.Console.WriteLine("---------------------------------------");
            kingsDemo1(new FOLOTTERLikeTheoremProver());
            System.Console.WriteLine("---------------------------------------");
            System.Console.WriteLine("OTTER Like Theorem Prover, Kings Demo 2");
            System.Console.WriteLine("---------------------------------------");
            kingsDemo2(new FOLOTTERLikeTheoremProver());
            System.Console.WriteLine("---------------------------------------");
            System.Console.WriteLine("OTTER Like Theorem Prover, Weapons Demo");
            System.Console.WriteLine("---------------------------------------");
            weaponsDemo(new FOLOTTERLikeTheoremProver());
            System.Console.WriteLine("--------------------------------------------");
            System.Console.WriteLine("OTTER Like Theorem Prover, Loves Animal Demo");
            System.Console.WriteLine("--------------------------------------------");
            lovesAnimalDemo(new FOLOTTERLikeTheoremProver());
            System.Console.WriteLine("--------------------------------------------------");
            System.Console.WriteLine("OTTER Like Theorem Prover, ABC Equality Axiom Demo");
            System.Console.WriteLine("--------------------------------------------------");
            abcEqualityAxiomDemo(new FOLOTTERLikeTheoremProver(false));
            System.Console.WriteLine("-----------------------------------------------------");
            System.Console.WriteLine("OTTER Like Theorem Prover, ABC Equality No Axiom Demo");
            System.Console.WriteLine("-----------------------------------------------------");
            abcEqualityNoAxiomDemo(new FOLOTTERLikeTheoremProver(true));
        }
    }
}
