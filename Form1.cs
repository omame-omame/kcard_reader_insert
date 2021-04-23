using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
namespace guitar_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //0xAAが返ってきたらデバイスがあると仮定する
        private bool is_device_ok;
        
        //デバイスからの応答データで0xAAから何文字目か
        private int ret_data_req_index;
        //カードがマウントされてからのカウント
        private int card_mount_count;

        Acio_node node;
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                combo_com.Items.Add(port);
            }
            if (combo_com.Items.Count != 0)
            {
                combo_com.SelectedIndex = 0;
            }
            is_device_ok = false;            
            node = new Acio_node();
        }

        private void acio_device_find()
        {
            byte[] send_data = new byte[1];
            send_data[0] = 0xAA;
            is_device_ok = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 1024; j++)
                {
                    serialPort2.Write(send_data, 0, 1);
                }
                if (serialPort2.BytesToRead != 0)
                {
                    if (serialPort2.ReadByte().ToString("X2") == "AA")
                    {
                        is_device_ok = true;
                        serialPort2.DiscardInBuffer();
                        break;
                    }
                }
            }
        }
        private void acio_init()
        {
            byte[] send_data = new byte[10];

            send_data = new byte[8];
            send_data[0] = 0xAA;
            send_data[1] = 0x00;
            send_data[2] = 0x00;
            send_data[3] = 0x01;
            send_data[4] = 0x01;
            send_data[5] = 0x01;
            send_data[6] = 0x00;
            send_data[7] = 0x03;
            serialPort2.Write(send_data, 0, send_data.Length);

            send_data = new byte[7];
            send_data[0] = 0xAA;
            send_data[1] = 0x00;
            send_data[2] = 0x00;
            send_data[3] = 0x02;
            send_data[4] = 0x02;
            send_data[5] = 0x00;
            send_data[6] = 0x05;
            serialPort2.Write(send_data, 0, send_data.Length);

            send_data = new byte[7];
            send_data[0] = 0xAA;
            send_data[1] = 0x01;
            send_data[2] = 0x00;
            send_data[3] = 0x03;
            send_data[4] = 0x03;
            send_data[5] = 0x00;
            send_data[6] = 0x07;
            serialPort2.Write(send_data, 0, send_data.Length);


            send_data = new byte[8];
            send_data[0] = 0xAA;
            send_data[1] = 0x01;
            send_data[2] = 0x01;
            send_data[3] = 0x30;
            send_data[4] = 0x04;
            send_data[5] = 0x01;
            send_data[6] = 0x00;
            send_data[7] = 0x37;
            serialPort2.Write(send_data, 0, send_data.Length);
        }

        private void get_packet()
        {
            if (is_device_ok == true)
            {
                for (int i = 0; i < serialPort2.BytesToRead; i++)
                {
                    string temp = serialPort2.ReadByte().ToString("X2");
                    if(temp=="AA")
                    {
                        ret_data_req_index = 0;
                    }
                    ret_data_req_index++;
                    if (ret_data_req_index == 8)
                    {
                        this.Text = temp;
                        if(temp=="30")
                        {
                            card_mount_count++;
                        }
                    }

                }
            }
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            try
            {
                if (button_open.Text == "Open")
                {
                    this.Text = "CARDREADER";
                    serialPort2.PortName = combo_com.SelectedItem.ToString();
                    serialPort2.BaudRate = 57600;
                    serialPort2.Open();
                    serialPort2.DiscardInBuffer();
                    node.set_reqid(5);
                    button_open.Text = "Close";
                }else
                {
                    serialPort2.Close();
                    button_open.Text = "Open";
                }
            }
            catch
            {
                    button_open.Text = "Open";
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            try
            {
                if (card_mount_count == 10)
                {
                    icca_set_status(0x12);
                    icca_set_status(0x10);
                    icca_set_status(0x11);
                    card_mount_count = 0;
                } else{
                    icca_set_status(0x11);
                }
                icca_get_status();
            }
            catch (Exception e2)
            {
            }

        }

        //ボタン類
        private void button_init_all_Click(object sender, EventArgs e)
        {
            if (button_init_all.Text == "Start")
            {
                acio_device_find();
                if (is_device_ok == true)
                {
                    button_init_all.Text = "Stop";
                    acio_init();
                    icca_set_status(0x12);
                    icca_get_status();
                    timer1.Enabled = true;
                }
            }
            else
            {
                timer1.Enabled = false;
                button_init_all.Text = "Start";
            }
        }
        private void icca_set_status(int command)
        {
            byte[] send_data = new byte[2];
            send_data[0] = 0x10;
            send_data[1] = (byte)command;
            byte[] data = node.make_packet(0x01, 0x01, 0x35, 0x01, send_data);
            serialPort2.Write(data, 0, data.Length);
        }
        private void icca_get_status()
        {
            byte[] send_data = new byte[1];
            send_data[0] = 0x10;
            byte[] data = node.make_packet(0x01, 0x01, 0x34, 0x00, send_data);
            serialPort2.Write(data, 0, data.Length);
        }

        private void serialPort2_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
           get_packet();
        }
    }
}