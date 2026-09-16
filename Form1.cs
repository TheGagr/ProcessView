using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace ProcessView
{
    public partial class Form1 : Form
    {
        private Button btnRefresh = new Button();
        private ListBox lstProcesses = new ListBox();
        private ListBox lstThreads = new ListBox();
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
            btnRefresh.Left = 10;
            btnRefresh.Top = 10;
            btnRefresh.Width = 100;

            lstProcesses.Left = 10;
            lstProcesses.Top = 40;
            lstProcesses.Width = 400;
            lstProcesses.Height = 500;

            lstThreads.Left = 420;
            lstThreads.Top = 40;
            lstThreads.Width = 450;
            lstThreads.Height = 500;



            lstProcesses.Items.Add("Text 1");

            lstProcesses.Items.Add("Text 2");

            LoadProcesses();

            Controls.Add(btnRefresh);
            Controls.Add(lstProcesses);
            Controls.Add(lstThreads);

            lstProcesses.SelectedIndexChanged += LstProcesses_SelectedIndexChanged;
        }
        private void LoadProcesses()
        {
            lstProcesses.Items.Clear();
            lstThreads.Items.Clear();
            foreach (Process process in Process.GetProcesses())
            {
                lstProcesses.Items.Add($"{process.Id} - {process.ProcessName}");
            }

        }

        private void LstProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstThreads.Items.Clear();
            if (lstProcesses.SelectedItem == null)
                return;

            string selected = lstProcesses.SelectedItem.ToString();
            int processId = int.Parse(selected.Split('-')[0].Trim());
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
    }
}

