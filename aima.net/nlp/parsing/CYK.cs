﻿using aima.net.collections.api;
using aima.net.nlp.parsing.grammars;

namespace aima.net.nlp.parsing
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 894.<br>
     * <br>
     * 
     * <pre>
     * function CYK-PARSE(words, grammar) returns P, a table of probabilities
     *   N <- LENGTH(words)
     *   M <- the number of nonterminal symbols in grammar
     *   P <- an array of size[M,N,N], initially all 0
     *   /* Insert Lexical rules for each word *\
     *   for i = 1 to N do
     *   	for each rule of form( X -> words<sub>i</sub>[p]) do
     *   		P[X,i,1] <- p
     *   /* Combine first and second parts of right-hand sides of rules, from short to long *\
     *   for length = 2 to N do
     *   	for start = 1 to N - length + 1 do
     *   		for len1 = 1 to N -1 do 
     *   			len2 <- length - len1
     *   		for each rule of the form( X -> Y Z [p] ) do
     *   			p[X, start, length] <- MAX(P[X, start, length],
     *   						P[Y, start, len1] * P[Z, start + len1, len2] * p)
     *   return P
     * </pre>
     * 
     * Figure 23.5 The CYK algorithm for parsing. Given a sequence of words,
     * it finds the most probable derivation for the whole sequence and for
     * each subsequence. It returns the whole table, P, in which an entry 
     * P[X, start, len] is the probability of the most probable X of length
     * len starting at position start. If there is no X of that size at that
     * location, the probability is 0.<br>
     * <br>
     * 
     * @author Jonathon Belotti (thundergolfer)
     *
     */
    public class CYK
    {

        public float[,,] parse(ICollection<string> words, ProbCNFGrammar grammar)
        {
            int N = length(words);
            int M = grammar.vars.Size();
            float[,,] P = new float[M, N, N]; // initialised to 0.0
            for (int i = 0; i < N;++i)
            {
                //for each rule of form( X -> words<sub>i</sub>[p]) do
                //   P[X,i,1] <- p
                for (int j = 0; j < grammar.rules.Size(); j++)
                {
                    Rule r = grammar.rules.Get(j);
                    if (r.derives(words.Get(i)))
                    {                   // rule is of form X -> w, where w = words[i]
                        int x = grammar.vars.IndexOf(r.lhs.Get(0)); // get the index of rule's LHS variable
                        P[x, i, 0] = r.PROB;                        // not P[X][i][1] because we use 0-based indexing
                    }
                }
            }
            for (int length = 2; length <= N; length++)
            {
                for (int start = 1; start <= N - length + 1; start++)
                {
                    for (int len1 = 1; len1 <= length - 1; len1++)
                    { // N.B. the book incorrectly has N-1 instead of length-1
                        int len2 = length - len1;
                        // for each rule of the form X -> Y Z, where Y,Z are variables of the grammar
                        foreach (Rule r in grammar.rules)
                        {
                            if (r.rhs.Size() == 2)
                            {
                                // get index of rule's variables X, Y, and Z
                                int x = grammar.vars.IndexOf(r.lhs.Get(0));
                                int y = grammar.vars.IndexOf(r.rhs.Get(0));
                                int z = grammar.vars.IndexOf(r.rhs.Get(1));
                                P[x, start - 1, length - 1] = System.Math.Max(P[x, start - 1, length - 1],
                                                                  P[y, start - 1, len1 - 1] *
                                                                  P[z, start + len1 - 1, len2 - 1] * r.PROB);
                            }
                        }
                    }
                }
            }
            return P;
        }

        /**
         * Simple function to make algorithm more closely resemble pseudocode
         * @param ls
         * @return the length of the list
         */
        public int length(ICollection<string> ls)
        {
            return ls.Size();
        }

        /**
         * Print out the probability table produced by the CYK Algorithm
         * @param probTable
         * @param words
         * @param g
         */
        public void printProbTable(float[,,] probTable, ICollection<string> words, ProbUnrestrictedGrammar g)
        {
            int N = words.Size();
            int M = g.vars.Size(); // num non-terminals in grammar

            for (int i = 0; i < M;++i)
            {
                System.Console.WriteLine("Table For : " + g.vars.Get(i) + "(" + i + ")");
                for (int j = 0; j < N; j++)
                {
                    System.Console.Write(j + "| ");
                    for (int k = 0; k < N; k++)
                        System.Console.Write(probTable[i, j, k] + " | ");
                    System.Console.WriteLine();
                }
                System.Console.WriteLine();
            }
        }

        /**
         * The probability table get's us halfway there, but this method can provide the 
         * derivation chain that most probably derives the words provided to the parser.
         * @param probTable
         * @param g
         * @return
         */
        public ICollection<string> getMostProbableDerivation(float[,,] probTable, ProbUnrestrictedGrammar g)
        {
            // TODO
            return null;
        }

    } // end of CYKParse() 
}
