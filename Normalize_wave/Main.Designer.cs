namespace Normalize_wave {
  partial class Main {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      Run_Button = new Button();
      FileListBox = new ListBox();
      OpenOut_Button = new Button();
      OpenIn_Button = new Button();
      label3 = new Label();
      label2 = new Label();
      OutputFile = new TextBox();
      label1 = new Label();
      InputFile = new TextBox();
      pictureBox1 = new PictureBox();
      EnableInputOutput = new CheckBox();
      EnableAllSearch = new CheckBox();
      Reload_Button = new Button();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
      SuspendLayout();
      // 
      // Run_Button
      // 
      Run_Button.Location = new Point(458, 391);
      Run_Button.Name = "Run_Button";
      Run_Button.Size = new Size(98, 23);
      Run_Button.TabIndex = 6;
      Run_Button.Text = "実行";
      Run_Button.UseVisualStyleBackColor = true;
      Run_Button.Click += Run_Button_Click;
      // 
      // FileListBox
      // 
      FileListBox.FormattingEnabled = true;
      FileListBox.ItemHeight = 15;
      FileListBox.Location = new Point(12, 170);
      FileListBox.Name = "FileListBox";
      FileListBox.Size = new Size(544, 199);
      FileListBox.TabIndex = 0;
      // 
      // OpenOut_Button
      // 
      OpenOut_Button.Location = new Point(512, 92);
      OpenOut_Button.Name = "OpenOut_Button";
      OpenOut_Button.Size = new Size(25, 23);
      OpenOut_Button.TabIndex = 4;
      OpenOut_Button.Text = "...";
      OpenOut_Button.UseVisualStyleBackColor = true;
      OpenOut_Button.Click += OpenOut_Button_Click;
      // 
      // OpenIn_Button
      // 
      OpenIn_Button.Location = new Point(512, 47);
      OpenIn_Button.Name = "OpenIn_Button";
      OpenIn_Button.Size = new Size(25, 23);
      OpenIn_Button.TabIndex = 2;
      OpenIn_Button.Text = "...";
      OpenIn_Button.UseVisualStyleBackColor = true;
      OpenIn_Button.Click += OpenIn_Button_Click;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(24, 4);
      label3.Name = "label3";
      label3.Size = new Size(74, 15);
      label3.TabIndex = 0;
      label3.Text = "フォルダー指定";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(23, 74);
      label2.Name = "label2";
      label2.Size = new Size(45, 15);
      label2.TabIndex = 0;
      label2.Text = "Output";
      // 
      // OutputFile
      // 
      OutputFile.Location = new Point(23, 92);
      OutputFile.Name = "OutputFile";
      OutputFile.Size = new Size(483, 23);
      OutputFile.TabIndex = 3;
      OutputFile.Leave += OutputFile_Leave;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(23, 28);
      label1.Name = "label1";
      label1.Size = new Size(35, 15);
      label1.TabIndex = 0;
      label1.Text = "Input";
      // 
      // InputFile
      // 
      InputFile.Location = new Point(23, 46);
      InputFile.Name = "InputFile";
      InputFile.Size = new Size(483, 23);
      InputFile.TabIndex = 1;
      InputFile.Leave += InputFile_Leave;
      // 
      // pictureBox1
      // 
      pictureBox1.BorderStyle = BorderStyle.FixedSingle;
      pictureBox1.Location = new Point(12, 12);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new Size(544, 138);
      pictureBox1.TabIndex = 14;
      pictureBox1.TabStop = false;
      // 
      // EnableInputOutput
      // 
      EnableInputOutput.AutoSize = true;
      EnableInputOutput.Location = new Point(24, 121);
      EnableInputOutput.Name = "EnableInputOutput";
      EnableInputOutput.Size = new Size(254, 19);
      EnableInputOutput.TabIndex = 5;
      EnableInputOutput.Text = "Inputで設定したフォルダーと同じ場所を選択する";
      EnableInputOutput.UseVisualStyleBackColor = true;
      EnableInputOutput.CheckedChanged += EnableInputOutput_CheckedChanged;
      // 
      // EnableAllSearch
      // 
      EnableAllSearch.AutoSize = true;
      EnableAllSearch.Location = new Point(23, 375);
      EnableAllSearch.Name = "EnableAllSearch";
      EnableAllSearch.Size = new Size(196, 19);
      EnableAllSearch.TabIndex = 15;
      EnableAllSearch.Text = "mp3, wav以外のファイルも検索する\r\n";
      EnableAllSearch.UseVisualStyleBackColor = true;
      EnableAllSearch.CheckedChanged += EnableAllSearch_CheckedChanged;
      // 
      // Reload_Button
      // 
      Reload_Button.Location = new Point(408, 121);
      Reload_Button.Name = "Reload_Button";
      Reload_Button.Size = new Size(98, 23);
      Reload_Button.TabIndex = 16;
      Reload_Button.Text = "更新";
      Reload_Button.UseVisualStyleBackColor = true;
      Reload_Button.Click += Reload_Button_Click;
      // 
      // Main
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(577, 426);
      Controls.Add(Reload_Button);
      Controls.Add(EnableAllSearch);
      Controls.Add(EnableInputOutput);
      Controls.Add(Run_Button);
      Controls.Add(FileListBox);
      Controls.Add(OpenOut_Button);
      Controls.Add(OpenIn_Button);
      Controls.Add(label3);
      Controls.Add(label2);
      Controls.Add(OutputFile);
      Controls.Add(label1);
      Controls.Add(InputFile);
      Controls.Add(pictureBox1);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "Main";
      ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Button Run_Button;
    private ListBox FileListBox;
    private Button OpenOut_Button;
    private Button OpenIn_Button;
    private Label label3;
    private Label label2;
    private TextBox OutputFile;
    private Label label1;
    private TextBox InputFile;
    private PictureBox pictureBox1;
    private CheckBox EnableInputOutput;
    private CheckBox EnableAllSearch;
    private Button Reload_Button;
  }
}