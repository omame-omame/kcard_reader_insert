namespace guitar_test
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_open = new System.Windows.Forms.Button();
            this.combo_com = new System.Windows.Forms.ComboBox();
            this.button_init_all = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.SuspendLayout();
            // 
            // button_open
            // 
            this.button_open.Location = new System.Drawing.Point(12, 60);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(274, 38);
            this.button_open.TabIndex = 0;
            this.button_open.Text = "Open";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // combo_com
            // 
            this.combo_com.FormattingEnabled = true;
            this.combo_com.Location = new System.Drawing.Point(12, 17);
            this.combo_com.Name = "combo_com";
            this.combo_com.Size = new System.Drawing.Size(274, 26);
            this.combo_com.TabIndex = 2;
            // 
            // button_init_all
            // 
            this.button_init_all.Location = new System.Drawing.Point(12, 104);
            this.button_init_all.Name = "button_init_all";
            this.button_init_all.Size = new System.Drawing.Size(274, 68);
            this.button_init_all.TabIndex = 10;
            this.button_init_all.Text = "Start";
            this.button_init_all.UseVisualStyleBackColor = true;
            this.button_init_all.Click += new System.EventHandler(this.button_init_all_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // serialPort2
            // 
            this.serialPort2.DtrEnable = true;
            this.serialPort2.RtsEnable = true;
            this.serialPort2.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort2_DataReceived_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 184);
            this.Controls.Add(this.button_init_all);
            this.Controls.Add(this.combo_com);
            this.Controls.Add(this.button_open);
            this.MaximumSize = new System.Drawing.Size(320, 240);
            this.MinimumSize = new System.Drawing.Size(320, 240);
            this.Name = "Form1";
            this.Text = "CARDREADER";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.ComboBox combo_com;
        private System.Windows.Forms.Button button_init_all;
        private System.Windows.Forms.Timer timer1;
        private System.IO.Ports.SerialPort serialPort2;
    }
}

