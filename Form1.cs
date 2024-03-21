// Jeffrey Wong
// ICS3U
// January 15th 2024
// Final Project

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_Project___Jeffrey_Wong_ICS3U
{
    public partial class Form1 : Form
    {
        Rectangle player, enemy, collect;
        Rectangle[] barrier;
        Image grass, farm, fences;
        Image[] playerMovements, enemyMovements; // Creating an array to store multiple movements for animation
        Image farmer, child, carrot, corn;
        Timer walkTimer, enemyTimer;
        Label title, intro;
        Button startButton, introButton;
        int dx, dy;
        int steps, barrierCounter;
        string[] row;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(1440, 1000);
            this.Width = 1440;
            this.Height = 1000;
            this.Text = "Final Project";
            this.CenterToScreen();

            // Each array will be size 16 to play through to animate movement
            playerMovements = new Image[16];
            enemyMovements = new Image[16];

            // Initializing the images necessary for the graphics
            grass = Image.FromFile(Application.StartupPath + @"\grass.jpg", true);
            farm = Image.FromFile(Application.StartupPath + @"\farm.jpg", true);
            carrot = Image.FromFile(Application.StartupPath + @"\carrot.png", true);
            farmer = Image.FromFile(Application.StartupPath + @"\farmer_idle_down.png", true); // The default stance of the farmer
            child = Image.FromFile(Application.StartupPath + @"\child_walk_down1.png", true); // The default stance of the child
            
            // These four images will animate moving down
            playerMovements[0] = Image.FromFile(Application.StartupPath + @"\farmer_walk_down1.png", true);
            playerMovements[1] = Image.FromFile(Application.StartupPath + @"\farmer_walk_down2.png", true);
            playerMovements[2] = Image.FromFile(Application.StartupPath + @"\farmer_walk_down3.png", true);
            playerMovements[3] = Image.FromFile(Application.StartupPath + @"\farmer_walk_down4.png", true);
            // These four images will animate moving left
            playerMovements[4] = Image.FromFile(Application.StartupPath + @"\farmer_walk_left1.png", true);
            playerMovements[5] = Image.FromFile(Application.StartupPath + @"\farmer_walk_left2.png", true);
            playerMovements[6] = Image.FromFile(Application.StartupPath + @"\farmer_walk_left3.png", true);
            playerMovements[7] = Image.FromFile(Application.StartupPath + @"\farmer_walk_left4.png", true);
            // These four images will animate moving right
            playerMovements[8] = Image.FromFile(Application.StartupPath + @"\farmer_walk_right1.png", true);
            playerMovements[9] = Image.FromFile(Application.StartupPath + @"\farmer_walk_right2.png", true);
            playerMovements[10] = Image.FromFile(Application.StartupPath + @"\farmer_walk_right3.png", true);
            playerMovements[11] = Image.FromFile(Application.StartupPath + @"\farmer_walk_right4.png", true);
            // These four images will animate moving up
            playerMovements[12] = Image.FromFile(Application.StartupPath + @"\farmer_walk_up1.png", true);
            playerMovements[13] = Image.FromFile(Application.StartupPath + @"\farmer_walk_up2.png", true);
            playerMovements[14] = Image.FromFile(Application.StartupPath + @"\farmer_walk_up3.png", true);
            playerMovements[15] = Image.FromFile(Application.StartupPath + @"\farmer_walk_up4.png", true);

            // These four images will animate moving down
            enemyMovements[0] = Image.FromFile(Application.StartupPath + @"\child_walk_down1.png", true);
            enemyMovements[1] = Image.FromFile(Application.StartupPath + @"\child_walk_down2.png", true);
            enemyMovements[2] = Image.FromFile(Application.StartupPath + @"\child_walk_down3.png", true);
            enemyMovements[3] = Image.FromFile(Application.StartupPath + @"\child_walk_down4.png", true);
            // These four images will animate moving left
            enemyMovements[4] = Image.FromFile(Application.StartupPath + @"\child_walk_left1.png", true);
            enemyMovements[5] = Image.FromFile(Application.StartupPath + @"\child_walk_left2.png", true);
            enemyMovements[6] = Image.FromFile(Application.StartupPath + @"\child_walk_left3.png", true);
            enemyMovements[7] = Image.FromFile(Application.StartupPath + @"\child_walk_left4.png", true);
            // These four images will animate moving right
            enemyMovements[8] = Image.FromFile(Application.StartupPath + @"\child_walk_right1.png", true);
            enemyMovements[9] = Image.FromFile(Application.StartupPath + @"\child_walk_right2.png", true);
            enemyMovements[10] = Image.FromFile(Application.StartupPath + @"\child_walk_right3.png", true);
            enemyMovements[11] = Image.FromFile(Application.StartupPath + @"\child_walk_right4.png", true);
            // These four images will animate moving up
            enemyMovements[12] = Image.FromFile(Application.StartupPath + @"\child_walk_up1.png", true);
            enemyMovements[13] = Image.FromFile(Application.StartupPath + @"\child_walk_up2.png", true);
            enemyMovements[14] = Image.FromFile(Application.StartupPath + @"\child_walk_up3.png", true);
            enemyMovements[15] = Image.FromFile(Application.StartupPath + @"\child_walk_up4.png", true);

            // Initializing the things necessary for the starting screen
            title = new Label();
            startButton = new Button();
            
            // Setting up the title for the starting screen
            title.Width = 300;
            title.Height = 300;
            title.BackColor = Color.Transparent;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Font = new System.Drawing.Font("showcard gothic", 50f, FontStyle.Italic);
            title.Text = "Farm Escape";
            title.Top = (this.Top + 0);
            title.Left = (this.Width / 2) - (title.Width / 2);

            // Setting up the button for the starting screen
            startButton.Width = 300;
            startButton.Height = 150;
            startButton.Font = new System.Drawing.Font("MS Reference Sans Serif", 50f, FontStyle.Italic);
            startButton.BackColor = Color.Transparent;
            startButton.Text = "START";
            startButton.Top = (this.Top + 300);
            startButton.Left = (this.Width / 2) - (startButton.Width / 2);

            startButton.Click += StartButton_Click;

            // Add the start button and title for the title screen
            this.Controls.Add(startButton);
            this.Controls.Add(title);
            this.BackgroundImage = farm;

            // Setting up the input from the keys
            this.KeyUp += Form1_KeyUp;
            this.KeyDown += Form1_KeyDown;

            // Setting up the timer for the player's movements
            walkTimer = new Timer();
            walkTimer.Interval = 1000 / 10;
            walkTimer.Tick += WalkTimer_Tick;
            walkTimer.Start();

            // Setting up the timer for the enemy's movements
            enemyTimer = new Timer();
            enemyTimer.Interval = 1000 / 10;
            enemyTimer.Tick += EnemyTimer_Tick;

            // Initializing the barrier array which will be used to create barriers on the map
            barrier = new Rectangle[15 * 15]; // the maximum possible amount of 1s is 15 * 15
        }

        private void EnemyTimer_Tick(object sender, EventArgs e)
        {
            
            this.Invalidate();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = grass;
            this.Width = 770;
            this.Height = 1000;
            this.MaximumSize = new Size(800, 1000);
            this.CenterToScreen();           

            this.Controls.Remove(title);
            this.Controls.Remove(startButton);

            player = new Rectangle(50, 50, 50, 50);
            enemy = new Rectangle(655, 50, 40, 50);

            intro = new Label();
            intro.Width = 300;
            intro.Height = 300;
            intro.TextAlign = ContentAlignment.MiddleCenter;
            intro.Font = new System.Drawing.Font("showcard gothic", 15f, FontStyle.Regular);
            intro.Text = "Objective:\nDuring the summer seasons, the pesky kids are always messing with Earl (the farmer) during their time off. Help Earl collect all his crops before the kids get him!";
            intro.Top = (this.Top);
            intro.Left = (this.Width / 2) - (intro.Width / 2);

            introButton = new Button();
            introButton.Width = 50;
            introButton.Height = 50;
            introButton.Font = new System.Drawing.Font("algerian", 15f, FontStyle.Regular);
            introButton.Text = "OK!";
            introButton.Top = intro.Bottom + 25;
            introButton.Left = (this.Width / 2) - (introButton.Width / 2);

            fences = Image.FromFile(Application.StartupPath + @"\fences.png", true);

            this.Controls.Add(intro);
            this.Controls.Add(introButton);

            introButton.Click += IntroButton_Click;
        }

        private void IntroButton_Click(object sender, EventArgs e)
        {
            this.Controls.Remove(intro);
            this.Controls.Remove(introButton);

            // Setting up the paint function to display the graphics
            this.DoubleBuffered = true;
            this.Paint += Form1_Paint;

            drawWalls();

            walkTimer.Start();
            enemyTimer.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(farmer, player);
            e.Graphics.DrawImage(child, enemy);
            for (int i = 0; i < barrier.Length; i++)
            {
                e.Graphics.DrawImage(fences, barrier[i]);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                farmer = Image.FromFile(Application.StartupPath + @"\farmer_idle_up.png", true);
                dy = -10;
            }
            if(e.KeyCode == Keys.Down)
            {
                farmer = Image.FromFile(Application.StartupPath + @"\farmer_idle_down.png", true);
                dy = 10;
            }
            if(e.KeyCode == Keys.Right)
            {
                farmer = Image.FromFile(Application.StartupPath + @"\farmer_idle_right.png", true);
                dx = 10;
            }
            if(e.KeyCode == Keys.Left)
            {
                farmer = Image.FromFile(Application.StartupPath + @"\farmer_idle_left.png", true);
                dx = -10;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                dy = 0;
            }
            if(e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                dx = 0;
            }
        }

        private void WalkTimer_Tick(object sender, EventArgs e)
        {
            if (player.Bottom < (this.ClientSize.Height - 20) && dy > 0 && !player.IntersectsWith(barrier[barrierCounter])) // Allows the player to move downwards
            {
                player.Y += dy;
                AnimateMovement(0, 3);
            }
            if (player.Left > 0 && dx < 0 && !player.IntersectsWith(barrier[barrierCounter])) // Allows the player to move leftwards
            {
                player.X += dx;
                AnimateMovement(4, 7);
            }
            if (player.Right < (this.ClientSize.Width - 20) && dx > 0 && !player.IntersectsWith(barrier[barrierCounter])) // Allows the player to move rightwards
            {
                player.X += dx;
                AnimateMovement(8, 11);
            }
            if (player.Top > 0 && dy < 0 && !player.IntersectsWith(barrier[barrierCounter])) // Allows the player to move upwards
            {
                player.Y += dy;
                AnimateMovement(12, 15);
            }
            if (player.IntersectsWith(enemy))
            {
                walkTimer.Stop();
                enemyTimer.Stop();
                Application.Exit();
            }
            for (int i = 0; i < barrier.Length; i++)
            {
                if (player.IntersectsWith(barrier[i]))
                {
                }
            }

            this.Invalidate();
        }

        private void AnimateMovement(int start, int end)
        {
            steps++;

            if (steps > end || steps < start) // if the steps value is not within the range of the parameters
            {
                steps = start; // force steps to go to the start
            }

            farmer = playerMovements[steps];
            //child = enemyMovements[steps];
        }

        private void drawWalls()
        {
            row = new string[15];

            row[0] = "111111111111111";
            row[1] = "100000000000001";
            row[2] = "100000000000001";
            row[3] = "100000000000001";
            row[4] = "100000000000001";
            row[5] = "100000000000001";
            row[6] = "100000000000001";
            row[7] = "100000000000001";
            row[8] = "100000000000001";
            row[9] = "100000000000001";
            row[10] = "100000000000001";
            row[11] = "100000000000001";
            row[12] = "100000000000001";
            row[13] = "100000000000001";
            row[14] = "111111111111111";

            // Temporary counter to count how many barriers are needed;
            barrierCounter = 0;      

            for (int i = 0; i < row.Length; i++) // this for loop will represent how many rows there are
            {               
                for (int j = 0; j < row.Length; j++) // iteratng through every column
                {
                    if (row[i].Substring(j, 1) == "1") // if any of the row's characters is a "1", we will print a fence
                    {
                        barrier[barrierCounter] = new Rectangle((j * 50) + 5, (i * 50), 40, 50); // adding a new rectangle to the barrier array based on where the 1 is placed
                        barrierCounter++; // temporary counter to store however many 1's there are
                    }
                }
            }       
        }

    }
}