using System;
using System.Collections;

namespace MPTranslator
{ 
    class myHTable
    {
        public ArrayList table = new ArrayList() { new ArrayList(), new ArrayList()};

        public myHTable(ArrayList InputSigma, ArrayList OutputSigma)
        {
            if (InputSigma.Count != OutputSigma.Count)
            {
                Console.WriteLine("����������� ��������� ������� (������� ����������)");
                throw new RankException();
            }

            this.table = new ArrayList() { InputSigma, OutputSigma };
        }

        public void debugHTable()
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

        public string h(ArrayList input)
        {
            string output = "";

            if (input.Count == 1 && (string)input[0] == "")
                return "";

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