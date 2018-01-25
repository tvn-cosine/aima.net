using aima.net.exceptions;

namespace aima.net
{
    public static class SourceVersion
    {
        public static int CodePointAt(this string value, int index)
        {
            if ((index < 0) || (index >= value.Length))
            {
                throw new IndexOutOfRangeException(index.ToString());
            }
            return Character.CodePointAtImpl(value.ToCharArray(), index, value.Length);
        }

        public static bool IsIdentifier(string name)
        {
            string id = name.ToString();

            if (id.Length == 0)
            {
                return false;
            }
            int cp = id.CodePointAt(0);
            if (!Character.isSourceCodeIdentifierStart(cp))
            {
                return false;
            }
            for (int i = Character.CharCount(cp);
                    i < id.Length;
                    i += Character.CharCount(cp))
            {
                cp = id.CodePointAt(i);
                if (!Character.IsSourceCodeIdentifierPart(cp))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
