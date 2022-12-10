using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace TP
{

    public partial class Order : Form
    {
        private string DB_Server_Info = "Data Source = localhost;" +
           "User ID = system; Password = 1228;";
        private string categori = "음료";

        public Order()
        {
            InitializeComponent();
            dataview();
        }
        private void dataview()
        {
            try
            {
                string sqltxt = "select * from 제품";
                OracleConnection conn = new OracleConnection(DB_Server_Info);
                conn.Open();
                OracleDataAdapter adapt = new OracleDataAdapter();
                adapt.SelectCommand = new OracleCommand(sqltxt, conn);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                adapt.Fill(ds);
                dt.Reset();
                dt = ds.Tables[0];
                dataGridView1.Columns.Clear();

                dt.DefaultView.RowFilter = $"카테고리 ='{categori}'";
                DataRow[] ca = dt.Select($"카테고리 ='{categori}'");    //카테고리별 테이블 생성
                dataGridView1.AllowUserToAddRows = false; //빈레코드 표시x
                var chkCol = new DataGridViewCheckBoxColumn
                {
                    Name = "chk",
                    HeaderText = "선택"
                };
                dataGridView1.Columns.Add(chkCol);
                dataGridView1.DataSource = dt;   //데이터 추가 부분
                dataGridView1.Columns.Add("발주량", "발주량");
                dataGridView1.Columns.Add("비고", "비고");

                //크기 조절부분 
                dataGridView1.Columns[0].Width = 35;


                //dataGridView1.ReadOnly = true; //전부 읽기 전용           
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[6].ReadOnly = true;
                dataGridView1.Columns[7].ReadOnly = true;

                conn.Close();
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //카테고리 선택시
        public void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource; ///전부를 받아옴

            if (radioButton1.Checked == true)
            {
                categori = radioButton1.Text;
                dataview();
                //DataTable cate = dt.Select($"카테고리 ='{categori}'").CopyToDataTable();
                //DataView dv = new DataView(cate);
                //DataRow[] cate = dt.Select($"카테고리 ='{categori}'");
                //DataTable cate = new DataTable();
                //if(categori == dataGridView1.Columns[1])
                //{

                //}
            }
            else if (radioButton2.Checked == true)
            {
                categori = radioButton2.Text;
                dataview();
                //DataTable cate = dt.Select($"카테고리 ='{categori}'").CopyToDataTable();
                //DataView dv = new DataView(cate);
                //                DataRow[] cate = dt.Select($"카테고리 ='{categori}'");
            }
            else
            {
                categori = radioButton3.Text;
                dataview();

                //DataTable cate = dt.Select($"카테고리 ='{categori}'").CopyToDataTable();
                //DataView dv = new DataView(cate);
                //DataRow[] cate = dt.Select($"카테고리 ='{categori}'");
            }
        }
        
        //검색시 표시
        private void button2_Click(object sender, EventArgs e)
        {
            dataview();     //중복 색칠 방지를 위해
            string standard = comboBox1.Text;   //combobox로 가지고 오기
            string keyword = textBox1.Text;//textbox에 입력된 메시지를 keyword 저장
            string ca = categori;   //카테고리 값

            DataTable dt = (DataTable)dataGridView1.DataSource; ///전부를 받아옴
            DataTable cate = dt.Select($"카테고리 ='{ca}'").CopyToDataTable();  //선택한 카테고리의 값만 넣는 테이블 생성

            try
            {
                DataRow[] dr = cate.Select($"{standard} = '{keyword}'"); //제품명에서 비교
                int i = cate.Rows.IndexOf(dr[0]);     //찾은 배열의 특정컬럼으로뽑기
                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;  //색칠
            }
            catch
            {
                MessageBox.Show("없는 제품입니다.");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //save 부분
        }

        private void Order_FormClosed(object sender, FormClosedEventArgs e)
        {
            //닫혔을때 save 하는지 물어보는 부분 
            MessageBox.Show("저장하시겠습니까?"); //예,아니요,취소 부분 되게 
        }


        

        
    }
}
