using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        // Use Random obj to get random icons
        Random random = new Random();

        // List of Webdings fonts for icons
        List<string> icons = new List<string>()
        {
            "A", "A", "n", "n", ".", ".", "s", "s", "d", "d", "V", "V", "u", "u", "e", "e", "X", "X", "r", "r", "P", "P", "J", "J", "#", "#", "U", "U", "i", "i", "a", "a", "3", "3", "8", "8"
        };

        // variable for first player click, null if none made
        Label firstClicked = null;

        // variable for second player click, null if none made
        Label secondClicked = null;

        // Play sound on success/fail/win
        private SoundPlayer _soundPlayerWin;
        private SoundPlayer _soundPlayerSuccess;
        private SoundPlayer _soundPlayerFail;

        private void AssignIconsToSquares()
        {
            // Assign all icons to a random squares
            foreach (Control control in tableLayoutPanel1.Controls)
            {   
                // Assign control to label called iconLabel
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {   
                    // Choose random icon from icon list
                    int randomNumber = random.Next(icons.Count);

                    // Assign random icon to label text
                    iconLabel.Text = icons[randomNumber];

                    // Hide icons 
                    iconLabel.ForeColor = iconLabel.BackColor;

                    // Remove the chosen icon from the list
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        public Form1()
        {   
            // Open program
            InitializeComponent();
            // Assign icons 
            AssignIconsToSquares();
            // Sound players
            _soundPlayerWin = new SoundPlayer("tada.wav");
            _soundPlayerSuccess = new SoundPlayer("start.wav");
            _soundPlayerFail = new SoundPlayer("stop.wav");
        }

        private void label_Click(object sender, EventArgs e)
        {
            // Ignore clicks if timer is on
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;
            
            if (clickedLabel != null)
            {
                // If icon is already visible; ignore click
                if (clickedLabel.ForeColor == Color.White)
                    return;

                // If firstClicked is null; add firstClicked label and reveal icon 
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.White;
                    //Skip rest of method
                    return;
                }

                // If secondClicked is null; add secondClicked label and reveal icon
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.White;

                // Check if last remaining icon
                CheckForWinner();

                // If icons matches, reset first and second click, but keep visible
                if (firstClicked.Text == secondClicked.Text)
                {
                    _soundPlayerSuccess.Play();
                    firstClicked = null;
                    secondClicked = null;
                    return;
                } else
                {
                    _soundPlayerFail.Play();
                }

                // Two clicks have been made; start timer
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Stop timer
            timer1.Stop();

            // Hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Reset firstClicked and secondClicked
            firstClicked = null;
            secondClicked = null;
        }

        private void CheckForWinner()
        {
            // Check all labels to see if all are matched
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            // If loop didnt return, no unmatched icons are present; player won
            _soundPlayerWin.Play();
            MessageBox.Show("Tillykke - du fandt alle par!");
            Close();
        }
    }
}
