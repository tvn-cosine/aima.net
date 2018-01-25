using aima.net;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofPrinter
    {
        /// <summary>
        /// Utility method for outputting proofs in a formatted textual
        /// representation.
        /// </summary>
        /// <param name="proof"></param>
        /// <returns></returns>
        public static string printProof(Proof proof)
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();

            sb.Append("Proof, Answer Bindings: ");
            sb.Append(proof.getAnswerBindings());
            sb.Append("\n");

            ICollection<ProofStep> steps = proof.getSteps();

            int maxStepWidth = "Step".Length;
            int maxProofWidth = "Proof".Length;
            int maxJustificationWidth = "Justification".Length;

            // Calculate the maximum width for each column in the proof
            foreach (ProofStep step in steps)
            {
                string sn = "" + step.getStepNumber();
                if (sn.Length > maxStepWidth)
                {
                    maxStepWidth = sn.Length;
                }
                if (step.getProof().Length > maxProofWidth)
                {
                    maxProofWidth = step.getProof().Length;
                }
                if (step.getJustification().Length > maxJustificationWidth)
                {
                    maxJustificationWidth = step.getJustification().Length;
                }
            }

            // Give a little extra padding
            maxStepWidth += 1;
            maxProofWidth += 1;
            maxJustificationWidth += 1;

            string f = "|{0}-" + maxStepWidth + "s| {1}-" + maxProofWidth + "s|{2}-" + maxJustificationWidth + "s|\n";

            int barWidth = 5 + maxStepWidth + maxProofWidth + maxJustificationWidth;
            IStringBuilder bar = TextFactory.CreateStringBuilder();
            for (int i = 0; i < barWidth;++i)
            {
                bar.Append("-");
            }
            bar.Append("\n");

            sb.Append(bar);
            sb.Append(string.Format(f, "Step", "Proof", "Justification"));
            sb.Append(bar);
            foreach (ProofStep step in steps)
            {
                sb.Append(string.Format(f, "" + step.getStepNumber(), step.getProof(), step.getJustification()));
            }
            sb.Append(bar);

            return sb.ToString();
        }
    }
}
