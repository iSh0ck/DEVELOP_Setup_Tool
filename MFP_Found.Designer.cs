namespace Vela31_Ineo
{
    partial class MFP_Found
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MFP_Found));
            listView1 = new System.Windows.Forms.ListView();
            this.column_IP_Addr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_model = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_select_found_printer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_IP_Addr,
            this.column_model});
            listView1.HideSelection = false;
            listView1.Location = new System.Drawing.Point(12, 12);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(336, 172);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            // 
            // column_IP_Addr
            // 
            this.column_IP_Addr.Text = "IP Address";
            this.column_IP_Addr.Width = 120;
            // 
            // column_model
            // 
            this.column_model.Text = "Model name";
            this.column_model.Width = 210;
            // 
            // btn_select_found_printer
            // 
            this.btn_select_found_printer.Location = new System.Drawing.Point(139, 192);
            this.btn_select_found_printer.Name = "btn_select_found_printer";
            this.btn_select_found_printer.Size = new System.Drawing.Size(75, 23);
            this.btn_select_found_printer.TabIndex = 1;
            this.btn_select_found_printer.Text = "SELECT";
            this.btn_select_found_printer.UseVisualStyleBackColor = true;
            this.btn_select_found_printer.Click += new System.EventHandler(this.btn_select_found_printer_Click);
            // 
            // MFP_Found
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 227);
            this.Controls.Add(this.btn_select_found_printer);
            this.Controls.Add(listView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MFP_Found";
            this.Text = "MFP_Found";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_select_found_printer;
        public static System.Windows.Forms.ListView listView1;
        public System.Windows.Forms.ColumnHeader column_IP_Addr;
        public System.Windows.Forms.ColumnHeader column_model;
    }
}