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
using System.Media;
using System.Drawing.Text;
using System.Windows.Forms;
using AudioPlayer;

namespace Final_Project___Jeffrey_Wong_ICS3U
{
    public partial class Level1 : Form
    {
        Rectangle playerRect, enemy1Rect, enemy2Rect, barrierRect;
        Rectangle[] wall;
        List<Rectangle> crops;
        Image grass, fences, farmer, child1, child2, corn, potato, haybale;
        Image[] playerMovements, enemyMovements;
        Timer playerTimer, enemy1Timer, enemy2Timer, clockTimer;
        Label storyLabel, menuLabel, clockLabel, nextLevelLabel;
        Button storyButton, nextLevelButton;
        SoundPlayer walkSound, collectSound;
        AudioFilePlayer backgroundMusic;
        PrivateFontCollection fontCollection;
        bool faceUp, faceDown, faceLeft, faceRight, placeBarrier, inBatch1, inBatch2;
        string[] row;
        int dx, dy;
        int playerSteps, enemy1Steps, enemy2Steps;
        int wallCounter, batch1Counter, batch2Counter;
        int time = 100;
        int temp = 0;

        public Level1()
        {
            InitializeComponent();
        }

        private void Level1_Load(object sender, EventArgs e)
        {
            // Each array will be size 16 to play through to animate movement
            playerMovements = new Image[16];
            enemyMovements = new Image[16];

            // Initializing the images necessary for the graphics
            grass = Image.FromFile(Application.StartupPath + @"\grass.jpg", true); // The grass image is for the background during the game
            fences = Image.FromFile(Application.StartupPath + @"\fences.png", true); // This image is for the fences which will act as the walls
            farmer = Image.FromFile(Application.StartupPath + @"\farmer_idle_down.png", true); // The default stance of the farmer
            child1 = Image.FromFile(Application.StartupPath + @"\child_walk_down1.png", true); // The default stance of the child
            child2 = Image.FromFile(Application.StartupPath + @"\child_walk_down1.png", true); // The default stance of the child
            haybale = Image.FromFile(Application.StartupPath + @"\haybale.png", true); // The image of the haybale (barrier)
            corn = Image.FromFile(Application.StartupPath + @"\corn.png", true); // The image of a crop (corn)
            potato = Image.FromFile(Application.StartupPath + @"\potato.png", true); // The image of a crop (potato)
             
            crops = new List<Rectangle>(); // initializing a new list for crops

            // Changes the size of the screen and background
            this.Width = 770;
            this.Height = 1000;
            this.MaximumSize = new Size(800, 1000);
            this.CenterToScreen();
            this.BackgroundImage = grass; // Changes the background to grass instead of the farm

            // Adding two versions of pixelated fonts into a fontCollection
            fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(Application.StartupPath + @"\pixelated.ttf");
            fontCollection.AddFontFile(Application.StartupPath + @"\pixelated1.ttf");

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
            
            // Setting up the input from the keys
            this.KeyUp += Level1_KeyUp;
            this.KeyDown += Level1_KeyDown;

            // Setting up the timer for the player's movements and actions
            playerTimer = new Timer();
            playerTimer.Interval = 1000 / 20;
            playerTimer.Tick += PlayerTimer_Tick;
            playerTimer.Start();

            // Setting up the timer for the enemy1's movements
            enemy1Timer = new Timer();
            enemy1Timer.Interval = 1000 / 10;
            enemy1Timer.Tick += Enemy1Timer_Tick;

            // Setting up the timer for the enemy2's movements
            enemy2Timer = new Timer();
            enemy2Timer.Interval = 1000 / 10;
            enemy2Timer.Tick += Enemy2Timer_Tick;

            // Setting up a timer to countdown the time remaining
            clockTimer = new Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += ClockTimer_Tick;

            // Setting up the sound player for the walk sound and collection sound
            walkSound = new SoundPlayer();
            walkSound.SoundLocation = Application.StartupPath + @"\walk.wav";
            collectSound = new SoundPlayer();
            collectSound.SoundLocation = Application.StartupPath + @"\collect.wav";

            // This audio player will start the background music and loops it
            backgroundMusic = new AudioFilePlayer();
            if (backgroundMusic.setAudioFile(Application.StartupPath + @"\background_music.mp3"))
            {
                backgroundMusic.playLooping();
            }

            // Initializing the barrier array which will be used to create barriers on the map, this way, it can be used globally
            wall = new Rectangle[15 * 15]; // the maximum possible amount of 1s is 15 * 15   

            // Creating a new label to display the objective of the game and some backstory
            storyLabel = new Label();
            storyLabel.Width = 500;
            storyLabel.Height = 500;
            storyLabel.TextAlign = ContentAlignment.MiddleCenter;
            storyLabel.Font = new Font(fontCollection.Families[1], 15);
            storyLabel.Text = "Objective:\nDuring the summer seasons, the pesky kids are always messing with Earl (the farmer) during their time off. Help Earl collect all his crops before nighttime and before the kids get him!\n\nUse the WASD or arrow keys to control Earl and use the SPACEBAR to place a haybale to stop the enemy!";
            storyLabel.Top = (this.Top);
            storyLabel.Left = (this.Width / 2) - (storyLabel.Width / 2);

            // Setting up a button to click once the player has read the objective and is ready to start the game
            storyButton = new Button();
            storyButton.Width = 100;
            storyButton.Height = 100;
            storyButton.Font = new Font(fontCollection.Families[1], 15);
            storyButton.Text = "OK!";
            storyButton.Top = storyLabel.Bottom + 25;
            storyButton.Left = (this.Width / 2) - (storyButton.Width / 2);

            // Once the intro button and text is set up, add it to the screen  
            this.Controls.Add(storyLabel);
            this.Controls.Add(storyButton);

            storyButton.Click += StoryButton_Click; // A new method for when the user has read the objective and is ready to start the game
        }

        // When the player is ready to start the game, loading up the screen
        private void StoryButton_Click(object sender, EventArgs e)
        {
            // Removing the text and button from the previous screen
            this.Controls.Remove(storyLabel);
            this.Controls.Remove(storyButton);

            // Adding the enemies and player into their starting positions
            playerRect = new Rectangle(350, 350, 40, 50);
            enemy1Rect = new Rectangle(655, 50, 40, 50);
            enemy2Rect = new Rectangle(505, 200, 40, 50);

            // The starting position of the player is facing down
            faceDown = true;
            faceUp = false;
            faceLeft = false;
            faceRight = false;

            // Starting off in batch 1
            inBatch1 = true;
            inBatch2 = false;

            // adding a label at the bottom of the screen to act as a scoreboard
            menuLabel = new Label();
            menuLabel.Top = 750;
            menuLabel.Left = 0;
            menuLabel.Width = this.Width/2;
            menuLabel.Height = 300;
            menuLabel.BackColor = Color.LightYellow;
            menuLabel.Font = new Font(fontCollection.Families[1], 25);
            menuLabel.Text = ("\nBatch 1: " + batch1Counter + "/24\n" + "Batch 2: " + batch2Counter + "/21");

            // adding a label at the bottom of the screen to be a timer
            clockLabel = new Label();
            clockLabel.Top = 750;
            clockLabel.Left = this.Width/2;
            clockLabel.Width = this.Width/2;
            clockLabel.Height = 300;
            clockLabel.BackColor = Color.LightYellow;
            clockLabel.Font = new Font(fontCollection.Families[1], 25);
            clockLabel.Text = "\nTime: " + time.ToString();

            // Add the two new labels onto the screen
            this.Controls.Add(menuLabel);
            this.Controls.Add(clockLabel);

            DrawWalls(); // drawing the walls/barriers for whatever level the user is on

            // Starting the timers to allow the player and enemies to start moving, as well as the countdown
            playerTimer.Start();
            enemy1Timer.Start();
            enemy2Timer.Start();
            clockTimer.Start();

            // Setting up the paint function to display the graphics
            this.DoubleBuffered = true;
            this.Paint += Level1_Paint;
        }

        // If any action key is pressed
        private void Level1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W) // if either up arrow or W is pressed
            {
                // set other positions to false
                faceDown = false;
                faceLeft = false;
                faceRight = false;

                faceUp = true; // facing up is true
                dx = 0;
                dy = -10; // move the player 10 pixels upward 
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S) // if either down arrow or S is pressed
            {
                // set other positions to false
                faceUp = false;
                faceLeft = false;
                faceRight = false;

                faceDown = true; // facing down is true
                dx = 0;
                dy = 10; // move the player 10 pixels downward 
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) // if either right arrow or D is pressed
            {
                // set other positions to false
                faceDown = false;
                faceLeft = false;
                faceUp = false;

                faceRight = true; // facing right is true
                dy = 0;
                dx = 10; // move the player 10 pixels rightward 
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) // if either left arrow or A is pressed
            {
                // set other positions to false
                faceDown = false;
                faceUp = false;
                faceRight = false;

                faceLeft = true; // facing left is true
                dy = 0;
                dx = -10; // move the player 10 pixels leftward 
            }
            if (e.KeyCode == Keys.Space)
            {
                // make sure that the enemies are moving
                enemy1Timer.Start();
                enemy2Timer.Start();
                placeBarrier = true; // place barrier is true
            } 
        }

        // A movement key is let go of
        private void Level1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.W || e.KeyCode == Keys.S) // if any of the vertical movement keys are let go of
            {
                dy = 0; // stop the vertical movement
                if (faceUp) // if the player is facing up, change the farmer's image to idle positon, facing up
                {
                    farmer = Image.FromFile(Application.StartupPath + @"\farmer_idle_up.png", true);
                }
                if (faceDown) // if the player is facing down, change the farmer's image to idle positon, facing down
                {
                    farmer = Image.FromFile(Application.StartupPath + @"\farmer_idle_down.png", true);
                }
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.A || e.KeyCode == Keys.D) // if any of the horizontal movement keys are let go of
            {
                dx = 0; // stop the horizontal movement
                if (faceLeft) // if the player is facing left, change the farmer's image to idle positon, facing left
                {
                    farmer = Image.FromFile(Application.StartupPath + @"\farmer_idle_left.png", true);
                }
                if (faceRight) // if the player is facing right, change the farmer's image to idle positon, facing right
                {
                    farmer = Image.FromFile(Application.StartupPath + @"\farmer_idle_right.png", true);
                }  
            }
        }

        // This timer does all the features for the player: movement, collision, placing barriers
        private void PlayerTimer_Tick(object sender, EventArgs e)
        {
            if (batch1Counter >= 24 && inBatch1) // if the player has collected all 24 crops in batch 1
            {
                crops.Clear(); // clear the crops list
                inBatch1 = false; // no longer in batch 1
                inBatch2 = true; // prepare batch 2
                DrawWalls(); // refresh the screen by drawing batch 2
            }
            if (batch2Counter >= 21) // if the player has collected all the crops in batch 2
            {
                crops.Clear(); // clear the crops list
                // stop all timers
                enemy1Timer.Stop();
                enemy2Timer.Stop();
                playerTimer.Stop();
                clockTimer.Stop();

                // initialize and prepare a label to indicate to the player they have completed the level
                nextLevelLabel = new Label();
                nextLevelLabel.Width = 325;
                nextLevelLabel.Height = 200;
                nextLevelLabel.Top = (this.Top);
                nextLevelLabel.Left = (this.Width / 2) - (nextLevelLabel.Width / 2);
                nextLevelLabel.TextAlign = ContentAlignment.MiddleCenter;
                nextLevelLabel.BackColor = Color.LightYellow;
                nextLevelLabel.Font = new Font(fontCollection.Families[1], 25);
                nextLevelLabel.Text = ("Level 1 Complete!");

                // Under the label, add a button to proceed to the next level
                nextLevelButton = new Button();
                nextLevelButton.Width = 200;
                nextLevelButton.Height = 100;
                nextLevelButton.Top = nextLevelLabel.Bottom + 25;
                nextLevelButton.Left = (this.Width / 2) - (nextLevelButton.Width / 2);
                nextLevelButton.BackColor = Color.LightYellow;
                nextLevelButton.Font = new Font(fontCollection.Families[1], 25);
                nextLevelButton.Text = ("NEXT LEVEL");

                // Add the next level label and button
                this.Controls.Add(nextLevelLabel);
                this.Controls.Add(nextLevelButton);

                // this event will be entered when the next level button is clicked
                nextLevelButton.Click += NextLevelButton_Click;              
            }
            if (playerRect.Bottom < (this.ClientSize.Height - 20) && dy > 0 && IsWalkable()) // Allows the player to move downwards
            {
                AnimatePlayerMovement(0, 3); // Animates the player moving down
                playerRect.Y += dy; // Move the player down based on the value of the dy integer
            }
            if (playerRect.Left > 0 && dx < 0 && IsWalkable()) // Allows the player to move leftwards
            {
                AnimatePlayerMovement(4, 7); // Animates the player moving left
                playerRect.X += dx; // Move the player left based on the value of the dx integer 
            }
            if (playerRect.Right < (this.ClientSize.Width - 20) && dx > 0 && IsWalkable()) // Allows the player to move rightwards
            {
                AnimatePlayerMovement(8, 11); // Animates the player moving right
                playerRect.X += dx; // Move the player right based on the value of the dx integer
            }
            if (playerRect.Top > 0 && dy < 0 && IsWalkable()) // Allows the player to move upwards
            {
                AnimatePlayerMovement(12, 15); // Animates the player moving up
                playerRect.Y += dy; // Move the player up based on the value of the dy integer
            }
            if (playerRect.IntersectsWith(enemy1Rect) || playerRect.IntersectsWith(enemy2Rect)) // if the player intersects with either enemy, load the game over screen
            {
                GameOverLoad(new GameOver());
            }
            for (int i = 0; i < crops.Count; i++) // iterating through the crops list
            {
                if (inBatch1) // if the player is in batch 1
                { 
                    if (playerRect.IntersectsWith(crops[i])) // if the player intersects with a crop while in batch 1
                    {
                        collectSound.Play(); // play a sound to indicate they have collected the crop
                        crops.Remove(crops[i]); // remove the crop from the list
                        batch1Counter++; // add one to the batch 1 counter
                        menuLabel.Text = ("\nBatch 1: " + batch1Counter + "/24\n" + "Batch 2: " + batch2Counter + "/21"); // update the scoreboard/menu with the new batch 1 counter value
                    }
                }
                if (inBatch2) // if the player is in batch 2
                {
                    if (playerRect.IntersectsWith(crops[i])) // if the player intersects with a crop while in batch 2
                    {
                        collectSound.Play(); // play a sound to indicate they have collected the crop
                        crops.Remove(crops[i]); // remove the crop from the list
                        batch2Counter++; // add one to the batch 2 counter
                        menuLabel.Text = ("\nBatch 1: " + batch1Counter + "/24\n" + "Batch 2: " + batch2Counter + "/21"); // update the scoreboard/menu with the new batch 2 counter value
                    }
                }
            }           
            if (faceRight && placeBarrier) // if the player is facing right and places a barrier
            {
                barrierRect = new Rectangle(playerRect.X + 50, playerRect.Y, 50, 50); // add the barrier 50 pixels to the right of the player
                placeBarrier = false; // the player is no longer trying to place a barrier
            }
            else if (faceLeft && placeBarrier) // if the player is facing left and places a barrier
            {
                barrierRect = new Rectangle(playerRect.X - 50, playerRect.Y, 50, 50); // add the barrier 50 pixels to the left of the player
                placeBarrier = false; // the player is no longer trying to place a barrier
            }
            else if (faceUp && placeBarrier) // if the player is facing up and places a barrier 
            {
                barrierRect = new Rectangle(playerRect.X, playerRect.Y - 50, 50, 50); // add the barrier 50 pixels above the player
                placeBarrier = false; // the player is no longer trying to place a barrier
            }
            else if (faceDown && placeBarrier)
            {
                barrierRect = new Rectangle(playerRect.X, playerRect.Y + 50, 50, 50); // add the barrier 50 pixels below the player
                placeBarrier = false; // the player is no longer trying to place a barrier
            }
            else // otherwise, the player does not meet necessary requirements to place a barrier
            {
                placeBarrier = false;
            }

            if (playerRect.IntersectsWith(playerRect)) // stops the player when they intersect with the barrier
            {
                IsWalkable();
            }

            this.Invalidate(); // refreshes the screen
        }

        // If the player clicks the "next level" button, load level 2
        private void NextLevelButton_Click(object sender, EventArgs e)
        {
            Level2Load(new Level2());
        }

        // This is used to move enemy 1 continuously in a set path
        private void Enemy1Timer_Tick(object sender, EventArgs e)
        {
            if (enemy1Rect.X >= 655 && enemy1Rect.Y <= 650) // these are the coordinates in which the enemy will be moving downward
            {
                AnimateEnemy1Movement(0, 3); // animates moving down
                enemy1Rect.Y += 10; // moves the enemy 10 pixels down
            }
            if (enemy1Rect.X <= 655 && enemy1Rect.Y >= 650) // these are the coordinates in which the enemy will be moving leftward
            { 
                AnimateEnemy1Movement(4, 7); // animates moving left
                enemy1Rect.X -= 10; // moves the enemy 10 pixels left
            }
            if (enemy1Rect.X <= 55 && enemy1Rect.Y <= 650) // these are the coordinates in which the enemy will be moving upward
            {
                AnimateEnemy1Movement(12, 15); // animates moving up
                enemy1Rect.Y -= 10; // moves the enemy 10 pixels up
            }
            if (enemy1Rect.X >= 55 && enemy1Rect.Y <= 50) // these are the coordinates in which the enemy will be moving rightward
            {
                AnimateEnemy1Movement(8, 11); // animates moving right
                enemy1Rect.X += 10; // moves the enemy 10 pixels right
            }
            if (enemy1Rect.IntersectsWith(playerRect)) // if the enemy intersects with the player, load the game over screen
            {
                GameOverLoad(new GameOver());
            }
            if (enemy1Rect.IntersectsWith(barrierRect)) // stop the enemy's timer and movement if it intersects with a barrier placed by the player
            {
                enemy1Timer.Stop();
            }
            this.Invalidate(); // refreshes the screen
        }

        // This is used to move enemy 2 continuously in a set path
        private void Enemy2Timer_Tick(object sender, EventArgs e)
        {
            if (enemy2Rect.X >= 505 && enemy2Rect.Y <= 500) // these are the coordinates in which the enemy will be moving downward
            {
                AnimateEnemy2Movement(0, 3); // animates moving down
                enemy2Rect.Y += 10; // moves the enemy 10 pixels down
            }
            if (enemy2Rect.X <= 505 && enemy2Rect.Y >= 500) // these are the coordinates in which the enemy will be moving upward 
            {
                AnimateEnemy2Movement(4, 7); // animates moving left
                enemy2Rect.X -= 10; // moves the enemy 10 pixels left 
            }
            if (enemy2Rect.X <= 205 && enemy2Rect.Y <= 500) // these are the coordinates in which the enemy will be moving upward 
            {
                AnimateEnemy2Movement(12, 15); // animates moving up
                enemy2Rect.Y -= 10; // moves the enemy 10 pixels up
            }
            if (enemy2Rect.X >= 205 && enemy2Rect.Y <= 200) // these are the coordinates in which the enemy will be moving rightward
            {
                AnimateEnemy2Movement(8, 11); // animates moving right
                enemy2Rect.X += 10; // moves the enemy 10 pixels right
            }
            if (enemy2Rect.IntersectsWith(playerRect)) // if the enemy intersects with the player, load the game over screen
            {
                GameOverLoad(new GameOver());
            }
            if (enemy2Rect.IntersectsWith(barrierRect)) // stop the enemy's timer and movement if it intersects with a barrier placed by the player
            {
                enemy2Timer.Stop();
            }
            this.Invalidate(); // refreshes the screen
        }

        // This will update the clock at the bottom of the screen every second
        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            time--; // subtract one from time every interval (1 second)
            clockLabel.Text = "\nTime: " + time.ToString(); // update the text on the clock label
            if (time <= 0) // once the time reaches 0 or lower, show the game over screen
            { 
                GameOverLoad(new GameOver());
            }
        }

        // Plays through an array of images to animate the player's movements
        private void AnimatePlayerMovement(int start, int end)
        {
            playerSteps++; // add one to player's steps to iterate through the desired numbers
            temp++; // temporary variable to track when the player has cycled through a full animation (4 images)

            if (playerSteps > end || playerSteps < start) // if the steps value is not within the range of the parameters
            {
                playerSteps = start; // force steps to go to the start
            }
            if (temp == 4) // if the temp variable is equal to 4, the player has played through 4 images
            {
                temp = 0; // reverse the temp back to 0
                walkSound.Play(); // play the walk sound every time the player has cycled through a full animation
            }

            farmer = playerMovements[playerSteps]; // changing the image of the farmer based on the index of the steps
        }

        // Plays through an array of images to animate the enemy's movements
        private void AnimateEnemy1Movement(int start, int end)
        {
            enemy1Steps++; // add one to enemy steps to iterate through the desired numbers

            if (enemy1Steps > end || enemy1Steps < start) // if the steps value is not within the range of the parameters
            {
                enemy1Steps = start; // force steps to go to the start
            }

            child1 = enemyMovements[enemy1Steps]; // changing the image of child1 based on the index of the steps
        }

        // Plays through an array of images to animate the enemy's movements
        private void AnimateEnemy2Movement(int start, int end)
        {
            enemy2Steps++; // add one to enemy steps to iterate through the desired numbers

            if (enemy2Steps > end || enemy2Steps < start) // if the steps value is not within the range of the parameters
            {
                enemy2Steps = start; // force steps to go to the start
            }

            child2 = enemyMovements[enemy2Steps]; // changing the image of child2 based on the index of the steps
        }

        // The paint function to display all the graphics and images
        private void Level1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(farmer, playerRect); // painting the farmer (player)
            e.Graphics.DrawImage(child1, enemy1Rect); // painting the first child (child1)
            e.Graphics.DrawImage(child2, enemy2Rect); // painting the second child (child2)
            e.Graphics.DrawImage(haybale, barrierRect); // painting the barrier

            // this for loop will iterate through the barrier array to print all of the walls necessary
            for (int i = 0; i < wall.Length; i++)
            {
                e.Graphics.DrawImage(fences, wall[i]); // painting the walls based on the index of the for loop          
            }

            // this for loop will iterate through the crops list and print all of the crops necessary
            for (int i = 0; i < crops.Count; i++)
            {
                if (inBatch1) // if the player is still collecting from batch 1
                {
                    e.Graphics.DrawImage(potato, crops[i]); // print every crop as a potato
                }
                if (inBatch2) // if the player is collecting from batch 2
                {
                    e.Graphics.DrawImage(corn, crops[i]); // print every crop as corn
                }
            }
        }

        // will scan through the rows/columns to print out the walls, crops, and walkable space
        private void DrawWalls()
        {
            row = new string[15]; // setting up an array of 15 rows
             
            if (inBatch1) // if the player is still collecting the first batch of crops
            {
                // 0s represent walkable space, 1s represent a wall, 2s represent a collectable crop, each taking up their own block of 50 x 50
                // the following is how level 1, batch 1 will look
                row[0] =  "111111111111111";
                row[1] =  "122000000000221";
                row[2] =  "120000000000021";
                row[3] =  "100111000111001";
                row[4] =  "100122000221001";
                row[5] =  "100120000021001";
                row[6] =  "100000000000001";
                row[7] =  "100000000000001";
                row[8] =  "100000000000001";
                row[9] =  "100120000021001";
                row[10] = "100122000221001";
                row[11] = "100111000111001";
                row[12] = "120000000000021";
                row[13] = "122000000000221";
                row[14] = "111111111111111";

                wallCounter = 0; // Temporary counter to count how many walls are needed; 

                for (int i = 0; i < row.Length; i++) // this for loop will represent how many rows there are
                {
                    for (int j = 0; j < row.Length; j++) // iteratng through every column
                    {
                        if (row[i].Substring(j, 1) == "1") // if any of the row's characters is a "1", we will print a fence
                        {
                            wall[wallCounter] = new Rectangle((j * 50) + 5, (i * 50), 50, 50); // adding a new rectangle to the wall array based on where the 1 is placed
                            wallCounter++; // temporary counter to store however many 1s (walls) there are
                        }
                    }
                }
                for (int i = 0; i < row.Length; i++) // this for loop will represent how many rows there are
                {
                    for (int j = 0; j < row.Length; j++) // iterating through every column
                    {
                        if (row[i].Substring(j, 1) == "2") // if any of the row's characters is a "2", we will print a crop in that location
                        {
                            crops.Add(new Rectangle((j * 50) + 10, (i * 50) + 10, 30, 30)); // adding a new rectangle to the crop list based on where the 2 is placed
                        }
                    }
                }
            }
            if (inBatch2)
            {
                // 0s represent walkable space, 1s represent a wall, 2s represent a collectable crop, each taking up their own block of 50 x 50
                // the following is how level 1, batch 2 will look
                row[0] =  "111111111111111";
                row[1] =  "100000222000001";
                row[2] =  "100000000000001";
                row[3] =  "100111000111001";
                row[4] =  "100100000001001";
                row[5] =  "100100000001001";
                row[6] =  "120000222000021";
                row[7] =  "120000222000021";
                row[8] =  "120000222000021";
                row[9] =  "100100000001001";
                row[10] = "100100000001001";
                row[11] = "100111000111001";
                row[12] = "100000000000001";
                row[13] = "100000222000001";
                row[14] = "111111111111111";

                for (int i = 0; i < row.Length; i++) // this for loop will represent how many rows there are
                {
                    for (int j = 0; j < row.Length; j++) // iteratng through every column
                    {
                        if (row[i].Substring(j, 1) == "1") // if any of the row's characters is a "1", we will print a fence
                        {
                            wall[wallCounter] = new Rectangle((j * 50) + 5, (i * 50), 50, 50); // adding a new rectangle to the wall array based on where the 1 is placed
                        }
                    }
                }
                for (int i = 0; i < row.Length; i++) // this for loop will represent how many rows there are
                {
                    for (int j = 0; j < row.Length; j++) // iterating through every column
                    {
                        if (row[i].Substring(j, 1) == "2") // if any of the row's characters is a "2", we will print a crop in that location
                        {
                            crops.Add(new Rectangle((j * 50) + 10, (i * 50) + 10, 30, 30)); // adding a new rectangle to the crop list based on where the 2 is placed
                        }
                    }
                }
            }               
        }

        // This function will check if the area the player/enemy is approaching is walkable
        private bool IsWalkable()
        {
            for (int i = 0; i < wall.Length; i++) // iterating through the wall array
            {
                if (playerRect.IntersectsWith(wall[i]) || playerRect.IntersectsWith(barrierRect)) // if the player intersects with either a wall or barrier
                {
                    if (faceLeft || faceRight) // if the player if facing left or right, move them in the opposite direction x-coordinate wise
                    {
                        playerRect.X += (dx * -1);
                    }
                    if (faceUp || faceDown) // if the player if facing up or down, move them in the opposite direction y-coordinate wise
                    {
                        playerRect.Y += (dy * -1);
                    }
                    return false; // this means it is not walkable
                }
            }
            return true; // the player can walk
        }

        // Loads the next level (level 2)
        private void Level2Load(Form level2)
        {
            backgroundMusic.stop(); // stops the music
            level2.FormClosed += Level2_FormClosed; // if the form is closed, the application will also close
            this.Hide(); // hide the current form
            level2.Show(); // show the form from the parameters (level 2)
        }

        // Loads the game over screen
        private void GameOverLoad(Form gameOver)
        {
            // stop all the music and timers
            backgroundMusic.stop();
            enemy1Timer.Stop();
            enemy2Timer.Stop();
            playerTimer.Stop();
            clockTimer.Stop();
            gameOver.FormClosed += GameOver_FormClosed; // if the form is closed, the application will also close
            this.Hide(); // hide the current form
            gameOver.Show(); // show the form from the parameters (game over screen)
        }

        // If the level 2 form is closed, exit the application
        private void Level2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // If the game over form is closed, exit the application
        private void GameOver_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
