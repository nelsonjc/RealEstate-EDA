using RealEstate.Shared.Interfaces;
using System.Linq.Expressions;

namespace RealEstate.Shared.Utils
{
    public class ReplaceVisitor : ExpressionVisitor, IReplaceVisitor
    {
        private Expression from, to;

        ///<inheritdoc/>
        public void SetData(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }

        ///<inheritdoc/>
        public override Expression Visit(Expression node) => node == from ? to : base.Visit(node);
    }
}
