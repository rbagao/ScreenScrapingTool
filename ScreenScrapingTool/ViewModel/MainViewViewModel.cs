using ScreenScrapingTool.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScreenScrapingTool.ViewModel
{
    public class MainViewViewModel : ViewModelBase
    {

        private string _keywords = "conveyancing software";
        public string Keywords
        {
            get
            {
                return _keywords;
            }
            set
            {
                _keywords = value;
                OnPropertyChanged(nameof(Keywords));
                OnPropertyChanged(nameof(CanRank));
            }
        }

        private string _url = "www.smokeball.com.au";
        public string URL
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                OnPropertyChanged(nameof(URL));
                OnPropertyChanged(nameof(CanRank));
            }
        }

        private string _ranking = "###";
        public string Ranking
        {
            get
            {
                return _ranking;
            }
            set
            {
                _ranking = value;
                OnPropertyChanged(nameof(Ranking));
            }
        }
        

        private bool _inProgress;
        public bool InProgress
        {
            get
            {
                return _inProgress;
            }
            set
            {
                _inProgress = value;
                OnPropertyChanged(nameof(InProgress));
                OnPropertyChanged(nameof(CanRank));
            }
        }

        public bool CanRank
        {
            get
            {
                return (!string.IsNullOrEmpty(this.Keywords) && !string.IsNullOrEmpty(this.URL) && !InProgress);
            }
        }


        // GetRanking Command
        private RelayCommand _getRankingCommand;
        public ICommand GetRankingCommand
        {
            get
            {
                if (_getRankingCommand == null)
                {
                    this._getRankingCommand = new RelayCommand(async x => await GetRankingAsync(Keywords, URL));
                }
                return this._getRankingCommand;
            }
        }
        public async Task<int> GetRankingAsync(string keywords, string url)
        {
            int counter = 0;
            try
            {
                if (string.IsNullOrEmpty(keywords) || string.IsNullOrEmpty(url)) return counter;

                InProgress = true;
                Ranking = "###";
                
                await Task.Run(() =>
                {
                    //string searchUrl = "https://www.google.com.au/search?num=100&q=conveyancing+software";
                    string searchUrl = string.Format($"https://www.google.com.au/search?num=100&q={keywords}");
                    Uri uri = new Uri(searchUrl);
                    HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
                    req.Method = "GET";
                    req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:x.x.x) Gecko/20041107 Firefox/x.x";

                    HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
                    Stream s = resp.GetResponseStream();
                    TextReader reader = new StreamReader(s, true);
                    string Text = reader.ReadToEnd();

                    //<cite>(.*?)<\/cite>                               // problems with youtube result, doesn't use cite tag
                    //string exp = "<h3 class=\"r\">(.*?)<\\/h3>";      // working but what if they changed the header size to 4 or any value
                    string exp = "<div class=\"g\">(.*?)<\\/div>";     // settle with this one because it is unlikely they will change the class
                    var matches = Regex.Matches(Text, exp);             // I've read that you shouldn't use regular expression in html but without using htmlagilitypack or google api
                                                                        // in my opinion this is the best I can use, and the expression is only a simple one and not that complicated
                    
                    foreach (var match in matches)
                    {
                        counter++;
                        var res = match.ToString();
                        if (res.Contains(url))
                        {
                            Ranking = counter.ToString();
                            break;
                        }
                    }
                });
                
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            finally
            {
                InProgress = false;
            }
            return counter;
        }


        // Exit Command
        private RelayCommand _exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                    this._exitCommand = new RelayCommand(x => Cancel(Window.GetWindow(x as DependencyObject)));
                return this._exitCommand;
            }
        }
        public void Cancel(Window window)
        {
            window.Close();
        }
    }
}
