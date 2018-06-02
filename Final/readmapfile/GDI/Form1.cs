using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace GDI
{
    public partial class Form1 : Form
    {
        char[,]twodarray = new char[50,50];
        Graphics dc;
        int size = 16;
        
        static int x = 20;
        static int y = 20;

        int timeLeft = 60;

        static int tries = 1;

        static int yourTime = 0;

        bool left;
        bool up;
        bool right;
        bool down;

        Bitmap curBitmap;
        //Create a temporary offscreen Graphics object from the bitmap
        Graphics offscreen;

        public Form1()
        {
            InitializeComponent();
            this.Show();
            //create the onscreen graphics
            dc = this.CreateGraphics();
            readFile("h:\\array.txt");

            // create a blank bitmap
            curBitmap = new Bitmap(this.Width+300, this.Height+300);

            //Create a temporary Graphics object from the bitmap
            offscreen = Graphics.FromImage(curBitmap);
        }

        private void readFile(string filename)
        {
            int count = 0;
            StreamReader re = File.OpenText(filename);

            while (!re.EndOfStream)
            {
                char ch = (char)re.Read();
                if (ch != '\n' && ch != '\r' )
                {
                    twodarray[count % 35, count / 35] = ch;
                   count++;
                }
               
            }
            re.Close();
        }
        
        //this code copies offscreen to onscreen
        private void timer1_Tick(object sender, EventArgs e)
        {
            offscreen.Clear(Color.White);
        
            for (int x = 0; x < 35; x++)
            {
                for (int y =0;y<35;y++)
                {
                    if (twodarray[x,y] == '3')
                    {
                        offscreen.FillRectangle(new SolidBrush(Color.Blue) , x * size, y * size,size,size);
                    }
                    if (twodarray[x, y] == '2')
                    {
                        offscreen.FillRectangle(new SolidBrush(Color.Red), x * size, y * size, size, size);
                    }
                    if (twodarray[x, y] == '1')
                    {
                        offscreen.FillRectangle(new SolidBrush(Color.Black), x * size, y * size, size, size);
                    }
                }
            }
            //Creating the character
           
            offscreen.FillRectangle(new SolidBrush(Color.Green), x, y, 8, 8);

            //moving lol
            if (left == true)
            {
                x -= 1;
            }
            if (up == true)
            {
                y -= 1;
            }
            if (right == true)
            {
                x += 1;
            }
            if (down == true)
            {
                y += 1;
            }

            // Create string to draw.
            String drawTitle = "Adib's Maze Game!";

            // Create font and brush.
            Font fontTitle = new Font("Arial", 30);
            SolidBrush brushTitle = new SolidBrush(Color.Black);

            // Create point for upper-left corner of drawing.
            Point pointTitle = new Point(560, 0);

            // Draw string to screen.
            offscreen.DrawString(drawTitle, fontTitle, brushTitle, pointTitle);



            // Create string to draw.
            String drawRules = "1. You control the green square in the top left corner.\n2.Use WASD to control your character\n3. Solve the maze by getting to the blue square.\n4. Solve the maze before time runs out\n5. Make sure CAPS LOCK is off ";

            // Create font and brush.
            Font fontRules = new Font("Arial", 12);
            SolidBrush brushRules = new SolidBrush(Color.Black);

            // Create point for upper-left corner of drawing.
            Point pointRules = new Point(560, 50);

            // Draw string to screen.
            offscreen.DrawString(drawRules, fontRules, brushRules, pointRules);






            // Create string to draw.
            String drawString = "Tries: " + tries;

            // Create font and brush.
            Font drawFont = new Font("Arial", 25);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Create point for upper-left corner of drawing.
            Point drawPoint = new Point(560, 200);

            // Draw string to screen.
            offscreen.DrawString(drawString, drawFont, drawBrush, drawPoint);


            // Create string to draw.
            String drawTimer = "Time left: " + timeLeft;

            // Create font and brush.
            Font fontTimer = new Font("Arial", 25);
            SolidBrush brushTimer = new SolidBrush(Color.OrangeRed);

            // Create point for upper-left corner of drawing.
            Point pointTimer = new Point(560, 260);

            // Draw string to screen.
            offscreen.DrawString(drawTimer, fontTimer, brushTimer, pointTimer);



            //Collision Code using color at specific location
            Color character = curBitmap.GetPixel(x - 1, y - 1);
            Color characterVertical = curBitmap.GetPixel(x + 5, y + 5);
            Color winner = curBitmap.GetPixel(528, 528);
            Color colorFail = curBitmap.GetPixel(5, 5);

            if (character == colorFail || characterVertical == colorFail)
            {
               x = 20;
               y = 20;

                left = false;
                down = false;
                right = false;
                up = false;

                tries++;
            }
            if (character == winner)
            {
                timer1.Enabled = false;
                timer3.Enabled = false;
                if (tries == 1)
                {
                    yourTime = 60 - timeLeft;
                    MessageBox.Show("You won in " + tries + " try! It took you " + yourTime + " to finish.");
                    Application.Exit();

                }
                else
                {
                    yourTime = 60 - timeLeft;
                    MessageBox.Show("You won in " + tries + " tries! It took you " + yourTime + " seconds to finish.");
                    Application.Exit();

                }
            }

            dc.DrawImage(curBitmap, 0, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(950, 550);

            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == 'a')
            {
                //x -= 4;
                left = true;
                down = false;
                right = false;
                up = false;
            }
            else if (e.KeyChar == 's')
            {
                //y += 4;
                left = false;
                down = true;
                right = false;
                up = false;
            }
            else if (e.KeyChar == 'd')
            {
                //x += 4;
                left = false;
                down = false;
                right = true;
                up = false; ;
            }
            else if (e.KeyChar == 'w')
            {
                //y -= 4;
                left = false;
                down = false;
                right = false;
                up = true;
            }
            //else if (e.KeyChar == 'p')
            //{
            //    pause();
                
            //}

            if (e.KeyChar == '`')
            {
                MessageBox.Show("You discovered the secret easter egg. Next game: ");
                Application.Exit();
            }
        }

        //public void pause()
        //{
            
        //    timer1.Enabled = false;
        //    timer3.Enabled = false;

            
        //}

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                // Create string to draw.
                String drawTimer = "Time left: " + timeLeft;

                // Create font and brush.
                Font fontTimer = new Font("Arial", 25);
                SolidBrush brushTimer = new SolidBrush(Color.Black);

                // Create point for upper-left corner of drawing.
                Point pointTimer = new Point(560, 400);

                // Draw string to screen.
                offscreen.DrawString(drawTimer, fontTimer, brushTimer, pointTimer);

                timeLeft = timeLeft - 1;

            }
            else
            {
                timer1.Enabled = false;
                timer3.Enabled = false;
                MessageBox.Show("You did not finish in time.");
                Application.Exit();

            }
        }
    }
}
