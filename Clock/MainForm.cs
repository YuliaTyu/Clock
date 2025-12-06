using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Clock
{
    public partial class MainForm : Form
    {
        ColorDialog backgroundDialog;
        ColorDialog foregroundDialog;
        ChooseFont fontDialog;
        AlarmsForm alarms;
        Alarm alarm;
        public MainForm()
        {
            InitializeComponent();
            SetVisibility(false);//скрыть элементы
            backgroundDialog = new ColorDialog();//цвет фона
            foregroundDialog = new ColorDialog();//цвет шрифтв
            fontDialog = new ChooseFont();//Шрифт кастомный 
            alarms = new AlarmsForm(this);
            this.Location = new Point//открывать в парвом верхнем углу
                (Screen.PrimaryScreen.Bounds.Width - this.labelTime.Width - 150,
                50);
            tsmiTopmost.Checked = this.TopMost = true;
           // AllocConsole(); // показывает консоль

        }

        private void timer_Tick(object sender, EventArgs e)//обработчик событий
        {
            //labelTime.Text = DateTime.Now.ToString("HH:mm:ss");// 24 часовой формат
            labelTime.Text = DateTime.Now.ToString("hh:mm:ss tt",
                System.Globalization.CultureInfo.InvariantCulture); //AM/PM fix - 12 часовой формат 
            if (checkBoxShowDate.Checked)
                labelTime.Text += $"\n{DateTime.Now.ToString("yyyy.MM.dd")}";
            if (checkBoxShowWeekday.Checked)
                labelTime.Text += $"\n{DateTime.Now.DayOfWeek}";
            notifyIcon.Text = labelTime.Text;
            
            if (alarms.Alarm != null)
            {
                alarm = alarms.Alarm;
                if(alarm.Time.Hour == DateTime.Now.Hour && alarm.Time.Minute == DateTime.Now.Minute && alarm.Time.Second ==DateTime.Now.Second)
                {
                    //MessageBox.Show(alarm.ToString());
                    axWindowsMediaPlayer.URL = alarm.Filename;
                    axWindowsMediaPlayer.Visible = true;
                    axWindowsMediaPlayer.Ctlcontrols.play();
                }
            }
        }
        //скрыть / показать 
        void SetVisibility(bool visible)
        {
            checkBoxShowDate.Visible = visible;
            checkBoxShowWeekday.Visible = visible;
            buttonHideControls.Visible = visible;
            this.FormBorderStyle =visible? FormBorderStyle.FixedToolWindow :FormBorderStyle.None;
            this.TransparencyKey = visible ? Color.Empty : this.BackColor;
            this.ShowInTaskbar = visible;
        }
        private void buttonHideControls_Click(object sender, EventArgs e)=>        
            SetVisibility(tsmiShowControls.Checked = false);
        
        private void labelTime_DoubleClick(object sender, EventArgs e)=>
            SetVisibility(tsmiShowControls.Checked = true);
        
        //system Tray по двойному щелчку
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.TopMost = false;
        }

        private void tsmiQuit_Click(object sender, EventArgs e)=>
            this.Close();
     
        //не закрыть поверх окон
        private void tsmiTopmost_Click(object sender, EventArgs e)=>
            this.TopMost = tsmiTopmost.Checked;
        
        //показать дату в контекстном меню
        private void tsmiShowDate_Click(object sender, EventArgs e)=>
            checkBoxShowDate.Checked = tsmiShowDate.Checked;
       
        //синхрон галки конктсного меню и бокса
        private void checkBoxShowDate_CheckedChanged(object sender, EventArgs e)=>
            tsmiShowDate.Checked = checkBoxShowDate.Checked;
        
        //показать день недели в контекстном меню
        private void tsmiShowWeekday_Click(object sender, EventArgs e)=>
            checkBoxShowWeekday.Checked = tsmiShowWeekday.Checked;
        
        //синхрон галки конктсного меню и бокса
        private void checkBoxShowWeekday_CheckedChanged(object sender, EventArgs e)=> //отправитель события и параметры сообщения
            tsmiShowWeekday.Checked = (sender as CheckBox).Checked;//as преобразование типов
        

        private void tsmiShowControls_Click(object sender, EventArgs e)=>
            SetVisibility(tsmiShowControls.Checked);

        private void tsmiBackgroundColor_Click(object sender, EventArgs e)
        {
            if (backgroundDialog.ShowDialog() == DialogResult.OK)
                labelTime.BackColor = backgroundDialog.Color;
        }

        private void tsmiForegroundColor_Click(object sender, EventArgs e)
        {
            if(foregroundDialog.ShowDialog() == DialogResult.OK)
               labelTime.ForeColor = foregroundDialog.Color;
        }
        private void tsmiChooseFont_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
                labelTime.Font = fontDialog.Font;
        }

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
        private void tsmiShowConsole_CheckedChanged(object sender, EventArgs e)
        {
            bool console = (sender as ToolStripMenuItem).Checked ? AllocConsole() : FreeConsole();
        }

        private void tsmiAlarms_Click(object sender, EventArgs e)
        {
            alarms.ShowDialog();
        }
    }
}
