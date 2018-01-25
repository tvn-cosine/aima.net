namespace aima.net.environment.cellworld
{
    public class CellWorldFactory
    {  
        /// <summary>
        /// Create the cell world as defined in Figure 17.1 in AIMA3e. (a) A simple 4
        /// x 3 environment that presents the agent with a sequential decision
        /// problem.
        /// </summary>
        /// <returns>a cell world representation of Fig 17.1 in AIMA3e.</returns>
        public static CellWorld<double> CreateCellWorldForFig17_1()
        {
            CellWorld<double> cw = new CellWorld<double>(4, 3, -0.04);

            cw.RemoveCell(2, 2);

            cw.GetCellAt(4, 3).setContent(1.0);
            cw.GetCellAt(4, 2).setContent(-1.0);

            return cw;
        }
    }
}
