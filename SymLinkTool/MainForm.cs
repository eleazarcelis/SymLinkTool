using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Threading;
using System.Reflection;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace SymLinkTool
{
    public partial class MainForm : Form
    {
        #region Constants
        public static Color DARK_BACKCOLOR_CONTAINER = Color.FromArgb(30, 30, 30);
        public static Color DARK_BACKCOLOR = Color.FromArgb(45, 45, 48);
        public static Color DARK_FORECOLOR = Color.FromArgb(220, 220, 220);
        protected const string PLACEHOLDER_FORMAT = "Escriba o busque la ruta de {0}...";
        private const int cGrip = 16;      // Grip size
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int EM_SETMODIFY = 0x00B9;
        private const int HT_CAPTION = 0x2;
        #endregion


        #region Common definations
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public enum Glyphs
        {
            Light = 0xE793,
            BlueLight = 0xF08C,
            ChromeMinimize = 0xE921,
            ChromeMinimizeContrast = 0xEF2D,
            ChromeMaximize = 0xE922,
            ChromeMaximizeContrast = 0xEF2E,
            ChromeRestore = 0xE923,
            ChromeRestoreContrast = 0xEF2F,
            ChromeClose = 0xE8BB,
            ChromeCloseContrast = 0xEF2C
            //Add more...
        }
        #endregion


        #region Fields
        public List<string> SourceFolders { get; set; }
        public List<string> TargetFolders { get; set; }
        public bool DarkTheme { get; set; }
        private Dispatcher _dispatcher { get; set; }
        private Message _lblTitleWndProcM { get; set; }
        #endregion


        public MainForm()
        {
            InitializeComponent();
            SuspendLayout();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            //txtSource.AutoSize = txtTarget.AutoSize = false;
            DarkTheme = Properties.Settings.Default.DarkMode;
            ChangeTheme(DarkTheme);

            PaintGlyphButtons();
            ResumeLayout();

            txtSource.Text = string.Format(PLACEHOLDER_FORMAT, "origen");
            Color forecolorSource = txtSource.ForeColor;
            txtSource.ForeColor = Color.FromArgb(50, forecolorSource);

            txtSource.GotFocus += TxtWithPlaceholder_GotFocus;
            txtSource.LostFocus += TxtWithPlaceholder_LostFocus; ;

            txtTarget.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTarget.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTarget.Text = string.Format(PLACEHOLDER_FORMAT, "destino");
            Color forecolorTarget = txtTarget.ForeColor;
            txtTarget.ForeColor = Color.FromArgb(50, forecolorTarget);

            txtTarget.GotFocus += TxtWithPlaceholder_GotFocus;
            txtTarget.LostFocus += TxtWithPlaceholder_LostFocus;
        }


        #region Method's control
        private void TxtWithPlaceholder_LostFocus(object sender, EventArgs e)
        {
            string replacement = string.Empty;
            TextBox control = (sender as TextBox);
            if (control.Name == "txtSource")
            {
                replacement = "origen";
            }
            else if (control.Name == "txtTarget")
            {
                replacement = "destino";
            }
            if (string.IsNullOrWhiteSpace(control.Text))
            {
                control.Text = string.Format(PLACEHOLDER_FORMAT, replacement);
                Color fo = control.ForeColor;
                control.ForeColor = Color.FromArgb(50, fo);
            }
        }

        private void TxtWithPlaceholder_GotFocus(object sender, EventArgs e)
        {
            string replacement = string.Empty;
            TextBox control = (sender as TextBox);
            if (control.Name == "txtSource")
            {
                replacement = "origen";
            }
            else if(control.Name == "txtTarget")
            {
                replacement = "destino";
            }
            if (control.Text == string.Format(PLACEHOLDER_FORMAT, replacement))
            {
                control.Text = "";
                Color fo = control.ForeColor;
                control.ForeColor = Color.FromArgb(100, fo);
            }
        }

        private async void txtSource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(await FindSource())
                {
                    EnableTarget();
                    SetTargetAutocomplete();
                }
            }
        }

        private void chkSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkSource.Items.Count > 1)
            {
                if (chkSource.SelectedIndex == 0)
                {
                    SourceFolders = new List<string>();
                    for (int i = 1; i < chkSource.Items.Count; i++)
                    {
                        chkSource.SetItemChecked(i, !chkSource.GetItemChecked(i));
                    }
                }
            }
        }

        private async void txtTarget_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!Directory.Exists(txtTarget.Text))
                {
                    if (MessageBox.Show(string.Format("El directorio {0} no existe en el destino, crearlo?", txtTarget.Text), "SymLink Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Directory.CreateDirectory(txtTarget.Text);
                        lbOutput.Text = string.Format("directorio creado con exito '{0}'", txtTarget.Text);
                    }
                }

                if (await FindTarget())
                {
                    EnableStart();
                }
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            lbOutput.Text = "Iniciando, espere...";
            chkSource.Enabled =   lsTarget.Enabled =  txtSource.Enabled =  txtTarget.Enabled = btnStart.Enabled = false;
            try
            {
                _dispatcher = Dispatcher.CurrentDispatcher;

                pbMain.Visible = true;
                pbMain.Minimum = 0;
                pbMain.Maximum = SourceFolders.Count;

                await ProcessList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbMain.Value = pbMain.Maximum;
            }
            finally
            {
                await FindSource();
                await FindTarget();

                SetTargetAutocomplete();

                chkSource.Enabled = lsTarget.Enabled = txtSource.Enabled = txtTarget.Enabled = btnStart.Enabled = true;

                lbOutput.Text = "Completado";
            }
        }

        private void chkSource_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string item = chkSource.Items[e.Index].ToString();
            if (e.NewValue == CheckState.Checked)
            {
                SourceFolders.Add(txtSource.Text.TrimEnd('\\') + "\\" + item);
            }
            else if(e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
            {
                SourceFolders.Remove(txtSource.Text.TrimEnd('\\') + "\\" + item);
            }
        }

        private async void btnOpenSource_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtSource.Text))
            {
                fdbMain.SelectedPath = txtSource.Text;
            }
            if(fdbMain.ShowDialog(this) == DialogResult.OK)
            {
                txtSource.Text = fdbMain.SelectedPath;
                if (await FindSource())
                {
                    EnableTarget();

                    SetTargetAutocomplete();
                }
            }
        }

        private async void btnOpenTarget_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTarget.Text))
            {
                fdbMain.SelectedPath = txtTarget.Text;
            }
            if (fdbMain.ShowDialog(this) == DialogResult.OK)
            {
                txtTarget.Text = fdbMain.SelectedPath;
                if (await FindTarget())
                {
                    EnableStart();
                }
            }
        }

        private void bntChangeTheme_Click(object sender, EventArgs e)
        {
            DarkTheme = !DarkTheme;
            
            PaintGlyphButtons();

            Properties.Settings.Default.DarkMode = DarkTheme;
            Properties.Settings.Default.Save();
            ChangeTheme(DarkTheme);
        }

        private void txtTarget_EnabledChanged(object sender, EventArgs e)
        {
            lblBGTarget.Enabled = txtTarget.Enabled;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaxRest_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;

                if (btnMaxRest.BackgroundImage != null)
                    btnMaxRest.BackgroundImage.Dispose();

                btnMaxRest.BackgroundImage = CreateGlyphIcon(Glyphs.ChromeRestore, DarkTheme).ToBitmap();
            }
            else
            {
                this.WindowState = FormWindowState.Normal;

                if (btnMaxRest.BackgroundImage != null)
                    btnMaxRest.BackgroundImage.Dispose();

                btnMaxRest.BackgroundImage = CreateGlyphIcon(Glyphs.ChromeMaximize, DarkTheme).ToBitmap();
            }
        }

        private void lblTitle_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion


        #region Overrides
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            //rc = new Rectangle(0, 0, this.ClientSize.Width, 24);
            //e.Graphics.FillRectangle(Brushes.DarkBlue, rc);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }
        #endregion


        #region Class Methods
        private void PaintGlyphButtons()
        {
            btnClose.Text = "";
            btnMaxRest.Text = "";
            btnMin.Text = "";
            if (btnClose.BackgroundImage != null)
                btnClose.BackgroundImage.Dispose();

            if (btnMaxRest.BackgroundImage != null)
                btnMaxRest.BackgroundImage.Dispose();

            if (btnMin.BackgroundImage != null)
                btnMin.BackgroundImage.Dispose();

            if (btnChangeTheme.BackgroundImage != null)
                btnChangeTheme.BackgroundImage.Dispose();

            btnClose.BackgroundImage = CreateGlyphIcon(Glyphs.ChromeClose, DarkTheme, btnClose.Height).ToBitmap();
            btnMaxRest.BackgroundImage = CreateGlyphIcon(this.WindowState == FormWindowState.Normal ? Glyphs.ChromeMaximize : Glyphs.ChromeRestore, DarkTheme, btnMaxRest.Height).ToBitmap();
            btnMin.BackgroundImage = CreateGlyphIcon(Glyphs.ChromeMinimize, DarkTheme, btnMin.Height).ToBitmap();
            btnChangeTheme.BackgroundImage = CreateGlyphIcon(Glyphs.Light, DarkTheme, btnChangeTheme.Height).ToBitmap();
        }

        private void EnableTarget()
        {
            txtTarget.Enabled = btnOpenTarget.Enabled = true;
        }

        private async Task<bool> FindSource()
        {
            if (Directory.Exists(txtSource.Text))
            {
                string[] source = await GetDirectoriesAsync(txtSource.Text);

                chkSource.Items.Clear();
                SourceFolders = new List<string>();
                chkSource.Items.Add("All");

                foreach (var item in source)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(item);
                    if (!dirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    {
                        chkSource.Items.Add(Path.GetFileName(item));
                    }
                }
                return true;
            }
            else
            {
                chkSource.Enabled = false;
            }
            return false;
        }

        private void SetTargetAutocomplete()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            string[] source = new string[] { };
            if (!string.IsNullOrEmpty(txtSource.Text))
            {
                foreach (DriveInfo drive in drives)
                {
                    string letter = txtSource.Text.Substring(0, 1);
                    string driveLetter = drive.Name.Substring(0, 1);
                    if(letter != driveLetter)
                    {
                        source = source.Append(driveLetter + txtSource.Text.Substring(1, txtSource.Text.Length - 1)).ToArray();
                    }
                }
                var autocompleteList = new AutoCompleteStringCollection();
                autocompleteList.AddRange(source);
                txtTarget.AutoCompleteCustomSource = autocompleteList;
            }
        }

        private async Task<string[]> GetDirectoriesAsync(string path)
        {
            return await Task.Factory.StartNew(() =>
            {
                return Directory.GetDirectories(path);
            });
        }

        private void EnableStart()
        {
            chkSource.Enabled = btnStart.Enabled = true;
        }

        private async Task<bool> FindTarget()
        {
            if (Directory.Exists(txtTarget.Text))
            {
                lsTarget.Enabled = true;
                string[] source = await GetDirectoriesAsync(txtTarget.Text);

                lsTarget.Items.Clear();
                TargetFolders = new List<string>();

                foreach (var item in source)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(item);
                    if (!dirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    {
                        lsTarget.Items.Add(Path.GetFileName(item));
                        TargetFolders.Add(item);
                    }
                }
                return true;
            }
            else
            {
                lsTarget.Enabled = false;
            }
            return false;
        }

        private Task ProcessList()
        {
            return Task.Factory.StartNew(() =>
            {
                int i = 0;
                foreach (string pathSource in SourceFolders)
                {
                    string folderName = Path.GetFileName(pathSource);
                    try
                    {
                        if (TargetFolders.Contains(txtTarget.Text.TrimEnd('\\') + "\\" + folderName))
                        {
                            DispatcherOperation<bool> inv = _dispatcher.InvokeAsync(() =>
                            {
                                DialogResult dialogResult = MessageBox.Show("El directorio '" + folderName + "' existe en el destino, reemplazar?", "SymLink Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dialogResult == DialogResult.No)
                                {
                                    return false;
                                }
                                else
                                {
                                    lbOutput.Text = string.Format("Eliminando '{0}' de la ruta '{1}'", folderName, txtTarget.Text);
                                    return true;
                                }
                            });
                            if (inv.Result)
                            {
                                Directory.Delete(txtTarget.Text.TrimEnd('\\') + "\\" + folderName, true);
                            }
                            else
                            {
                                continue;
                            }
                        }

                        _dispatcher.Invoke(() =>
                        {
                            lbOutput.Text = string.Format("copiando '{0}' a la ruta '{1}'", folderName, txtTarget.Text);
                        });
                        DirectoryCopy(pathSource, txtTarget.Text.TrimEnd('\\') + "\\" + folderName, true);

                        _dispatcher.Invoke(() =>
                        {
                            lbOutput.Text = string.Format("borrando '{0}' de la ruta '{1}'", folderName, txtSource.Text);
                        });
                        Directory.Delete(pathSource, true);

                        _dispatcher.Invoke(() =>
                        {
                            lbOutput.Text = string.Format("aplicando link simbolico a '{0}' en la ruta '{1}'", folderName, txtTarget.Text);
                        });

                        string cmd = "/C mklink /d \"" + pathSource + "\" \"" + txtTarget.Text.TrimEnd('\\') + "\\" + folderName.TrimEnd('\\') + "\"";
                        System.Diagnostics.Process.Start("cmd.exe", cmd);
                        _dispatcher.Invoke(() =>
                        {
                            lbOutput.Text = string.Format("link simbolico aplicado correctamente a '{0}'", folderName, txtTarget.Text);
                        });
                    }
                    catch (System.IO.IOException ioex)
                    {
                        _dispatcher.Invoke(() =>
                        {
                            lbOutput.Text = ioex.Message;
                        });
                    }
                    catch (Exception ex)
                    {
                        _dispatcher.Invoke(() =>
                        {
                            lbOutput.Text = ex.Message;
                        });

                        DirectoryCopy(txtTarget.Text.TrimEnd('\\') + "\\" + folderName, pathSource, true);
                        Directory.Delete(txtTarget.Text.TrimEnd('\\') + "\\" + folderName, true);
                    }
                    finally
                    {
                        i++;
                        _dispatcher.Invoke(() =>
                        {
                            pbMain.Value = i;
                        });
                    }
                }
            });
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        private void ChangeTheme(bool isDark, Control container = null)
        {
            if (container == null)
            {
                container = this;
            }

            container.BackColor = isDark ? DARK_BACKCOLOR_CONTAINER : SystemColors.ControlLight;
            container.ForeColor = isDark ? DARK_FORECOLOR : SystemColors.ControlText;

            foreach (var control in container.Controls)
            {
                if ((new string[] { "lblTitle" }.Contains((control as Control).Name)))
                {
                    continue;
                }

                if (control is ContainerControl || control is Panel)
                {
                    ChangeTheme(isDark, (Control)control);
                }
                else
                {
                    (control as Control).BackColor = isDark ? DARK_BACKCOLOR : SystemColors.Control;
                    (control as Control).ForeColor = isDark ? DARK_FORECOLOR : SystemColors.ControlText;
                }
            }
        }

        private static Icon CreateGlyphIcon(Glyphs glyph, bool isDark = false, int size = 25)
        {
            using (var b = new Bitmap(size + 1, size + 1))
            using (Graphics g = Graphics.FromImage(b))
            using (var f = new Font("Segoe MDL2 Assets", 11, FontStyle.Regular))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                Brush brush = isDark ? new SolidBrush(MainForm.DARK_FORECOLOR) : SystemBrushes.ControlText;
                g.DrawString(((char)glyph).ToString(), f, brush, new Rectangle(0, 0, size + 1, size + 1), sf);

                return Icon.FromHandle(b.GetHicon());
            }
        }
        #endregion
    }
}
