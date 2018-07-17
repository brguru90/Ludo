using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chowka_Bhara
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int n = 5;
        player[] p;
        int[,] mat1 = new int[5, 5];
        int[,] mat2 = new int[5, 5];
        System.Drawing.Graphics g;
        private void Form1_Load(object sender, EventArgs e)
        {
            g = panel1.CreateGraphics();
            g.Clear(Color.White);
            display();
            init();
        }
        public void init()
        {
            p = new player[2];
            p[0] = new player();
            p[1] = new player();
            int[,] mat=
            {
                {11,10,9,8,7},
                {12,19,20,21,6},
                {13,18,25,22,5},
                {14,17,24,23,4},
                {15,16,1,2,3}
            };
	        Console.WriteLine("Before\n");
            show_mat(mat);
            copy_mat(mat,0);
            swap_h(mat);
            Console.WriteLine("swapped1\n");
            show_mat(mat);
            copy_mat(mat,1);
        }
        public void copy_mat(int[,] mat, int f)
        {
            int i, j;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    if (f == 0)
                        mat1[i,j] = mat[i,j];
                    else
                        mat2[i,j] = mat[i,j];
                }
            }
        }

        public void show_mat(int[,] mat)
        {
            int i, j;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    Console.WriteLine( i+""+j+"="+mat[i,j]+"\t");
                }
               Console.WriteLine("\n");
            }
        }
        public void swap_h(int[,] mat)
        {
            int i, j, temp;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j <= i; j++)
                {
                    if ((i >= n / 2 && j >= n / 2 && i == j))
                        continue;
                    temp = mat[n - i - 1,n - j - 1];
                    mat[n - i - 1,n - j - 1] = mat[i,j];
                    mat[i,j] = temp;
                }
            }
        }

        int flag = 1, dice_val=0;
        public void play()
        {
            flag++;
            Random r = new Random();
            if (flag % 2 == 0)
            {
                label1.Text = flag+")PLAYER 1";
                label1.BackColor = Color.Red;
            }
            else
            {
                label1.Text = flag + ")PLAYER 2";
                label1.BackColor = Color.Blue;
            }
            label1.ForeColor = Color.White;
            dice_val = r.Next(1, 7);
            label2.Text = "Dice Value: " + dice_val;
            button1.Enabled = false;
        }
        public void move_coin(int f, int ch, int c)
        {
            f = f % 2;
            if (ch == -1)
            {
                MessageBox.Show("Select any one of pawn");
                return;
            }
            if (c == 0)
            {
                MessageBox.Show("Through Dice First");
                return;
            }
            int i, j, k, temp = 0;
            if (p[f].coin[ch].coin_val + c <= 25)
                p[f].coin[ch].coin_val += c;
            richTextBox1.AppendText("Player "+f + 1+" score of "+ch+" coin= "+ p[f].coin[ch].coin_val+"\n");
            for (k = 0; k < 4; k++)
                temp += p[0].coin[k].coin_val;
            if (temp == 100)
            {
                MessageBox.Show("User1 Wins");
                this.Close();
            }
            temp = 0;
            for (k = 0; k < 4; k++)
                temp += p[1].coin[k].coin_val;
            if (temp == 100)
            {
                MessageBox.Show("User2 Wins");
                this.Close();
            }
            draw_pawn();
            check_pawn_strike(f);
            draw_pawn();
            button1.Enabled = true;
            dice_val = 0;
            score();
            if (c == 6)
                flag--;
        }
        public void draw_pawn()
        {
            panel1.Refresh();
            int i, j,k;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    for (k = 0; k < 4; k++)
                        if (mat1[i, j] == p[0].coin[k].coin_val)
                        {
                            p[0].coin[k].i = i;
                            p[0].coin[k].j = j;
                            if (i % 2 == 1 || j % 2 == 1)
                                p[0].coin[k].safe = false;
                            else
                                p[0].coin[k].safe = true;
                            draw_circle(j, i, 0, k);
                        }
                        else
                        if (mat2[i, j] == p[1].coin[k].coin_val)
                        {
                            p[1].coin[k].i = i;
                            p[1].coin[k].j = j;
                            if (i % 2 == 1 || j % 2 == 1)
                                p[1].coin[k].safe = false;
                            else
                                p[1].coin[k].safe = true;
                            draw_circle(j, i, 1, k);
                        }
                }
            }
        }
        public void check_pawn_strike(int f)
        {
            int a = (f == 0) ? 1 : 0;
            for (int k = 0; k < 4; k++)
            {
                for (int m = 0; m < 4; m++)
                {
                    if ((p[f].coin[k].i == p[a].coin[m].i && p[f].coin[k].j == p[a].coin[m].j) && (p[f].coin[k].coin_val>0 && p[a].coin[m].coin_val > 0 && p[a].coin[m].safe==false))
                    {
                        p[a].coin[m].coin_val = 0;
                        p[a].coin[m].i = 0;
                        p[a].coin[m].j = 0;
                        richTextBox1.AppendText("Striked: " + "P" + a + "=" + (m + 1)+"\n");
                        richTextBox1.ScrollToCaret();
                        flag--;
                        return;
                    }
                }
            }
        }

        public void score()
        {
            richTextBox1.AppendText("\n\nplayer 1\n");
            for (int i = 0; i < 4; i++)
                richTextBox1.AppendText("p1[" + i + "]=" + p[0].coin[i].coin_val + "\tx=" + p[0].coin[i].i + "\ty=" + p[0].coin[i].j +"\tsafe="+ p[0].coin[i].safe+"\n");
            richTextBox1.AppendText("player 2\n");
            for (int i = 0; i < 4; i++)
                richTextBox1.AppendText("p1[" + i + "]=" + p[1].coin[i].coin_val + "\tx=" + p[1].coin[i].i + "\ty=" + p[1].coin[i].j + "\tsafe=" + p[1].coin[i].safe + "\n");
            richTextBox1.ScrollToCaret();
        }

        public void display()
        {            
            draw_box();
            g.Save();
        }
        void draw_box()
        {
            int h = panel1.Width-1;
            int w = panel1.Height-1;
            int hbyn = (int)h / n;
            int wbyn = (int)w / n;
            for (int i = 0; i <= n; i++)
            {
                draw_line(0, i * hbyn, w, i * hbyn);
                draw_string("y=" + i * hbyn , g, 0, i * hbyn, Color.Green);
            }
            for (int i = 0; i <=n; i++)
            { 
                draw_line(i *wbyn, 0, i * wbyn, h);
                draw_string("x=" + i * wbyn, g, i * wbyn, 0, Color.Green);
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i % 2 == 0 && j % 2 == 0)
                    {
                        draw_line(i * hbyn, j * wbyn, (i+1) * hbyn, (j+1) * wbyn);
                        draw_line((i + 1) * hbyn, j * wbyn, i * hbyn, (j+1) * wbyn);
                    }
                }
            }
        }
        public void draw_line(int x1, int y1, int x2, int y2)
        {
            g.DrawLine(new Pen(Color.Blue, 1), new Point(x1, y1), new Point(x2 , y2));
           // draw_string("x1=" + x1 + ",y1=" + y1 + ",x2=" + x2 + ",y2=" + y2, g, x1, y1, Color.Red);
        }
        public void draw_circle(int x,int y,int c,int k)
        {
            int h = panel1.Width - 1;
            int w = panel1.Height - 1;
            int hbyn = (int)h / n;
            int wbyn = (int)w / n;
            int radius = 30;
            Color color;
            if (c == 0)
            {
                x = x * hbyn + (int)(hbyn * 0.20) ;
                color = Color.Red;
            }
            else
            {
                x = x * hbyn + (int)(hbyn * 0.60);
                color = Color.Blue;
            }
            y = y * wbyn + (int)(wbyn * 0.22 * (k + 1));
            y -= 20;
            p[c].coin[k].x = x;
            p[c].coin[k].y = y;
            g.DrawEllipse(new Pen(Color.Yellow, 5), x,y, radius, radius);
            g.FillEllipse(new SolidBrush(color), x, y, radius, radius);
            draw_string(k+1+"", g, x+10, y+10, Color.White,10,20,20);
        }
        public void draw_string(String str, System.Drawing.Graphics g, int x, int y, System.Drawing.Color color, int size = 12, int width = 180, int height = 150)
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);
            System.Drawing.PointF pt = new System.Drawing.PointF(x, y);
            System.Drawing.Size sz = new System.Drawing.Size(width, height);
            System.Drawing.Font ft = new Font("Times New Roman", size);
            RectangleF rf = new RectangleF(pt, sz);
            StringFormat sf = new StringFormat();
            g.DrawString(str, ft, myBrush, rf, sf);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
             display();
        }
      
        private void button1_Click(object sender, EventArgs e)
        {            
            play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            move_coin(flag, 0, dice_val);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            move_coin(flag, 1, dice_val);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            move_coin(flag, 2, dice_val);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            move_coin(flag, 3, dice_val);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            for(int i=0;i<2;i++)
                for(int j=0;j<4;j++)
                {
                    if(e.X>=p[i].coin[j].x && e.X <= p[i].coin[j].x+30 && e.Y >= p[i].coin[j].y && e.Y <= p[i].coin[j].y + 30)
                    {
                        if (flag % 2 == i)
                            move_coin(flag, j, dice_val);
                        else
                        {
                            MessageBox.Show("Choose Appropriate pawn");
                        }
                    }
                }
                  
        }
    }
}
