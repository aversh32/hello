using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
namespace IIlab1
{
    

    
    public partial class Form1 : Form
    {
        State start, found;
        State[] finish;
        int iter = 0;
        List<State> OpenList;
        List<State> CloseList;
        public Form1()
        {
           
            InitializeComponent();
            
        }

        public void DrawState(State state)
        {
            GridInit();
            
                
                    dataGridView1[state.knigts[0]%3, state.knigts[0]/3].Style.BackColor = Color.Gray;
            dataGridView1[state.knigts[1] % 3, state.knigts[1] / 3].Style.BackColor = Color.Gray;


            dataGridView1[state.knigts[2] % 3, state.knigts[2] / 3].Style.BackColor = Color.Black;
            dataGridView1[state.knigts[3] % 3, state.knigts[3] / 3].Style.BackColor = Color.Black;

            textBox6.Text = found.rasst.ToString();
            Refresh();
        }

        public void GridInit()
        {
          

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            
            for (int i = 0; i < 3; i++)
            {
                dataGridView1.Columns.Add(i.ToString(), "");
                dataGridView1.Columns[i].Width = dataGridView1.Width / 3 - 1;
            }

            dataGridView1.RowTemplate.Height = dataGridView1.Height / 3 - 1;
            dataGridView1.Rows.Add(3);
            dataGridView1.ClearSelection();
        }
        private void OpenState(State st, klet[] board)
        {
            iter++;
            int maxIndex = Math.Max(OpenList.Count - 1, 0);
            for (int i=0;i<4;i++)
            {
                for(int m=0;m<2;m++)
                {
                    State newState = new State(st);
                    newState.knigts[i] = board[st.knigts[i]].moves[m];
                    
                    if (CheckState(newState, board))
                    {
                        newState.parent = st;
                        newState.deep++;
                        bool exist = false;
                        for (int l = 0; l < OpenList.Count; l++)
                        {
                            if (OpenList[l] == newState)
                            {
                                exist = true;
                                if (newState.rasst < OpenList[l].rasst)
                                {
                                    OpenList[l].rasst = newState.rasst;
                                    OpenList[l].parent = st;
                                    if (OpenList[maxIndex].rasst < OpenList[l].rasst)
                                        maxIndex = l;
                                }
                                break;
                            }
                        }
                        if (!exist)
                            for (int l = 0; l < CloseList.Count; l++)
                            {
                                if (CloseList[l] == newState)
                                {
                                    exist = true;
                                    if (newState.rasst < CloseList[l].rasst)
                                    {
                                        CloseList.RemoveAt(l);
                                        if (OpenList.Count > 30)
                                        {
                                            OpenList.RemoveAt(maxIndex);
                                        }
                                        OpenList.Add(newState);
                                        if (OpenList[maxIndex].rasst < newState.rasst)
                                            maxIndex = OpenList.Count - 1;
                                    }
                                    break;
                                }
                            }
                        if (!exist)
                        {
                                if (OpenList.Count + CloseList.Count > 20 && OpenList[maxIndex] != start)
                                {
                                    bool inCloseList = false;
                                    for (int l = 0; l < CloseList.Count; l++)
                                    {
                                        if (CloseList[l] == OpenList[maxIndex].parent)
                                        {
                                            inCloseList = true;
                                            OpenList[maxIndex].parent.rasst = OpenList[maxIndex].rasst;
                                            OpenList.Add(OpenList[maxIndex].parent);
                                            CloseList.RemoveAt(l);
                                            break;
                                        }
                                    }
                                    if (!inCloseList)
                                        for (int l = 0; l < OpenList.Count; l++)
                                        {
                                            if (OpenList[l] == OpenList[maxIndex].parent)
                                            {
                                                OpenList[maxIndex].parent.rasst = Math.Min(OpenList[maxIndex].parent.rasst, OpenList[maxIndex].rasst);
                                            }
                                        }

                                    OpenList.RemoveAt(maxIndex);
                                    maxIndex = OpenList.Count - 1;
                                }

                            OpenList.Add(newState);
                            if (OpenList[maxIndex].rasst < newState.rasst)
                                maxIndex = OpenList.Count - 1;
                        }
                    }
                }
                
            }
            
        }
        public void Checkrasst(State st)
        {
            int count = 0;
            for (int i = 0; i < 2; i++)
            {
                if (st.knigts[i] == 6 || st.knigts[i] == 8) count++;
            }
            for (int i = 2; i < 4; i++)
            {
                if (st.knigts[i] == 0 || st.knigts[i] == 2) count++;
            }
            st.rasst = 4 - count;
            
        }
        private bool CheckState(State st, klet[] board)
        {

            if (Cont(OpenList, st) || Cont(CloseList, st))
                return false;
            if ( Cont(CloseList, st))
                return false;
            for (int i=0;i<4;i++)
            {
                for(int j=i+1;j<4;j++)
                {
                    if (st.knigts[i] == st.knigts[j]) return false;
                }
            }
            Checkrasst(st);
          
            /* State c = new State(Cont(CloseList, st));
             if (c.rasst < st.rasst)
             {

                 st.rasst = c.rasst;
                 st = c;
                 OpenList.Push(st);
             }*/
            return true;
        }

      
        private bool Cont(List<State> stack, State st)
        {
            foreach (State s in stack)
                if (s == st)
                    return true;
            return false;
        }
        private bool Cont(Queue<State> qu, State st)
        {
            foreach (State s in qu)
                if (s == st)
                    return true;
            return false;
        }

        private void FinishStates()
        {
            finish = new State[4];
           for(int i=0;i<4;i++)
            {
                finish[i] = new State();
            }
            finish[0].knigts[0]=6;
            finish[0].knigts[1] = 8;
            finish[0].knigts[2] = 0;
            finish[0].knigts[3] = 2;

            finish[1].knigts[0] = 8;
            finish[1].knigts[1] = 6;
            finish[1].knigts[2] = 0;
            finish[1].knigts[3] = 2;

            finish[2].knigts[0] = 6;
            finish[2].knigts[1] = 8;
            finish[2].knigts[2] = 2;
            finish[2].knigts[3] = 0;

            finish[3].knigts[0] = 8;
            finish[3].knigts[1] = 6;
            finish[3].knigts[2] = 2;
            finish[3].knigts[3] = 0;

        }
      
        public void ShowRes()
        {
            Stack<State> Way = new Stack<State>();
            Way.Push(found);
            while (found != start)
            {
                found = found.parent;
                Way.Push(found);
            }

            while (Way.Count > 0)
            {
                GridInit();
                found = Way.Pop();
                DrawState(found);
                
                Thread.Sleep(1000);
            }
        }

        public void Drawboard()
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Brown);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.CreateGraphics();
            formGraphics.FillRectangle(myBrush, new Rectangle(50, 50, 70, 70));
            formGraphics.FillRectangle(myBrush, new Rectangle(190, 50, 70, 70));
            formGraphics.FillRectangle(myBrush, new Rectangle(120, 120, 70, 70));
            formGraphics.FillRectangle(myBrush, new Rectangle(50, 190, 70, 70));
            formGraphics.FillRectangle(myBrush, new Rectangle(190, 190, 70, 70));
            myBrush.Color = System.Drawing.Color.White;
            formGraphics.FillRectangle(myBrush, new Rectangle(120, 50, 70, 70));
            formGraphics.FillRectangle(myBrush, new Rectangle(50, 120, 70, 70));
            formGraphics.FillRectangle(myBrush, new Rectangle(190, 120, 70, 70));
            formGraphics.FillRectangle(myBrush, new Rectangle(120, 190, 70, 70));
            myBrush.Dispose();
            formGraphics.Dispose();
        }
      
        public void Findmin(List<State> stack)
        {
            
            State a = new State() ;

            int min = 5;
            for(int i=0; i<stack.Count;i++)
            {
                if (stack[i].rasst<min)
                {
                    min = stack[i].rasst;
                    a = stack[i];
                    stack.RemoveAt(i);
                    stack.Add(a);
                }
            }
           
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
           // Drawboard();
            start = new State();
            start.knigts[0] = 0;
            start.knigts[1] = 2;
            start.knigts[2] = 6;
            start.knigts[3] = 8;
            start.rasst = 4;
            klet[] board = new klet[9];
            for(int i=0;i<9;i++)
            {
                board[i] = new klet();
            board[i].isEmpty = true;
            }
            board[0].isEmpty = false; board[2].isEmpty = false; board[6].isEmpty = false; board[8].isEmpty = false;
                board[0].isWhite = true; board[2].isWhite = true; board[6].isWhite = false; board[8].isWhite = false;
                board[0].moves[0] = 5; board[0].moves[1] = 7;
                board[1].moves[0] = 6; board[1].moves[1] = 8;
                board[2].moves[0] = 3; board[2].moves[1] = 7;
                board[3].moves[0] = 2; board[3].moves[1] = 8;
                board[4].moves[0] = 0; board[4].moves[1] = 0;
                board[5].moves[0] = 0; board[5].moves[1] = 6;
                board[6].moves[0] = 1; board[6].moves[1] = 5;
                board[7].moves[0] = 0; board[7].moves[1] = 2;
                board[8].moves[0] = 1; board[8].moves[1] = 3;
            FinishStates();
            OpenList = new List<State>();
            OpenList.Add(start);
            CloseList = new List<State>();
            int maxDeep = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int iter = 1; OpenList.Count > 0; iter++)
            {
                Findmin(OpenList);
                State curState = OpenList[OpenList.Count-1];
                    OpenList.RemoveAt(OpenList.Count - 1);
             /*   int clmax = 0; int qw = 0;
                for(int i=0;i<CloseList.Count;i++)
                {
                    if(CloseList[i].rasst>clmax)
                    {
                        clmax = CloseList[i].rasst;
                        qw = i;
                    }
                }
                if (CloseList.Count > 30)
                {
                    CloseList.RemoveAt(qw);
                }*/

                CloseList.Add(curState);

                if (curState == finish[0] || curState == finish[1] || curState == finish[2] || curState == finish[3])
                {
                    MessageBox.Show("Решение найдено на глубине " + curState.deep.ToString());
                    found = curState;
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    // Format and display the TimeSpan value.
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);
                    textBox1.Text = elapsedTime;
                    textBox2.Text = iter.ToString();
                    textBox3.Text = CloseList.Count.ToString();
                    textBox4.Text = OpenList.Count.ToString() ;
                    
                     ShowRes();
                   
                    return;
                }
                
                OpenState(curState, board);
                maxDeep = Math.Max(maxDeep, curState.deep);
                textBox1.Text = curState.knigts[0].ToString();
                textBox2.Text = maxDeep.ToString();
                textBox3.Text = CloseList.Count.ToString();
                textBox4.Text = OpenList.Count.ToString();
                textBox5.Text = curState.deep.ToString();
                textBox6.Text = curState.rasst.ToString();
                    Refresh();

            }
            MessageBox.Show("Решение НЕ найдено");
        }
    
    }

   
    public class State
    {
      
        public int[] knigts = new int[4];
        public State parent;
        public int deep;
        public int rasst;
        public State()
        {
            for (int i = 0; i < 4; i++)
            {
                knigts[i] = new int();
            }
        }
        public State(State st)
        {
            for(int i=0;i<4;i++)
            {
                knigts[i] = new int();
            }
            knigts[0] = st.knigts[0];
            knigts[1] = st.knigts[1];
            knigts[2] = st.knigts[2];
            knigts[3] = st.knigts[3];
            
            parent = st.parent;
            deep = st.deep;
            rasst = st.rasst;
        }

        
        public static bool operator ==(State st1, State st2)
        {
            //for (int i = 0; i < 4; i++)
                if (((st1.knigts[0]!= st2.knigts[0] && st1.knigts[0] != st2.knigts[1]) || (st1.knigts[1] != st2.knigts[0] && st1.knigts[1] != st2.knigts[1])) || ((st1.knigts[2] != st2.knigts[2] && st1.knigts[2] != st2.knigts[3]) || (st1.knigts[3] != st2.knigts[2] && st1.knigts[3] != st2.knigts[3])))
                    return false;
            return true;
        }

        public static bool operator !=(State st1, State st2)
        {
            if (st1 == st2)
                return false;
            return true;
        }
    }
    public class klet
    {
        public bool isEmpty;
        public bool isWhite;
        public int[] moves = new int[2];
        
        public klet()
        {
            for(int i=0;i<2;i++)
            {
                moves[i] = new int();
            }
        }
    }
    public class Graph
    {
        public List<GraphNode> Nodes;
        public List<GraphNode> Open;
        public List<GraphNode> Close;
    }
    public class GraphNode
    {
        public bool New;
        public State curst;
        public State prevst;
        public List<GraphNode> Links;
       
    }

   
    
}
