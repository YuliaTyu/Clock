using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetVisibility(false);//скрыть элементы 
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

       
    }
}
