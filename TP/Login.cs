using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;

namespace TP
{
    public partial class Login : Form
    {
        Main main;
        private string DB_Server_Info = "Data Source = localhost;" +
           "User ID = system; Password = 1228;";


        public bool test = false;
        TextBox[] txtList;
        const string IdPlaceholder = "아이디";
        const string PwPlaceholder = "비밀번호";

        public Login(Main main)
        {
            InitializeComponent();
            this.main = main;
            txtList = new TextBox[] { textBox1, textBox2 };
            foreach (var txt in txtList)
            {
                txt.ForeColor = Color.DarkGray;
                if (txt == textBox1)
                    txt.Text = IdPlaceholder;
                else if (txt == textBox2)
                    txt.Text = PwPlaceholder;
                txt.GotFocus += RemovePlaceholder;
                txt.LostFocus += SetPlaceholder;
            }
        }
        private void RemovePlaceholder(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Text == IdPlaceholder | txt.Text == PwPlaceholder)
            {
                txt.ForeColor = Color.Black;
                txt.Text = String.Empty;
                if (txt == textBox2)
                    textBox2.PasswordChar = '●';
            }
        }
        private void SetPlaceholder(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (string.IsNullOrEmpty(txt.Text))
            {
                txt.ForeColor = Color.DarkGray;
                if (txt == textBox1)
                    txt.Text = IdPlaceholder;
                else if (txt == textBox2)
                {
                    txt.Text = PwPlaceholder;
                    textBox2.PasswordChar = default;
                }
            }
        }

        private int ls = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            string sqltxt = "select * from 회원";
            OracleConnection conn = new OracleConnection(DB_Server_Info); //db 연결
            conn.Open();
            string id = textBox1.Text;
            string pw = textBox2.Text;
            string strSelect = "SELECT * from 회원 where 회원아이디 = " + $"'{id}'";
            OracleCommand cmd = new OracleCommand(sqltxt, conn);
            OracleDataReader reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                string db_id = reader["회원아이디"].ToString().Trim(); //db상 아이디 비번뒤 공백 삭제
                string db_pw = reader["회원비번"].ToString().Trim();
                if (textBox1.Text == IdPlaceholder || textBox2.Text == PwPlaceholder)
                {
                    MessageBox.Show("ID 또는 Password를입력하세요.");
                }
                else if (db_id == id)
                {
                    if (db_pw == pw)
                    {
                        ls = 1;
                        MessageBox.Show("로그인에 성공했습니다.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("잘못된 비밀번호 입니다.");
                    }
                }
                else
                {
                    MessageBox.Show("사용자 정보가 없습니다.");
                }
            }

            conn.Close();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ls != 1) //로그인 성공하면 그냥 메인문도 닫히기 때문에 로그인 성공하지 못한상태에서 닫을시 메인문도 닫힘
                main.Close();
        }
    }
}
