using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entity_Lec5
{
    public partial class Form1 : Form
    {
        Class1 Entity;
        Department Dept;
        public Form1()
        {
            InitializeComponent();
            Entity= new Class1();
        }

        //Department Combo Box
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            int ID = int.Parse(comboBox1.Text);
            Dept = Entity.Departments.Find(ID);
            if (Dept != null)
            {
                textBox1.Text = Dept.Id.ToString();
                textBox2.Text = Dept.DeptName.ToString();
                foreach (var d in Dept.Employees)
                {
                    listBox1.Items.Add(d.Name);
                }
            }
        }

        // Loading Depts Into Combo box
        private void Form1_Load(object sender, EventArgs e)
        {
            var Dept = from d in Entity.Departments select d.Id;
            foreach (var d in Dept)
            {
                comboBox1.Items.Add(d);
            }
        }

        // Add Departments
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!= "" && textBox2.Text!= "")
            {
                listBox1.Items.Clear();
                int ID = int.Parse(textBox1.Text);
                string Name = textBox2.Text;
                Department dept = new Department();
                dept.Id = ID;
                dept.DeptName = Name;
                var IdCheck = Entity.Departments.Find(ID);
                if (IdCheck == null)
                {
                    Entity.Departments.Add(dept);
                }
                Entity.SaveChanges();
                RefreshComboBox();
            }
            else
            {
                MessageBox.Show("Department Data Is Required");
            }
            

        }


        public void RefreshComboBox()
        {
            comboBox1.Items.Clear();
            var Dept = from d in Entity.Departments select d.Id;
            foreach (var d in Dept)
            {
                comboBox1.Items.Add(d);
            }
        }

        public void RefreshListBox()
        {

            listBox1.Items.Clear();
            int ID = int.Parse(comboBox1.Text);
            Dept = Entity.Departments.Where(d => d.Id == ID).Select(d => d).FirstOrDefault();
            if (Dept != null)
            {
                textBox1.Text = Dept.Id.ToString();
                textBox2.Text = Dept.DeptName.ToString();
                foreach (var d in Dept.Employees)
                {
                    listBox1.Items.Add(d.Name);
                }
            }
        }


        // Update Department Button
        private void button2_Click(object sender, EventArgs e)
        {
            string newName = textBox2.Text;
            int ID = int.Parse(textBox1.Text);
            Dept = Entity.Departments.Find(ID);
            if (Dept != null)
            {
                Dept.DeptName = newName;
            }
            Entity.SaveChanges();
            RefreshComboBox();
            
        }


        // Delete Department Button
        private void button3_Click(object sender, EventArgs e)
        {
            int DeptID = int.Parse(textBox1.Text);
            var emps = from em in Entity.Employees where em.DeptId==DeptID select em;
            foreach (var em in emps)
            {
                Entity.Employees.Remove(em);

            }
            Entity.SaveChanges();

            Dept = Entity.Departments.Find(DeptID);
            if (Dept == null)
            {
                MessageBox.Show("Department Doesnt Exist");
            }
            else
            {
                Entity.Departments.Remove(Dept);
                comboBox1.Items.Remove(DeptID);
            }
            textBox1.Text = textBox2.Text = "";
            Entity.SaveChanges();
            RefreshComboBox();
            RefreshListBox();
        }


        // Selecting Employee From list Box
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string EmpName = listBox1.SelectedItem.ToString();
            Employee Emp = (from d in Entity.Employees where d.Name == EmpName select d).FirstOrDefault();
            if (Emp != null)
            {
                textBox3.Text = Emp.Name;
                textBox4.Text = Emp.Id.ToString();
            }
        }


        // Add Employee
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3!= null && textBox4!=null)
            {
                int ChosenDep = int.Parse(textBox1.Text);
                string empName = textBox3.Text;
                int empId = int.Parse(textBox4.Text);
                Employee newEmp = new Employee();
                newEmp.Name = empName;
                newEmp.DeptId = ChosenDep;

                newEmp.Id = empId;
                var IdCheck = Entity.Employees.Find(empId);
                if (IdCheck == null)
                {
                    Entity.Employees.Add(newEmp);
                    listBox1.Items.Add(empName);
                }
                else
                {
                    MessageBox.Show("Employee Already Exists!");
                }
                Entity.SaveChanges();
                textBox3.Text = textBox4.Text = "";
            }
            else
            {
                MessageBox.Show("Employee Data Required");
            }
            
        }

        // Update Employee
        private void button5_Click(object sender, EventArgs e)
        {
            int EmpId = int.Parse(textBox4.Text);
            string Empname = textBox3.Text;
            Employee Emp = Entity.Employees.Find(EmpId);
            if (Emp != null)
            {
                Emp.Name = Empname;
                Entity.SaveChanges();
            }
            textBox3.Text = textBox4.Text = "";
            RefreshListBox();
        }


        // Delete Employee
        private void button6_Click(object sender, EventArgs e)
        {

            int EmpId = int.Parse(textBox4.Text);
            Employee Emp = Entity.Employees.Find(EmpId);
            if (Emp == null)
            {
                MessageBox.Show("Employee Doesn't Exist");
            }
            else
            {
                Entity.Employees.Remove(Emp);
                Entity.SaveChanges();
                textBox3.Text = textBox4.Text = "";
                RefreshListBox();
            }
        }
    }
}
