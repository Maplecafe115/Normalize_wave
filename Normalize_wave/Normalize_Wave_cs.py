# *********************************************************************** #
# 		Normalize_wave.py
# 		  Wave(mp3)音源の周波数と音量(dB)を取得して平均化する
#       ★ EBU R 128による欧州放送連合推奨の計算式メソッドを用いる
#
# 			Build By Oura Chihiro(Maplecafe) [2023.08.20]
# *********************************************************************** #
from ffmpeg_normalize import FFmpegNormalize
from pathlib import Path
import sys
import traceback

class NormalizeAudioClass:
  def NormalizeAudio(pDirectory_in:str, pDirectory_out:str) -> int:
    strInputDir = Path(pDirectory_in)
    strOutputDir = Path(pDirectory_out)

    # Input側のファイル名を列挙
    aryFileNameList = [file for file in strInputDir.iterdir() if file.is_file()]

    # 処理実行
    for i in range(len(aryFileNameList)):
      strInputPath = str(aryFileNameList[i])
      strOutputPath  = str(strOutputDir) + "\\" + aryFileNameList[i].stem + aryFileNameList[i].suffix

      # 拡張子がmp3なら、オプション"libmp3lame"を付与する
      # wavならオプション付与なし
      # それ以外は実行しない
      strInputExtension = aryFileNameList[i].suffix
      if strInputExtension == ".mp3":
        normalizer = FFmpegNormalize(
          normalization_type = 'ebu',
          target_level = -23,
          print_stats = True,
          audio_codec = "libmp3lame"
        )
      elif strInputExtension == ".wav":
        normalizer = FFmpegNormalize(
          normalization_type = 'ebu',
          target_level = -23,
          print_stats = True
        )
      else:
        continue
      
      normalizer.add_media_file(strInputPath, strOutputPath)
      
      try:
        normalizer.run_normalization()
      except Exception:
        continue

    return 0


if __name__ == '__main__':
  # CSharpから受け取る
  impListCount = len(sys.argv)

  try:
    strInput = Path(sys.argv[1])
    strOutput = Path(sys.argv[2])
  except IndexError as e:
    print(traceback.print_exc())
    print("Catched Exception and exit py.")
    sys.exit(0)

  intRtnCode = NormalizeAudioClass.NormalizeAudio(str(strInput), str(strOutput))
