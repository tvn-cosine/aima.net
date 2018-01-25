using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.datastructures
{ 
    public class Table<RowHeaderType, ColumnHeaderType, ValueType> : IStringable
    {
        private ICollection<RowHeaderType> rowHeaders;
        private ICollection<ColumnHeaderType> columnHeaders;
        private IMap<RowHeaderType, IMap<ColumnHeaderType, ValueType>> rows;
         
        /// <summary>
        /// Constructs a Table with the specified row and column headers.
        /// </summary>
        /// <param name="rowHeaders">a list of row headers</param>
        /// <param name="columnHeaders">a list of column headers</param>
        public Table(ICollection<RowHeaderType> rowHeaders, ICollection<ColumnHeaderType> columnHeaders)
        {
            this.rowHeaders = rowHeaders;
            this.columnHeaders = columnHeaders;
            this.rows = CollectionFactory.CreateInsertionOrderedMap<RowHeaderType, IMap<ColumnHeaderType, ValueType>>();
            foreach (RowHeaderType rowHeader in rowHeaders)
            {
                rows.Put(rowHeader, CollectionFactory.CreateInsertionOrderedMap<ColumnHeaderType, ValueType>());
            }
        }
         
        /// <summary>
        /// Maps the specified row and column to the specified value in the table.
        /// Neither the row nor the column nor the value can be null <para />
        /// The value can be retrieved by calling the get method with a
        /// row and column that is equal to the original row and column.
        /// </summary>
        /// <param name="r">the table row</param>
        /// <param name="c">the table column</param>
        /// <param name="v">the value</param> 
        public void Set(RowHeaderType r, ColumnHeaderType c, ValueType v)
        { 
            rows.Get(r).Put(c, v);
        }

         
        /// <summary>
        /// Returns the value to which the specified row and column is mapped in this table.
        /// </summary>
        /// <param name="r">a row in the table</param>
        /// <param name="c">a column in the table</param>
        /// <returns>
        /// the value to which the row and column is mapped in this table;
        /// null if the row and column is not mapped to any
        /// values in this table.
        /// </returns>
        public ValueType Get(RowHeaderType r, ColumnHeaderType c)
        {
            IMap<ColumnHeaderType, ValueType> rowValues = rows.Get(r);
            return rowValues == null ? default(ValueType) : rowValues.Get(c);

        }

        public override string ToString()
        {
            IStringBuilder buf = TextFactory.CreateStringBuilder();
            foreach (RowHeaderType r in rowHeaders)
            {
                foreach (ColumnHeaderType c in columnHeaders)
                {
                    buf.Append(Get(r, c));
                    buf.Append(" ");
                }
                buf.Append("\n");
            }
            return buf.ToString();
        }

        class Row<R>
        {
            private IMap<ColumnHeaderType, ValueType> _cells;

            public Row()
            {

                this._cells = CollectionFactory.CreateInsertionOrderedMap<ColumnHeaderType, ValueType>();
            }

            public IMap<ColumnHeaderType, ValueType> cells()
            {
                return this._cells;
            }

        }

        class Cell<ValueHeaderType>
        {
            private ValueHeaderType _value;

            public Cell()
            {
                _value = default(ValueHeaderType);
            }

            public Cell(ValueHeaderType value)
            {
                this._value = value;
            }

            public void set(ValueHeaderType value)
            {
                this._value = value;
            }

            public ValueHeaderType value()
            {
                return _value;
            }
        }
    }
}
