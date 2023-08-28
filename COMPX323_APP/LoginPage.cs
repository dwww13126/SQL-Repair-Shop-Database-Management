﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMPX323_APP
{
    public partial class LoginPage : Form
    {
        /// <summary>
        /// This is the first method called when the program form loads.
        /// </summary>
        public LoginPage()
        {
            InitializeComponent();
            //This line of code allows us to obscure the password visually and limit the max chars in textbox
            textBoxPassword.PasswordChar = '*';     //password character to hide password characters
            textBoxPassword.MaxLength = 20;         //max textbox character count

            //Sets the Staff buton to be set by default and the other options to be hidden
            radioButtonStaff.Checked = true;
            radioButtonStaff.Visible = false;
            radioButtonCustomer.Visible = false;
            groupBox1.Visible = false;
        }


        /// <summary>
        /// Clicked when user decides they are ready to log in, 
        /// Will get loginname and password, use that to query database and check that loginname and password are correct/exist.
        /// A message box will be used to state whether or not we logged in successfully
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            //Stores the read in loginname and password
            string loginname = "", password = "";
            //Check if boxes are empty or white space
            if ("".Equals(textBoxUserName.Text.Trim()) || "".Equals(textBoxPassword.Text.Trim()))
            {
                MessageBox.Show("Please make sure you enter a Loginname and Password");
                return;
            }
            //Checks if the user still needs to pick a staff or customer login
            if (!(radioButtonCustomer.Checked||radioButtonStaff.Checked))
            {
                MessageBox.Show("Please make sure you select either Staff or Customer login from the radio button options");
                return;
            }
            //Get the loginname and password from the text boxes
            try
            {
                loginname = textBoxUserName.Text.Trim();
                password = textBoxPassword.Text.Trim();
                //(2) SELECT statement getting all data from users, i.e. SELECT * FROM Users
                SQL.selectQuery("SELECT LoginName FROM UserAccount WHERE LoginName = '" + loginname + "' AND Password = '" + password + "'");
                //(3) Check each loginname and password combination
                if (SQL.dr.HasRows)
                {
                    //Checks the value of the radio button to see if they are wanting to login as a staff or customer
                    if (radioButtonCustomer.Checked)
                    {
                        //Performs another query to check if the user is of the right type 
                        SQL.selectQuery("SELECT LoginName FROM Customer WHERE LoginName = '" + loginname + "'");
                        if (SQL.dr.HasRows)
                        {
                            SQL.dr.Read();
                            loginname = SQL.dr[0].ToString();
                            MessageBox.Show("Customer Login Successful "  + loginname + "! You will be informed once User portal is up and running :)");
                        }
                        else
                        {
                            MessageBox.Show("Error: You are not a Customer Member, Please Select Staff.");
                            return;
                        }
                    }
                    else if (radioButtonStaff.Checked)
                    {
                        //Performs another query to check if the user is of the right type 
                        SQL.selectQuery("SELECT LoginName FROM Staff WHERE LoginName = '" + loginname + "'");
                        if (SQL.dr.HasRows)
                        {
                            SQL.dr.Read();
                            loginname = SQL.dr[0].ToString();
                            //Brings up the staff user page
                            initialiseTextBoxes();
                            this.Hide();
                            StaffActions display = new StaffActions(loginname);
                            display.ShowDialog();
                            this.Close();
                            //Console.WriteLine(loginname);
                        }
                        else
                        {
                            MessageBox.Show("Error: You are not a Staff Member, Please goto the Customer Login.");
                            return;
                        }
                    }
                }
                //If no matching users exist
                else
                {
                    MessageBox.Show("Error! Either Username or Password are incorrect!");
                    textBoxUserName.Focus();
                    return;
                }
                SQL.end();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Clears the text boxes on the page focuses on top one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            initialiseTextBoxes();
        }

        /// <summary>
        /// Initialises all textboxes to blank text
        /// Re focus to first text box
        /// </summary>
        public void initialiseTextBoxes()
        {
            //goes through and clears all of the textboxes
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    (c as TextBox).Clear();
                }
            }
            //makes next place user types the text box
            textBoxUserName.Focus();
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
