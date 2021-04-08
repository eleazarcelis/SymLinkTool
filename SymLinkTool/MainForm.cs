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

namespace SymLinkTool
{
    public partial class MainForm : Form
    {
        public List<string> SourceFolders { get; set; }
        public List<string> TargetFolders { get; set; }
        public bool DarkTheme { get; set; }

        private Color DARK_BACKCOLOR_CONTAINER = Color.FromArgb(30, 30, 30);
        private Color DARK_BACKCOLOR = Color.FromArgb(45, 45, 48);
        private Color DARK_FORECOLOR = Color.FromArgb(220, 220, 220);
        private const string PLACEHOLDER_FORMAT = "Escriba o busque la ruta de {0}...";

        private Dispatcher _dispatcher { get; set; }

        public MainForm()
        {
            InitializeComponent();
            SuspendLayout();
            txtSource.AutoSize = txtTarget.AutoSize = false;
            ChangeTheme(Properties.Settings.Default.DarkMode);
            ResumeLayout();

            txtSource.Text = string.Format(PLACEHOLDER_FORMAT, "origen");
            Color forecolorSource = txtSource.ForeColor;
            txtSource.ForeColor = Color.FromArgb(50, forecolorSource);

            txtSource.GotFocus += TxtWithPlaceholder_GotFocus;
            txtSource.LostFocus += TxtWithPlaceholder_LostFocus; ;

            txtTarget.Text = string.Format(PLACEHOLDER_FORMAT, "destino");
            Color forecolorTarget = txtTarget.ForeColor;
            txtTarget.ForeColor = Color.FromArgb(50, forecolorSource);

            txtTarget.GotFocus += TxtWithPlaceholder_GotFocus;
            txtTarget.LostFocus += TxtWithPlaceholder_LostFocus;
        }

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
                control.Text = string.Format(PLACEHOLDER_FORMAT, "origen");
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
            if (control.Text == string.Format(PLACEHOLDER_FORMAT, "origen"))
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
                }
            }
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

        private async Task<string[]> GetDirectoriesAsync(string path)
        {
            return await Task.Factory.StartNew(() =>
            {
                return Directory.GetDirectories(path);
            });
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

                chkSource.Enabled = lsTarget.Enabled = txtSource.Enabled = txtTarget.Enabled = btnStart.Enabled = true;

                lbOutput.Text = "Completado";
            }
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
                            if(inv.Result)
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

        private void ChangeTheme(bool isDark, Control container = null)
        {
            if(container == null)
            {
                container = this;
            }

            container.BackColor = isDark ? DARK_BACKCOLOR_CONTAINER : SystemColors.ControlLight;
            container.ForeColor = isDark ? DARK_FORECOLOR : SystemColors.ControlText;

            foreach (var control in container.Controls)
            {
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

        private void bntChangeTheme_Click(object sender, EventArgs e)
        {
            DarkTheme = !DarkTheme;

            if (DarkTheme)
            {
                this.bntChangeTheme.BackgroundImage = global::SymLinkTool.Properties.Resources.ligthmode;
            }
            else
            {
                this.bntChangeTheme.BackgroundImage = global::SymLinkTool.Properties.Resources.darkmode;
            }
            Properties.Settings.Default.DarkMode = DarkTheme;
            Properties.Settings.Default.Save();
            ChangeTheme(DarkTheme);
        }

        private void txtTarget_EnabledChanged(object sender, EventArgs e)
        {
            lblTargetBackground.Enabled = txtTarget.Enabled;
        }
    }
}
