using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MPTranslator
{
    class Node
    {
        public string value;
        public ArrayList childrens = new ArrayList();

        public Node(string val)
        {
            value = val;
        }

        public void Create(ref mySDTranslator SDT, ref ArrayList rules)
        {
            //Console.WriteLine($"����� � {Utility.convertInt( rules)}");
            if (SDT.NotTerminalSymbols.Contains(value.Split('_')[0]))
            {
                int rule = (int) rules[0];
                rules.RemoveAt(0);

                ArrayList rrule = (ArrayList)SDT.Rules[rule-1];
                if (value.Split('_')[0] != ((string)rrule[0]).Split('_')[0])
                    throw new Exception("������������ ����� �������");

                ArrayList child = (ArrayList)rrule[1];

                for (int i = 0; i < child.Count; i++)
                {
                    //Console.WriteLine($"������� ������� {child[i]}");
                    childrens.Add(new Node((string)child[i]));
                    ((Node)childrens[i]).Create(ref SDT, ref rules);
                }
            } 
            else
            {
                return;
            }
        }

        public void Transform(ref mySDTranslator SDT, ref Node root)
        {
            ArrayList which_rule = new ArrayList();

            
            foreach(Node node in childrens)
            {
                which_rule.Add(node.value);
            }

            //Console.WriteLine($"�������: {value}, ����: {Utility.convert(which_rule)}");

            foreach (ArrayList rules in SDT.Rules)
            {
                //Console.WriteLine($"���� ������� ������ ��� {value}, ������: {rules[0]} --- {Utility.convert((ArrayList)rules[1])} | {Utility.convert((ArrayList)rules[2])}");

                if ((string)rules[0] == value.Split('_')[0] && Utility.IsSameArrayList(which_rule, (ArrayList)rules[1])) // ����� ������ �������
                {
                    ArrayList second_rules = (ArrayList)rules[2];

                    ArrayList new_childrens = new ArrayList();

                    foreach (string symbol in second_rules)
                    {
                        if (SDT.OutPutTerminalSymbols.Contains(symbol)) // ���� ��������
                        {
                            new_childrens.Add(new Node(symbol));
                        } 
                        else // �� �������� 
                        {
                            int index = 0;
                            for (; index < childrens.Count; index++)
                            {
                                if (symbol == ((Node)childrens[index]).value)
                                {
                                    break;
                                }
                            }

                            ((Node)childrens[index]).Transform(ref SDT, ref root);
                            new_childrens.Add((Node)childrens[index]);
                            childrens.RemoveAt(index);
                        }
                    }

                    childrens = new_childrens;
                    break;
                }

                
            }
        }

        public bool IsLeaf()
        {
            return childrens.Count == 0;
        }

        public void Print(int tab)
        {
            for (int i = 0; i < tab; i++) Console.Write("   ");

            if (childrens.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine($" {value} ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.WriteLine($" {value} ");
            }

            for (int i = childrens.Count-1; i >= 0; i--)
            {
                Node child = (Node)childrens[i];
                child.Print(tab+1);
            }

        }

        public void PrintCrown()
        {
            for (int i = 0; i < childrens.Count; i++)
            {
                Node child = (Node)childrens[i];
                child.PrintCrown();
            }

            if (childrens.Count == 0)
            {
                Console.Write(value);
            }
        }

    }

    class Tree
    {
        public mySDTranslator SDT;
        public Node root;

        public Tree(mySDTranslator sdt, ArrayList rules, string FS)
        {
            SDT = sdt;
            root = new Node(FS);
            root.Create(ref SDT, ref rules);
        }

        public void Transform()
        {
            root.Transform(ref SDT, ref root);
        }

        public void Print()
        {
            Console.WriteLine($"\n������ ������ ������� �������:");
            root.Print(0);
        }

        public void PrintCrown()
        {
            Console.Write($"\n�����: ");
            root.PrintCrown();
            Console.WriteLine();
        }
    }

    class mySDTranslator
    {
        public ArrayList NotTerminalSymbols;
        public ArrayList InPutTerminalSymbols;
        public ArrayList OutPutTerminalSymbols;
        public ArrayList Rules;
        public string FirstSymbol;

        public mySDTranslator(ArrayList NTS, ArrayList IPTS, ArrayList OPTS, string FS)
        {
            NotTerminalSymbols = NTS;
            InPutTerminalSymbols = IPTS;
            OutPutTerminalSymbols = OPTS;
            if (!NotTerminalSymbols.Contains(FS))
                throw new Exception($"��������� ������ {FS} �� ����������� ��������� ������������");
            FirstSymbol = FS;
            Rules = new ArrayList();
        }

        public void addRule(string NotTerminal, ArrayList Chain1, ArrayList Chain2)
        {
            if (!NotTerminalSymbols.Contains(NotTerminal))
                throw new Exception($"� ������� {NotTerminal} --- ({Utility.convert(Chain1)}, {Utility.convert(Chain2)}) ���������� {NotTerminal} �� ����������� ��������� ������������");

            foreach (string symbol in Chain1)
            {
                if (!NotTerminalSymbols.Contains(symbol.Split('_')[0]) && !InPutTerminalSymbols.Contains(symbol))
                    throw new Exception($"� ������� {NotTerminal} --- ({Utility.convert(Chain1)}, {Utility.convert(Chain2)}) ������ {symbol} �� ����������� ��������� ������������ ��� �������� ��������");
            }

            foreach (string symbol in Chain2)
            {
                if (!NotTerminalSymbols.Contains(symbol.Split('_')[0]) && !OutPutTerminalSymbols.Contains(symbol))
                    throw new Exception($"� ������� {NotTerminal} --- ({Utility.convert(Chain1)}, {Utility.convert(Chain2)}) ������ {symbol} �� ����������� ��������� ������������ ��� ��������� ��������");
            }

            Rules.Add(new ArrayList() { NotTerminal, Chain1, Chain2});
        }

        public void debugSDT()
        {
            Console.WriteLine("\n");
            Console.WriteLine($"�������������� �������: {Utility.convert(NotTerminalSymbols)}");
            Console.WriteLine($"������� �������: {Utility.convert(InPutTerminalSymbols)}");
            Console.WriteLine($"�������� �������: {Utility.convert(OutPutTerminalSymbols)}");
            Console.WriteLine($"��������� ������: {FirstSymbol}");
            Console.WriteLine();

            for (int i = 0; i < Rules.Count; i++)
            {
                ArrayList s = (ArrayList)Rules[i];
                string NT = (string)s[0];
                ArrayList Chain1 = (ArrayList)s[1];
                ArrayList Chain2 = (ArrayList)s[2];

                Console.WriteLine($"{i+1}. {NT} --- ({Utility.convert(Chain1)}, {Utility.convert(Chain2)})");
            }
        }

        public void ParseWithRulesNumbers(ArrayList RulesNumbers)
        {
            
            ArrayList Chain1 = new ArrayList() { FirstSymbol };
            ArrayList Chain2 = new ArrayList() { FirstSymbol };
            Console.Write("\n");
            Console.WriteLine($"����� � ������ ��-����� �� ��������: {Utility.convertInt(RulesNumbers)}");
            Console.Write($"({Utility.convert(Chain1)}, {Utility.convert(Chain2)})");

            foreach (int rule in RulesNumbers)
            {
                // ������� ����� ���������� �� ������� rule-1 � �������, ����� ������ ����� �� ����������� � ��������� ������ ����
                int real_rule = rule - 1;

                if (real_rule >= Rules.Count || real_rule < 0)
                    throw new Exception($"������������ ������� � ������� {real_rule}. ����� �� ��������");

                ArrayList s = (ArrayList)Rules[real_rule];
                string NT = (string)s[0];
                ArrayList l = new ArrayList( (ArrayList)s[1]);
                ArrayList r = new ArrayList((ArrayList)s[2]);

                // ����� ������� �����������
                int index1 = -1;
                int index2 = -1;
                {
                    for (int i = 0; i < Chain1.Count; i++)
                    {
                        string chain = (string)Chain1[i];
                        if (chain == NT || chain.Split('_')[0] == NT)
                        {
                            index1 = i;
                            NT = chain;
                            break;
                        }
                    }

                    for (int i = 0; i < Chain2.Count; i++)
                    {
                        string chain = (string)Chain2[i];
                        if (chain == NT || chain.Split('_')[0] == NT)
                        {
                            index2 = i;
                            break;
                        }
                    }
                }

                Chain1.RemoveAt(index1);
                
                ArrayList ll = l;
                ArrayList rr = r;
                for (int i = 0; i < ll.Count; i++)
                {
                    string symbol = (string)ll[i];
                    if (!InPutTerminalSymbols.Contains(symbol)) { // ���� ��������
                        int max = 0;
                        foreach (string ss in Chain1)
                        {
                            if (ss.Split('_')[0] == symbol.Split('_')[0])
                                if (!NotTerminalSymbols.Contains(ss))
                                    if (max < Convert.ToInt32(ss.Split('_')[1]))
                                        max = Convert.ToInt32(ss.Split('_')[1]);
                        }
                        max++;
                        Chain1.Insert(index1, symbol.Split('_')[0] + "_" + max.ToString());
                        rr[rr.IndexOf(symbol)] = symbol.Split('_')[0] + "_" + max.ToString();

                    } else {
                        Chain1.Insert(index1, symbol);
                    }
                    index1++;
                }

                Chain2.RemoveAt(index2);
                Chain2.InsertRange(index2, rr);

                Console.Write($" |- {rule}\n({Utility.convert(Chain1)}, {Utility.convert(Chain2)})");
            }

            Console.WriteLine();
        }

        private bool function(ArrayList current_chain, ArrayList real_chain, int depth, ref ArrayList answer)
        {
            // answer - ����� �������� ������� �������, ������� ���� �����
            // depth - ������������ ���������� ������, ������� �� ����� ��������� ��� ��������� ������
            // real_chain - �������, ���� ������� ����� �����
            // current_chain - �������, ���������� �� ������ ����. ���������� ����� ���������� �������

            if (depth == 0) 
            {
                // ���� ������ ������ ��������� �� �����, �� � ������� ��������, �� ����� ������� � ���������� answer
                if (Utility.IsSameArrayList(current_chain, real_chain)) 
                    return true;

                return false; // ����� ����� �������� �������� ������� ������, ���� �������� �����������, ����� ������� �� ����������� �����, ��������� ���������
            }

            
            if (Utility.IsSameArrayList(current_chain, real_chain)) // ���� �� ������ �������, �� ����� �������
            { 
                return true;
            } 
            else // ����� ���� ����������, � �������� ����� ��������� �������
            {
                string NT = "";
                foreach (string symbol in current_chain) // ������� ������ ����������, � �������� ����� ��������� �������
                {
                    if (NotTerminalSymbols.Contains(symbol.Split('_')[0]))
                    {
                        NT = symbol.Split('_')[0];
                        break;
                    }
                }

                for (int i = 0; i < Rules.Count; i++)
                {
                    ArrayList rule = (ArrayList)Rules[i];
                    if ((string)rule[0] == NT) // ��� ������� ������� �����������
                    {
                        // ��������� ���

                        ArrayList next_chain = new ArrayList(current_chain);
                        int index = next_chain.IndexOf(NT);
                        next_chain.RemoveAt(index);

                        ArrayList r = new ArrayList((ArrayList) rule[1]);
                        for (int j = 0; j < r.Count; j++)
                        {
                            r[j] = ((string)r[j]).Split('_')[0];
                        }
                        next_chain.InsertRange(index, r);


                        if (function(next_chain, real_chain, depth-1, ref answer))
                        {
                            answer.Add(i+1);
                            return true;
                        }
                    }
                }

                return false;
            }

        }

        public ArrayList RuleChain(ArrayList left_chain)
        {
            ArrayList s = new ArrayList();

            for (int i = 2; i < 20; i++)
            {
                bool flag = function(new ArrayList() { FirstSymbol }, left_chain, i, ref s);
                if (flag)
                {
                    break;
                }
                else
                {
                    s.Clear();
                }
            }

            s.Reverse();
            return s;
        }

        public void ParseWithString(ArrayList left_chain)
        {
            Console.WriteLine();
            Console.Write("������� ������: ");
            Console.WriteLine(Utility.convert(left_chain));

            ArrayList s = RuleChain(left_chain);

            Console.Write("Ÿ ����� �� ������� ����������: ");
            foreach (int rule in s)
            {
                Console.Write(rule);
                Console.Write(" ");
            }
            Console.WriteLine();

            ParseWithRulesNumbers(s);
        }

        public Tree CreateTree(ArrayList left_chain)
        {
            ArrayList s = RuleChain(left_chain);
            //Console.WriteLine(Utility.convertInt(s));

            Tree tr = new Tree(this, s, FirstSymbol);

            return tr;
        }
    }
}