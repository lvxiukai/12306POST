using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms12306POST
{
    public partial class Form1 : Form
    {
        CookieContainer Cookies = new CookieContainer();
        HttpWebRequest myHttpWebRequest;
        byte[] oneData = { };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread t2 = new Thread(new ThreadStart(LoadCodeImg));
            t2.Start();
        }

        private void LoadCodeImg()
        {
            try
            {
                string oneUrl = "http://wssb2.szsi.gov.cn/NetApplyWeb/CImages";
                oneUrl = "http://localhost:84/Ajax/VerifyImage";
                oneUrl = "https://kyfw.12306.cn/otn/passcodeNew/getPassCodeNew?module=login&rand=sjrand";

                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                myHttpWebRequest = (HttpWebRequest)WebRequest.Create(oneUrl);//请求的URL
                myHttpWebRequest.ClientCertificates.Add(new X509Certificate("srca12306\\srca.cera"));

                myHttpWebRequest.CookieContainer = Cookies;//*发送COOKIE
                myHttpWebRequest.Method = "GET";
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";

                //获取返回资源
                HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();

                //获取流
                Image bitmapImage = Bitmap.FromStream(response.GetResponseStream()) as Bitmap;

                this.pictureBox1.Image = bitmapImage;
            }
            catch (Exception ex)
            {
                server(ex.Message);
                //throw new Exception(ex.Message);
            }
        }

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 总是接受  
            return true;
        }

        public delegate void CreateErrorLog();

        public void server(string err)
        {
            CreateErrorLog callback = delegate()//使用委托
            {
                this.label1.Text = err;
            };
            label1.Invoke(callback);

        }

    }
}
