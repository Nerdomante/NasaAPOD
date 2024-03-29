﻿namespace Nasa
{
    partial class FormSettings
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            labelBlurLevel = new Label();
            textBoxApiKey = new TextBox();
            textBoxEndpoint = new TextBox();
            textBoxFillerImage = new TextBox();
            labelApiKey = new Label();
            labelEndpoint = new Label();
            labelHoursInterval = new Label();
            labelLanguage = new Label();
            labelFillerPath = new Label();
            labelRatio = new Label();
            labelScaleThresholdHeight = new Label();
            labelScaleThresholdWidth = new Label();
            labelFillerTransparency = new Label();
            helperTooltip = new ToolTip(components);
            numericBlurLevel = new NumericUpDown();
            numericHoursInterval = new NumericUpDown();
            numericRatio = new NumericUpDown();
            numericFillerTransparency = new NumericUpDown();
            numericScaleThresholdWidth = new NumericUpDown();
            numericScaleThresholdHeight = new NumericUpDown();
            buttonSaveSettings = new Button();
            listBoxLanguages = new ListBox();
            checkBoxAutostart = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)numericBlurLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericHoursInterval).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericRatio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericFillerTransparency).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericScaleThresholdWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericScaleThresholdHeight).BeginInit();
            SuspendLayout();
            // 
            // labelBlurLevel
            // 
            labelBlurLevel.AutoSize = true;
            labelBlurLevel.Location = new Point(305, 152);
            labelBlurLevel.Name = "labelBlurLevel";
            labelBlurLevel.Size = new Size(45, 15);
            labelBlurLevel.TabIndex = 0;
            labelBlurLevel.Text = "Blur Lvl";
            // 
            // textBoxApiKey
            // 
            textBoxApiKey.Location = new Point(63, 9);
            textBoxApiKey.Name = "textBoxApiKey";
            textBoxApiKey.Size = new Size(341, 23);
            textBoxApiKey.TabIndex = 2;
            // 
            // textBoxEndpoint
            // 
            textBoxEndpoint.Location = new Point(71, 38);
            textBoxEndpoint.Name = "textBoxEndpoint";
            textBoxEndpoint.Size = new Size(332, 23);
            textBoxEndpoint.TabIndex = 3;
            // 
            // textBoxFillerImage
            // 
            textBoxFillerImage.Location = new Point(84, 67);
            textBoxFillerImage.Name = "textBoxFillerImage";
            textBoxFillerImage.Size = new Size(320, 23);
            textBoxFillerImage.TabIndex = 6;
            // 
            // labelApiKey
            // 
            labelApiKey.AutoSize = true;
            labelApiKey.Location = new Point(10, 12);
            labelApiKey.Name = "labelApiKey";
            labelApiKey.Size = new Size(47, 15);
            labelApiKey.TabIndex = 11;
            labelApiKey.Text = "Api Key";
            // 
            // labelEndpoint
            // 
            labelEndpoint.AutoSize = true;
            labelEndpoint.Location = new Point(10, 41);
            labelEndpoint.Name = "labelEndpoint";
            labelEndpoint.Size = new Size(55, 15);
            labelEndpoint.TabIndex = 12;
            labelEndpoint.Text = "Endpoint";
            // 
            // labelHoursInterval
            // 
            labelHoursInterval.AutoSize = true;
            labelHoursInterval.Location = new Point(150, 118);
            labelHoursInterval.Name = "labelHoursInterval";
            labelHoursInterval.Size = new Size(87, 15);
            labelHoursInterval.TabIndex = 13;
            labelHoursInterval.Text = "Hours Interaval";
            // 
            // labelLanguage
            // 
            labelLanguage.AutoSize = true;
            labelLanguage.Location = new Point(10, 97);
            labelLanguage.Name = "labelLanguage";
            labelLanguage.Size = new Size(62, 15);
            labelLanguage.TabIndex = 14;
            labelLanguage.Text = "Language:";
            // 
            // labelFillerPath
            // 
            labelFillerPath.AutoSize = true;
            labelFillerPath.Location = new Point(10, 70);
            labelFillerPath.Name = "labelFillerPath";
            labelFillerPath.Size = new Size(68, 15);
            labelFillerPath.TabIndex = 15;
            labelFillerPath.Text = "Filler Image";
            // 
            // labelRatio
            // 
            labelRatio.AutoSize = true;
            labelRatio.Location = new Point(305, 186);
            labelRatio.Name = "labelRatio";
            labelRatio.Size = new Size(34, 15);
            labelRatio.TabIndex = 16;
            labelRatio.Text = "Ratio";
            // 
            // labelScaleThresholdHeight
            // 
            labelScaleThresholdHeight.AutoSize = true;
            labelScaleThresholdHeight.Location = new Point(150, 186);
            labelScaleThresholdHeight.Name = "labelScaleThresholdHeight";
            labelScaleThresholdHeight.Size = new Size(107, 15);
            labelScaleThresholdHeight.TabIndex = 17;
            labelScaleThresholdHeight.Text = "Scale Thres. Height";
            // 
            // labelScaleThresholdWidth
            // 
            labelScaleThresholdWidth.AutoSize = true;
            labelScaleThresholdWidth.Location = new Point(150, 152);
            labelScaleThresholdWidth.Name = "labelScaleThresholdWidth";
            labelScaleThresholdWidth.Size = new Size(103, 15);
            labelScaleThresholdWidth.TabIndex = 18;
            labelScaleThresholdWidth.Text = "Scale Thres. Width";
            // 
            // labelFillerTransparency
            // 
            labelFillerTransparency.AutoSize = true;
            labelFillerTransparency.Location = new Point(305, 118);
            labelFillerTransparency.Name = "labelFillerTransparency";
            labelFillerTransparency.Size = new Size(48, 15);
            labelFillerTransparency.TabIndex = 19;
            labelFillerTransparency.Text = "Opacity";
            // 
            // helperTooltip
            // 
            helperTooltip.AutoPopDelay = 30000;
            helperTooltip.InitialDelay = 100;
            helperTooltip.IsBalloon = true;
            helperTooltip.ReshowDelay = 0;
            helperTooltip.ShowAlways = true;
            helperTooltip.ToolTipIcon = ToolTipIcon.Info;
            helperTooltip.ToolTipTitle = "Help";
            // 
            // numericBlurLevel
            // 
            numericBlurLevel.Location = new Point(362, 150);
            numericBlurLevel.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericBlurLevel.Name = "numericBlurLevel";
            numericBlurLevel.Size = new Size(40, 23);
            numericBlurLevel.TabIndex = 21;
            // 
            // numericHoursInterval
            // 
            numericHoursInterval.Location = new Point(259, 116);
            numericHoursInterval.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericHoursInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericHoursInterval.Name = "numericHoursInterval";
            numericHoursInterval.Size = new Size(40, 23);
            numericHoursInterval.TabIndex = 22;
            numericHoursInterval.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // numericRatio
            // 
            numericRatio.Location = new Point(362, 184);
            numericRatio.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericRatio.Name = "numericRatio";
            numericRatio.Size = new Size(40, 23);
            numericRatio.TabIndex = 23;
            // 
            // numericFillerTransparency
            // 
            numericFillerTransparency.Location = new Point(362, 116);
            numericFillerTransparency.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericFillerTransparency.Name = "numericFillerTransparency";
            numericFillerTransparency.Size = new Size(40, 23);
            numericFillerTransparency.TabIndex = 24;
            // 
            // numericScaleThresholdWidth
            // 
            numericScaleThresholdWidth.Location = new Point(259, 150);
            numericScaleThresholdWidth.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericScaleThresholdWidth.Name = "numericScaleThresholdWidth";
            numericScaleThresholdWidth.Size = new Size(40, 23);
            numericScaleThresholdWidth.TabIndex = 25;
            // 
            // numericScaleThresholdHeight
            // 
            numericScaleThresholdHeight.Location = new Point(259, 184);
            numericScaleThresholdHeight.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericScaleThresholdHeight.Name = "numericScaleThresholdHeight";
            numericScaleThresholdHeight.Size = new Size(40, 23);
            numericScaleThresholdHeight.TabIndex = 26;
            // 
            // buttonSaveSettings
            // 
            buttonSaveSettings.Location = new Point(329, 217);
            buttonSaveSettings.Name = "buttonSaveSettings";
            buttonSaveSettings.Size = new Size(75, 23);
            buttonSaveSettings.TabIndex = 27;
            buttonSaveSettings.Text = "Save";
            buttonSaveSettings.UseVisualStyleBackColor = true;
            buttonSaveSettings.Click += buttonSaveSettings_Click;
            // 
            // listBoxLanguages
            // 
            listBoxLanguages.FormattingEnabled = true;
            listBoxLanguages.ItemHeight = 15;
            listBoxLanguages.Location = new Point(12, 115);
            listBoxLanguages.Name = "listBoxLanguages";
            listBoxLanguages.ScrollAlwaysVisible = true;
            listBoxLanguages.Size = new Size(134, 124);
            listBoxLanguages.TabIndex = 29;
            // 
            // checkBoxAutostart
            // 
            checkBoxAutostart.AutoSize = true;
            checkBoxAutostart.Location = new Point(150, 220);
            checkBoxAutostart.Name = "checkBoxAutostart";
            checkBoxAutostart.Size = new Size(155, 19);
            checkBoxAutostart.TabIndex = 30;
            checkBoxAutostart.Text = "Start at Windows startup";
            checkBoxAutostart.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(419, 251);
            Controls.Add(checkBoxAutostart);
            Controls.Add(listBoxLanguages);
            Controls.Add(buttonSaveSettings);
            Controls.Add(numericScaleThresholdHeight);
            Controls.Add(numericScaleThresholdWidth);
            Controls.Add(numericFillerTransparency);
            Controls.Add(numericRatio);
            Controls.Add(numericHoursInterval);
            Controls.Add(numericBlurLevel);
            Controls.Add(labelFillerTransparency);
            Controls.Add(labelScaleThresholdWidth);
            Controls.Add(labelScaleThresholdHeight);
            Controls.Add(labelRatio);
            Controls.Add(labelFillerPath);
            Controls.Add(labelLanguage);
            Controls.Add(labelHoursInterval);
            Controls.Add(labelEndpoint);
            Controls.Add(labelApiKey);
            Controls.Add(textBoxFillerImage);
            Controls.Add(textBoxEndpoint);
            Controls.Add(textBoxApiKey);
            Controls.Add(labelBlurLevel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSettings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)numericBlurLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericHoursInterval).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericRatio).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericFillerTransparency).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericScaleThresholdWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericScaleThresholdHeight).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelBlurLevel;
        private TextBox textBoxApiKey;
        private TextBox textBoxEndpoint;
        private TextBox textBoxFillerImage;
        private Label labelApiKey;
        private Label labelEndpoint;
        private Label labelHoursInterval;
        private Label labelLanguage;
        private Label labelFillerPath;
        private Label labelRatio;
        private Label labelScaleThresholdHeight;
        private Label labelScaleThresholdWidth;
        private Label labelFillerTransparency;
        private ToolTip helperTooltip;
        private NumericUpDown numericBlurLevel;
        private NumericUpDown numericHoursInterval;
        private NumericUpDown numericRatio;
        private NumericUpDown numericFillerTransparency;
        private NumericUpDown numericScaleThresholdWidth;
        private NumericUpDown numericScaleThresholdHeight;
        private Button buttonSaveSettings;
        private ListBox listBoxLanguages;
        private CheckBox checkBoxAutostart;
    }
}