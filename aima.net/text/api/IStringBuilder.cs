using aima.net.api;

namespace aima.net.text.api
{
    public interface IStringBuilder : IStringable, IEquatable<IStringBuilder>
    {
        IStringBuilder Append(object value);
        IStringBuilder Append(char[] value);
        IStringBuilder Append(ulong value);
        IStringBuilder Append(uint value);
        IStringBuilder Append(ushort value);
        IStringBuilder Append(decimal value);
        IStringBuilder Append(double value);
        IStringBuilder Append(float value);
        IStringBuilder Append(int value);
        IStringBuilder Append(short value);
        IStringBuilder Append(char value);
        IStringBuilder Append(long value);
        IStringBuilder Append(sbyte value);
        IStringBuilder Append(bool value);
        IStringBuilder Append(string value);
        IStringBuilder Append(byte value);
        IStringBuilder Append(char value, int repeatCount);
        IStringBuilder Append(string value, int startIndex, int count);
        IStringBuilder Append(char[] value, int startIndex, int charCount);
        IStringBuilder AppendLine();
        IStringBuilder AppendLine(string value);
        IStringBuilder Clear();
        int Decrement();
        int GetLength();
        char Get(int index);
        IStringBuilder Insert(int index, ushort value);
        IStringBuilder Insert(int index, object value);
        IStringBuilder Insert(int index, ulong value);
        IStringBuilder Insert(int index, uint value);
        IStringBuilder Insert(int index, decimal value);
        IStringBuilder Insert(int index, bool value);
        IStringBuilder Insert(int index, float value);
        IStringBuilder Insert(int index, double value);
        IStringBuilder Insert(int index, sbyte value);
        IStringBuilder Insert(int index, byte value);
        IStringBuilder Insert(int index, short value);
        IStringBuilder Insert(int index, string value);
        IStringBuilder Insert(int index, char[] value);
        IStringBuilder Insert(int index, int value);
        IStringBuilder Insert(int index, long value);
        IStringBuilder Insert(int index, char value);
        IStringBuilder Insert(int index, string value, int count);
        IStringBuilder Insert(int index, char[] value, int startIndex, int charCount);
        IStringBuilder Remove(int startIndex, int length);
        IStringBuilder Replace(string oldValue, string newValue);
        IStringBuilder Replace(char oldChar, char newChar);
        IStringBuilder Replace(string oldValue, string newValue, int startIndex, int count);
        IStringBuilder Replace(char oldChar, char newChar, int startIndex, int count);
        string ToString(int startIndex, int length);
    }
}
