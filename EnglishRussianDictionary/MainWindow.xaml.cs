using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Windows.Data;

namespace EnglishRussianDictionary
{
    public partial class MainWindow : Window
    {
        private WordTranslate wordTranslate;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            WindowUpdate();
        }

        private void WindowUpdate()
        {
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;

            tbxWord.Text = "";
            tbxTranslate.Text = "";

            using (var db = new DBEntity())
            {
                db.Dictionary.Load();
                dataGrid.ItemsSource = db.Dictionary.Local.ToObservableCollection();
            }

            CollectionViewSource.GetDefaultView(dataGrid.ItemsSource).Filter = ToFilter;
        }

        private void BtnAdd(object sender, RoutedEventArgs e)
        {
            var wordTranslate = new WordTranslate()
            {
                Word = tbxWord.Text,
                Translate = tbxTranslate.Text
            };

            using (var db = new DBEntity())
            {
                db.Dictionary.Add(wordTranslate);
                db.SaveChanges();
            }

            WindowUpdate();
        }

        private void BtnUpdate(object sender, RoutedEventArgs e)
        {
            wordTranslate.Word = tbxWord.Text;
            wordTranslate.Translate = tbxTranslate.Text;            

            using (var db = new DBEntity())
            {
                db.Dictionary.Update(wordTranslate);
                db.SaveChanges();
            }

            WindowUpdate();
        }

        private void BtnDelete(object sender, RoutedEventArgs e)
        {
            using (var db = new DBEntity())
            {
                db.Dictionary.Remove(wordTranslate);
                db.SaveChanges();
            }

            WindowUpdate();
        }

        private void DataGridSelectionChanged(object sender, RoutedEventArgs e)
        {
            wordTranslate = (WordTranslate)dataGrid.SelectedItem;

            if (wordTranslate == null) return;

            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;

            tbxWord.Text = wordTranslate.Word;
            tbxTranslate.Text = wordTranslate.Translate;            
        }

        private void TbxSearchTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGrid.ItemsSource).Refresh();
        }

        private bool ToFilter(object row)
        {
            wordTranslate = (WordTranslate)row;

            return wordTranslate.Word.StartsWith(tbxSearch.Text, StringComparison.OrdinalIgnoreCase)
                    || wordTranslate.Translate.StartsWith(tbxSearch.Text, StringComparison.OrdinalIgnoreCase);
        }
    }
}
