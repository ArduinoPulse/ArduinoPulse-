using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.IO.Ports;
using System.Threading;

namespace Pulse
{
    public partial class frmMain : Form
    {
        // enregistrements.csv -> LaDateDuTest;LheureDuTest;BPM;DureeDuTestEnSec (dd.MM.yyyy;hh:mm;00;00)

        // Variables pour le mouvement de la fenêtre
        int mov;
        int movX;
        int movY;

        // Variables global
        int tim3sec = 4;
        int x = 0;
        int z = 0;

        public frmMain()
        {
            InitializeComponent();
        }

        // Déplacement de la fenêtre 1
        private void flowLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        // Déplacement de la fenêtre 2
        private void flowLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        // Déplacement de la fenêtre 3
        private void flowLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Selectionne le menu 1
            SelectMenu(1);

            // Load settings
            tbxDuree.Text = getSettings("testTime");

            // Charger les enregistrements
            loadEnregistrements();

            // Changement d'affichages
            lblTmpRestant.Text = "Temps restant : " + tbxDuree.Text + " secondes";

            
            tryToConnection();
         
        }

        private void updateSettings(string sKey, string sValue)
        {
            try
            {
                // Variables
                string sSettingsPath = Properties.Settings.Default.settingsPath;
                string sSettings = File.ReadAllText(sSettingsPath);
                string[] aSettings = File.ReadAllText(sSettingsPath).Split('\n');
                string sOldSetting = "";
                string sNewSetting = "";

                // Traitement
                foreach (string sSetting in aSettings)
                {
                    // Lorsque il est sur la bonne ligne
                    if (sSetting.StartsWith(sKey))
                    {
                        sOldSetting = sSetting;
                        sNewSetting = sKey + "=\"" + sValue + "\"";
                    }
                }

                // Modification dans le fichier
                File.WriteAllText(sSettingsPath, sSettings.Replace(sOldSetting, sNewSetting));
            }
            catch
            {
                MessageBox.Show("Une erreur est survenue lors de la mise à jour d'un paramètre");
            }
        }

        private string getSettings(string sKey)
        {
            try
            {
                // Variables
                string sSettingsPath = Properties.Settings.Default.settingsPath;
                string[] aSettings = File.ReadAllText(sSettingsPath).Split('\n');
                string sData = "[SETTING_NOT_FOUND]";

                // Traitement
                foreach (string sSetting in aSettings)
                {
                    // Lorsque il est sur la bonne ligne
                    if (sSetting.StartsWith(sKey))
                    {
                        sData = sSetting.Replace(sKey + "=", "").Replace("\"", "").Replace("\r", "").Replace("\n", "");
                    }
                }

                // Retour
                return sData;
            }
            catch
            {
                MessageBox.Show("Une erreur est survenue lors de la recherche d'un paramètre");
                return "[SETTING_NOT_FOUND]";
            }
        }

        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRetrecir_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void SelectMenu(int iMenu)
        {
            // Variables
            FlowLayoutPanel panel = new FlowLayoutPanel();
            Label lbl = new Label();
            GroupBox menu = new GroupBox();
            int unselectedColor = 60;
            int selectedColor = 122;

            // Menus
            if (iMenu == 1) { panel = btnMenu1; lbl = lblMenu1; menu = menu1; }
            if (iMenu == 2) { panel = btnMenu2; lbl = lblMenu2; menu = menu2; }
            if (iMenu == 3) { panel = btnMenu3; lbl = lblMenu3; menu = menu3; }

            // Reset les couleurs
            lblMenu1.Font = new Font(lblMenu1.Font, FontStyle.Regular);
            lblMenu1.BackColor = Color.FromArgb(unselectedColor, unselectedColor, unselectedColor);
            lblMenu2.Font = new Font(lblMenu2.Font, FontStyle.Regular);
            lblMenu2.BackColor = Color.FromArgb(unselectedColor, unselectedColor, unselectedColor);
            lblMenu3.Font = new Font(lblMenu3.Font, FontStyle.Regular);
            lblMenu3.BackColor = Color.FromArgb(unselectedColor, unselectedColor, unselectedColor);
            btnMenu1.BackColor = Color.FromArgb(unselectedColor, unselectedColor, unselectedColor);
            menu1.Visible = false;
            btnMenu2.BackColor = Color.FromArgb(unselectedColor, unselectedColor, unselectedColor);
            menu2.Visible = false;
            btnMenu3.BackColor = Color.FromArgb(unselectedColor, unselectedColor, unselectedColor);
            menu3.Visible = false;

            // Mettre en couleur le menu sélectionné
            panel.BackColor = Color.FromArgb(selectedColor, selectedColor, selectedColor);
            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
            lbl.BackColor = Color.FromArgb(selectedColor, selectedColor, selectedColor);

            // Affiche le menu
            menu.Visible = true;
        }

        private void HoverMenu(int iMenu, bool isEntering) // True = Il entre, False = Il sort
        {
            // Variables
            FlowLayoutPanel panel = new FlowLayoutPanel();
            Label lbl = new Label();

            // Savoir quel menu est sélectionné actuellement
            int iMenuSelectionne = MenuSelectionne();
            if(iMenu == iMenuSelectionne) { return; }

            // Menus
            if (iMenu == 1) { panel = btnMenu1; lbl = lblMenu1; }
            if (iMenu == 2) { panel = btnMenu2; lbl = lblMenu2; }
            if (iMenu == 3) { panel = btnMenu3; lbl = lblMenu3; }

            // effet sur le bouton en hover
            if (isEntering)
            {
                panel.BackColor = Color.FromArgb(50, 50, 50);
                lbl.BackColor = Color.FromArgb(50, 50, 50);
            } 
            else
            {
                panel.BackColor = Color.FromArgb(60, 60, 60);
                lbl.BackColor = Color.FromArgb(60, 60, 60);
            }
        }

        private void btnMenu1_MouseClick(object sender, MouseEventArgs e)
        {
            SelectMenu(1);
        }

        private void btnMenu2_MouseClick(object sender, MouseEventArgs e)
        {
            SelectMenu(2);
        }

        private void btnMenu3_MouseClick(object sender, MouseEventArgs e)
        {
            SelectMenu(3);
        }
        private void lblMenu1_Click(object sender, EventArgs e)
        {
            SelectMenu(1);
        }

        private void lblMenu2_Click(object sender, EventArgs e)
        {
            SelectMenu(2);
        }

        private void lblMenu3_Click(object sender, EventArgs e)
        {
            SelectMenu(3);
        }

        private void tbxDuree_TextChanged(object sender, EventArgs e)
        {
            // Sauvegarder la nouvelle durée
            ChangeDuree();
        }

        private void tbxDuree_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private int MenuSelectionne()
        {
            int iSelectedMenu = 0;
            if (menu1.Visible == true) { iSelectedMenu = 1; }
            if (menu2.Visible == true) { iSelectedMenu = 2; }
            if (menu3.Visible == true) { iSelectedMenu = 3; }
            return iSelectedMenu;
        }

        private void btnMenu1_MouseEnter(object sender, EventArgs e)
        {
            HoverMenu(1, true);
        }

        private void btnMenu1_MouseLeave(object sender, EventArgs e)
        {
            HoverMenu(1, false);
        }

        private void btnMenu2_MouseEnter(object sender, EventArgs e)
        {
            HoverMenu(2, true);
        }

        private void btnMenu2_MouseLeave(object sender, EventArgs e)
        {
            HoverMenu(2, false);
        }

        private void btnMenu3_MouseEnter(object sender, EventArgs e)
        {
            HoverMenu(3, true);
        }

        private void btnMenu3_MouseLeave(object sender, EventArgs e)
        {
            HoverMenu(3, false);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            // Refresh
            x = 0;
            z = 0;
            tim3sec = 4;
            lblBPM2.Visible = false;
            lblBPM2.Text = "BPM: 0";
            chart.Visible = false;

            lblBPM2.ForeColor = Color.White;

            // Chargement du graphique
            chart.Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<ObservablePoint>
                        {

                        },
                        PointGeometrySize = 10
                    },
                };

            // Afficher le compteur
            panel_menu_1_1.Visible = false;
            tim3secondes.Enabled = true;
            lbl3sec.Text = "Début dans " + (tim3sec + 1) + " secondes...";

            lblBPM2.Location = new Point(178, 110);

            lblBPM2.Font = new Font("Microsoft Sans Serif", 22, FontStyle.Bold);

            btnTest.Enabled = false;
            btnTest.Text = "Lecture en cours...";

            tbxDuree.Enabled = false;
        }

        private void tim3secondes_Tick(object sender, EventArgs e)
        {
            // Compte à rebous
            if(tim3sec >= 1)
            {
                lbl3sec.Text = "Début dans " + tim3sec + " secondes...";
                Application.DoEvents();
            } 
            else
            {
                // Fin du compte à rebours
                tim3secondes.Enabled = false;
                panel_menu_1_1.Visible = true;
                lblBPM1.Visible = true;
                lblBPM2.Visible = true;
                lblTmpRestant.Visible = true;
                chart.Visible = true;

                timDuree.Enabled = true;
            }

            // Incrémentation
            tim3sec--;
        }

        private void timDuree_Tick(object sender, EventArgs e)
        {
            // x = nombre de secondes qui en est actuellement dans le calcule
            // z = bpmTOTAL

            // Calcule du BPM
            if (x < Convert.ToInt32(tbxDuree.Text))
            {
                // Récupère la donnée du BPM
                int iCurrentBPM = Convert.ToInt32(lblBPM.Text);
                z += iCurrentBPM;

                // Graphique
                var aPoint = new ObservablePoint(z, iCurrentBPM);
                chart.Series[0].Values.Add(aPoint);


                // Afficher le calcule en direct
                lblBPM2.Text = "BPM: " + iCurrentBPM;
                lblTmpRestant.Text = "Temps restant : " + (Convert.ToInt32(tbxDuree.Text) - 1 - x) + " secondes";
                Application.DoEvents();

                // Incrémentation du temps
                x++;
            }
            else
            {
                // FIN DU TEST

                // Calcule du BPM moyen durant le test
                double dBPM = z / Convert.ToInt32(tbxDuree.Text);

                // Arrêter le calcule
                timDuree.Enabled = false;

                btnTest.Text = "Recommencer";
                btnTest.Enabled = true;

                lblBPM2.Text = "BPM: " + dBPM; // Moyenne des BPM depuis le début du test
                lblBPM2.ForeColor = Color.Green;
                lblBPM2.Font = new Font("Microsoft Sans Serif", 32, FontStyle.Bold);
                lblBPM2.Location = new Point(158, 100);

                lblTmpRestant.Visible = false;
                lblBPM1.Visible = false;

                tbxDuree.Enabled = true;

                // Ajouter dans les enregistrements
                string sDateDuTest = DateTime.Today.ToString("dd.MM.yyyy");
                string sHeureDuTest = DateTime.Now.ToString("HH:mm");
                string sBPM = dBPM.ToString();
                string sDureeDuTest = tbxDuree.Text;
                ajouterEnregistrement(sDateDuTest, sHeureDuTest, sBPM, sDureeDuTest);
            }
        }

        private void ajouterEnregistrement(string sDate, string sHeure, string sBPM, string sDuree)
        {
            // Lecture
            string sEnregistrements = File.ReadAllText(Properties.Settings.Default.enregistrementsPath);

            // Modification
            string sNewEnregistrements = sEnregistrements + "\n" + sDate + ";" + sHeure + ";" + sBPM + ";" + sDuree;

            // Ecriture
            File.WriteAllText(Properties.Settings.Default.enregistrementsPath, sNewEnregistrements);

            // Reload les enregistrements
            loadEnregistrements();

        }
        private void updateGraphique()
        {
            chart.Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<ObservablePoint>
                        {
                            new ObservablePoint(z,60),
                            new ObservablePoint(z,60),
                            new ObservablePoint(z,60),
                            new ObservablePoint(z,60),
                            new ObservablePoint(z,60)
                        },
                        PointGeometrySize = 10
                    },
                };
        }

        private void btnBPMvirtuel_Click(object sender, EventArgs e)
        {
            // Démarrer le reçu de BPM virtuel
            timBPMvirtuel.Enabled = true;
            btnBPMvirtuel.Enabled = false;
            btnBPMvirtuel.Text = "BPM virtuel activé";
            btnBPMvirtuel.ForeColor = Color.LightGreen;

            lblArduinoDetecte.Text = "L'arduino a été détecté.";
            lblArduinoDetecte.ForeColor = Color.Green;
        }

        private void timBPMvirtuel_Tick(object sender, EventArgs e)
        {
            // Génération d'un BPM
            Random rdm = new Random();
            int iRandom = rdm.Next(65, 69);

            // Affichage du BPM
            lblBPM.Text = Convert.ToString(iRandom);
            Application.DoEvents();
        }

        private void ChangeDuree()
        {
            // Variables
            string sNewDuree = tbxDuree.Text;

            // Sauvegarder la nouvelle durée
            updateSettings("testTime", sNewDuree);

            // Changement d'affichages
            lblTmpRestant.Text = "Temps restant : " + tbxDuree.Text + " secondes";
        }

        private void loadEnregistrements()
        {
            // Vider la table
            lvEnregistrements.Items.Clear();

            // Variables
            try
            {
                // Recherche des enregistrements
                string[] aEnregistrements = File.ReadAllText(Properties.Settings.Default.enregistrementsPath).Split('\n');

                // Chercher les données dans le document
                foreach (var enregistrement in aEnregistrements)
                {
                    // Valeurs
                    string sDateDuTest = enregistrement.Split(';')[0];
                    string sHeureDuTest = enregistrement.Split(';')[1];
                    string sBPM = enregistrement.Split(';')[2];
                    string sDureeDuTestEnSec = enregistrement.Split(';')[3];

                    // Ajouter dans la listview
                    string[] row = { "", sDateDuTest, sHeureDuTest, sBPM, sDureeDuTestEnSec };
                    var listViewItem = new ListViewItem(row);
                    lvEnregistrements.Items.Add(listViewItem);
                }

            } catch
            {
                // Enregistrements introuvable
                MessageBox.Show("Les enregistrements n'ont pas été trouvé !");
            }
        }

        private void lblMenu1_MouseEnter(object sender, EventArgs e)
        {
            HoverMenu(1, true);
        }

        private void lblMenu1_MouseLeave(object sender, EventArgs e)
        {
            HoverMenu(1, false);
        }

        private void lblMenu2_MouseEnter(object sender, EventArgs e)
        {
            HoverMenu(2, true);
        }

        private void lblMenu2_MouseLeave(object sender, EventArgs e)
        {
            HoverMenu(2, false);
        }

        private void lblMenu3_MouseEnter(object sender, EventArgs e)
        {
            HoverMenu(3, true);
        }

        private void lblMenu3_MouseLeave(object sender, EventArgs e)
        {
            HoverMenu(3, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart.Visible = true;
            chart.Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<ObservablePoint>
                        {
                            new ObservablePoint(0, 0)
                        },
                        PointGeometrySize = 10
                    },
                };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var aPoint = new ObservablePoint(3, 5);
            chart.Series[0].Values.Add(aPoint);
        }
        
        private void timBPMReel_Tick(object sender, EventArgs e)
        {
            string a = serialPort.ReadExisting();
            lblBPMTest.Text = a;
            Thread.Sleep(200);
        }
        
        static SerialPort serialPort;

        void tryToConnection() 
        {
            //Inisialisation des variables
            int iPort = 0;
            //int iTemp = 0;
            bool bPort = false;

            //Conncetion 

            while (bPort == false)
            {
                //teste si le port COM fonctionne et si il n est pas vide 
                try
                {
                    serialPort = new SerialPort();
                    serialPort.PortName = "COM" + Convert.ToString(iPort);
                    serialPort.BaudRate = 9600;
                    serialPort.Open();
                    string t = serialPort.ReadExisting();

                    if (string.IsNullOrEmpty(t))
                    {
                        bPort = false;
                        lblConnectionTest.Text = Convert.ToString(iPort)+ " : vide";
                        iPort++;
                    }
                    else
                    {
                        bPort = true;
                        lblConnectionTest.Text = "arduino detecter : COM" + Convert.ToString(iPort);
                        timBPMReel.Enabled = true;
                    }
                }
                catch
                {
                    bPort = false;
                    lblConnectionTest.Text = Convert.ToString(iPort);
                    iPort++;
                }

                if (iPort >= 50)
                {
                    bPort = true;
                    lblConnectionTest.Text = "arduino pas detecter";
                }
            }
        }
    }
}
