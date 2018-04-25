using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace BaseballQuery
{
    public partial class ShowQuery : Form
    {

        //declare min and max batting average variables
        private decimal min;
        private decimal max;
        //value for checking for numbers with tryparse
        double parsedValue;

        public ShowQuery()
        {
            InitializeComponent();
        }

        private BaseballLibrary.BaseballEntities bbCon = new BaseballLibrary.BaseballEntities();

        //         ****************** on load event ******************

        private void ShowQuery_Load(object sender, EventArgs e)
        {
            //load entity object
            bbCon.Players.Load();
            //set data source
            playerBindingSource.DataSource = bbCon.Players.Local;
        }

        //          ****************** find by last name button event ******************

        private void btnLastName_Click(object sender, EventArgs e)
        {
            try{
                //check for numbers
                if (double.TryParse(txtLastName.Text, out parsedValue))
                {
                    throw new FormatException();
                }
                else
                {
                    //create string to hold user input
                    string nameInput = txtLastName.Text;
                    //capitalize input 
                    string searchName = nameInput.First().ToString().ToUpper() + String.Join("", nameInput.Skip(1));
                    //create data source and query
                    playerBindingSource.DataSource = bbCon.Players.Local
                        .Where(player => player.LastName == searchName);
                }
            }
            catch(FormatException)
            {
                MessageBox.Show("Must enter a name for last name search", "Invalid Input"); //error message
                Clear(); //clear entries
            }
        }

        //          ****************** show all button event *****************

        private void btnAll_Click(object sender, EventArgs e)
        {
            //set data source to all entries
            playerBindingSource.DataSource = bbCon.Players.Local;
        }

        //         ****************** show by batting average button event ******************

        private void btnAverage_Click(object sender, EventArgs e)
        {
            try{
                //if no input for min 
                if (String.IsNullOrEmpty(txtMin.Text))
                {
                    //assign min value the default
                    min = 0;
                }
                //if input for min
                else
                {
                    min = Convert.ToDecimal(txtMin.Text);
                }
                //if no input for max 
                if (String.IsNullOrEmpty(txtMax.Text))
                {
                    //assign max value the default
                    max = 1;
                }
                //if input for max
                else
                {
                    max = Convert.ToDecimal(txtMax.Text);
                }
                
                
                //make sure entered values are in range, if not throw format exception
                if (min < 0 || max > 1)
                {
                    throw new FormatException();
                }

                //set data source to query in range
                playerBindingSource.DataSource = bbCon.Players.Local
                    .Where(player => player.BattingAverage >= min && player.BattingAverage <= max);

                //had hoped to insert some code to check if there are no results but had no luck with everything I tried**
            }

            //    catch number format exception
            catch(FormatException)
            {
                MessageBox.Show("Must enter a number between 0 and 1 for batting average search", "Invalid search entry"); //error message
                Clear(); //clear entries
            }
        }

        //method to clear entries
        private void Clear()
        {
            txtLastName.Text = "";
            txtMax.Text = "";
            txtMin.Text = "";
        }

    }
}
