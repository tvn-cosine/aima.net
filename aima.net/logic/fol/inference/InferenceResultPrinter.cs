using aima.net.text;
using aima.net.text.api;
using aima.net.logic.fol.inference.proof;

namespace aima.net.logic.fol.inference
{
    public class InferenceResultPrinter
    {
        /**
         * Utility method for outputting InferenceResults in a formatted textual
         * representation.
         * 
         * @param ir
         *            an InferenceResult
         * @return a string representation of the InferenceResult.
         */
        public static string printInferenceResult(InferenceResult ir)
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();

            sb.Append("InferenceResult.isTrue=" + ir.isTrue());
            sb.Append("\n");
            sb.Append("InferenceResult.isPossiblyFalse=" + ir.isPossiblyFalse());
            sb.Append("\n");
            sb.Append("InferenceResult.isUnknownDueToTimeout=" + ir.isUnknownDueToTimeout());
            sb.Append("\n");
            sb.Append("InferenceResult.isPartialResultDueToTimeout=" + ir.isPartialResultDueToTimeout());
            sb.Append("\n");
            sb.Append("InferenceResult.#Proofs=" + ir.getProofs().Size());
            sb.Append("\n");
            int proofNo = 0;
            foreach (Proof p in ir.getProofs())
            {
                proofNo++;
                sb.Append("InferenceResult.Proof#" + proofNo + "=\n" + ProofPrinter.printProof(p));
            }

            return sb.ToString();
        }
    }
}
