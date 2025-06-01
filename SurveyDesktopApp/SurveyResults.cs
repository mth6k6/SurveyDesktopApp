using System;
using System.Windows.Forms;

namespace SurveyDesktopApp
{
    public partial class SurveyResults : Form
    {
        private Form1 _mainForm; 
        public SurveyResults(Form1 mainForm)
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase();
            _mainForm = mainForm;
        }
        private void SurveyResults_Load(object sender, EventArgs e)
        {
            int totalSurveys = DatabaseHelper.GetSurveyCount();

            if (totalSurveys == 0)
            {
                MessageBox.Show("No survey data available.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                return;
            }
            DisplayTotalSurveys();
            DisplayMaxAge();
            DisplayMinAge();
            DisplayAverageAge();
            DisplayFoodPercentages();
            DisplayAllRatings();
        }

    
        public void DisplayTotalSurveys()
        {
            int totalSurveys = DatabaseHelper.GetSurveyCount();
            lblTotalSuv.Text = totalSurveys.ToString()+ " Surveys";
        }
        public void DisplayMaxAge()
        {
            int maxAge = DatabaseHelper.GetMaxAge();
            lblMaxAge.Text = maxAge.ToString() + " years old";
        }
        public void DisplayMinAge()
        {
            int miAge = DatabaseHelper.GetMinAge();
            lblMinAge.Text = miAge.ToString() + " years old";
        }
        public void DisplayAverageAge()
        {
            double avgAge = DatabaseHelper.GetAverageAge();
            lblAvgAge.Text = avgAge.ToString();
        }
        public void DisplayFoodPercentages()
        {
            lblPizzaPercent.Text = $"{DatabaseHelper.GetFoodPercentage("Pizza"):0.0}%";
            lblPasta.Text = $"{DatabaseHelper.GetFoodPercentage("Pasta"):0.0}%";
            lblPapW.Text = $"{DatabaseHelper.GetFoodPercentage("Pap and Wors"):0.0}%";
        }
        public void DisplayAllRatings()
        {
            lblMovies.Text = $"{DatabaseHelper.GetAverageRating("Movies"):0.0} / 5";
            lblTv.Text = $"{DatabaseHelper.GetAverageRating("TV"):0.0} / 5";
            lblRadio.Text = $"{DatabaseHelper.GetAverageRating("Radio"):0.0} / 5";
            lblEat.Text = $"{DatabaseHelper.GetAverageRating("EatOut"):0.0} / 5";
        }

        private void lnkToForm1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            _mainForm.Show();
        }
    }
}
