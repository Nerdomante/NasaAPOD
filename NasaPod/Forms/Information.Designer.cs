﻿namespace Nasa
{
    partial class Information
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Information));
            Description = new RichTextBox();
            Title = new Label();
            More = new Button();
            TranslateLANG = new Button();
            TranslateEN = new Button();
            SuspendLayout();
            // 
            // Description
            // 
            Description.BorderStyle = BorderStyle.None;
            Description.Location = new Point(10, 25);
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.ScrollBars = RichTextBoxScrollBars.Vertical;
            Description.Size = new Size(380, 250);
            Description.TabIndex = 0;
            Description.TabStop = false;
            Description.Text = "";
            // 
            // Title
            // 
            Title.AutoSize = true;
            Title.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            Title.Location = new Point(10, 0);
            Title.Name = "Title";
            Title.Size = new Size(0, 19);
            Title.TabIndex = 1;
            // 
            // More
            // 
            More.Location = new Point(328, 285);
            More.Name = "More";
            More.Size = new Size(60, 25);
            More.TabIndex = 3;
            More.Text = "More";
            More.UseVisualStyleBackColor = true;
            More.Click += More_Click;
            // 
            // TranslateLANG
            // 
            TranslateLANG.Location = new Point(250, 285);
            TranslateLANG.Name = "TranslateLANG";
            TranslateLANG.Size = new Size(65, 25);
            TranslateLANG.TabIndex = 4;
            TranslateLANG.Text = "Translate";
            TranslateLANG.UseVisualStyleBackColor = true;
            TranslateLANG.Click += TranslateLANG_Click;
            // 
            // TranslateEN
            // 
            TranslateEN.Location = new Point(250, 285);
            TranslateEN.Name = "TranslateEN";
            TranslateEN.Size = new Size(65, 25);
            TranslateEN.TabIndex = 4;
            TranslateEN.Text = "Translate";
            TranslateEN.UseVisualStyleBackColor = true;
            TranslateEN.Visible = false;
            TranslateEN.Click += TranslateENG_Click;
            // 
            // Information
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 320);
            Controls.Add(More);
            Controls.Add(TranslateLANG);
            Controls.Add(TranslateEN);
            Controls.Add(Title);
            Controls.Add(Description);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Information";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Information";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox Description;
        private Label Title;
        private Button More;
        private Button TranslateLANG;
        private Button TranslateEN;
    }
}