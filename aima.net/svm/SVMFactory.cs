using aima.net.collections;
using aima.net.exceptions;
using aima.net.text;

namespace aima.net.svm
{
    public static class SVMFactory
    {
        public static Problem LoadSVMFile(string filename, Parameter param)
        {
            System.IO.StreamReader fp = new System.IO.StreamReader(filename);
            List<double> vy = new List<double>();
            List<Node[]> vx = new List<Node[]>();
            int max_index = 0;

            while (true)
            {
                string line = fp.ReadLine();
                if (line == null) break;

                var st = line.Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });
                int counter = 0;

                vy.Add(TextFactory.ParseDouble(st[counter++]));
                int m = (st.Length - 1) / 2;
                Node[] x = new Node[m];
                for (int j = 0; j < m; j++)
                {
                    x[j] = new Node();
                    x[j].index = TextFactory.ParseInt(st[counter++]);
                    x[j].value = TextFactory.ParseDouble(st[counter++]);
                }
                if (m > 0) max_index = System.Math.Max(max_index, x[m - 1].index);
                vx.Add(x);
            }

            Problem prob = new Problem();
            prob.l = vy.Size();
            prob.x = new Node[prob.l][];
            for (int i = 0; i < prob.l; i++)
                prob.x[i] = vx.Get(i);
            prob.y = new double[prob.l];
            for (int i = 0; i < prob.l; i++)
                prob.y[i] = vy.Get(i);

            if (param.gamma == 0 && max_index > 0)
                param.gamma = 1.0 / max_index;

            if (param.kernel_type == Parameter.PRECOMPUTED)
                for (int i = 0; i < prob.l; i++)
                {
                    if (prob.x[i][0].index != 0)
                    {
                        throw new Exception("Wrong kernel matrix: first column must be 0:sample_serial_number\n");
                    }
                    if ((int)prob.x[i][0].value <= 0 || (int)prob.x[i][0].value > max_index)
                    {
                        throw new Exception("Wrong input format: sample_serial_number out of range\n");
                    }
                }

            fp.Close();
            return prob;
        }


    }
}
