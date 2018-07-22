using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using Xceed.Wpf.Toolkit;

namespace UI.Formatters
{
    public class CustomFormatter : ITextFormatter
    {
        public string GetText(FlowDocument document)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                new TextRange(document.ContentStart, document.ContentEnd).Save((Stream)memoryStream, DataFormats.Xaml);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public void SetText(FlowDocument document, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    document.Blocks.Clear();
                }
                else
                {
                    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
                        new TextRange(document.ContentStart, document.ContentEnd).Load((Stream)memoryStream, DataFormats.Xaml);
                }
            }
            catch
            {
                throw new InvalidDataException("Data provided is not in the correct Xaml format.");
            }
        }
    }
}
