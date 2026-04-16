// See https://aka.ms/new-console-template for more information

//using static System.Net.Mime.MediaTypeNames;

namespace Calculator.Numbers
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new TClcPnl());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при запуске приложения: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
//* /
