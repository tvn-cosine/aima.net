using aima.net.collections.api;
using aima.net.logic.common;

namespace aima.net.logic.fol.parsing.ast
{
    public interface FOLNode : ParseTreeNode
    {
        string getSymbolicName(); 
        bool isCompound(); 
        ICollection<FOLNode> getArgs(); 
        object accept(FOLVisitor v, object arg); 
        FOLNode copy();
    }
}
