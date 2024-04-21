/* *********************************************************************** 
		ini, xml設定ファイルを参照するクラス（プロジェクト参照で使用する）

			Build By Oura Chihiro(Maplecafe) [2023.4.11]
   *********************************************************************** */
using System.ComponentModel.Design;
using System.Xml.Linq;

namespace MapleLib_IniFileReadAndWrite {

  /* *********************************************************************** */
  //		IniFileClass
  //		概要 Ini設定ファイルを参照するクラス
  //      ★ コマンドライン引数はタブ文字で区切る
  //
  //			Build By Oura Chihiro(Maplecafe) [2023.04.11]
  /* *********************************************************************** */
  public class IniFileClass {
    public static string[] IniReadMain(string args) {



      return new string[1] { "return string" };
    }



    /* ==== iniのファイルを参照する関数 ========================================= */
    //	第1引数 | string     | 参照するファイルパス（空白か0で指定なし）
    //	返り値1 | string     | ファイルパス（ない場合は^FileNotFoundを返す）
    //    ★ 指定なし：アセンブリ直下にある最初のiniファイル
    //    ★ 2を指定：
    //    ★ アセンブリ直下でもファイル名を指定すれば、それを開くように改良する。
    /* ======================================================================= */
    public static string IniFilePath(string filePath) {
      string[] arguments = filePath.Split("##");

      string iniPath = arguments[0];
      string assemblyFileName = string.Empty;
      if (arguments.Length == 2) assemblyFileName = arguments[1];

      int iniReadMode;
      if (int.TryParse(iniPath, out _) == true) {
        if (Convert.ToInt32(iniPath) == 0) iniReadMode = 0;
        else if (Convert.ToInt32(iniPath) == 2) iniReadMode = 2;
        else iniReadMode = 1;
      }
      else iniReadMode = 1;
      
      // 空白か0または2の場合はアセンブリの直下からiniファイルを探す
      if (iniReadMode == 0 || iniReadMode == 2 || string.IsNullOrEmpty(iniPath) == true) {

        // 直下からxmlファイルを探す
      }

      

      return string.Empty;
    }
  }


  /* *********************************************************************** */
  //		XMLFileClass
  //		概要 XML設定ファイルを参照するクラス
  //      ★ コマンドライン引数はタブ文字で区切る
  //
  //			Build By Oura Chihiro(Maplecafe) [2023.04.11]
  /* *********************************************************************** */
  public class XmlFileClass {

    /* summary */
    /// <summary>
    /// <para>第1引数 [xmlPath] ：参照するXMLのファイル</para>
    /// <para> ファイル参照：空白または"0"でアセンブリ直下の一番最初のXMLファイル                </para>
    /// <para>              "2"でアセンブリ直下のファイル指定（"##"で区切ってファイル名を指定）  </para>
    /// <para>              "9"でOneDrive内のXMLを指定（"##"で区切ってOneDrive以下のパスを指定）</para>
    /// <para>第2引数 [xmlTable]：記述テーブル</para>
    /// <para>第3引数 [xmlTag]  ：記述要素</para>
    /// <para>第4引数 [xmlAttribute]   ：記述タグの属性（指定がない場合は空白を指定）</para>
    /// <para>第5引数以降 [xmlVariable]：参照する記述タグ</para>
    /// <para>★ 引数区切りはタブ文字（\t）を使用する</para>
    /// </summary>
    /// <param name="args">
    ///   コマンドライン引数
    /// </param>
    /// <returns>
    /// <para>返り値1：参照したXMLのファイル</para>
    /// <para>返り値2：参照した記述テーブル</para>
    /// <para>返り値3：参照したタグの属性（指定がない場合はNull）</para>
    /// <para>返り値4以降：設定値またメッセージ値</para>
    /// <para>★ 返り値は配列で返され、第4以降はタブ文字で区切られる</para>
    /// </returns>

    public static string[] XmlReadMain(string args) {
      // コマンドライン引数の分割：タブ文字
      string[] arguments = args.Split('\t');

      /* コマンドライン引数：[第1引数] XMLファイルの場所 */
      string argsPath = arguments[0];
      string xmlPath = XmlFilePath(argsPath);

      if (Path.Exists(xmlPath) == false) return new string[1] {"-1^FileNotFound"};

      /* コマンドライン引数：[第2引数] XMLの設定値が格納されているテーブル */
      string xmlTable = arguments[1];

      /* コマンドライン引数：[第3引数] XMLの記述要素 */
      string xmlTag = arguments[2];

      /* コマンドライン引数：[第4引数] XMLの設定値の属性指定 指定しない場合はNull */
      string xmlAttribute;
      if (string.IsNullOrEmpty(arguments[3]) == true) xmlAttribute = null; else xmlAttribute = arguments[3];

      /* コマンドライン引数：[第5引数以降] 取得する設定値 */
      string[] xmlRtn = new string[arguments.Length];
      xmlRtn[0] = xmlPath;
      xmlRtn[1] = xmlTable;
      xmlRtn[2] = xmlTag;
      xmlRtn[3] = xmlAttribute;

      for (int i = 4; i < arguments.Length; i++) {
        string xmlVariable = arguments[i];
        xmlRtn[i] = XmlValue(xmlPath, xmlTable, xmlTag, xmlVariable, xmlAttribute);
      }

      return xmlRtn;
    }


    /* summary */
    /// <summary>
    /// <para>第1引数 [xmlPath] ：参照するXMLのファイル</para>
    /// <para> ファイル参照：空白または"0"でアセンブリ直下の一番最初のXMLファイル                </para>
    /// <para>              "2"でアセンブリ直下のファイル指定（"##"で区切ってファイル名を指定）  </para>
    /// <para>              "9"でOneDrive内のXMLを指定（"##"で区切ってOneDrive以下のパスを指定）</para>
    /// <para>第2引数 [xmlTable]：記述テーブル</para>
    /// <para>第3引数 [xmlTag]  ：記述要素</para>
    /// <para>第4引数 [xmlAttribute]   ：記述タグの属性（属性と設定値を"##"で区切ってください / 指定がない場合は空白を指定）</para>
    /// <para>第5引数以降 [xmlVariable]：参照する記述タグ（タグと設定値を"##"で区切ってください）</para>
    /// <para>★ 引数区切りはタブ文字（\t）を使用する</para>
    /// </summary>
    /// <param name="args">
    ///   コマンドライン引数
    /// </param>
    /// <returns>
    /// <para>返り値1：参照したXMLのファイル</para>
    /// <para>返り値2：参照した記述テーブル</para>
    /// <para>返り値3：参照したタグの属性（指定がない場合はNull）</para>
    /// <para>返り値4以降：メッセージ値</para>
    /// <para>★ 返り値は配列で返され、第4以降はタブ文字で区切られる</para>
    /// </returns>
    public static string[] XmlWriteMain(string args) {
      // コマンドラインの分割：タブ文字
      string[] arguments = args.Split('\t');

      /* コマンドライン引数：[第1引数] XMLファイルの場所 */
      string argsPath = arguments[0];
      string xmlPath = XmlFilePath(argsPath);

      if (Path.Exists(xmlPath) == false) return new string[1] { "-1^FileNotFound" };

      /* コマンドライン引数：[第2引数] XMLの設定値が格納されているテーブル */
      string xmlTable = arguments[1];

      /* コマンドライン引数：[第3引数] XMLの記述要素 */
      string xmlTag = arguments[2];

      /* コマンドライン引数：[第4引数] XMLの設定値の属性指定（設定する値と"##"で分割する / 指定しない場合はnull） */
      string xmlAttribute;
      if (string.IsNullOrEmpty(arguments[3]) == true) xmlAttribute = null; else xmlAttribute = arguments[3];

      /* コマンドライン引数：[第5引数] 設定する値（設定する値と"##"で分割する） */
      string[] xmlRtn = new string[arguments.Length];
      xmlRtn[0] = xmlPath;
      xmlRtn[1] = xmlTable;
      xmlRtn[2] = xmlTag;
      xmlRtn[3] = xmlAttribute;

      for (int i = 4; i < arguments.Length; i++) {
        string xmlVariable = arguments[i];
        xmlRtn[i] = XmlSet(xmlPath, xmlTable, xmlTag, xmlVariable, xmlAttribute);
      }

      return xmlRtn;
    }


    /* ==== Xmlのファイルを参照する関数 ========================================= */
    //	第1引数 | string     | 参照するファイルパス（空白か0で指定なし）
    //	返り値1 | string     | ファイルパス（ない場合は^FileNotFoundを返す）
    //    ★ 指定なし：絶対パス参照
    //    ★ 2を指定：アセンブリ直下にあるXMLファイル（空白または0も同様）
    //    ★ 9を指定：OneDrive
    /* ======================================================================= */
    public static string XmlFilePath(string filePath) {
      string[] arguments = filePath.Split("##");

      string xmlPath = arguments[0];
      string assemblyFileName = string.Empty;
      if (arguments.Length == 2) assemblyFileName = arguments[1];
      
      int xmlReadMode;
      if (int.TryParse(xmlPath, out _) == true) {
        if (Convert.ToInt32(xmlPath) == 0) xmlReadMode = 0;
        else if (Convert.ToInt32(xmlPath) == 2) xmlReadMode = 2;    // Assembly
        else if (Convert.ToInt32(xmlPath) == 9) xmlReadMode = 9;    // OneDrive
        else xmlReadMode = 1;
      }
      else xmlReadMode = 1;

      // 空白か0 または 2 の場合はアセンブリの直下からXMLファイルを探す
      if (xmlReadMode == 0 || xmlReadMode == 2 || string.IsNullOrEmpty(xmlPath) == true) {
        string xmlFileName;
        if (xmlReadMode == 2) xmlFileName = assemblyFileName; else xmlFileName = ".xml";

        string myLocationDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        int foundFlg = 0;

        // 直下からxmlファイルを探す
        foreach (string file in Directory.GetFiles(myLocationDirectory)) {
          if (file.EndsWith(xmlFileName) == true) {
            xmlFileName = file;
            foundFlg = 1;
            break;
          }
        }

        if (foundFlg == 0) return string.Empty;
        xmlPath = Path.Combine(myLocationDirectory, xmlFileName);
      }
      // 9の場合はOneDriveに含まれたXMLファイルを参照する（環境変数からOneDriveの親ディレクトリを取得する）
      else if (xmlReadMode == 9) {
        string oneDriveDir = CommonClass.GetEnvironmentVariable();
        if (oneDriveDir == null) return string.Empty;

        xmlPath = Path.Combine(oneDriveDir, assemblyFileName);
      }

      return xmlPath;
    }

    /* ==== Xmlの設定値を参照する関数 ========================================== */
    //	第1引数 | string     | 参照するファイルパス
    //	第2引数 | string     | 参照するテーブル
    //	第3引数 | string     | 参照カテゴリー
    //	第4引数 | string     | 参照タグ
    //	第5引数 | string     | タグに付与されれた属性 オプション
    //	返り値1 | string     | 設定値またはメッセージ（^Message）
    /* ======================================================================= */
    public static string XmlValue(string xmlFilePath, string xmlTable, string xmlTag, string xmlVariable, string xmlAttribute = null) {
      XDocument xmlDoc = XDocument.Load(xmlFilePath);
      XElement xmlEle = xmlDoc.Element(xmlTable);

      if (xmlEle == null) return "-2^Null_Table";

      IEnumerable<XElement> xmlVariableRow = xmlEle.Elements(xmlTag);
      if (xmlVariableRow.Count() < 1) return "-3^Null_Tag";

      int elementCount = 0;
      string xmlRtn = string.Empty;
      string xmlRtnValue;

      foreach (XElement xmlVariableElement in xmlVariableRow) {
        elementCount ++;

        // 属性を指定しているかどうかで読み取る値を変える
        if (xmlAttribute == null) {
          if (xmlVariableElement.Element(xmlVariable) == null) {
            xmlRtnValue = "-4^Null_Element";
          }
          else {
            xmlRtnValue = CommonClass.IsNullFunction(xmlVariableElement.Element(xmlVariable).Value);
          }
        }
        else {
          if (xmlVariableElement.Element(xmlVariable) == null) {
            xmlRtnValue = "-5^Null_Attribute(Ele)";
          }
          else if (xmlVariableElement.Element(xmlVariable).Attribute(xmlAttribute) == null) {
            xmlRtnValue = "-5^Null_Attribute(Att)";
          }
          else {
            xmlRtnValue = CommonClass.IsNullFunction(xmlVariableElement.Element(xmlVariable).Attribute(xmlAttribute).Value);
          }
        }

        if (elementCount <= 1) xmlRtn += xmlRtnValue; else xmlRtn += "\t" + xmlRtnValue;
      }

      return xmlRtn;
    }


    /* ==== Xmlの設定値を更新する関数 ========================================== */
    //	第1引数 | string     | 参照するファイルパス
    //	第2引数 | string     | 参照するテーブル
    //	第3引数 | string     | 参照カテゴリー
    //	第4引数 | string     | 参照タグと設定値（##で区切る）
    //	第5引数 | string     | タグに付与されれた属性と設定値（##で区切る） オプション
    //	返り値1 | string     | 結果のメッセージ（^Message）
    /* ======================================================================= */
    public static string XmlSet(string xmlFilePath, string xmlTable, string xmlTag, string xmlVariable, string xmlAttribute = null) {
      XDocument xmlDoc = XDocument.Load(xmlFilePath);
      XElement xmlEle = xmlDoc.Element(xmlTable);

      if (xmlEle == null) return "-2^Null_Table";

      IEnumerable<XElement> xmlVariableRow = xmlEle.Elements(xmlTag);
      if (xmlVariableRow.Count() < 1) return "-3^Null_Tag";

      int elementCount = 0;
      string xmlRtn = string.Empty;

      // 属性と設定値の分割（必ず2要素にならなければならない）
      string[] setAttributeValue;
      if (xmlAttribute != null) {
        setAttributeValue = xmlAttribute.Split("##");
      }
      else setAttributeValue = new string[1] { string.Empty };

      // 設定項目と設定値の分割（必ず2要素にならなければならない）
      string[] setVariableValue = xmlVariable.Split("##");
      if (setVariableValue.Length != 2) return "-4^ValueError";

      foreach (XElement xmlVariableElement in xmlVariableRow) {
        elementCount ++;

        // 要素検証
        XAttribute xAttribute = xmlVariableElement.Element(setVariableValue[0]).Attribute(setAttributeValue[0]);

        // 要素更新
        xmlVariableElement.Element(setVariableValue[0]).Value = setVariableValue[1];
        xmlRtn = "0^ValueModified";

        // 属性指定がある場合
        if (setAttributeValue.Length != 2 || xAttribute == null) {
          xmlRtn += " -5^AttributeError";
        }
        else {
          xmlVariableElement.Element(setVariableValue[0]).Attribute(setAttributeValue[0]).Value = setAttributeValue[1];
          xmlRtn += " 0^AttributeModified";
        }
      }

      xmlDoc.Save(xmlFilePath);
      return xmlRtn;
    }


    public static string XmlAdd(string xmlFilePath, string xmlTable, string xmlTag, string xmlVariable, string xmlAttribute = null) {
      XDocument xmlDoc = XDocument.Load(xmlFilePath);
      XElement xmlEle = xmlDoc.Element(xmlTable);

      if (xmlEle == null) return "-2^Null_Table";

      IEnumerable<XElement> xmlVariableRow = xmlEle.Elements(xmlTag);
      if (xmlVariableRow.Count() < 1) return "-3^Null_Tag";

      //int elementCount = 0;
      //string xmlRtn = string.Empty;

      // 属性と設定値の分割（この場合、必ず2要素にならなければならない）
      string[] setAttributeValue;
      if (xmlAttribute != null) {
        setAttributeValue = xmlAttribute.Split("##");
      }
      else setAttributeValue = new string[1] {string.Empty};

      return string.Empty;
    }


  }

  /* *********************************************************************** */
  //		JSONFileClass
  //		概要 JSONファイルを参照するクラス
  //      ★ コマンドライン引数はタブ文字で区切る
  //
  //			Build By Oura Chihiro(Maplecafe) [2024.03.29]
  /* *********************************************************************** */
  public class JsonFileClass {
    public static string JsonFilePath(string filePath) {
      string[] arguments = filePath.Split(',');
      string JSONPath = arguments[0];
      string assemblyFileName = string.Empty;

      if (arguments.Length == 2) assemblyFileName = arguments[1];

      int JSONReadMode;
      if (int.TryParse(JSONPath, out _) == true) {
        if (Convert.ToInt32(JSONPath) == 0) JSONReadMode = 0;
        else if (Convert.ToInt32(JSONPath) == 2) JSONReadMode = 2;
        else JSONReadMode = 1;
      }
      else JSONReadMode = 1;

      // 空白か0, または2の場合はアセンブリの直下からJSONファイルを探す
      if (JSONReadMode == 0 || JSONReadMode == 2 || string.IsNullOrEmpty(JSONPath) == true) {
        string JSONFileName;
        if (JSONReadMode == 2) JSONFileName = assemblyFileName; else JSONFileName = ".json";

        string myLocationDirectory =  Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        int foundFlg = 0;

        // 直下からjsonファイルを探す
        foreach (string file in Directory.GetFiles(myLocationDirectory)) {
          if (file.EndsWith(JSONFileName) == true) {
            JSONFileName = file;
            foundFlg = 1;    
            break;
          }
        }

        if (foundFlg == 0) return string.Empty;
        JSONPath = Path.Combine(myLocationDirectory, JSONFileName);
      }

      return JSONPath;
    }
  }


  /* *********************************************************************** */
  //		共通クラス
  //		概要 すべての処理に関わるクラスはすべて共通として集約する
  //
  //			Build By Oura Chihiro(Maplecafe) [2024.03.08]
  /* *********************************************************************** */
  public class CommonClass {
    public static string IsNullFunction(string checkString) {
      if (checkString == null) return "-6^Null_Value";
      else if (checkString == string.Empty) return "-7^Empty_Value";

      else return checkString;
    }

    public static string GetEnvironmentVariable() {
      string variableName = "OneDriveConsumer";

      // システム環境変数の取得
      string environmentValue = Environment.GetEnvironmentVariable(variableName);
      return environmentValue;
    }
  }


  /* *********************************************************************** */
  //		使用例を格納するクラス
  //		概要 特に使うことはないので、Privateにしておく
  //
  //			Build By Oura Chihiro(Maplecafe) [2024.03.08]
  /* *********************************************************************** */
  public class HowToUseClass { 

    private static string HowToUseXmlRead(string category, string variable, string attribute = null) {
      
      string xmlFilePath    = "[XML_FilePath]"        + "\t";
    //string xmlFilePath    = "0"                     + "\t";
    //string xmlFilePath    = "2##xmlFileName.xml"    + "\t";
      string xmlTable       = "[XML_Table]"           + "\t";
      string xmlCategory    = "[XML_Category]"        + "\t";
      string xmlAttribute   = "[XML_Attribute_InTAG]" + "\t";
      string xmlVariable_1  = "[XML_TAG]"             + "\t";
      string xmlVariable_2  = "[XML_TAG]"             + "\t";
      string xmlVariable_3  = "[XML_TAG]"                   ;

      string xmlArgs = xmlFilePath + xmlTable + xmlCategory + xmlAttribute + xmlVariable_1 + xmlVariable_2 + xmlVariable_3;
      string[] rtn = MapleLib_IniFileReadAndWrite.XmlFileClass.XmlReadMain(xmlArgs);

      // 配列の要素数が5未満であれば、エラーを返す
      if (rtn.Length < 5) return "XML_Error."; else return rtn[4];
    }

    private static string HowToUseXmlWrite(string category, string variable, string attribute = null) {

      string xmlFilePath   = "[XML_FilePath]"                               + "\t";
    //string xmlFilePath   = "0"                                            + "\t";
    //string xmlFilePath   = "2##xmlFileName.xml"                           + "\t";
      string xmlTable      = "[XML_Table]"                                  + "\t";
      string xmlCategory   = "[XML_Category]"                               + "\t";
      string xmlAttribute  = "[XML_Attribute_InTAG]##[XML_Attribute_Value]" + "\t";
      string xmlVariable_1 = "[XML_TAG]##[XML_Variable_Value]"              + "\t";
      string xmlVariable_2 = "[XML_TAG]##[XML_Variable_Value]"              + "\t";
      string xmlVariable_3 = "[XML_TAG]##[XML_Variable_Value]"                    ;

      string xmlArgs = xmlFilePath + xmlTable + xmlCategory + xmlAttribute + xmlVariable_1 + xmlVariable_2 + xmlVariable_3;
      string[] rtn = MapleLib_IniFileReadAndWrite.XmlFileClass.XmlWriteMain(xmlArgs);

      // 配列の要素数が5未満であれば、エラーを返す
      if (rtn.Length < 5) return "XML_Error."; else return rtn[4];
    }
  }

}
