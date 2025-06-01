using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SurveyDesktopApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetRadioTags(grpEatOut);
            SetRadioTags(grpMovies);
            SetRadioTags(grpWatchTV);
            SetRadioTags(grpRadio);
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            string fName = txtFname.Text.Trim();
            string email = txtMail.Text.Trim();
            string date = DobTimePicker.Value.ToString("yyyy-MM-dd");
            string contactNum = txtContact.Text.Trim();
            int age = calculateAge();

            var selectedFoods = new[] { chkPizza, chkPasta, chkPapWors }
                .Where(c => c.Checked)
                .Select(c => c.Text);
            string foods = string.Join(", ", selectedFoods);

            int eatOut = GetSelectedRating(grpEatOut);
            int watchMovies = GetSelectedRating(grpMovies);
            int watchTV = GetSelectedRating(grpWatchTV);
            int listenToRadio = GetSelectedRating(grpRadio);

            DatabaseHelper.InsertSurvey(fName, email, contactNum, date, age, foods, eatOut, watchMovies, watchTV, listenToRadio);
            MessageBox.Show("Survey submitted successfully.");
            ClearForm();
        }

        private int calculateAge()
        {
            DateTime today = DateTime.Today;
            DateTime birthDate = DobTimePicker.Value;
            int age = today.Year - birthDate.Year;
            return age;
        }

        private bool ValidateForm()
        {
            int currentAge = calculateAge();

            if (string.IsNullOrWhiteSpace(txtFname.Text) || string.IsNullOrWhiteSpace(txtMail.Text))
            {
                MessageBox.Show("Please enter name and email.");
                return false;
            }

            if (currentAge <= 5 || currentAge >= 120)
            {
                MessageBox.Show("Age must be between 5 and 120.");
                return false;
            }

            if (!new[] { chkPizza, chkPasta, chkPapWors }.Any(c => c.Checked))
            {
                MessageBox.Show("Please select at least one favourite food.");
                return false;
            }

            int eatOutRating = GetSelectedRating(grpEatOut);
            int movieRating = GetSelectedRating(grpMovies);
            int tvRating = GetSelectedRating(grpWatchTV);
            int radioRating = GetSelectedRating(grpRadio);

            if (eatOutRating == 0 || movieRating == 0 || tvRating == 0 || radioRating == 0)
            {
                MessageBox.Show("Please select a rating for all questions.");
                return false;
            }

            return true;
        }

        private int GetSelectedRating(GroupBox groupBox)
        {
            foreach (Control control in groupBox.Controls)
            {
                if (control is RadioButton radio && radio.Checked)
                {
                    if (radio.Tag != null && int.TryParse(radio.Tag.ToString(), out int rating))
                    {
                        return rating;
                    }
                }
            }
            return 0;
        }

        private void ClearForm()
        {
            txtFname.Clear();
            txtMail.Clear();
            txtContact.Clear();
            DobTimePicker.Value = DateTime.Today;

            foreach (var chk in new[] { chkPizza, chkPasta, chkPapWors })
                chk.Checked = false;

            foreach (var group in new[] { grpEatOut, grpMovies, grpWatchTV, grpRadio })
            {
                foreach (RadioButton rb in group.Controls.OfType<RadioButton>())
                    rb.Checked = false;
            }
        }

       
        private void SetRadioTags(GroupBox groupBox)
        {
            int tagValue = 1;
            foreach (var rb in groupBox.Controls.OfType<RadioButton>().OrderBy(r => r.TabIndex))
            {
                rb.Tag = tagValue++;
            }
        }

        private void lnkSurveyResults_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SurveyResults resultsForm = new SurveyResults(this); 
            this.Hide();                        
            resultsForm.Show();

        }
    }
}

        
