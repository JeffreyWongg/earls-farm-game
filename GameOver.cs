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
    public partial class GameOver : Form
    {
        Image farm;
        Label titleLabel;
        Button replayButton, exitButton;
        AudioFilePlayer sadMusic;
        PrivateFontCollection fontCollection;

        public GameOver()
        {
            InitializeComponent();
        }

        private void GameOver_Load(object sender, EventArgs e)
        {
            // Initializing the starting screen size and formatting it
            this.MaximumSize = new Size(1440, 1000);
            this.Width = 1440;
            this.Height = 1000;
            this.Text = "Final Project";
            this.CenterToScreen();

            farm = Image.FromFile(Application.StartupPath + @"\farm_night.jpg", true); // The farm image is for the background in the starting screen

            // adding in a font collection for two different fonts
            fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(Application.StartupPath + @"\pixelated.ttf");
            fontCollection.AddFontFile(Application.StartupPath + @"\pixelated1.ttf");

            // Setting up the audio file player for the sad music
            sadMusic = new AudioFilePlayer();
            if (sadMusic.setAudioFile(Application.StartupPath + @"\sad_music.mp3"))
            {
                sadMusic.playLooping();
            }

            // Initializing the things necessary for the starting screen
            titleLabel = new Label();
            replayButton = new Button();
            exitButton = new Button();

            // Setting up the title for the game over screen
            titleLabel.Width = 800;
            titleLabel.Height = 300;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Font = new Font(fontCollection.Families[1], 50);
            titleLabel.Text = "Earl has gone bankrupt :(";
            titleLabel.Top = this.Top;
            titleLabel.Left = (this.Width / 2) - (titleLabel.Width / 2);

            // Setting up the replay button for the game over screen
            replayButton.Width = 300;
            replayButton.Height = 150;
            replayButton.Font = new Font(fontCollection.Families[1], 30);
            replayButton.BackColor = Color.Transparent;
            replayButton.Text = "PLAY AGAIN";
            replayButton.Top = (this.Top + 300);
            replayButton.Left = (this.Width / 2) - 400;

            // Setting up the exit button for the game over screen
            exitButton.Width = 300;
            exitButton.Height = 150;
            exitButton.Font = new Font(fontCollection.Families[1], 30);
            exitButton.BackColor = Color.Transparent;
            exitButton.Text = "EXIT";
            exitButton.Top = (this.Top + 300);
            exitButton.Left = replayButton.Left + 500;

            replayButton.Click += ReplayButton_Click; // A new event for when the start button on the starting screen is clicked
            exitButton.Click += ExitButton_Click; // A new event

            // Add the buttons and labels to the screen
            this.Controls.Add(replayButton);
            this.Controls.Add(exitButton);
            this.Controls.Add(titleLabel);
            this.BackgroundImage = farm; // set the background to the farm image
        }

        // If the replay button is clicked we will load up the starting screen form
        private void ReplayButton_Click(object sender, EventArgs e)
        {
            // Remove all labels/buttons and stop the music
            this.Controls.Remove(titleLabel);
            this.Controls.Remove(replayButton);
            this.Controls.Remove(exitButton);
            sadMusic.stop();

            StartingScreenLoad(new StartingScreen()); // load the starting screen
        }

        // If the exit button is clicked, close the application
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Loads the starting screen when replay button is clicked
        private void StartingScreenLoad(Form StartingScreen)
        {
            StartingScreen.FormClosed += StartingScreen_FormClosed; // new method for when the starting screen form is closed
            this.Hide(); // hide the current form
            StartingScreen.Show(); // show the starting screen
        }

        // If the form is closed, exit  the application
        private void StartingScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
