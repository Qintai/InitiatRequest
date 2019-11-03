namespace InitiatRequest
{
    partial class ReadExcel
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
            this.ExcelList = new CCWin.SkinControl.SkinListBox();
            this.actionName = new CCWin.SkinControl.SkinLabel();
            this.SuspendLayout();
            // 
            // ExcelList
            // 
            this.ExcelList.Back = null;
            this.ExcelList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ExcelList.FormattingEnabled = true;
            this.ExcelList.Location = new System.Drawing.Point(19, 51);
            this.ExcelList.Name = "ExcelList";
            this.ExcelList.Size = new System.Drawing.Size(164, 277);
            this.ExcelList.TabIndex = 0;
            // 
            // actionName
            // 
            this.actionName.AutoSize = true;
            this.actionName.BackColor = System.Drawing.Color.Transparent;
            this.actionName.BorderColor = System.Drawing.Color.White;
            this.actionName.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.actionName.Location = new System.Drawing.Point(189, 51);
            this.actionName.Name = "actionName";
            this.actionName.Size = new System.Drawing.Size(226, 52);
            this.actionName.TabIndex = 1;
            this.actionName.Text = "skinLabel1";
            // 
            // ReadExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 370);
            this.Controls.Add(this.actionName);
            this.Controls.Add(this.ExcelList);
            this.Name = "ReadExcel";
            this.Text = "ReadExcel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinListBox ExcelList;
        private CCWin.SkinControl.SkinLabel actionName;
    }
}