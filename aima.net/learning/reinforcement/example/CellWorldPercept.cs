using aima.net.environment.cellworld;
using aima.net.learning.reinforcement.api;

namespace aima.net.learning.reinforcement.example
{
    /// <summary> 
    /// An implementation of the PerceptStateReward interface for the cell world
    /// environment. Note: The getCell() and setCell() methods allow a single percept
    /// to be instantiated per agent within the environment. However, if an agent
    /// tracks its perceived percepts it will need to explicitly copy the relevant
    /// information. 
    /// </summary>
    public class CellWorldPercept : IPerceptStateReward<Cell<double>>
    {
        private Cell<double> cell = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cell">the cell within the environment that the percept refers to.</param>
        public CellWorldPercept(Cell<double> cell)
        {
            this.cell = cell;
        }

        /// <summary>
        /// The cell within the environment that the percept refers to.
        /// </summary>
        /// <returns>the cell within the environment that the percept refers to.</returns>
        public Cell<double> getCell()
        {
            return cell;
        }

        /// <summary>
        /// Set the cell within the environment that the percept refers to.
        /// </summary>
        /// <param name="cell">the cell within the environment that the percept refers to.</param>
        public void setCell(Cell<double> cell)
        {
            this.cell = cell;
        }

        public double reward()
        {
            return cell.getContent();
        }


        public Cell<double> state()
        {
            return cell;
        }
    }
}
