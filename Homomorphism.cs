using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MPTranslator
{ 
    class Homomorphism
    {
        public ArrayList table = new ArrayList() { new ArrayList(), new ArrayList()};

        public Homomorphism(ArrayList Sigma, ArrayList h)
        {
            if (Sigma.Count != h.Count)
            {
                Console.WriteLine("����������� ��������� ������� (������� ����������)");
                throw new RankException();
            }

            this.table = new ArrayList() {Sigma, h};
        }

        public void debugHomomorphism()
        {
            ArrayList a = (ArrayList) table[0];
            ArrayList h_a = (ArrayList) table[1];
            for (int i = 0; i < a.Count; i++)
            {
                Console.Write(a[i]);
                Console.Write(" --- ");
                Console.WriteLine(h_a[i]);
            }
        }

        public string Parse(ArrayList input)
        {
            string output = "";

            if (input.Count == 1 && (string)input[0] == "")
            {
                return "";
            }

            foreach (string symbol in input) // ��� ������� ������� �� �������� ������������������
            {
                ArrayList s = (ArrayList) table[0];
                int ss = s.IndexOf(symbol);

                if (ss == -1)
                {
                    Console.WriteLine("������ �� �� ��������\n������� ������� �� ���� ���������");
                    return "";
                }

                output += ((ArrayList)this.table[1])[ss] + " "; // � �������� ����� ���������� ��������������� �������� h(a)
            }

            return output;
        }
    }
}