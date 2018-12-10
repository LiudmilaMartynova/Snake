using System;
using System.Windows.Forms;

namespace Snake
{
    public partial class FormLogo : Form
    {
        public FormLogo()
        {
            InitializeComponent();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            Hide();
            var formSnake = new FormSnake();
            formSnake.OnShowResults += ShowResults;
            formSnake.Show();
        }

        private void ShowResults(int scores)
        {
            labelScores.Visible = true;
            labelResult.Visible = true;
            labelResult.Text = scores.ToString();
            Show();
        }
    }
}
