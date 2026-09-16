using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace ProcessView
{
    public partial class Form1 : Form
    {
        private Button btnRefresh = new Button();
        private ListView lstProcesses = new ListView();
        private ListView lstThreads = new ListView();
        private Label lblTotal = new Label();
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
            btnRefresh.Height = 35;

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

            lstThreads = new ListView();
            lstThreads.Left = 620;
            lstThreads.Top = 45;
            lstThreads.Width = 250;
            lstThreads.Height = 500;
            lstThreads.View = View.Details;
            lstThreads.FullRowSelect = true;
            lstThreads.GridLines = true;

            lstThreads.Columns.Add("ID", 60);
            lstThreads.Columns.Add("Приоритет", 80);
            lstThreads.Columns.Add("Уровень", 100);

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
                try
                {
                    ListViewItem item = new ListViewItem(process.Id.ToString());
                    item.SubItems.Add(process.ProcessName);

                    double memoryMb = process.WorkingSet64 / 1024.0 / 1024.0;
                    item.SubItems.Add(memoryMb.ToString("F1"));

                    string priority = $"{process.PriorityClass} ({process.BasePriority})";
                    item.SubItems.Add(priority);

                    int threadCount = process.Threads.Count;
                    item.SubItems.Add(threadCount.ToString());

                    lstProcesses.Items.Add(item);
                }
                catch
                {
                    
                }
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
                    ListViewItem tItem = new ListViewItem(thread.Id.ToString());
                    tItem.SubItems.Add(thread.CurrentPriority.ToString());
                    tItem.SubItems.Add(thread.PriorityLevel.ToString());
                    lstThreads.Items.Add(tItem);
                }
            }
            catch
            {
                ListViewItem errItem = new ListViewItem("—");
                errItem.SubItems.Add("нет доступа");
                errItem.SubItems.Add("");
                lstThreads.Items.Add(errItem);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadProcesses();
        }
    }
}

