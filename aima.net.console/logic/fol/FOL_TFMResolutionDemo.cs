using aima.net.logic.fol.inference;

namespace aima.net.demo.logic.fol
{
    public class FOL_TFMResolutionDemo : FolDemoBase
    {
        static void Main(params string[] args)
        {
            fOL_TFMResolutionDemo();
        }

        static void fOL_TFMResolutionDemo()
        {
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("TFM Resolution, Kings Demo 1");
            System.Console.WriteLine("----------------------------");
            kingsDemo1(new FOLTFMResolution());
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("TFM Resolution, Kings Demo 2");
            System.Console.WriteLine("----------------------------");
            kingsDemo2(new FOLTFMResolution());
            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine("TFM Resolution, Weapons Demo");
            System.Console.WriteLine("----------------------------");
            weaponsDemo(new FOLTFMResolution());
            System.Console.WriteLine("---------------------------------");
            System.Console.WriteLine("TFM Resolution, Loves Animal Demo");
            System.Console.WriteLine("---------------------------------");
            lovesAnimalDemo(new FOLTFMResolution());
            System.Console.WriteLine("---------------------------------------");
            System.Console.WriteLine("TFM Resolution, ABC Equality Axiom Demo");
            System.Console.WriteLine("---------------------------------------");
            abcEqualityAxiomDemo(new FOLTFMResolution());
        }
    }
}
