using System.Diagnostics;

namespace Normalize_wave {
  public partial class Main : Form {

    // グローバル変数の読み込み
    public Global.GlobalVariableClass gb = new();

    public Main() {
      InitializeComponent();

      // 入出力ファイルの初期状態
      string inputDir = XMLClass.XMLReadMain("Setting", "InputDirectory", "path");
      string outputDir = XMLClass.XMLReadMain("Setting", "OutputDirectory", "path");

      if (outputDir != "^IsEmpty") {
        OutputFile.Text = outputDir;
        gb.G_OutputDir = outputDir;
      }

      if (inputDir != "^IsEmpty") {
        InputFile.Text = inputDir;
        gb.G_InputDir = inputDir;

        string[] fileList = CommonClass.ListUpFile_Input(inputDir, EnableAllSearch.Checked);
        foreach (string file in fileList) FileListBox.Items.Add(file);
      }
    }

    private void EnableInputOutput_CheckedChanged(object sender, EventArgs e) {
      bool enable = EnableInputOutput.Checked;

      // 入力と出力を同一にする
      if (enable == true) {
        string selectFile = InputFile.Text;
        OutputFile.Text = selectFile;
        OutputFile.Enabled = false;
        OpenOut_Button.Enabled = false;
      }
      // 入力と出力を同一にはしない
      else {
        OutputFile.Text = gb.G_OutputDir;
        OutputFile.Enabled = true;
        OpenOut_Button.Enabled = true;
      }
    }

    /* ファイルダイアログ */
    private void OpenIn_Button_Click(object sender, EventArgs e) {
      string selectFile = FileOpenClass.FileOpen(Path.GetDirectoryName(gb.G_InputDir));
      if (string.IsNullOrEmpty(selectFile) == false) {
        InputFile.Text = selectFile;
        InputFile_Leave(sender, e);
      }

      bool enable = EnableInputOutput.Checked;
      if (enable == true) {
        OutputFile.Text = selectFile;
      }
    }

    private void OpenOut_Button_Click(object sender, EventArgs e) {
      string selectFile = FileOpenClass.FileOpen(Path.GetDirectoryName(gb.G_OutputDir));
      if (string.IsNullOrEmpty(selectFile) == false) {
        OutputFile.Text = selectFile;
        OutputFile_Leave(sender, e);
      }
    }

    /* 入力フォルダ設定欄のフォーカスが外れた時 */
    private void InputFile_Leave(object sender, EventArgs e) {
      FileListBox.Items.Clear();
      FileListBox.Enabled = true;

      string inputDir = InputFile.Text;
      if (string.IsNullOrEmpty(inputDir) == true) return;

      if (Path.Exists(inputDir) == false) {
        FileListBox.Items.Add("選択したフォルダは存在しません");
        FileListBox.Enabled = false;
        return;
      }

      string[] fileList = CommonClass.ListUpFile_Input(inputDir, EnableAllSearch.Checked, 1);
      if (fileList.Length > 0) foreach (string file in fileList) FileListBox.Items.Add(file);

      EnableInputOutput_CheckedChanged(sender, e);
    }

    /* 出力フォルダ設定欄のフォーカスが外れた時 */
    private void OutputFile_Leave(object sender, EventArgs e) {
      string outputDir = OutputFile.Text;
      if (string.IsNullOrEmpty(outputDir) == true) return;

      if (Path.Exists(outputDir) == false) {
        MessageBox.Show("選択したフォルダは存在しません");
        return;
      }

      string rtn = XMLClass.XMLWriteMain("Setting", "OutputDirectory##", "path##" + outputDir);
      gb.G_OutputDir = outputDir;
    }


    /* 拡張子の条件指定（mp3, wav） */
    private void EnableAllSearch_CheckedChanged(object sender, EventArgs e) {
      InputFile_Leave(sender, e);
    }

    /* 実行ボタン */
    private void Run_Button_Click(object sender, EventArgs e) {
      // 入出力フォルダの検証
      string inputDirectory = InputFile.Text;
      string outputDirectory = OutputFile.Text;

      if (string.IsNullOrEmpty(inputDirectory) == true) {
        MessageBox.Show("入力フォルダが指定されていません");
        return;
      } else if (string.IsNullOrEmpty(outputDirectory) == true) {
        DialogResult rtnMsg = new();

        rtnMsg = MessageBox.Show("出力フォルダが指定されていません\n入力フォルダと同じ場所を指定しますか", "", MessageBoxButtons.YesNo);
        if (rtnMsg != DialogResult.Yes) return;
        OutputFile.Text = inputDirectory;
      }

      // pythonのエスケープに合わせる
      string[] directoryList = new string[1] { inputDirectory + " " + outputDirectory };
      string pyArgument = CommonClass.ReplaceFilePathToPy(directoryList);

      // pythonの呼び出しモード（0: src, 1: exe）
      int callMode = Convert.ToInt32(XMLClass.XMLReadMain("Setting", "PythonFileName", "mode"));

      // pythonの実行
      string[] rtnPy;
      if (callMode == 0) {
        rtnPy = CallPythonClass.CallPythonOnSrc(pyArgument);
      } else {
        rtnPy = CallPythonClass.CallPythonOnExe(pyArgument);
      }
    }

    private void Reload_Button_Click(object sender, EventArgs e) {
      InputFile_Leave(sender, e);
    }
  }

  /* *********************************************************************** */
  //		CommonClass
  //		概要 共通基本クラス
  //			Build By Oura Chihiro(Maplecafe) [2024.03.02]
  /* *********************************************************************** */
  public static class CommonClass {
    public static string ReplaceFilePathToPy(string[] filePaths) {
      string pyArguments = string.Empty;

      foreach (string filePath in filePaths) {
        string rplcPath = filePath.Replace("\\", "\\\\");
        pyArguments += rplcPath;
      }

      return pyArguments;
    }

    public static string[] ListUpFile_Input(string directoryPath, bool enableAllSearch, int XMLWriteFlg = 0) {
      string[] fileList = Directory.GetFiles(directoryPath);
      List<string> fileNameList = new();

      foreach (string file in fileList) {
        if (enableAllSearch == false) {
          string extension = Path.GetExtension(file);
          if (extension != ".mp3" && extension != ".wav") continue;
        }

        string fileName = Path.GetFileName(file);
        fileNameList.Add(fileName);
      }

      // ディレクトリに1つ以上ファイルがある場合は、XMLを上書きする（ただし、起動時は除外する）
      if (fileNameList.Count > 0 && XMLWriteFlg != 0) XMLClass.XMLWriteMain("Setting", "InputDirectory##", "path##" + directoryPath);
      return fileNameList.ToArray();
    }
  }


  /* *********************************************************************** */
  //		XMLClass
  //		概要 XMLの処理を行う
  //
  //			Build By Oura Chihiro(Maplecafe) [2024.04.01]
  /* *********************************************************************** */
  public static class XMLClass {
    public static string XMLReadMain(string category, string variable, string attribute) {

      string xmlFilePath = "2##ContentsOption.xml" + "\t";
      string xmlTable = "SettingFile" + "\t";
      string xmlCategory = category + "\t";
      string xmlAttribute = attribute + "\t";
      string xmlVariable_1 = variable;
      string xmlArgs = xmlFilePath + xmlTable + xmlCategory + xmlAttribute + xmlVariable_1;

      string[] rtn = MapleLib_IniFileReadAndWrite.XmlFileClass.XmlReadMain(xmlArgs);
      string xmlRtnValue;

      if (rtn.Length < 4) xmlRtnValue = rtn[0]; else xmlRtnValue = rtn[4];

      return xmlRtnValue;
    }

    public static string XMLWriteMain(string category, string variable, string attribute) {

      string xmlFilePath = "2##ContentsOption.xml" + "\t";
      string xmlTable = "SettingFile" + "\t";
      string xmlCategory = category + "\t";
      string xmlAttribute = attribute + "\t";
      string xmlVariable_1 = variable;

      string xmlArgs = xmlFilePath + xmlTable + xmlCategory + xmlAttribute + xmlVariable_1;
      string[] rtn = MapleLib_IniFileReadAndWrite.XmlFileClass.XmlWriteMain(xmlArgs);

      return rtn[0];
    }
  }


  /* *********************************************************************** */
  //		FileOpenClass
  //		概要 ファイルを開く
  //			Build By Oura Chihiro(Maplecafe) [2024.03.02]
  /* *********************************************************************** */
  /* ==== FileOpen========================================================== */
  //	第1引数 | None     | ---
  //	返り値1 | String   | 選択したファイルパス
  /* ======================================================================= */
  public static class FileOpenClass {
    public static Global.GlobalVariableClass gb = new();
    public static string FileOpen(string folder = null) {
      FolderBrowserDialog folderBrowserDialog = new();

      if (string.IsNullOrEmpty(folder) == true) {
        folderBrowserDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
      } else {
        folderBrowserDialog.InitialDirectory = folder;
      }

      if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
        string selectDirectory = folderBrowserDialog.SelectedPath;
        return selectDirectory;
      } else {
        return string.Empty;
      }
    }
  }


  /* *********************************************************************** */
  //		CallPythonClass
  //		概要 Pythonを呼び出すクラス
  //			Build By Oura Chihiro(Maplecafe) [2024.03.02]
  /* *********************************************************************** */
  public static class CallPythonClass {
    public static Global.GlobalVariableClass gb = new();

    public static string[] CallPythonOnSrc(string arguments) {
      string pythonFileName = XMLClass.XMLReadMain("Setting", "PythonFileName", "src");
      string myLocationDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

      if (pythonFileName == "^Null") {
        MessageBox.Show("XMLの登録が正しくありません、KeyValueを確認してください");
        return null;
      } else if (Path.Exists(Path.Combine(myLocationDirectory, pythonFileName)) == false) {
        MessageBox.Show("パスが存在しません");
        return null;
      }

      // pythonに渡すコマンドライン引数を組み立てる
      string pythonArguments = Path.Combine(myLocationDirectory, pythonFileName) + " " + arguments;

      // pythonプロセスを定義
      var pythonProcess = new Process {
        StartInfo = new ProcessStartInfo("Python.exe") {
          UseShellExecute = false,
          RedirectStandardInput = true,
          RedirectStandardOutput = true,
          Arguments = pythonArguments
        }
      };

      List<string> rtnStrings = new();
      string line;

      pythonProcess.Start();
      while ((line = pythonProcess.StandardOutput.ReadLine()) != null) rtnStrings.Add(line);

      return rtnStrings.ToArray();
    }

    public static string[] CallPythonOnExe(string arguments) {
      string pythonFileName = XMLClass.XMLReadMain("Setting", "PythonFileName", "exe");
      string myLocationDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

      if (pythonFileName == "Null") {
        MessageBox.Show("XMLの登録が正しくありません、KeyValueを確認してください");
        return null;
      } else if (Path.Exists(Path.Combine(myLocationDirectory, pythonFileName)) == false) {
        MessageBox.Show("パスが存在しません");
        return null;
      }

      // pythonに渡すコマンドライン引数を組み立てる
      string exePathName = Path.Combine(myLocationDirectory, pythonFileName);
      string pythonArguments = arguments;

      // pythonプロセスを定義
      var pythonProcess = new Process {
        StartInfo = new ProcessStartInfo(exePathName) {
          UseShellExecute = false,
          RedirectStandardInput = true,
          RedirectStandardOutput = true,
          Arguments = pythonArguments
        }
      };

      List<string> rtnStrings = new();
      string line;

      pythonProcess.Start();
      while ((line = pythonProcess.StandardOutput.ReadLine()) != null) rtnStrings.Add(line);

      return rtnStrings.ToArray();
    }
  }
}


/* =================== グローバル変数・定数宣言 ========================== */
namespace Global {
  public class GlobalVariableClass {
    public readonly string G_Title = "Normalize_wave";
    public string G_InputDir = string.Empty;
    public string G_OutputDir = string.Empty;
  }
}
