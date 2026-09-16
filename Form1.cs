using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace ProcessView
{
    public partial class Form1 : Form
    {
        private Button btnRefresh = new Button();
        private ListView lstProcesses = new ListView();
        private ListBox lstThreads = new ListBox();
        private Label lblTotal;
        public Form1()
        {
            InitializeUI();
        }


        private void InitializeUI()
        {
            Text = "Процессы и потоки";
            Width = 900;
            Height = 600;

            btnRefresh.Text = "Обновить";
            btnRefresh.Left = 5;
            btnRefresh.Top = 5;
            btnRefresh.Width = 100;

            lblTotal = new Label();
            lblTotal.Left = 120;
            lblTotal.Top = 15;
            lblTotal.AutoSize = true;
            lblTotal.Text = "Всего процессов: 0";

            lstProcesses.Left = 10;
            lstProcesses.Top = 45;
            lstProcesses.Width = 600;
            lstProcesses.Height = 500;
            lstProcesses.View = View.Details;
            lstProcesses.FullRowSelect = true;
            lstProcesses.GridLines = true;

            lstProcesses.Columns.Add("ID", 60);
            lstProcesses.Columns.Add("Имя", 200);
            lstProcesses.Columns.Add("Память (МБ)", 100);
            lstProcesses.Columns.Add("Приоритет", 100);
            lstProcesses.Columns.Add("Потоков", 70);

            lstThreads.Left = 620;
            lstThreads.Top = 40;
            lstThreads.Width = 250;
            lstThreads.Height = 500;



            lstProcesses.Items.Add("Text 1");

            lstProcesses.Items.Add("Text 2");

            LoadProcesses();

            Controls.Add(btnRefresh);
            Controls.Add(lstProcesses);
            Controls.Add(lstThreads);
            Controls.Add(lblTotal);


            lstProcesses.SelectedIndexChanged += LstProcesses_SelectedIndexChanged;

            btnRefresh.Click += BtnRefresh_Click;
        }
        private void LoadProcesses()
        {
            lstProcesses.Items.Clear();
            lstThreads.Items.Clear();
            Process[] processes = Process.GetProcesses();
            lblTotal.Text = $"Всего процессов: {processes.Length}";

            foreach (Process process in processes)
            {
                ListViewItem item = new ListViewItem(process.Id.ToString());
                item.SubItems.Add(process.ProcessName);
                lstProcesses.Items.Add(item);
                double memoryMb = process.WorkingSet64 / 1024.0 / 1024.0;
                item.SubItems.Add(memoryMb.ToString("F1"));
                item.SubItems.Add(""); // приоритет
                item.SubItems.Add(""); // потоков
            }

        }

        private void LstProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstThreads.Items.Clear();

            if (lstProcesses.SelectedItems.Count == 0)
                return;

            ListViewItem selected = lstProcesses.SelectedItems[0];
            int processId = int.Parse(selected.Text);   // Text — первая ячейка, там ID

            try
            {
                Process process = Process.GetProcessById(processId);

                foreach (ProcessThread thread in process.Threads)
                {
                    lstThreads.Items.Add(
                        $"ID: {thread.Id}, приоритет: {thread.CurrentPriority}, уровень: {thread.PriorityLevel}"
                    );
                }
            }
            catch
            {
                lstThreads.Items.Add("Нет доступа к потокам этого процесса");
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadProcesses();
        }
    }
}

