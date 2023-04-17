using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Resources;

namespace AstroMathClient
{

    public partial class FRMMain : Form
    {

        ChannelFactory<IAstroContract> pipeFactory = new ChannelFactory<IAstroContract>(new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/PipeAstroMath"));
        IAstroContract pipeProxy;

        public FRMMain()
        {
            InitializeComponent();

        }
        

        private void UpdateStatusStrip(string text)
        {
            statusStrip.Items.Clear();
            statusStrip.Items.Add(text);

        }

        private void numserverErrorMessage(string type)
        {
            string currentlanguage = Thread.CurrentThread.CurrentUICulture.Name;
            if (currentlanguage == "fr-FR")
            {
                MessageBox.Show("Le numéro que vous avez entré est peut-être invalide ou vous n'êtes pas connecté au serveur, veuillez réessayer.", type, MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatusStrip("Veuillez entrer un nombre valide pour l'entrée Star Velocity ou vous connecter au serveur.");
            }
            else if (currentlanguage == "de-DE")
            {
                MessageBox.Show("Die eingegebene Nummer ist möglicherweise ungültig oder Sie sind nicht mit dem Server verbunden. Bitte versuchen Sie es erneut.", type, MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatusStrip("Bitte geben Sie eine gültige Zahl für die Star Velocity-Eingabe ein oder verbinden Sie sich mit dem Server.");
            }
            else
            {
                MessageBox.Show("The number you have entered may be invalid or you aren't connected to the server, please try again.", type, MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatusStrip("Please enter in a valid number for Star Velocity input or connect to the server.");
            }
        }

        private void btnStarVelocity_Click(object sender, EventArgs e)
        {
            string currentlanguage = Thread.CurrentThread.CurrentUICulture.Name;
            try
            {
                double oWL = double.Parse(tbxObservedWavelength.Text);
                double rWL = double.Parse(tbxRestWavelength.Text);
                double result = pipeProxy.StarVelocity(oWL, rWL);

                addData(result.ToString() + " m/s", 0);
                if (currentlanguage == "fr-FR")
                {
                    UpdateStatusStrip("Vitesse des étoiles: " + result.ToString() + " ajouté avec succès");
                } else if (currentlanguage == "de-DE")
                {
                    UpdateStatusStrip("Sterngeschwindigkeit: " + result.ToString() + " erfolgreich hinzugefügt");
                }
                else
                {
                    UpdateStatusStrip("Star Velocity: " + result.ToString() + " added in successfully");
                }
                
            }
            catch (Exception)
            {
                if (currentlanguage == "fr-FR")
                {
                    numserverErrorMessage("Vitesse des étoiles");
                } else if (currentlanguage == "de-DE")
                {
                    numserverErrorMessage("Sterngeschwindigkeit");
                } else
                {
                    numserverErrorMessage("Star Velocity");
                }
            }

        }

        private void FRMMain_Load(object sender, EventArgs e)
        {
            pipeProxy = pipeFactory.CreateChannel();
        }

        private int getRowandCell(int column)
        {
            int rows = dgvMain.Rows.Count;
            for (int i = 0; i < rows; i++)
            {

                object value = dgvMain.Rows[i].Cells[column].Value;

                if (value == null)
                {
                    return i;
                }
            }
            return rows;
        }

        private void addData(string data, int column)
        {
            int placement = getRowandCell(column);
            int rows = dgvMain.Rows.Count;
            if (placement == rows)
            {
                dgvMain.Rows.Add();
                placement = getRowandCell(column);
            }
            dgvMain.Rows[placement].Cells[column].Value = data;
        }

        private void btnStarDistance_Click(object sender, EventArgs e)
        {
            string currentlanguage = Thread.CurrentThread.CurrentUICulture.Name;
            try
            {
                double angle = double.Parse(tbxAngle.Text);
                double result = pipeProxy.StarDistance(angle);

                addData(result.ToString() + " parsec", 1);

                if (currentlanguage == "fr-FR")
                {
                    UpdateStatusStrip("Distance étoile: " + result.ToString() + " ajouté avec succès");
                } else if (currentlanguage == "de-DE")
                {
                    UpdateStatusStrip("Sternenentfernung: " + result.ToString() + " erfolgreich hinzugefügt");
                } else
                {
                    UpdateStatusStrip("Star Distance: " + result.ToString() + " added in successfully");
                }
                
            }
            catch (Exception)
            {
                if (currentlanguage == "fr-FR")
                {
                    numserverErrorMessage("Distance étoile");
                }
                else if (currentlanguage == "de-DE")
                {
                    numserverErrorMessage("Sternenentfernung");
                }
                else
                {
                    numserverErrorMessage("Star Distance");
                }
            }
  
        }

        private void btnCelciusKelvin_Click(object sender, EventArgs e)
        {
            string currentlanguage = Thread.CurrentThread.CurrentUICulture.Name;
            try
            {
                double celcius = double.Parse(tbxCelcius.Text);
                double result = pipeProxy.Kelvin(celcius);
                addData(result.ToString() + "K", 2);
                if (currentlanguage == "fr-FR")
                {
                    UpdateStatusStrip("Ceclius en Kelvin: " + result.ToString() + " ajouté avec succès");
                } else if (currentlanguage == "de-DE")
                {
                    UpdateStatusStrip("Ceclius zu Kelvin: " + result.ToString() + " erfolgreich hinzugefügt");
                } else
                {
                    UpdateStatusStrip("Ceclius to Kelvin: " + result.ToString() + " added in successfully");
                }
                
            }
            catch (Exception)
            {
                if (currentlanguage == "fr-FR")
                {
                    numserverErrorMessage("Ceclius en Kelvin");
                }
                else if (currentlanguage == "de-DE")
                {
                    numserverErrorMessage("Ceclius zu Kelvin");
                }
                else
                {
                    numserverErrorMessage("Ceclius to Kelvin");
                }
            }

        }

        private void btnEventHorizon_Click(object sender, EventArgs e)
        {
            string currentlanguage = Thread.CurrentThread.CurrentUICulture.Name;
            try
            {
                double mass = double.Parse(tbxBHMass.Text);
                double mass2 = double.Parse(tbxBHMass2.Text);
                double power = Decimal.ToDouble(nudEHPower.Value);
                double num = mass * Math.Pow(mass2, 36);
                double result = pipeProxy.EventHorizon(num);
                string scn = result.ToString("G2", CultureInfo.CurrentUICulture);
                addData(scn + " m", 3);
                if (currentlanguage == "fr-FR")
                {
                    UpdateStatusStrip("Horizon des événements: " + result.ToString() + " ajouté avec succès");
                } else if (currentlanguage == "de-DE")
                {
                    UpdateStatusStrip("Ereignishorizont: " + result.ToString() + " erfolgreich hinzugefügt");
                } else
                {
                    UpdateStatusStrip("Event Horizon: " + result.ToString() + " added in successfully");
                }
                
            }
            catch (Exception)
            {
                if (currentlanguage == "fr-FR")
                {
                    numserverErrorMessage("Horizon des événements");
                }
                else if (currentlanguage == "de-DE")
                {
                    numserverErrorMessage("Ereignishorizont");
                }
                else
                {
                    numserverErrorMessage("Event Horizon");
                }
            }

        }

        private void ChangeTheme(char themeID)
        {
            Color Background = Color.WhiteSmoke;
            Color Foretext = Color.Black;
            Color textboxesBack = Color.White;
            Color otherText = Color.Black;
            Image Wallpaper = null;
            
            if (themeID == '1') // Light
            {
                Background = Color.WhiteSmoke;
                Foretext = Color.Black;
                textboxesBack = Color.White;
                otherText = Foretext;
                Wallpaper = null;
            }
            else if (themeID == '2') // Dark
            {
                Background = Color.FromArgb(35, 35, 35);
                Foretext = Color.WhiteSmoke;
                textboxesBack = Color.FromArgb(35, 35, 35);
                otherText = Foretext;
                Wallpaper = null;
            }
            else if (themeID == '4') // Night
            {
                Background = Color.FromArgb(0, 0, 0);
                Foretext = Color.White;
                textboxesBack = Color.FromArgb(0, 0, 0);
                otherText = Foretext;
                Wallpaper = null;
            }
            else if (themeID == '3') // Day
            {
                Background = Color.FromArgb(255, 255, 255);
                Foretext = Color.Black;
                textboxesBack = Color.FromArgb(255, 255, 255);
                otherText = Foretext;
                Wallpaper = null;
            }
            else if (themeID == '5') // Crimson
            {
                Background = Color.FromArgb(184, 15, 10);
                Foretext = Color.White;
                textboxesBack = Color.FromArgb(184, 15, 10);
                otherText = Foretext;
                Wallpaper = null;
            }
            else if (themeID == '6') // Purple
            {
                Background = Color.FromArgb(102, 51, 153);
                Foretext = Color.Yellow;
                textboxesBack = Color.FromArgb(102, 51, 153);
                otherText = Foretext;
                Wallpaper = null;
            }
            else if (themeID == '7') // King Crimson
            {
                Wallpaper = Properties.Resources.flat_750x1000_075_f;
                this.BackgroundImageLayout = ImageLayout.Tile;
            }
            else if (themeID == '8') // Purple Haze
            {
                Background = Color.FromArgb(102, 51, 153);
                Wallpaper = Properties.Resources.images;
                textboxesBack = Background;
                Foretext = Color.White;
                otherText = Foretext;
                this.BackgroundImageLayout = ImageLayout.Tile;
            }
            else if (themeID == '9')
            {
                Wallpaper = Properties.Resources._20390907;
                Background = Color.Transparent;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                Foretext = Color.White;
                otherText = Color.Black;
                textboxesBack = Color.Black;
            }


            if (Background != Color.Transparent)
            {
                this.BackColor = Background;
                dgvMain.BackgroundColor = Background;
            }
            else
            {
                dgvMain.BackgroundColor = Color.WhiteSmoke;
            }

            this.BackgroundImage = Wallpaper;
            statusStrip.ForeColor = otherText;

            foreach (Control control in this.Controls)
            {

                if (control.Tag == "labelTag")
                {
                    Debug.Print(control.Name);
                }

                if (control is TextBox || control is ComboBox || control is NumericUpDown)
                {
                    control.ForeColor = Foretext;
                    control.BackColor = textboxesBack;
                }

                if (control is GroupBox)
                {
                    control.ForeColor = Foretext;
                    control.BackColor = Background;
                }

                if (control is Button)
                {
                    if (control.Tag != "LanguageBtn")
                    {
                        control.BackColor = Background;
                        control.ForeColor = Foretext;
                    }
                    else
                    {
                        control.BackColor = Foretext;
                    }
                }

            }
        }

        private void cbxTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            char themeID = cbxTheme.Text[0];
            ChangeTheme(themeID);
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            char themeID = toolStripComboBox1.Text[0];
            ChangeTheme(themeID);
        }

        private void btnBackgroundColour_Click(object sender, EventArgs e)
        {
            ColorDialog colourDialog = new ColorDialog();
            if (colourDialog.ShowDialog() == DialogResult.OK)
            {
                this.BackgroundImage = null;
                this.BackColor = colourDialog.Color;
            }


        }

        private void MenuStyleBackground_Click(object sender, EventArgs e)
        {
            ColorDialog colourDialog = new ColorDialog();
            if (colourDialog.ShowDialog() == DialogResult.OK)
            {
                this.BackgroundImage = null;
                this.BackColor = colourDialog.Color;
            }
        }

        private void ChangeLanguage(string language)
        {
            if (language == "French")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");
            } else if (language == "English")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            } else if (language == "German")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            }
            
            Controls.Clear();
            InitializeComponent();
        }

        private void btnEnglish_Click(object sender, EventArgs e)
        {
            ChangeLanguage("English");
        }

        private void btnFrench_Click(object sender, EventArgs e)
        {
            ChangeLanguage("French");
        }

        private void btnGerman_Click(object sender, EventArgs e)
        {
            ChangeLanguage("German");
        }
    }
}
