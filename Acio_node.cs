using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace guitar_test
{
    class Acio_node
    {
        private struct Packet {
            public byte header;
            public byte device_id;
            public byte type;
            public byte command;
            public byte req_id;
            public byte data_length;
            public byte[] data;
            public byte sum;
        }
        Packet packet;
        private int req_id_in_class;
        public Acio_node()
        {
            packet = new Packet();
        }
        public byte[] make_packet(byte Device_id,byte Type, byte Command, byte Req_id, byte[] Data)
        {
            packet.header = 0xAA;
            packet.device_id = Device_id;
            packet.type = Type;
            packet.command = Command;
            packet.req_id = (byte)req_id_in_class;
            req_id_in_class++;
            if (req_id_in_class > 0xFF)
            {
                req_id_in_class = 0;
            }
            packet.data_length = Convert.ToByte(Data.Length);
            packet.data = new byte[packet.data_length];
            Array.Copy(Data, packet.data, packet.data_length);
            packet.sum = 0x00;

            return encode_packet(packet);
        }
        private byte[] encode_packet(Packet packet)
        {


            byte[] temp_buf = new byte[512];
            byte[] ret_buf = new byte[512];
            temp_buf[0] = packet.header;
            temp_buf[1] = packet.device_id;
            temp_buf[2] = packet.type;
            temp_buf[3] = packet.command;
            temp_buf[4] = packet.req_id;
            temp_buf[5] = packet.data_length;
            Array.Copy(packet.data, 0, temp_buf, 6, packet.data_length);

            //calc sum
            byte sum = 0;
            int bufsize = 7 + temp_buf[5];

            for (int i = 1; i < bufsize ; i++)
            {
                sum += temp_buf[i];
            }
            temp_buf[bufsize-1] = sum;

            int ret_buf_index = 1;
            ret_buf[0] = temp_buf[0];
            for (int i = 1; i < bufsize ; i++)
            {
                if (temp_buf[i] == 0xAA || temp_buf[i]  == 0xFF) // escape these chars
                {
                    temp_buf[i] = Convert.ToByte((~temp_buf[i])&0xFF);
                    ret_buf[ret_buf_index] = 0xFF ;
                    ret_buf_index++;
                }
                ret_buf[ret_buf_index] = temp_buf[i];
                ret_buf_index++;

            }
            byte[] ret = new byte[ret_buf_index];
            Array.Copy(ret_buf, ret, ret_buf_index);
            return ret;
        }
        public byte[] decode_packet(byte[] packet)
        {


            byte[] temp_buf = new byte[512];
            byte[] ret_buf = new byte[512];

            int ret_buf_index = 0;
            int dec_length = packet.Length;
            ret_buf[0] = temp_buf[0];
            for (int i = 0; i < packet.Length ; i++)
            {
                if (ret_buf_index < packet.Length)
                {
                    if (packet[ret_buf_index] == 0xFF)
                    {
                        ret_buf[i] = Convert.ToByte((~packet[ret_buf_index + 1]) & 0xFF);
                        ret_buf_index++;
                        ret_buf_index++;
                        dec_length--;
                    }
                    else
                    {
                        ret_buf[i] = packet[ret_buf_index];
                        ret_buf_index++;
                    }
                }
                else
                {
                    //ret_buf[i] = packet[ret_buf_index];
                     //   ret_buf_index++;
                }

            }
            byte[] ret = new byte[dec_length];
            Array.Copy(ret_buf, ret, dec_length);
            return ret;
        }
        
        public string hex_to_string(byte[] data)
        {
            string ret = "";
            for(int i=0;i<data.Length;i++)
            {
                ret = ret + ":" + data[i].ToString("X2");
            }
            return ret;
        }
        public void set_reqid(int i)
        {
            req_id_in_class = i;
        }
    }
}
