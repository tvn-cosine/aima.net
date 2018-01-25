﻿using aima.net.collections.api;
using aima.net.logic.fol.inference.proof;

namespace aima.net.logic.fol.inference
{
    public interface InferenceResult
    {
        /**
         * 
         * @return true, if the query is not entailed from the premises. This just
         *         means the query is not entailed, the query itself may be true.
         */
        bool isPossiblyFalse();

        /**
         * 
         * @return true, if the query is entailed from the premises (Note: can get
         *         partial results if the original query contains variables
         *         indicating that there can possibly be more than 1 proof/bindings
         *         for the query, see: isPartialResultDueToTimeout()).
         */
        bool isTrue();

        /**
         * 
         * @return true, if the inference procedure ran for a length of time and
         *         found no proof one way or the other before it timed out.
         */
        bool isUnknownDueToTimeout();

        /**
         * 
         * @return true, if the inference procedure found a proof for a query
         *         containing variables (i.e. possibly more than 1 proof can be
         *         returned) and the inference procedure was still looking for other
         *         possible answers before it timed out.
         */
        bool isPartialResultDueToTimeout();

        /**
         * 
         * @return a list of 0 or more proofs (multiple proofs can be returned if
         *         the original query contains variables).
         */
        ICollection<Proof> getProofs();
    }
}
