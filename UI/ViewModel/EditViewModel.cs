using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using RegularExpressionBL;
using RegularExpressionData;

namespace UI.ViewModel
{
    class EditViewModel:ViewModelBase
    {
        private string _regExp = "";
        public string RegExp
        {
            get => _regExp;
            set
            {
                _regExp = value;
                RaisePropertyChanged();
                if (!_running)
                {
                    _running = !_running;
                    Messenger.Default.Send(nameof(RegExp), "RegExpChange");
                }
            }
        }
        private string _mainText = "";
        public string MainText
        {
            get => _mainText;
            set
            {
                _mainText = value;
                RaisePropertyChanged();
                if (!_running)
                {
                    _running = !_running;
                    Messenger.Default.Send(nameof(MainText), "MainTextChange");
                }
            }
        }
        public List<ExtendText> MainTexts { get; set; }
            = new List<ExtendText>();
        public List<ExtendText> RegexTexts { get; set; }
            = new List<ExtendText>();
        public RegexBuilder Regex { get; set; }
        public List<IMatch> Matches { get; set ; } 
            = new List<IMatch>();

        private static bool _running;
        public string MatchesCount
        {
            get => Matches.Count.ToString();
        }

        private static Section _mainTextSection;

        public ObservableCollection<IRegex> RegExpCollection { get; } 
            = new ObservableCollection<IRegex>();

        public EditViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(Matches, this);
            BindingOperations.EnableCollectionSynchronization(MainTexts, this);
            BindingOperations.EnableCollectionSynchronization(RegExpCollection, this);
            BindingOperations.EnableCollectionSynchronization(RegexTexts, this);
            var b = new UserRegex()
                { Color = Brushes.Aqua, Name = "UserRegex", Regex = @"\d"};
            RegExpCollection.Add(b);
            Regex = new RegexBuilder(RegExpCollection.ToList());
            Messenger.Default.Register<string>(this, "RegExpChange",
                 (message) => {
                     try
                     {
                         var regExpSection = GetText(RegExp);
                         RegexTexts = GetListExt(regExpSection);
                         GetRegexList(RegExpCollection, RegexTexts);
                         GetBuilderRegex(RegExpCollection);
                         if (_mainTextSection != null)
                         {
                             MainTexts = GetListExt(_mainTextSection);
                             FindMatches();
                         }
                         else
                         {
                             _running = !_running;
                         }
                     }
                     catch (Exception e)
                     {
                         Debug.WriteLine(e);
                         throw;
                     }
                });
            Messenger.Default.Register<string>(this, "MainTextChange",
                (message) => {
                    _mainTextSection = GetText(MainText);
                    MainTexts = GetListExt(_mainTextSection);
                    FindMatches();
                });

        }


        private async void FindMatches()
        {
            if (MainTexts != null)
            {
                var position = (from match in await MatchFinder.Parse(MainTexts, Regex)
                    let brush = match.Regex.Color
                    let pos = new TextRange(match.StartTextPointer.GetPositionAtOffset(match.StartOffset),
                        match.EndTextPointer.GetPositionAtOffset(match.EndOffset))
                    select new {pos, brush}).ToList();
                foreach (var textRange in position)
                {
                    textRange.pos.ApplyPropertyValue(TextElement.BackgroundProperty,textRange.brush);
                }
                MainText = XamlWriter.Save(_mainTextSection);
                _running = !_running;
            }
        }

        private  Section GetText(string str)
        {
            return(Section) XamlReader.Parse(str);
        }
        private List<ExtendText> GetListExt(Section section)
        {
            List<ExtendText> texts = new List<ExtendText>();
            foreach (var paragraph in Parser.ExtractParagraphs(section.Blocks).ToList())
            {
                var text = new TextRange(paragraph.ContentStart, paragraph.ContentEnd);
                text.ClearAllProperties();
                texts.AddRange(Parser.ExtractText(paragraph.Inlines).ToList());
            }
            return texts;
        }
        private void GetRegexList(ObservableCollection<IRegex> regexes, List<ExtendText> list)
        {
            foreach (var inile in list)
            {
                regexes.First(x => x.Name == "UserRegex").Regex = inile.Text;
            }
        }

        private void GetBuilderRegex(IEnumerable<IRegex> textRegexes)
        {
             Regex = new RegexBuilder(textRegexes.ToList());
        }
    }
}
