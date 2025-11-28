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

namespace Clock
{
    public partial class MainForm : Form
    {
            Timer timer01 = new Timer();
            SoundPlayer sp = new SoundPlayer("D:\\1.wav");
            bool b = false;//завести будильник/убрать будильник
        public MainForm()
        {
            InitializeComponent();
            SetVisibility(false);//скрыть элементы 
        }

        private void timer_Tick(object sender, EventArgs e)//обработчик событий
        {
            //labelTime.Text = DateTime.Now.ToString("HH:mm:ss");// 24 часовой формат
            labelTime.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
            labelTime.Text = DateTime.Now.ToString("hh:mm:ss tt",
                System.Globalization.CultureInfo.InvariantCulture); //AM/PM fix - 12 часовой формат 
            if (checkBoxShowDate.Checked)
                labelTime.Text += $"\n{DateTime.Now.ToString("yyyy.MM.dd")}";
            if (checkBoxShowWeekday.Checked)
                labelTime.Text += $"\n{DateTime.Now.DayOfWeek}";
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

        private void button1_Click(object sender, EventArgs e)
        {
            //this.ForeColor = Color.Green;
            if (b == false)
            {
                labelTime.Text = maskedTextBox1.Text;
                timer.Start();
                maskedTextBox1.Visible = false;
                button1.Text = "Убрать будильник";
                b = true;
            }
            else if(b==true)
            {
                labelTime.Text = "00:00";
                timer.Stop();
                maskedTextBox1.Visible = true;
                button1.Text = "Завести будильник";
                b = false;
            }
        }

        private void labelTime_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.Maroon;
        }

        private void tsmiBackgroundColor_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(255,55,123);//Система RGB (0-255)
        }

        private void tsmiForegroundColor_Click(object sender, EventArgs e)
        {
            this.ForeColor = Color.Yellow;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this.ForeColor = Color.Green;
            sp.Stop();
            button2.Enabled = false;
            maskedTextBox1.Visible = true;
            button1.Text = "Завести будильник";
            b = false;
        
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            timer01.Interval = 1000;
            timer01.Tick += new EventHandler(timer_Tick);//срабатывает при событии
            timer01.Start();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            if(labelTime.Text == labelTime.Text + ":00")
            {
                button2.Enabled = true;
                sp.Play();
            }
        }
    }
}
