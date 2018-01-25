using aima.net.api;
using aima.net.text.api;

namespace aima.net.text
{
    public class StringBuilder : IStringBuilder, IEquatable<StringBuilder>
    {
        private readonly System.Text.StringBuilder backingStringBuilder;

        public StringBuilder()
        {
            backingStringBuilder = new System.Text.StringBuilder();
        }
        public StringBuilder(string value)
        {
            backingStringBuilder = new System.Text.StringBuilder(value);
        }

        public IStringBuilder Append(object value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(char[] value)
        {
            backingStringBuilder.Append(value);
            return this;
        }
         
        public IStringBuilder Append(ulong value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(uint value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(ushort value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(decimal value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(double value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(float value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(int value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(short value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(char value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(long value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(sbyte value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(bool value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(string value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(byte value)
        {
            backingStringBuilder.Append(value);
            return this;
        }

        public IStringBuilder Append(char value, int repeatCount)
        {
            backingStringBuilder.Append(value, repeatCount);
            return this;
        }

        public IStringBuilder Append(string value, int startIndex, int count)
        {
            backingStringBuilder.Append(value, startIndex, count);
            return this;
        }

        public IStringBuilder Append(char[] value, int startIndex, int charCount)
        {
            backingStringBuilder.Append(value, startIndex, charCount);
            return this;
        }

        public IStringBuilder AppendLine()
        {
            backingStringBuilder.AppendLine();
            return this;
        }

        public IStringBuilder AppendLine(string value)
        {
            backingStringBuilder.AppendLine(value);
            return this;
        }

        public IStringBuilder Clear()
        {
            backingStringBuilder.Clear();
            return this;
        }

        public int Decrement()
        {
            return --backingStringBuilder.Length;
        }

        public bool Equals(IStringBuilder other)
        {
            return Equals(other as StringBuilder);
        }

        public bool Equals(StringBuilder other)
        {
           if (null == other)
            {
                return false;
            }

            return this.backingStringBuilder.Equals(other.backingStringBuilder);
        }

        public char Get(int index)
        {
            return backingStringBuilder[index];
        }

        public int GetLength()
        {
            return backingStringBuilder.Length;
        }
         
        public IStringBuilder Insert(int index, ushort value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, object value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, ulong value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, uint value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, decimal value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, bool value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, float value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, double value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, sbyte value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, byte value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, short value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, string value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, char[] value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, int value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, long value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, char value)
        {
            backingStringBuilder.Insert(index, value);
            return this;
        }

        public IStringBuilder Insert(int index, string value, int count)
        {
            backingStringBuilder.Insert(index, value, count);
            return this;
        }

        public IStringBuilder Insert(int index, char[] value, int startIndex, int charCount)
        {
            backingStringBuilder.Insert(index, value, startIndex, charCount);
            return this;
        }

        public IStringBuilder Remove(int startIndex, int length)
        {
            backingStringBuilder.Remove(startIndex, length);
            return this;
        }

        public IStringBuilder Replace(string oldValue, string newValue)
        {
            backingStringBuilder.Replace(oldValue, newValue);
            return this;
        }

        public IStringBuilder Replace(char oldChar, char newChar)
        {
            backingStringBuilder.Replace(oldChar, newChar);
            return this;
        }

        public IStringBuilder Replace(string oldValue, string newValue, int startIndex, int count)
        {
            backingStringBuilder.Replace(oldValue, newValue, startIndex, count);
            return this;
        }

        public IStringBuilder Replace(char oldChar, char newChar, int startIndex, int count)
        {
            backingStringBuilder.Replace(oldChar, newChar, startIndex, count);
            return this;
        }

        public string ToString(int startIndex, int length)
        {
            return backingStringBuilder.ToString(startIndex, length);
        }

        public override string ToString()
        {
            return backingStringBuilder.ToString();
        }
    }
}
