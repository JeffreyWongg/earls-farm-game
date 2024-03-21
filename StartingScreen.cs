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
using System.Drawing.Text;
using AudioPlayer;

namespace Final_Project___Jeffrey_Wong_ICS3U
{
    public partial class StartingScreen : Form
    {
        Image farm;
        Label title;
        Button startButton;
        AudioFilePlayer backgroundMusic;
        PrivateFontCollection fontCollection;

        public StartingScreen()
        {
            InitializeComponent();
        }

        private void StartingScreen_Load(object sender, EventArgs e)
        {
            // Initializing the starting screen size and formatting it
            this.MaximumSize = new Size(1440, 1000);
            this.Width = 1440;
            this.Height = 1000;
            this.Text = "Final Project";
            this.CenterToScreen();

            farm = Image.FromFile(Application.StartupPath + @"\farm_sunset.jpg", true); // The farm image is for the background in the starting screen

            fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(Application.StartupPath + @"\pixelated.ttf");
            fontCollection.AddFontFile(Application.StartupPath + @"\pixelated1.ttf");

            // Setting up the sound player for the background music
            backgroundMusic = new AudioFilePlayer();

            if (backgroundMusic.setAudioFile(Application.StartupPath + @"\background_music.mp3"))
            {
                backgroundMusic.playLooping();
            }

            // Initializing the things necessary for the starting screen
            title = new Label();
            startButton = new Button();

            // Setting up the title for the starting screen
            title.Width = 500;
            title.Height = 300;
            title.BackColor = Color.Transparent;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Font = new Font(fontCollection.Families[1], 50);
            title.Text = "Earl's Farm";
            title.Top = this.Top;
            title.Left = (this.Width / 2) - (title.Width / 2);

            // Setting up the button for the starting screen
            startButton.Width = 300;
            startButton.Height = 150;
            startButton.Font = new Font(fontCollection.Families[1], 30);
            startButton.BackColor = Color.Transparent;
            startButton.Text = "START";
            startButton.Top = (this.Top + 300);
            startButton.Left = (this.Width / 2) - (startButton.Width / 2);

            startButton.Click += StartButton_Click; // A new method for when the start button on the starting screen is clicked

            // Add the start button and title for the title screen
            this.Controls.Add(startButton);
            this.Controls.Add(title);
            this.BackgroundImage = farm;
        }

        // When the start button on the starting screen is clicked, the program will continue to this code
        private void StartButton_Click(object sender, EventArgs e)
        {
            // Removing the title and button from the starting screen
            this.Controls.Remove(title);
            this.Controls.Remove(startButton);
            backgroundMusic.stop();

            Level1Load(new Level1()); // this will load level 1 when the start button is clicked
        }

        // Loading up level 1
        private void Level1Load(Form level1)
        {
            level1.FormClosed += Level1_FormClosed; // event for when the level 1 form is closed
            this.Hide(); // close the current form
            level1.Show(); // show the level 1 form
        }

        // If the level 1 form is closed, exit the application
        private void Level1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
