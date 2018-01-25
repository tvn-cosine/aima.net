namespace aima.net.logic.fol
{
    public class Connectors
    {
        public const string AND = "AND";
        public const string OR = "OR";
        public const string NOT = "NOT";
        public const string IMPLIES = "=>";
        public const string BICOND = "<=>";

        public static bool isAND(string connector)
        {
            return AND.Equals(connector);
        }

        public static bool isOR(string connector)
        {
            return OR.Equals(connector);
        }

        public static bool isNOT(string connector)
        {
            return NOT.Equals(connector);
        }

        public static bool isIMPLIES(string connector)
        {
            return IMPLIES.Equals(connector);
        }

        public static bool isBICOND(string connector)
        {
            return BICOND.Equals(connector);
        }
    }
}
