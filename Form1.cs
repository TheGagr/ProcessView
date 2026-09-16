using static System.Net.Mime.MediaTypeNames;

namespace ProcessView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeUI();
        }


        private void InitializeUI()
        {
            Text = "Процессы и потоки";
            Width = 900;
            Height = 600;

            Button btnRefresh = new Button();
            btnRefresh.Text = "Обновить";
            btnRefresh.Left = 10;
            btnRefresh.Top = 10;
            btnRefresh.Width = 100;

            ListBox lstProc = new ListBox();
            lstProc.Left = 10;
            lstProc.Top = 40;
            lstProc.Width = 400;
            lstProc.Height = 500;

            ListBox lstThreads = new ListBox();
            lstThreads.Left = 420;
            lstThreads.Top = 40;
            lstThreads.Width = 450;
            lstThreads.Height = 500;



            lstProc.Items.Add("Text 1");

            lstProc.Items.Add("Text 2");

            Controls.Add(btnRefresh);
            Controls.Add(lstProc);
            Controls.Add(lstThreads);
        }
    }
}

