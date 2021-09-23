# <p align="center">![image](https://github.com/Isayama-Kagura/TsubakiTranslator/blob/main/TsubakiTranslator/Resources/Icon/Tsubaki.png)<br/>TsubakiTranslator<br/>
  </p>

<p align="center">
      <a href="/LICENSE"><img src="https://img.shields.io/badge/license-GPL%203.0-blue.svg" alt="GPL 3.0 LICENSE"></a>
      <a href="https://ci.appveyor.com/project/Isayama-Kagura/tsubakitranslator"><img src="https://ci.appveyor.com/api/projects/status/7apiflt07telyd5w?svg=true" alt="Build status"></a>
</p>

<p align="center">终归还是来了呢。</p>

<p align="center">察觉到了我的，神明大人。</p>


## 项目的由来

在开发这个翻译器之前，我用过几款其他类似的翻译器，其中最喜欢的就是[YUKI Galgame 翻译器](https://github.com/project-yuki/YUKI)，它的简洁和可扩展非常对我的胃口，遗憾的是有各种奇奇怪怪的bug且GUI不够完善，作者也很久没有维护了。因此我产生了在该项目的设计的基础上开发新项目并进行完善的想法。

## 项目特点

- 以Hook方式提取游戏文本，支持32位和64位的游戏。
- 基于.NET 5.0 + WPF开发。
- 界面采用Material Design设计风格，简洁易用。
- 去除了所有的离线翻译接口，完全采用在线API进行翻译。
- 新增了多种类的翻译API，可同时对照翻译。
- 对于Hook文本文字重复的现象，提供了按重复次数去重的功能。（e.g. aaabbbccc的文本，即重复3次）

## 项目的未来发展方向

因本项目是以简单易用的翻译器为目标，因此精简了一些不太好用的功能（如离线翻译API），未来也基本不会进行大的功能更新，而只对翻译API进行新增、维护，或者对界面的使用进行完善。

## 联系作者

如对本项目有任何建议或疑问，可提出issue或者发送邮件至`isayama_kagura@qq.com`。

## 依赖的项目
- [Textractor](https://github.com/Artikash/Textractor)
- [MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
- [Windows Community Toolkit](https://github.com/CommunityToolkit/WindowsCommunityToolkit)

## 部分代码参考
- [御坂翻译器](https://github.com/hanmin0822/MisakaTranslator)
- [YUKI Galgame 翻译器](https://github.com/project-yuki/YUKI)

