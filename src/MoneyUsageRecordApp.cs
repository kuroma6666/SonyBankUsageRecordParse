using SonyBankUsageRecordParse.src;
using SonyBankUsageRecordParse.src.subsystems.CSV;
using SonyBankUsageRecordParse.src.subsystems.CSV.Common;
using SonyBankUsageRecordParse.src.subsystems.Tagging;
using SonyBankUsageRecordParse.src.subsystems.Transactions;

using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
namespace SonyBankUsageRecordParse
{
	public partial class MoneyUsageRecordApp : Form
	{
		private ComboBox comboBoxExpenseCategory;
		private List<ExpenseTransaction> transactions = [];


		public MoneyUsageRecordApp()
		{
			InitializeComponent();
			SetupComboBox();

		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void ButtoParseCSVFile_Click(object sender, EventArgs e)
		{
			String csvFilePath = CSVUtilitiy.SelectCSVFile();
			if (csvFilePath != null)
			{
				Debug.WriteLine($"選択されたCSVファイル: {csvFilePath}");
			}
			else
			{
				Debug.WriteLine("CSVファイルが選択されませんでした。");
				return;
			}
			var parser = new SonyBankCSV();
			Debug.WriteLine("Parse CSV Start");
			var transactions = parser.ParseCSV(csvFilePath);
			Debug.WriteLine("Parse CSV End");
			// パースした利用明細を利用明細ListViewに登録
			foreach (var transaction in transactions)
			{
				Debug.WriteLine(transaction);
				var listViewItem = new ListViewItem(transaction.Date.ToString("yyyy-MM-dd"));
				listViewItem.SubItems.Add(transaction.StoreName);
				listViewItem.SubItems.Add(transaction.Amount.ToString("C", CultureInfo.CurrentCulture));
				listViewItem.SubItems.Add(transaction.Balance.ToString("C", CultureInfo.CurrentCulture));

				transactionListView.Items.Add(listViewItem); // 利用明細ListViewに追加

				String expenseCategoryDefault = " ";
				ListViewItem expenseItem = new ListViewItem(
					[expenseCategoryDefault, transaction.StoreName, transaction.Amount.ToString("C", CultureInfo.CurrentCulture)]
					);
				listViewExpenseRegistration.Items.Add(expenseItem); // 費用項目登録用ListViewに追加
			}
		}

		private void SetupComboBox()
		{
			String[] expenseCategories = [" ", "食費", "外食費", "被服費", "娯楽費", "日用品・雑貨費", "サブスクリプション費"];
			comboBoxExpenseCategory = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Location = new Point(420, 50),
				Size = new Size(120, 30),
				BackColor = listViewExpenseRegistration.BackColor,
				ForeColor = listViewExpenseRegistration.ForeColor
			};
			comboBoxExpenseCategory.Items.AddRange(expenseCategories);
			comboBoxExpenseCategory.SelectedIndexChanged += ComboBoxExpenseCategory_SelectedIndexChanged;
			this.Controls.Add(comboBoxExpenseCategory);
			// Setup時点では費用項目選択コンボボックスは非表示で良い
			comboBoxExpenseCategory.Visible = false;
		}

		private void ButtonGenerateStatistics_Click(object sender, EventArgs e)
		{
			listViewExpenseStatistics.Items.Clear();

			// 費用項目登録用ListViewからデータを統計ListViewにまとめる
			foreach (ListViewItem item in listViewExpenseRegistration.Items)
			{
				if (item.SubItems[0].Text.Contains(" ")) continue;
				if (comboBoxExpenseCategory.SelectedItem != null && !String.IsNullOrEmpty(item.SubItems[1].Text))
				{
					String category = item.SubItems[0].Text;
					String amountText = item.SubItems[2].Text;
					Decimal amount = Decimal.Parse(amountText, NumberStyles.Currency, CultureInfo.CurrentCulture);
					Debug.WriteLine(amount);

					// 統計リストに集計
					var statItem = listViewExpenseStatistics.Items.Cast<ListViewItem>()
							.FirstOrDefault(i => i.SubItems[0].Text == category);

					if (statItem != null)
					{
						// 既存カテゴリがあれば合計を更新
						String totalText = statItem.SubItems[1].Text;
						Decimal totalAmount = Decimal.Parse(totalText, NumberStyles.Currency, CultureInfo.CurrentCulture);
						totalAmount += amount;
						statItem.SubItems[1].Text = totalAmount.ToString("C", CultureInfo.CurrentCulture);
					}
					else
					{
						// 新規カテゴリとして追加
						listViewExpenseStatistics.Items.Add(new ListViewItem([category, amountText.ToString()]));
					}
				}
			}
		}

		private void ListViewExpenseRegistration_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listViewExpenseRegistration.SelectedItems.Count > 0)
			{
				var selectedItem = listViewExpenseRegistration.SelectedItems[0];
				var itemBounds = selectedItem.GetBounds(ItemBoundsPortion.Label);
				var expenseCategoryColumnWidth = transactionListView.Columns[0].Width;
				// [[comboBoxExpenseCategory]]の描画更新
				comboBoxExpenseCategory.Location = new Point(itemBounds.Left + 20, itemBounds.Top + 90);
				comboBoxExpenseCategory.Width = expenseCategoryColumnWidth + 15;
				comboBoxExpenseCategory.Visible = true;
				comboBoxExpenseCategory.BringToFront();
				comboBoxExpenseCategory.Text = selectedItem.SubItems[0].Text;

				// [[textBox_StoreNameExpenseCategory]]の描画更新
				if (listViewExpenseRegistration.SelectedItems.Count > 0)
				{
					var selectedStoreName = selectedItem.SubItems[1].Text;
					// 選択された費用項目を取得
					var selectedExpenseCategory = selectedItem.SubItems[0].Text;
					if (selectedExpenseCategory.Contains(" ")) selectedExpenseCategory = "未選択";
					textBox_StoreNameExpenseCategory.Text = $"{selectedStoreName}:{selectedExpenseCategory}";
				}
			}
		}

		private void ComboBoxExpenseCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			// 選択されている ListView のアイテムを取得
			if (listViewExpenseRegistration.SelectedItems.Count > 0)
			{
				var selectedItem = listViewExpenseRegistration.SelectedItems[0];

				// 選択された費用項目を取得
				var selectedExpenseCategory = comboBoxExpenseCategory.SelectedItem.ToString();

				// 選択された行の店名を取得
				var selectedStoreName = selectedItem.SubItems[1].Text;

				foreach (ListViewItem item in listViewExpenseRegistration.Items)
				{
					if (item.SubItems[1].Text == selectedStoreName) // 店名を比較
					{
						item.SubItems[0].Text = selectedExpenseCategory; // 費用項目を更新
					}
				}
				textBox_StoreNameExpenseCategory.Text = $"{selectedStoreName}:{selectedExpenseCategory}";
			}
		}

		private void TabControlListViews_SelectedIndexChanged(object sender, EventArgs e)
		{
			comboBoxExpenseCategory.Visible = false;

		}

		private void LinkStoreToExpenseCategory(String storeNameTag)
		{
			// StoreConfigのインスタンスを作成
			StoreNameTagConfig storeConfig = new StoreNameTagConfig();
			// storeNameTagを「:」で分割して、店名と費用項目を抽出
			var parts = storeNameTag.Split(':');
			if (parts.Length != 2)
			{
				MessageBox.Show("無効なフォーマットです。正しいフォーマットは '店名:費用項目' です。");
				return;
			}
			var storeName = parts[0];
			var expenseCategory = parts[1];
			// 既に同じ店名が登録されているか確認
			var existingTag = storeConfig.StoreNameTags
					.FirstOrDefault(tag => tag.StartsWith($"{storeName}:"));
			if (existingTag != null)
			{
				// 既存の費用項目を取得
				var existingExpenseCategory = existingTag.Split(':')[1];

				// もし費用項目が異なる場合、変更ダイアログを表示
				if (existingExpenseCategory != expenseCategory)
				{
					var result = MessageBox.Show(
							$"店名 '{storeName}' には既に '{existingExpenseCategory}' が登録されています。変更しますか？",
							"費用項目の変更",
							MessageBoxButtons.YesNo
					);

					if (result == DialogResult.Yes)
					{
						// 費用項目を更新して保存
						storeConfig.StoreNameTags.Remove(existingTag);
						storeConfig.StoreNameTags.Add(storeNameTag);
						storeConfig.Write(); // TOMLファイルに保存
						var writeResult = MessageBox.Show(
							$"店名 '{storeName}' の費用項目を '{expenseCategory}' に変更しました。",
							"費用項目の変更",
							MessageBoxButtons.OK
					);
					}
				}
				else
				{
					MessageBox.Show($"{storeNameTag} は既に登録されています。");
				}
			}
			else
			{
				storeConfig.StoreNameTags.Add(storeNameTag);
				storeConfig.Write(); // TOMLファイルに保存
			}
		}

		private void buttonLinkStoreExpensPageRegister_Click(object sender, EventArgs e)
		{
			LinkStoreToExpenseCategory(textBox_StoreNameExpenseCategory.Text);
		}

		private void ButtonDisplayStoreNameTagList_Click(object sender, EventArgs e)
		{
			var storeNameTagLists = new StoreNameTagLists();
			storeNameTagLists.Show();
		}
	}
}
