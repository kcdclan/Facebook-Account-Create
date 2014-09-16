using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace FB_manager
{
    public partial class Form1 : Form
    {
        string path = "accounts.txt";
        string register = "https://m.facebook.com/r.php";
        string login = "https://m.facebook.com/login.php";
        string home = "https://m.facebook.com/home.php";
        string confirmemail = "https://m.facebook.com/confirmemail.php";
        string cr = "https://m.facebook.com/cr.php";
        string[] data;


        string lastpage = "home";

        int count = 0;

        public Form1()
        {
            InitializeComponent();
            exists(path);
        }

        void exists(string file)
        {
            if (!File.Exists(String.Format(@"{0}\"+file, Application.StartupPath)))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("email:passwd");
                }
            }
        }

        bool emailexists(string email)
        {
            bool state = false;
            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                data = line.Split(':');
                if (data[0] == email)
                {
                    state = true;
                    Console.WriteLine(line);
                    break;
                }
                counter++;
            }

            file.Close();
            return state;
        }

        void logout()
        {
            HtmlElementCollection links = webBrowser1.Document.GetElementsByTagName("A");
            foreach (HtmlElement link in links)  // this ex is given another SO post 
            {

                if ((link.InnerText != null) && (link.InnerText.Equals("Log Out")))
                    link.InvokeMember("Click");
            }
        }

        void write(string line)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(line);
            }
        }

        void fill()
        {
            count++;
            if (!emailexists(count + textBox3.Text))
            {
                webBrowser1.Document.GetElementById("firstname").InnerText = textBox1.Text;
                webBrowser1.Document.GetElementById("lastname").InnerText = textBox2.Text;
                webBrowser1.Document.GetElementById("email").InnerText = count + textBox3.Text;
                if (comboBox1.Text == "Male")
                {
                    webBrowser1.Document.GetElementById("gender").Children[2].SetAttribute("selected", "selected");
                }
                else
                {
                    webBrowser1.Document.GetElementById("gender").Children[1].SetAttribute("selected", "selected");
                    webBrowser1.Document.GetElementById("gender").InnerText = "1";
                }

                webBrowser1.Document.GetElementById("month").InnerText = comboBox2.Text;
                webBrowser1.Document.GetElementById("day").InnerText = comboBox3.Text;
                webBrowser1.Document.GetElementById("year").InnerText = comboBox4.Text;
                webBrowser1.Document.GetElementById("pass").InnerText = textBox4.Text;
                webBrowser1.Document.GetElementById("signup_button").InvokeMember("click");
                write(count + textBox3.Text + ":" + textBox4.Text);
            }
            else
            { 
                fill();
            }
        }

        string location()
        {
            if (webBrowser1.Url != null)
            {
                if (webBrowser1.Url.ToString().Contains(register))
                { return "register"; }
                else if (webBrowser1.Url.ToString().Contains(login))
                { return "login"; }
                else if (webBrowser1.Url.ToString().Contains(confirmemail))
                { return "confirmemail"; }
                else if (webBrowser1.Url.ToString().Contains(cr))
                { return "confirmemail"; }
                else if (webBrowser1.Url.ToString().Contains(home))
                { return "home"; }
                else
                { return null; }
            }
            else
            { return null; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lastpage != location())
            {
                Form1.ActiveForm.Text = webBrowser1.DocumentTitle;
                lastpage = location();
                switch (location())
                {
                    case "confirmemail":
                        logout();
                        webBrowser1.Navigate(register);
                        break;
                    case "register":
                        fill();
                        break;
                    case "login":
                        webBrowser1.Navigate(register);
                        break;
                    case "home":
                        webBrowser1.Navigate(register);
                        break;
                    default:
                        webBrowser1.Navigate(register);
                        break;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

    }
}
