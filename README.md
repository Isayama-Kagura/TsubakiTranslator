# <p align="center"> <img width="130" height="130" src="https://github.com/Isayama-Kagura/TsubakiTranslator/blob/main/TsubakiTranslator/Resources/Icon/Tsubaki.png"/> <br/>TsubakiTranslator<br/>
  </p>

<p align="center">
      <a href="/LICENSE"><img src="https://img.shields.io/badge/license-GPL%203.0-blue.svg" alt="GPL 3.0 LICENSE"></a>
      <a href="https://ci.appveyor.com/project/Isayama-Kagura/tsubakitranslator"><img src="https://ci.appveyor.com/api/projects/status/7apiflt07telyd5w?svg=true" alt="Build status"></a>
</p>

<p align="center">终归还是来了呢。</p>

<p align="center">我寻找到的，神明大人。</p>


## 项目的由来

在开发这个翻译器之前，我用过几款其他类似的翻译器，其中最喜欢的就是[YUKI Galgame 翻译器](https://github.com/project-yuki/YUKI)，它的简洁和可扩展非常对我的胃口，遗憾的是有各种奇奇怪怪的bug且GUI不够完善，作者也很久没有维护了。因此我产生了在该项目的设计的基础上开发新项目并进行完善的想法。

## 项目特点

- 以Hook方式提取游戏文本，支持32位和64位的游戏。
- 基于.NET 6 + WPF开发。
- 界面采用Material Design设计风格，简洁易用。
- 去除了所有的离线翻译接口，完全采用在线API进行翻译。
- 支持翻译中文和英文两种源语言的游戏。
- 新增了多种类的翻译API，可同时对照翻译。
- 对于Hook文本文字重复的现象，提供了按重复字数去重的功能。
- 对于混乱的Hook文本，提供自定义正则规则进行文本替换的功能。
- 支持翻译剪切板文本并提供文本处理功能。
- 支持Windows 10自带的OCR功能。

## 使用方法
- [下载地址](https://github.com/Isayama-Kagura/TsubakiTranslator/releases)
1. 第一次使用时，先进入设置，选择想要使用的翻译API，填写必要信息。
2. 打开游戏，点击右上角“进程号打开”，选择需要翻译的游戏进程，填写必要的信息后点“确定”。
3. 进入到hook文本选择界面，让游戏的文本变化，选择提取文本和游戏文本完全一致的项。
4. 愉快的进行游戏。
5. 翻译器正常退出后会保存本次使用翻译器的配置和游戏数据，下次进入游戏打开历史游戏记录，选择对应游戏进程按上述步骤进行翻译。
6. 当Hook获得的文本有规律的混乱时，支持自定义正则表达式进行文本匹配替换（e.g. aaabbbccc的文本要转换成abc，匹配表达式为`(.){3}`，替换表达式为`$1`）。**注意：正则表达式的匹配和替换的模式遵循C#规范，请认真学习相关格式后再进行配置！！！**

## 监视剪切板功能
监视剪切板功能，使翻译器除了对Hook提取的文本进行翻译，还可以对一些游戏（AGTH提取/RPGMaker/Unity）进行特殊处理后再进行翻译。详情可以去VNR吧找相关教程学习。

## 文本转语音（TTS）功能
通过TTS功能可以播放一些文本的语音。该功能采用目前TTS领域最先进的微软Azure的接口，最接近人类真实的语音语调。使用该功能需要用户自行注册一个Azure免费账号。

## 光学字符识别（OCR）功能
基于Windows 10 UWP自带的OCR接口实现，在Windows 10 Build 10240以上版本的系统可以使用，分为手动截图和选区自动截图。可在翻译界面中点击截图按钮或者按快捷键逐个翻译，或者选定区域后，自动对该区域截图翻译。

## 支持的翻译API
目前有支持的翻译API包括阿里、百度、彩云、DeepL、IBM、腾讯、小牛、火山、Yeekit。

## 疑问解答

Q：为什么我玩xxx游戏时提取不到文本/闪退/卡死/翻译API不正常？ <br/>
A：这类问题可能是本项目的程序设计有缺陷，也可能是依赖项目的不足。其中提取不到文本时请尝试用管理员权限运行翻译器。如遇这类问题无法解决，请详细描述现象、所做的操作、配置，最好能配图，然后提出issue或者给我发邮件，在我项目范围内的会尽量帮助解决。

Q：游戏的配置文件保存在哪里？ <br/>
A：在游戏根目录的`config/`文件夹下，更新软件时可以备份该目录，然后复制到新的翻译器根目录下。

Q：自动提取的游戏文本混乱怎么办？ <br/>
A：当文本单字重复时，可设置重复次数进行去重（e.g. aaabbbccc的文本，即重复3次），或自定义正则规则去除杂乱文字，或用其他方法把文本导出至剪切板进行翻译，仍无法解决请尝试使用特殊码。


## 联系作者

如对本项目有任何建议或疑问，可提出issue或者发送邮件至`isayama_kagura@qq.com`。

## 依赖的项目
- [Textractor](https://github.com/Artikash/Textractor)
- [MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
- [Windows Community Toolkit](https://github.com/CommunityToolkit/WindowsCommunityToolkit)
- [RestSharp](https://github.com/restsharp/RestSharp)

## 部分代码参考
- [御坂翻译器](https://github.com/hanmin0822/MisakaTranslator)
- [YUKI Galgame 翻译器](https://github.com/project-yuki/YUKI)

