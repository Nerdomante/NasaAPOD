namespace Nasa
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
        private void InitializeComponent(string title, string description)
        {
            Description = new RichTextBox();
            Title = new Label();
            More = new Button();
            Translate = new Button();
            SuspendLayout();
            // 
            // Description
            // 
            Description.Location = new Point(10, 25);
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.ScrollBars = RichTextBoxScrollBars.Vertical;
            Description.ShowSelectionMargin = false;
            Description.Size = new Size(380, 250);
            Description.TabIndex = 0;
            Description.TabStop = false;
            Description.Text = description;
            Description.BorderStyle = BorderStyle.None;
            // 
            // Title
            // 
            Title.AutoSize = true;
            Title.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            Title.Location = new Point(10, 0);
            Title.Name = "Title";
            Title.Size = new Size(54, 50);
            Title.TabIndex = 1;
            Title.Text = title;
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
            // Translate
            // 
            Translate.Location = new Point(250, 285);
            Translate.Name = "Translate";
            Translate.Size = new Size(65, 25);
            Translate.TabIndex = 4;
            Translate.Text = "Translate";
            Translate.UseVisualStyleBackColor = true;
            Translate.Click += Translate_Click;
            // 
            // Information
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 320);
            Controls.Add(More);
            Controls.Add(Translate);
            Controls.Add(Title);
            Controls.Add(Description);
            Name = "Information";
            Text = "Information";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Icon = new Icon("Res/icon.ico");
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox Description;
        private Label Title;
        private Button More;
        private Button Translate;
    }
}