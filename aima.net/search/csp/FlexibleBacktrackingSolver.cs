﻿using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.csp.inference;
using aima.net.search.csp.inference.api;

namespace aima.net.search.csp
{
    /**
     * This backtracking search implementation can be configured with arbitrary strategies for variable selection,
     * value ordering, and inference. These strategies are represented by objects implementing standard interfaces.
     * The design supports experiments with user-defined strategies of all kinds.
     *
     * @param <VAR> Type which is used to represent variables
     * @param <VAL> Type which is used to represent the values in the domains
     *
     * @author Ruediger Lunde
     */
    public class FlexibleBacktrackingSolver<VAR, VAL> : AbstractBacktrackingSolver<VAR, VAL>
        where VAR : Variable
    {

        private CspHeuristics.VariableSelection<VAR, VAL> varSelectionStrategy;
        private CspHeuristics.ValueSelection<VAR, VAL> valSelectionStrategy;
        private IInferenceStrategy<VAR, VAL> inferenceStrategy;


        /**
         * Selects the algorithm for SELECT-UNASSIGNED-VARIABLE. Uses the fluent interface design pattern.
         */
        public FlexibleBacktrackingSolver<VAR, VAL> set(CspHeuristics.VariableSelection<VAR, VAL> varStrategy)
        {
            varSelectionStrategy = varStrategy;
            return this;
        }

        /**
         * Selects the algorithm for ORDER-DOMAIN-VALUES. Uses the fluent interface design pattern.
         */
        public FlexibleBacktrackingSolver<VAR, VAL> set(CspHeuristics.ValueSelection<VAR, VAL> valStrategy)
        {
            valSelectionStrategy = valStrategy;
            return this;
        }

        /**
         * Selects the algorithm for INFERENCE. Uses the fluent interface design pattern.
         */
        public FlexibleBacktrackingSolver<VAR, VAL> set(IInferenceStrategy<VAR, VAL> iStrategy)
        {
            inferenceStrategy = iStrategy;
            return this;
        }

        /**
         * Selects MRV&DEG for variable selection, LCV for domain ordering and AC3 as inference method.
         */
        public FlexibleBacktrackingSolver<VAR, VAL> setAll()
        {
            set(CspHeuristics.mrvDeg<VAR, VAL>()).set(CspHeuristics.lcv<VAR, VAL>()).set(new AC3Strategy<VAR, VAL>());
            return this;
        }

        /**
         * Applies an initial inference step and then calls the super class implementation.
         */

        public override Assignment<VAR, VAL> solve(CSP<VAR, VAL> csp)
        {
            if (inferenceStrategy != null)
            {
                csp = csp.copyDomains(); // do not change the original CSP!
                IInferenceLog<VAR, VAL> log = inferenceStrategy.apply(csp);
                if (!log.isEmpty())
                {
                    fireStateChanged(csp, null, null);
                    if (log.inconsistencyFound())
                        return null;
                }
            }
            return base.solve(csp);
        }

        /**
         * Primitive operation, selecting a not yet assigned variable.
         */

        protected override VAR selectUnassignedVariable(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment)
        {
            ICollection<VAR> vars = CollectionFactory.CreateQueue<VAR>();
            foreach (var v in csp.getVariables())
            {
                if (!assignment.contains(v))
                {
                    vars.Add(v);
                }
            }
            if (varSelectionStrategy != null)
                vars = varSelectionStrategy.apply(csp, vars);
            return vars.Get(0);
        }

        /**
         * Primitive operation, ordering the domain values of the specified variable.
         */

        protected override IEnumerable<VAL> orderDomainValues(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR var)
        {
            if (valSelectionStrategy != null)
                return valSelectionStrategy.apply(csp, assignment, var);
            else
                return csp.getDomain(var);
        }

        /**
         * Primitive operation, which tries to optimize the CSP representation with respect to a new assignment.
         *
         * @param var The variable which just got a new value in the assignment.
         * @return An object which provides information about
         * (1) whether changes have been performed,
         * (2) possibly inferred empty domains, and
         * (3) how to restore the original CSP.
         */

        protected override IInferenceLog<VAR, VAL> inference(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR var)
        {
            if (inferenceStrategy != null)
                return inferenceStrategy.apply(csp, assignment, var);
            else
                return new InferenceEmptyLog<VAR, VAL>();
        }
    }
}
