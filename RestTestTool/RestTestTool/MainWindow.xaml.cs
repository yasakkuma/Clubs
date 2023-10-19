using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace RestTestTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, string> paramsDic = new Dictionary<string, string>();
        RequestModel model = new RequestModel();
        public MainWindow()
        {
            InitializeComponent();
            uriText.Text = Properties.Settings.Default.DEFAULT_URI;
        }

        private async void RequestForm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uriText.Text))
                {
                    MessageBox.Show("URIが未入力");
                    return;
                }
                paramsDic.Clear();
                if (!string.IsNullOrWhiteSpace(paramNameText1.Text) && !string.IsNullOrWhiteSpace(paramText1.Text))
                    paramsDic.Add(paramNameText1.Text, paramText1.Text);
                if (!string.IsNullOrWhiteSpace(paramNameText2.Text) && !string.IsNullOrWhiteSpace(paramText2.Text))
                    paramsDic.Add(paramNameText2.Text, paramText2.Text);
                if (!string.IsNullOrWhiteSpace(paramNameText3.Text) && !string.IsNullOrWhiteSpace(paramText3.Text))
                    paramsDic.Add(paramNameText3.Text, paramText3.Text);
                if (!string.IsNullOrWhiteSpace(paramNameText4.Text) && !string.IsNullOrWhiteSpace(paramText4.Text))
                    paramsDic.Add(paramNameText4.Text, paramText4.Text);

                switch (methodComboBox.SelectedIndex)
                {
                    case (int)MethodType.Get:
                        if (await model.DoGet(uriText.Text, paramsDic))
                        {
                            var dialog = new SaveFileDialog();
                            dialog.Filter = "JSONファイル|*.json";
                            if (dialog.ShowDialog() ?? false)
                            {
                                File.WriteAllText(dialog.FileName, model.json);
                            }

                        }
                        else
                        {
                            MessageBox.Show(model.json);
                        }

                        break;
                    case (int)MethodType.Post:
                        if (await model.DoPost(uriText.Text, paramsDic))
                        {
                            var dialog = new SaveFileDialog();
                            dialog.Filter = "JSONファイル|*.json";
                            if (dialog.ShowDialog() ?? false)
                            {
                                File.WriteAllText(dialog.FileName, model.json);
                            }
                        }
                        else
                        {
                            MessageBox.Show(model.json);
                        }
                        break;
                    case (int)MethodType.Put:
                        break;
                    case (int)MethodType.Delete:
                        break;
                    case (int)MethodType.Patch:
                        break;
                    default:
                        MessageBox.Show("メソッドタイプが未設定");
                        return;
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
    }
}
